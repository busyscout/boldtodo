using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace BoldTodo
{
    /// <summary>
    /// Classifier that classifies all text as an instance of the "TodoClassifier" classification type.
    /// </summary>
    internal class TodoClassifier : IClassifier
    {
        /// <summary>
        /// Classification type.
        /// </summary>
        private readonly IClassificationType classificationType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoClassifier"/> class.
        /// </summary>
        /// <param name="registry">Classification registry.</param>
        internal TodoClassifier(IClassificationTypeRegistryService registry)
        {
            this.classificationType = registry.GetClassificationType("TodoClassifier");
        }

        #region IClassifier

        /// <summary>
        /// An event that occurs when the classification of a span of text has changed.
        /// </summary>
        /// <remarks>
        /// This event gets raised if a non-text change would affect the classification in some way,
        /// for example typing /* would cause the classification to change in C# without directly
        /// affecting the span.
        /// </remarks>
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
        
        /// <summary>
        /// Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text.
        /// </summary>
        /// <remarks>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </remarks>
        /// <param name="span">The span currently being classified.</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var txt = span.GetText();
            var startCommentIndex = txt.IndexOf("//");
            if(startCommentIndex < 0)
            {
                //maybe it is multline comment?
                var isMultlineComment = false;
                for (var i = 0; i < txt.Length; i++)
                {
                    var c = txt[i];
                    if (char.IsWhiteSpace(c) || c == '/')
                    {
                        continue;
                    }
                    if(c == '*')
                    {
                        isMultlineComment = true;
                        startCommentIndex = i;
                        break;
                    }
                    else
                    {
                        return Array.Empty<ClassificationSpan>();
                    }
                }
                if (!isMultlineComment)
                {
                    return Array.Empty<ClassificationSpan>();
                }
            }

            List<ClassificationSpan> result = null;
            
            //real length without line break
            var txtLength = txt.Length;
            if (txtLength > 2) 
            {
                if (txt[txt.Length - 1] == '\n')
                {
                    txtLength--;
                    if (txt[txt.Length - 2] == '\r')
                    {
                        txtLength--;
                    }
                }
            }
            foreach(var token in tokens)
            {
                var tokenIndex = txt.IndexOf(token, startCommentIndex, StringComparison.InvariantCultureIgnoreCase);
                if (tokenIndex <= startCommentIndex)
                    continue;
                if((tokenIndex + token.Length == txtLength) || 
                    afterSymbols.Contains(txt[tokenIndex + token.Length]))
                {
                    if (!prevSymbols.Contains(txt[tokenIndex - 1]))
                        continue;

                    if (result == null)
                        result = new List<ClassificationSpan>(1); //we expect 1 keyword in comment for most cases
                    result.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(span.Start + tokenIndex, token.Length)), this.classificationType));
                }
            }

            return result ?? (IList<ClassificationSpan>)Array.Empty<ClassificationSpan>();
        }

        #endregion

        static readonly string[] tokens = new[] { "todo", "hack" };
        /// <summary>
        /// Symbols allowed after token
        /// </summary>
        static readonly char[] afterSymbols = new[] { ' ', ':', ')' };
        /// <summary>
        /// Symbols allowed before token
        /// </summary>
        static readonly char[] prevSymbols = new[] { ' ', ',', '(', '/' };
    }
}

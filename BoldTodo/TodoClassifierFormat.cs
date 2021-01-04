using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace BoldTodo
{
    /// <summary>
    /// Defines an editor format for the TodoClassifier type that makes it bold
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "TodoClassifier")]
    [Name("TodoClassifier")]
    [UserVisible(true)] // This should be visible to the end user
    [Order(After = "identifier")]
    internal sealed class TodoClassifierFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoClassifierFormat"/> class.
        /// </summary>
        public TodoClassifierFormat()
        {
            this.DisplayName = "TodoClassifier"; // Human readable version of the name
            this.IsBold = true;
        }
    }
}

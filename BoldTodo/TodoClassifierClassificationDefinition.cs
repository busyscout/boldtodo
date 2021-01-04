using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace BoldTodo
{
    /// <summary>
    /// Classification type definition export for TodoClassifier
    /// </summary>
    internal static class TodoClassifierClassificationDefinition
    {
        // The field is used by MEF. DO NOT DELETE!!!
        /// <summary>
        /// Defines the "TodoClassifier" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("TodoClassifier")]
        private static ClassificationTypeDefinition typeDefinition;
    }
}

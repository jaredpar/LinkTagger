using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace LinkTagger.Implementation.LinkTagger
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = LinkTaggerSource.TagName)]
    [Name(LinkTaggerSource.TagName)]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class LinkTaggerFormatDefinition : ClassificationFormatDefinition
    {
        internal LinkTaggerFormatDefinition()
        {
            DisplayName = "Link Tagger";
            TextDecorations = System.Windows.TextDecorations.Underline;
            ForegroundColor = Colors.Red;
        }
    }

    internal static class LinkTaggerFormatExports
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name(LinkTaggerSource.TagName)]
        internal static ClassificationTypeDefinition LinkTaggerDefinition = null;
    }
}

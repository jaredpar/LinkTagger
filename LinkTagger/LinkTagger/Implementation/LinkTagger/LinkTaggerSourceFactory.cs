using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using EditorUtils;
using Microsoft.VisualStudio.Text.Classification;

namespace LinkTagger.Implementation.LinkTagger
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [TagType(typeof(ClassificationTag))]
    internal sealed class LinkTaggerSourceFactory : IViewTaggerProvider
    {
        private readonly object _key = new object();
        private readonly IClassificationType _classificationType;
        private readonly ILinkTaggerUtil _linkTaggerUtil;

        [ImportingConstructor]
        internal LinkTaggerSourceFactory(ILinkTaggerUtil linkTaggerUtil, IClassificationTypeRegistryService classificationTypeRegistryService)
        {
            _linkTaggerUtil = linkTaggerUtil;
            _classificationType = classificationTypeRegistryService.GetClassificationType(LinkTaggerSource.TagName);
        }

        ITagger<T> IViewTaggerProvider.CreateTagger<T>(ITextView textView, ITextBuffer textBuffer)
        {
            if (textView.TextBuffer != textBuffer)
            {
                return null;
            }

            return EditorUtilsFactory.CreateBasicTagger(
                textView.Properties,
                _key,
                () => new LinkTaggerSource(textView, _classificationType, _linkTaggerUtil)) as ITagger<T>;
        }
    }
}

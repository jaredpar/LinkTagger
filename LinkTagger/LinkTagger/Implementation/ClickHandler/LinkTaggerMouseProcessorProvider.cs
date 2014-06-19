using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace LinkTagger.Implementation.ClickHandler
{
    [Export(typeof(IMouseProcessorProvider))]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    [Name("Link Tagger Mouse Provider")]
    internal sealed class LinkTaggerMouseProcessorProvider : IMouseProcessorProvider
    {
        private readonly ILinkTaggerUtil _linkTaggerUtil;

        [ImportingConstructor]
        internal LinkTaggerMouseProcessorProvider(ILinkTaggerUtil linkTaggerUtil)
        {
            _linkTaggerUtil = linkTaggerUtil;
        }

        public IMouseProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            return new LinkTaggerMouseProcessor(wpfTextView, _linkTaggerUtil);
        }
    }
}

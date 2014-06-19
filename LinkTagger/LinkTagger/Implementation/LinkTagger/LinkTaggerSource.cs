using EditorUtils;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Text.Classification;

namespace LinkTagger.Implementation.LinkTagger
{
    internal sealed class LinkTaggerSource : IBasicTaggerSource<ClassificationTag>
    {
        internal const string TagName = "LinkerTag";

        private readonly ITextView _textView;
        private readonly ClassificationTag _classificationTag;
        private readonly ILinkTaggerUtil _linkTaggerUtil;

        // The interface forces us to implement these event but we never raise it.  This is
        // by design.  The tag information we return for a given query is immutable 
#pragma warning disable 67
        public event EventHandler Changed;
#pragma warning restore 67

        public ITextSnapshot TextSnapshot
        {
            get { return _textView.TextSnapshot; }
        }

        internal LinkTaggerSource(ITextView textView, IClassificationType classificationType, ILinkTaggerUtil linkTaggerUtil)
        {
            _textView = textView;
            _classificationTag = new ClassificationTag(classificationType);
            _linkTaggerUtil = linkTaggerUtil;
        }

        public ReadOnlyCollection<ITagSpan<ClassificationTag>> GetTags(SnapshotSpan span)
        {
            var list = new List<ITagSpan<ClassificationTag>>();
            var snapshot = span.Snapshot;
            int i = 0; 
            while (i < span.Length)
            {
                int position = i + span.Start.Position;
                var linkText = _linkTaggerUtil.GetLinkTextFromStart(new SnapshotPoint(snapshot, position));
                if (linkText != null)
                {
                    var snapshotSpan = new SnapshotSpan(snapshot, position, linkText.Length);
                    list.Add(new TagSpan<ClassificationTag>(snapshotSpan, _classificationTag));

                    i += linkText.Length;
                }
                else
                {
                    i++;
                }
            }

            return new ReadOnlyCollection<ITagSpan<ClassificationTag>>(list);
        }
    }
}

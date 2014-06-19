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
        private event EventHandler _changed;

        internal LinkTaggerSource(ITextView textView, IClassificationType classificationType, ILinkTaggerUtil linkTaggerUtil)
        {
            _textView = textView;
            _classificationTag = new ClassificationTag(classificationType);
            _linkTaggerUtil = linkTaggerUtil;
        }

        private ReadOnlyCollection<ITagSpan<ClassificationTag>> GetTags(SnapshotSpan span)
        {
            var list = new List<ITagSpan<ClassificationTag>>();
            var snapshot = span.Snapshot;
            int i = 0; 
            while (i < span.Length)
            {
                int position = i + span.Start.Position;
                var linkText = _linkTaggerUtil.GetLinkText(new SnapshotPoint(snapshot, position));
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

        private static bool StartsWith(ITextSnapshot snapshot, int position, string target)
        {
            if (position + target.Length >= snapshot.Length)
            {
                return false;
            }

            for (int i = 0; i < target.Length; i++)
            {
                if (snapshot[i + position] != target[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetUserName(ITextSnapshot snapshot, int startPosition)
        {
            int endPosition = startPosition;
            while (endPosition < snapshot.Length && !Char.IsWhiteSpace(snapshot[endPosition]))
            {
                endPosition++;
            }

            return snapshot.GetText(startPosition, endPosition - startPosition);
        }

#region IBasicTaggerSource

        ITextSnapshot IBasicTaggerSource<ClassificationTag>.TextSnapshot
        {
            get { return _textView.TextSnapshot; }
        }

        event EventHandler IBasicTaggerSource<ClassificationTag>.Changed
        {
            add { _changed += value; }
            remove { _changed -= value; }
        }

        ReadOnlyCollection<ITagSpan<ClassificationTag>> IBasicTaggerSource<ClassificationTag>.GetTags(SnapshotSpan span)
        {
            return GetTags(span);
        }

#endregion
    }
}

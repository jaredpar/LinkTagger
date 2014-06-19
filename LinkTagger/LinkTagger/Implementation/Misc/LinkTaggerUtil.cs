using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text;

namespace LinkTagger.Implementation.Misc
{
    [Export(typeof(ILinkTaggerUtil))]
    internal sealed class LinkTaggerUtil : ILinkTaggerUtil
    {
        private const string LinkPrefix = "github:";

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

        private static string GetUserNameFromStart(ITextSnapshot snapshot, int startPosition)
        {
            int endPosition = startPosition;
            while (endPosition < snapshot.Length && !Char.IsWhiteSpace(snapshot[endPosition]))
            {
                endPosition++;
            }

            return snapshot.GetText(startPosition, endPosition - startPosition);
        }

        public string GetLinkTextFromStart(SnapshotPoint point)
        {
            string userName = GetUserNameFromStart(point);
            if (userName != null)
            {
                return LinkPrefix + userName;
            }

            return null;
        }

        public string GetUserName(SnapshotPoint point)
        {
            while (point.Position > 0 && !Char.IsWhiteSpace(point.GetChar()))
            {
                point = point.Subtract(1);
            }

            if (Char.IsWhiteSpace(point.GetChar()))
            {
                point = point.Add(1);
            }

            return GetUserNameFromStart(point);
        }

        public string GetUserNameFromStart(SnapshotPoint point)
        {
            if (StartsWith(point.Snapshot, point.Position, LinkPrefix))
            {
                return GetUserNameFromStart(point.Snapshot, point.Position + LinkPrefix.Length);
            }

            return null;
        }
    }
}

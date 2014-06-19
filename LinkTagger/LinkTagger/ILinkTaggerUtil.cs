using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkTagger
{
    interface ILinkTaggerUtil
    {
        /// <summary>
        /// Get the user name at the given point which is specified in a link.  If this 
        /// point does not refer to a link the method will return null
        /// </summary>
        string GetUserNameFromStart(SnapshotPoint point);

        /// <summary>
        /// Get the user name at the given point which is specified in a link.  The point
        /// can be anywhere inside the link text
        /// </summary>
        string GetUserName(SnapshotPoint point);

        /// <summary>
        /// Get the link text at the given point.  If this point does not erfer to a link 
        /// the method will return null
        /// </summary>
        string GetLinkTextFromStart(SnapshotPoint point);
    }
}

using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LinkTagger.Implementation.ClickHandler
{
    internal sealed class LinkTaggerMouseProcessor : MouseProcessorBase
    {
        private readonly IWpfTextView _wpfTextView;
        private readonly ILinkTaggerUtil _linkTaggerUtil;

        internal LinkTaggerMouseProcessor(IWpfTextView wpfTextView, ILinkTaggerUtil linkTaggerUtil)
        {
            _wpfTextView = wpfTextView;
            _linkTaggerUtil = linkTaggerUtil;
        }

        public override void PostprocessMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var point = _wpfTextView.Caret.Position.BufferPosition;
                var userName = _linkTaggerUtil.GetUserName(point);
                if (userName != null)
                {
                    var link = string.Format("https://github.com/{0}", userName);
                    Process.Start(link);
                }
            }
        }
    }
}

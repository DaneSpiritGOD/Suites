using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Indentation;

namespace Suites.Wpf.AvalonEdit
{
    public class XmlFoldingHelper
    {
        private readonly TextEditor _textEditor;
        private DispatcherTimer _foldingUpdateTimer;
        private FoldingManager _foldingManager;
        private XmlFoldingStrategy _foldingStrategy;

        private bool _started = false;

        public XmlFoldingHelper(TextEditor editor)
        {
            _textEditor = NamedNullException.Assert(editor, nameof(editor));

            _init();
        }

        private void _init()
        {
            if (_textEditor.SyntaxHighlighting == null)
            {
                _foldingStrategy = null;
            }
            else
            {
                switch (_textEditor.SyntaxHighlighting.Name)
                {
                    case "XML":
                        _foldingStrategy = new XmlFoldingStrategy();
                        _textEditor.TextArea.IndentationStrategy = new DefaultIndentationStrategy();
                        break;
                    default:
                        _textEditor.TextArea.IndentationStrategy = new DefaultIndentationStrategy();
                        _foldingStrategy = null;
                        break;
                }
            }
            if (_foldingStrategy != null)
            {
                if (_foldingManager == null)
                    _foldingManager = FoldingManager.Install(_textEditor.TextArea);
                _foldingStrategy.UpdateFoldings(_foldingManager, _textEditor.Document);
            }
            else
            {
                if (_foldingManager != null)
                {
                    FoldingManager.Uninstall(_foldingManager);
                    _foldingManager = null;
                }
            }

            _foldingUpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
        }

        public void Start()
        {
            if (_started)
            {
                return;
            }

            _started = true;

            _foldingUpdateTimer.Tick += foldingUpdateTimer_Tick;
            _foldingUpdateTimer.Start();
        }

        public void Stop()
        {
            if (_foldingUpdateTimer != null)
            {
                _started = false;
                _foldingUpdateTimer.Stop();
                _foldingUpdateTimer.Tick -= foldingUpdateTimer_Tick;
            }
        }

        private void foldingUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (_foldingStrategy != null)
            {
                _foldingStrategy.UpdateFoldings(_foldingManager, _textEditor.Document);
            }
        }

    }
}

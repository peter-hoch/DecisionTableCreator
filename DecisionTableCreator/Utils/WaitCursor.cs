using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DecisionTableCreator.Utils
{
    public class WaitCursor : IDisposable
    {
        public Cursor OldCursor { get; private set; }

        public Window Window { get; private set; }

        public WaitCursor(Window wnd)
        {
            OldCursor = wnd.Cursor;
            Window = wnd;
            Window.Cursor = Cursors.Wait;
        }

        public void Dispose()
        {
            if (Window != null)
            {
                Window.Cursor = OldCursor;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DecisionTableCreator.Utils
{
    public class WindowPosition
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public WindowState WindowState { get; set; }

        public bool IsValid { get; set; }

        public WindowPosition()
        {
            IsValid = false;
        }

        public void SetWindowPosition(Window window)
        {
            if (IsValid)
            {
                if (SystemParameters.VirtualScreenLeft < Left &&
                    SystemParameters.VirtualScreenTop < Top &&
                    SystemParameters.VirtualScreenWidth > Width &&
                    SystemParameters.VirtualScreenHeight > Height)
                {
                    if (WindowState == WindowState.Normal)
                    {
                        window.WindowState = WindowState;
                        window.Top = Top;
                        window.Left = Left;
                        window.Height = Height;
                        window.Width = Width;
                    }
                }
            }
        }

        public void GetWindowPosition(Window window)
        {
            if (window.WindowState == WindowState.Normal)
            {
                IsValid = true;
                WindowState = window.WindowState;
                Top = window.Top;
                Left = window.Left;
                Height = window.Height;
                Width = window.Width;
            }
        }
    }
}

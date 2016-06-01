/*
 * [The "BSD license"]
 * Copyright (c) 2016 Peter Hoch
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DecisionTableCreator.TestCases
{
    public enum BackgroundColor
    {
        White,
        Red,
        Aqua
    }


    public class Background : INotifyPropertyChanged
    {
        public Background(BackgroundColor color)
        {
            _backgroundColor = color;
        }

        private BackgroundColor _backgroundColor;

        public BackgroundColor BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (value != _backgroundColor)
                {
                    _backgroundColor = value;
                    OnPropertyChanged("BackgroundColor");
                    OnPropertyChanged("WpfBrush");
                    OnPropertyChanged("HtmlColor");
                }
            }
        }


        public string HtmlColor
        {
            get
            {
                switch (BackgroundColor)
                {
                    case BackgroundColor.White:
                        return "White";

                    case BackgroundColor.Red:
                        return "Red";

                    case BackgroundColor.Aqua:
                        return "Aqua";

                    default:
                        throw new NotSupportedException();
                }
            }
        }



        public Brush WpfBrush
        {
            get {
                switch (BackgroundColor)
                {
                    case BackgroundColor.White:
                        return Brushes.White;

                    case BackgroundColor.Red:
                        return Brushes.Red;

                    case BackgroundColor.Aqua:
                        return Brushes.Aqua;

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        #region event

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(name);
                PropertyChanged(this, args);
            }
        }

        #endregion
    }
}

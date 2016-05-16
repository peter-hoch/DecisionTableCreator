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


    public class Background
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
                        throw new ArgumentOutOfRangeException();
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
                        throw new ArgumentOutOfRangeException();
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

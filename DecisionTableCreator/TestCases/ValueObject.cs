using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DecisionTableCreator.TestCases
{
    public class ValueObject : INotifyPropertyChanged
    {
        public ValueObject(string value)
        {
            Value = value;
        }


        private String _value;

        public String Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        private Brush _background = Brushes.White;

        public Brush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged("Background");
            }
        }

        private Visibility _textBoxVisibility = Visibility.Visible;

        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
            set
            {
                _textBoxVisibility = value;
                OnPropertyChanged("TextBoxVisibility");
            }
        }



        public override string ToString()
        {
            return Value;
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

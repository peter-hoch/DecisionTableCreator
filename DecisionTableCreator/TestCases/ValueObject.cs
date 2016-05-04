using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public enum ValueObjejectDataType
        {
            Text,
            Enumeration,
            Bool    
        }

        public ValueObject(string text)
        {
            DataType = ValueObjejectDataType.Text;
            Text = text;
        }

        public ValueObject(bool value, string name)
        {
            DataType = ValueObjejectDataType.Bool;
            BoolValue = value;
            Name = name;
        }

        public ValueObject(ObservableCollection<Item> items)
        {
            DataType = ValueObjejectDataType.Enumeration;
            Items = items;
        }

        private ValueObjejectDataType _dataType;

        public ValueObjejectDataType DataType
        {
            get { return _dataType; }
            private set
            {
                _dataType = value;
                OnPropertyChanged("DataType");

                switch (value)
                {
                    case ValueObjejectDataType.Text:
                        TextBoxVisibility = Visibility.Visible;
                        ComboBoxVisibility = Visibility.Collapsed;
                        CheckboxVisibility = Visibility.Collapsed;
                        break;
                    case ValueObjejectDataType.Enumeration:
                        TextBoxVisibility = Visibility.Collapsed;
                        ComboBoxVisibility = Visibility.Visible;
                        CheckboxVisibility = Visibility.Collapsed;
                        break;
                    case ValueObjejectDataType.Bool:
                        TextBoxVisibility = Visibility.Collapsed;
                        ComboBoxVisibility = Visibility.Collapsed;
                        CheckboxVisibility = Visibility.Visible;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }


        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }


        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }


        public object Value
        {
            get
            {
                switch (DataType)
                {
                    case ValueObjejectDataType.Text:
                        return Text;

                    case ValueObjejectDataType.Enumeration:
                        return Items[SelectedItemIndex];

                    case ValueObjejectDataType.Bool:
                        return BoolValue;

                    default:
                        return "error " + DataType.ToString();
                }
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

        private Visibility _textBoxVisibility;

        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
            private set
            {
                _textBoxVisibility = value;
                OnPropertyChanged("TextBoxVisibility");
            }
        }

        private Visibility _comboBoxVisibility;

        public Visibility ComboBoxVisibility
        {
            get { return _comboBoxVisibility; }
            private set
            {
                _comboBoxVisibility = value;
                OnPropertyChanged("ComboBoxVisibility");
            }
        }


        private bool _boolValue;

        public bool BoolValue
        {
            get { return _boolValue; }
            set
            {
                _boolValue = value;
                OnPropertyChanged("BoolValue");
            }
        }


        private Visibility _checkboxVisibility;

        public Visibility CheckboxVisibility
        {
            get { return _checkboxVisibility; }
            set
            {
                _checkboxVisibility = value;
                OnPropertyChanged("CheckboxVisibility");
            }
        }


        private ObservableCollection<Item> _items;

        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }


        private int _selectedItemIndex;

        public int SelectedItemIndex
        {
            get { return _selectedItemIndex; }
            set
            {
                _selectedItemIndex = value;
                OnPropertyChanged("SelectedItemIndex");
            }
        }


        public override string ToString()
        {
            return Value.ToString();
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

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

        public static ValueObject Create(ConditionActionBase conditionOrAction)
        {
            ValueObject vo;
            switch (conditionOrAction.Type)
            {
                case ConditionActionBase.ConditionActionType.Text:
                    vo = new ValueObject(conditionOrAction.DefaultText);
                    break;
                case ConditionActionBase.ConditionActionType.Enum:
                    vo = CreateEnumValueObject(conditionOrAction);
                    break;
                case ConditionActionBase.ConditionActionType.Bool:
                    vo = new ValueObject(false, conditionOrAction.DefaultText);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            vo.ConditionOrActionParent = conditionOrAction;
            return vo;
        }

        private static ValueObject CreateEnumValueObject(ConditionActionBase conditionOrAction)
        {
            int defaultIndex = 0;
            int idx = 0;
            foreach (EnumValue value in conditionOrAction.EnumValues)
            {
                if (value.IsDefault)
                {
                    defaultIndex = idx;
                    break;
                }
                idx++;
            }
            ValueObject vo;
            vo = new ValueObject(conditionOrAction.EnumValues);
            vo.SelectedItemIndex = defaultIndex;
            return vo;
        }

        public ValueObject(string text)
        {
            DataType = ValueObjejectDataType.Text;
            Text = text;
        }

        public ValueObject(bool value, string text)
        {
            DataType = ValueObjejectDataType.Bool;
            BoolValue = value;
            Text = text;
        }

        public ValueObject(ObservableCollection<EnumValue> items)
        {
            DataType = ValueObjejectDataType.Enumeration;
            Items = items;
        }

        public ConditionActionBase ConditionOrActionParent { get; private set; }

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


        private ObservableCollection<EnumValue> _items;

        public ObservableCollection<EnumValue> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
                SetBackground();
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
                SetBackground();
            }
        }


        private string _tooltipText;

        public string TooltipText
        {
            get { return _tooltipText; }
            set
            {
                _tooltipText = value;
                OnPropertyChanged("TooltipText");
            }
        }

        private void SetBackground()
        {
            Brush brush = Brushes.White;

            if (DataType == ValueObjejectDataType.Enumeration)
            {
                EnumValue ev = Value as EnumValue;
                if (ev != null)
                {
                    if (ev.IsInvalid)
                    {
                        brush = Brushes.Red;
                    }
                    else if (ev.DontCare)
                    {
                        brush = Brushes.Aqua;
                    }
                }
            }

            if (Background != brush)
            {
                Background = brush;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        #region event

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
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

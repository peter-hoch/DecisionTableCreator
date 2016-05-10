using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    /// <summary>
    /// base class for condition and action
    /// </summary>
    public class ConditionActionBase
    {

        protected ConditionActionBase(string text, ValueDataType type)
        {
            Text = text;
            DataType = type;
        }

        protected ConditionActionBase(string text, ObservableCollection<EnumValue> enums) : this(text, ValueDataType.Enumeration)
        {
            EnumValues = enums;
        }

        public void Merge(ConditionActionBase clone)
        {
            if (Text != clone.Text)
            {
                Text = clone.Text;
            }
            if (DataType != clone.DataType)
            {
                DataType = clone.DataType;
            }
            if (DefaultText != clone.DefaultText)
            {
                DefaultText = clone.DefaultText;
            }
            DefaultBool = clone.DefaultBool;
            Background = clone.Background;

            while (EnumValues.Count > clone.EnumValues.Count)
            {
                EnumValues.RemoveAt(EnumValues.Count - 1);
            }
            for (int idx = 0; idx < EnumValues.Count; idx++)
            {
                EnumValue s = clone.EnumValues[idx];
                EnumValue t = EnumValues[idx];
                if (t.Name != s.Name)
                {
                    t.Name = s.Name;
                }
                if (t.Value != s.Value)
                {
                    t.Value = s.Value;
                }
                t.IsInvalid = s.IsInvalid;
                t.IsDefault = s.IsDefault;
                t.DontCare = s.DontCare;
            }
        }

        //protected void Clone(ConditionActionBase co)
        //{
        //    var co = new ConditionObject(Text, DataType);
        //    co.DefaultBool = DefaultBool;
        //    co.DefaultText = DefaultText;
        //    co.Background = Background;
        //    co.EnumValues = new ObservableCollection<EnumValue>();
        //    foreach (EnumValue value in EnumValues)
        //    {
        //        co.EnumValues.Add(value.Clone());
        //    }
        //    return co;
        //}


        /// <summary>
        /// only valid during load and save!!
        /// </summary>
        internal int LoadSaveId { get; set; }

        private ValueDataType _dataType;

        public ValueDataType DataType
        {
            get { return _dataType; }
            protected set
            {
                _dataType = value;
                OnPropertyChanged("DataType");
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

        private bool _defaultBool;

        public bool DefaultBool
        {
            get { return _defaultBool; }
            set
            {
                _defaultBool = value;
                OnPropertyChanged("DefaultBool");
            }
        }

        private string _defaultText;

        public string DefaultText
        {
            get { return _defaultText; }
            set
            {
                _defaultText = value;
                OnPropertyChanged("DefaultText");
            }
        }

        private ObservableCollection<EnumValue> _enumValues = new ObservableCollection<EnumValue>();

        public ObservableCollection<EnumValue> EnumValues
        {
            get { return _enumValues; }
            set
            {
                _enumValues = value;
                OnPropertyChanged("EnumValues");
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


        private Visibility _textBoxVisibility = Visibility.Visible;

        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
        }

        private Visibility _checkboxVisibility = Visibility.Collapsed;

        public Visibility CheckboxVisibility
        {
            get { return _checkboxVisibility; }
        }


        private Visibility _comboBoxVisibility = Visibility.Collapsed;

        public Visibility ComboBoxVisibility
        {
            get { return _comboBoxVisibility; }
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

    public class ConditionObject : ConditionActionBase
    {
        public ConditionObject(string name, ValueDataType type) : base(name, type)
        {

        }
        public ConditionObject(string name, ObservableCollection<EnumValue> enums) : base(name, enums)
        {

        }

        public ConditionObject Clone()
        {
            var co = new ConditionObject(Text, DataType);
            co.DefaultBool = DefaultBool;
            co.DefaultText = DefaultText;
            co.Background = Background;
            co.EnumValues = new ObservableCollection<EnumValue>();
            foreach (EnumValue value in EnumValues)
            {
                co.EnumValues.Add(value.Clone());
            }
            return co;
        }


    }

    public class ActionObject : ConditionActionBase
    {
        public ActionObject(string name, ValueDataType type) : base(name, type)
        {
        }

        public ActionObject(string name, ObservableCollection<EnumValue> enums) : base(name, enums)
        {
        }
    }
}

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

        private ValueDataType _dataType;

        public ValueDataType DataType
        {
            get { return _dataType; }
            private set
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

        private Visibility _textBoxVisibility = Visibility.Visible;

        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
            private set
            {
                _textBoxVisibility = value;
                OnPropertyChanged("TextBoxVisibility");
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


        protected static void LoadEnumValues(XmlElement element, ConditionActionBase co)
        {
            if (co.DataType == ValueDataType.Enumeration)
            {
                var xmlEnumValuesColl = element.GetElementsByTagName(XmlNames.EnumValuesName);
                var xmlEnumValues = xmlEnumValuesColl[0] as XmlElement;
                foreach (XmlElement xmlEnumValue in xmlEnumValues.GetElementsByTagName(XmlNames.EnumValueName))
                {
                    co.EnumValues.Add(EnumValue.Load(xmlEnumValue));
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

    public class ConditionObject : ConditionActionBase
    {
        public ConditionObject(string name, ValueDataType type) : base(name, type)
        {

        }
        public ConditionObject(string name, ObservableCollection<EnumValue> enums) : base(name, enums)
        {

        }

        public void Save(XmlElement parent)
        {
            var xmlCondition = parent.AddElement(XmlNames.ConditionName).AddAttribute(XmlNames.TextAttributeName, Text).AddAttribute(XmlNames.TypeAttributeName, DataType.ToString());
            xmlCondition.AddAttribute(XmlNames.DefaultTextAttributeName, DefaultText);
            xmlCondition.AddAttribute(XmlNames.DefaultBoolAttributeName, DefaultBool);
            if (EnumValues != null && EnumValues.Count != 0)
            {
                var xmlEnumValues = xmlCondition.AddElement(XmlNames.EnumValuesName);
                foreach (EnumValue value in EnumValues)
                {
                    value.Save(xmlEnumValues);
                }
            }
        }

        public static ConditionObject Load(XmlElement element)
        {
            string name = element.GetAttributeStringValue(XmlNames.TextAttributeName);
            ValueDataType type = element.GetAttributeEnumValue<ValueDataType>(XmlNames.TypeAttributeName);
            ConditionObject co = new ConditionObject(name, type);
            co.DefaultText = element.GetAttributeStringValue(XmlNames.DefaultTextAttributeName, XmlElementOption.Optional);
            co.DefaultBool = element.GetAttributeBoolValue(XmlNames.DefaultBoolAttributeName);
            LoadEnumValues(element, co);
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

        public void Save(XmlElement parent)
        {
            var xmlCondition = parent.AddElement(XmlNames.ActionName);
            xmlCondition.AddAttribute(XmlNames.TextAttributeName, Text);
            xmlCondition.AddAttribute(XmlNames.TypeAttributeName, DataType.ToString());
            xmlCondition.AddAttribute(XmlNames.DefaultTextAttributeName, DefaultText);
            xmlCondition.AddAttribute(XmlNames.DefaultBoolAttributeName, DefaultBool);
            if (EnumValues != null && EnumValues.Count != 0)
            {
                var xmlEnumValues = xmlCondition.AddElement(XmlNames.EnumValuesName);
                foreach (EnumValue value in EnumValues)
                {
                    value.Save(xmlEnumValues);
                }
            }
        }

        public static ActionObject Load(XmlElement element)
        {
            string name = element.GetAttributeStringValue(XmlNames.TextAttributeName);
            ValueDataType type = element.GetAttributeEnumValue<ValueDataType>(XmlNames.TypeAttributeName);
            ActionObject co = new ActionObject(name, type);
            co.DefaultText = element.GetAttributeStringValue(XmlNames.DefaultTextAttributeName, XmlElementOption.Optional);
            co.DefaultBool = element.GetAttributeBoolValue(XmlNames.DefaultBoolAttributeName);
            LoadEnumValues(element, co);
            return co;
        }
    }
}

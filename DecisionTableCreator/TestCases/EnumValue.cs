using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    public class EnumValue : INotifyPropertyChanged
    {
        public EnumValue(string name, string value, bool isInavlid = false, bool dontCare = false, bool isDefault = false)
        {
            Name = name;
            IsInvalid = isInavlid;
            DontCare = dontCare;
            IsDefault = isDefault;
            Value = value;
        }

        public EnumValue(string name, bool isInavlid = false, bool dontCare = false, bool isDefault = false) : this(name, name, isInavlid, dontCare, isDefault)
        {
        }

        private bool _isDefault;

        public bool IsDefault
        {
            get { return _isDefault; }
            set
            {
                _isDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }


        private bool _isInvalid;

        public bool IsInvalid
        {
            get { return _isInvalid; }
            set
            {
                _isInvalid = value;
                OnPropertyChanged("IsInvalid");
            }
        }

        private bool _dontCare;

        public bool DontCare
        {
            get { return _dontCare; }
            set
            {
                _dontCare = value;
                OnPropertyChanged("DontCare");
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

        public override string ToString()
        {
            return Name;
        }

        public void Save(XmlElement parent)
        {
            var xmlEnumValue = parent.AddElement(XmlNames.EnumValueName);
            xmlEnumValue.AddAttribute(XmlNames.NameAttributeName, Name);
            xmlEnumValue.AddAttribute(XmlNames.ValueAttributeName, Value);
            xmlEnumValue.AddAttribute(XmlNames.IsDefaultAttributeName, IsDefault);
            xmlEnumValue.AddAttribute(XmlNames.IsInvalidAttributeName, IsInvalid);
            xmlEnumValue.AddAttribute(XmlNames.DontCareAttributeName, DontCare);
        }

        public static EnumValue Load(XmlElement element)
        {
            EnumValue ev = new EnumValue(
                element.GetAttributeStringValue(XmlNames.NameAttributeName),
                element.GetAttributeStringValue(XmlNames.ValueAttributeName, XmlElementOption.Optional),
                element.GetAttributeBoolValue(XmlNames.IsInvalidAttributeName),
                element.GetAttributeBoolValue(XmlNames.DontCareAttributeName),
                element.GetAttributeBoolValue(XmlNames.IsDefaultAttributeName)
                );
            return ev;
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

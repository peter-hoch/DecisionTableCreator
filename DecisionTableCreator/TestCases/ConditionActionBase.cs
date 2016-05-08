using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    /// <summary>
    /// base class for condition and action
    /// </summary>
    public class ConditionActionBase : ValueObject
    {
        public enum ConditionActionType
        {
            Text,
            Enum,
            Bool
        }

        public ConditionActionBase(string name, ConditionActionType type) : base(name)
        {
            Type = type;
        }

        public ConditionActionBase(string name, ObservableCollection<EnumValue> enums) : base(name)
        {
            Type = ConditionActionType.Enum;
            EnumValues = enums;
        }

        public ConditionActionType Type { get; private set; }

        public ObservableCollection<EnumValue> EnumValues { get; private set; }

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


    }

    public class ConditionObject : ConditionActionBase
    {
        public ConditionObject(string name, ConditionActionType type) : base(name, type)
        {
            
        }
        public ConditionObject(string name, ObservableCollection<EnumValue> enums) : base(name, enums)
        {

        }
    }

    public class ActionObject : ConditionActionBase
    {
        public ActionObject(string name, ConditionActionType type) : base(name, type)
        {
            
        }
        public ActionObject(string name, ObservableCollection<EnumValue> enums) : base(name, enums)
        {

        }
    }

}

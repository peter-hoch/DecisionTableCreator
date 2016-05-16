using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    public class ActionObject : ConditionActionBase
    {
        private ActionObject() : base()
        {
            EditBoxName = "Edit action object";
        }

        public static ActionObject Create(string text, ValueDataType type)
        {
            ActionObject ao = new ActionObject();
            ao.Text = text;
            ao.DataType = type;
            return ao;
        }

        public static ActionObject Create(string text, ObservableCollection<EnumValue> enums)
        {
            ActionObject ao = new ActionObject();
            ao.Text = text;
            ao.DataType = ValueDataType.Enumeration;
            ao.EnumValues = enums;
            return ao;
        }

        public ActionObject Clone()
        {
            ActionObject clone = new ActionObject();
            Clone(clone);
            return clone;
        }

    }
}

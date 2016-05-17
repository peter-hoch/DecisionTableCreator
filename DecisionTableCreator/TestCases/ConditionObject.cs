using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    public class ConditionObject : ConditionActionBase
    {
        private ConditionObject() : base()
        {
            EditBoxName = "Edit condition object";
        }

        public static ConditionObject Create(string name)
        {
            ConditionObject ao = new ConditionObject();
            ao.Name = name;
            ao.DataType = ValueDataType.Enumeration;
            return ao;
        }

        /// <summary>
        /// condition objects are only supported with enumerations
        /// </summary>
        /// <param name="text"></param>
        /// <param name="enums"></param>
        /// <returns></returns>
        public static ConditionObject Create(string name, ObservableCollection<EnumValue> enums)
        {
            ConditionObject ao = new ConditionObject();
            ao.Name = name;
            ao.DataType = ValueDataType.Enumeration;
            ao.EnumValues = enums;
            return ao;
        }

        public ConditionObject Clone()
        {
            ConditionObject clone = new ConditionObject();
            Clone(clone);
            return clone;
        }

    }
}

using System;
using System.Collections.Generic;
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
        public ConditionActionBase(string name) : base(name)
        {
        }
    }

    public class ConditionObject : ConditionActionBase
    {
        public ConditionObject(string name) : base(name)
        {
            
        }
    }

    public class ActionObject : ConditionActionBase
    {
        public ActionObject(string name) : base(name)
        {
            
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    /// <summary>
    /// this class represents a test setting with reference
    /// </summary>
    public class TestSetting
    {
        public int ConditionIdx { get; private set; }

        public ValueObject Reference { get; private set; }

        public TestSetting(int conditionIdx, ValueObject reference)
        {
            ConditionIdx = conditionIdx;
            Reference = reference;
        }
    }
}

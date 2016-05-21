using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    public partial class TestCasesRoot
    {

        public int CalculatePossibleCombinations()
        {
            if (Conditions.Count == 0)
            {
                return 0;
            }
            int combinations = 1;
            foreach (ConditionObject condition in Conditions)
            {
                combinations *= Utilities.AtLeastValue(1, CalculateEnumValuesWithoutDontCareAndInvalid(condition));
            }
            return combinations;
        }

        /// <summary>
        /// calculate possibble enum values 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static int CalculateEnumValuesWithoutDontCareAndInvalid(ConditionObject condition)
        {
            int count = 0;
            foreach (EnumValue value in condition.EnumValues)
            {
                if (!value.DontCare && !value.IsInvalid)
                {
                    count++;
                }
            }
            return count;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    public class Statistics
    {
        public int PossibleCombinations { get; set; }

        public int CoveredTestCases { get; set; }

        public double Coverage { get; set; }

    }
    public partial class TestCasesRoot
    {
        public Statistics CalculateStatistics()
        {
            Statistics stat = new Statistics();

            stat.PossibleCombinations = CalculatePossibleCombinations();
            TestCase.UpdateUniqueness(TestCases);
            stat.CoveredTestCases = CalculateNumberOfUniqueCoveredTestCases(TestCases);
            stat.Coverage = CalculateCoverage();
            return stat;
        }

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
        /// invalid do not count --> this is an invalid value 
        /// don't care do not count --> this marks a value for don't care for this test case
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

        private double CalculateCoverage()
        {
            double combinations = CalculatePossibleCombinations();
            double result = CalculateNumberOfUniqueCoveredTestCases();
            return result / combinations * 100;
        }


        public int CalculateNumberOfUniqueCoveredTestCases()
        {
            return CalculateNumberOfUniqueCoveredTestCases(TestCases);
        }

        /// <summary>
        /// calculate the num ber of covered test cases
        /// DontCare counts with the count of valid enum values
        /// Invalid do not count
        /// </summary>
        /// <param name="testCases"></param>
        /// <returns></returns>
        public static int CalculateNumberOfUniqueCoveredTestCases(IList<TestCase> testCases)
        {
            for (int outerIdx = 0; outerIdx < testCases.Count; outerIdx++)
            {
                for (int innerIdx = outerIdx+1; innerIdx < testCases.Count; innerIdx++)
                {
                    TestCase outer = testCases[outerIdx];
                    TestCase inner = testCases[innerIdx];

                    if (outer.TestSettingIsEqual(inner))
                    {
                        inner.TestCaseIsUnique = false;
                    }                     
                }
            }

            var uniqueTestCases = testCases.Where(tc => tc.TestCaseIsUnique && tc.ContainsInvalid == false);
            int count = 0;
            foreach (var testCase in uniqueTestCases)
            {
                count += testCase.CalculateNumberOfCoveredTestCases();
            }
            return count;
        }

    }


}

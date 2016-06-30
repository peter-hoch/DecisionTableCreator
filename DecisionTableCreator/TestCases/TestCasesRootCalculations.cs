/*
 * [The "BSD license"]
 * Copyright (c) 2016 Peter Hoch
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
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

        public long CalculatePossibleCombinations()
        {
            if (Conditions.Count == 0)
            {
                return 0;
            }
            long combinations = 1;
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
        public static long CalculateEnumValuesWithoutDontCareAndInvalid(ConditionObject condition)
        {
            long count = 0;
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


        public long CalculateNumberOfUniqueCoveredTestCases()
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
        public static long CalculateNumberOfUniqueCoveredTestCases(IList<TestCase> testCases)
        {
            for (int outerIdx = 0; outerIdx < testCases.Count; outerIdx++)
            {
                for (int innerIdx = outerIdx + 1; innerIdx < testCases.Count; innerIdx++)
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
            long count = 0;
            foreach (var testCase in uniqueTestCases)
            {
                count += testCase.CalculateNumberOfCoveredTestCases();
            }
            return count;
        }


        private int _index = 0;
        public List<ITestCase> ExpandTestCases()
        {
            _index = 0;
            List<ITestCase> expandedTestCases = new List<ITestCase>();
            if (Conditions.Count > 0)
            {
                foreach (TestCase testCase in TestCases)
                {
                    List<int> values = new List<int>();
                    ExpandCondition(expandedTestCases, testCase, values, 0);
                }
            }
            return expandedTestCases;
        }

        private void ExpandCondition(List<ITestCase> expandedTestCases, TestCase testCase, List<int> values, int idx)
        {
            if (testCase.Conditions.Count - 1 > idx)
            {
                ValueObject condition = testCase.Conditions[idx];
                if (condition.SelectedValue.DontCare)
                {
                    foreach (int enumIndex in condition.ConditionOrActionParent.ValidEnumValueIndexes)
                    {
                        values.Add(enumIndex);
                        ExpandCondition(expandedTestCases, testCase, values, idx + 1);
                        values.RemoveAt(values.Count - 1);
                    }
                }
                else
                {
                    values.Add(condition.SelectedItemIndex);
                    ExpandCondition(expandedTestCases, testCase, values, idx + 1);
                    values.RemoveAt(values.Count - 1);
                }
            }
            else
            {
                ValueObject condition = testCase.Conditions[idx];
                if (condition.SelectedValue.DontCare)
                {
                    foreach (int enumIndex in condition.ConditionOrActionParent.ValidEnumValueIndexes)
                    {
                        values.Add(enumIndex);
                        var etc = new ExpandedTestCase("exp" + _index++ + "  " + testCase.Name);
                        for (int localIndex = 0; localIndex < testCase.Conditions.Count; localIndex++)
                        {
                            etc.Conditions.Add(new ValueObject(testCase.Conditions[localIndex].EnumValues, values[localIndex]));
                        }
                        foreach (ValueObject action in testCase.Actions)
                        {
                            etc.Actions.Add(new ValueObject(action.EnumValues, action.SelectedItemIndex));
                        }
                        values.RemoveAt(values.Count - 1);
                        expandedTestCases.Add(etc);
                    }
                }
                else
                {
                    values.Add(condition.SelectedItemIndex);
                    var etc = new ExpandedTestCase("exp" + _index++ + "  " + testCase.Name);
                    for (int localIndex = 0; localIndex < testCase.Conditions.Count; localIndex++)
                    {
                        etc.Conditions.Add(new ValueObject(testCase.Conditions[localIndex].EnumValues, values[localIndex]));
                    }
                    foreach (ValueObject action in testCase.Actions)
                    {
                        etc.Actions.Add(new ValueObject(action.EnumValues, action.SelectedItemIndex));
                    }
                    values.RemoveAt(values.Count - 1);
                    expandedTestCases.Add(etc);
                }

            }
        }
    }


}

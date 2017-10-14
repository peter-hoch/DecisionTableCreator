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
        public const int MaxPossibleCombinations = 1001;

        public bool CreateMissingTestcasesPossible { get; private set; }

        public Statistics CalculateStatistics()
        {
            Statistics stat = new Statistics();

            stat.PossibleCombinations = CalculatePossibleCombinations();
            if (stat.PossibleCombinations < MaxPossibleCombinations)
            {
                stat.CoveredTestCases = CalculateNumberOfUniqueCoveredTestCases();
                stat.Coverage = (double)stat.CoveredTestCases / stat.PossibleCombinations * 100.0;
                CreateMissingTestcasesPossible = true;
            }
            else
            {
                stat.CoveredTestCases = -1;
                stat.Coverage = -1;
                CreateMissingTestcasesPossible = false;
            }
            return stat;
        }

        public bool CalculateMissingTestCases()
        {
            var possibleCombinations = CalculatePossibleCombinations();
            if (possibleCombinations < MaxPossibleCombinations)
            {
                ExpandTestCases expand = new ExpandTestCases();
                List<TestCase> existingTestCases = expand.Expand(this);

                TestCaseCreator creator = new TestCaseCreator();
                creator.CreateMissingTestCases(this);

                List<TestCase> missingTestCases = creator.FilterForMissingTestCases(existingTestCases);
                AddMissingTestCases(missingTestCases);

                return true;
            }

            return false;
        }

        void AddMissingTestCases(List<TestCase> missingTestCases)
        {
            ConditionTable.Columns.Clear();
            ActionTable.Columns.Clear();

            foreach (TestCase tc in missingTestCases)
            {
                AppendTestCase(tc);
            }

            CreateBasicColumnDescriptions();
            AddColumnDescriptionsForTestCases();

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            FireActionsChanged();
            ProcessConditionsChanged();

        }


        private void AppendTestCase(TestCase tc)
        {
            int index = CalculateNextTestCaseId();
            string name = String.Format("TC{0}", index);
            tc.Name = name;
            TestCases.Add(tc);
            tc.DisplayIndex = TestCases.Count; // displayindex includes first column action or condition

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


        public long CalculateNumberOfUniqueCoveredTestCases()
        {
            DecisionTableCreator.TestCases.ExpandTestCases expand = new ExpandTestCases();
            return expand.Expand(this).Count;
        }

        /// <summary>
        /// calculate the num ber of covered test cases
        /// DontCare counts with the count of valid enum values
        /// Invalid do not count
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="testCases"></param>
        /// <returns></returns>
        public static long CalculateNumberOfUniqueCoveredTestCases(ObservableCollection<ConditionObject> conditions, IList<TestCase> testCases)
        {
            DecisionTableCreator.TestCases.ExpandTestCases expand = new ExpandTestCases();
            return expand.Expand(conditions, testCases).Count;
        }


    }


}

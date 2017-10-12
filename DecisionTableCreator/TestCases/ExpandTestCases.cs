using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DecisionTableCreator.TestCases
{
    public class ExpandTestCases
    {
        private int _index = 0;

        public List<TestCase> Expand(TestCasesRoot root)
        {
            return Expand(root.Conditions, root.TestCases);
        }
        public List<TestCase> Expand(IList<ConditionObject> conditions, IList<TestCase> testCases)
        {
            _index = 0;
            List<TestCase> expandedTestCases = new List<TestCase>();
            if (conditions.Count > 0)
            {
                foreach (TestCase testCase in testCases)
                {
                    List<int> values = new List<int>();
                    ExpandCondition(expandedTestCases, testCase, values, 0);
                }
            }
            TestCase.UpdateUniqueness(expandedTestCases);
            var uniqueTestCases = expandedTestCases.Where(tc => tc.TestCaseIsUnique);
            return new List<TestCase>(uniqueTestCases);
        }

        private void ExpandCondition(List<TestCase> expandedTestCases, TestCase testCase, List<int> values, int idx)
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
                        var etc = new TestCase("exp" + _index + "  " + testCase.Name);
                        for (int localIndex = 0; localIndex < testCase.Conditions.Count; localIndex++)
                        {
                            etc.Conditions.Add(new ValueObject(testCase.Conditions[localIndex].EnumValues, values[localIndex]));
                        }
                        foreach (ValueObject action in testCase.Actions)
                        {
                            etc.Actions.Add(new ValueObject(action.EnumValues, action.SelectedItemIndex));
                        }
                        values.RemoveAt(values.Count - 1);
                        var existing = expandedTestCases.FirstOrDefault(tc => tc.TestSettingIsEqual(etc));
                        if (existing != null)
                        {
                            existing.Name += testCase.Name;
                        }
                        else
                        {
                            expandedTestCases.Add(etc);
                            _index++;
                        }
                    }
                }
                else
                {
                    values.Add(condition.SelectedItemIndex);
                    var etc = new TestCase("exp" + _index + "  " + testCase.Name);
                    for (int localIndex = 0; localIndex < testCase.Conditions.Count; localIndex++)
                    {
                        etc.Conditions.Add(new ValueObject(testCase.Conditions[localIndex].EnumValues, values[localIndex]));
                    }
                    foreach (ValueObject action in testCase.Actions)
                    {
                        etc.Actions.Add(new ValueObject(action.EnumValues, action.SelectedItemIndex));
                    }
                    values.RemoveAt(values.Count - 1);
                    var existing = expandedTestCases.FirstOrDefault(tc => tc.TestSettingIsEqual(etc));
                    if (existing != null)
                    {
                        existing.Name += testCase.Name;
                    }
                    else
                    {
                        expandedTestCases.Add(etc);
                        _index++;
                    }
                }

            }
        }

    }
}

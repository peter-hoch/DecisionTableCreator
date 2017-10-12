using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    public class TestCaseCreator
    {
        public TestCasesRoot TestCasesRoot { get; private set; }

        public List<TestCase> CreatedTestCases { get; private set; }


        public void CreateMissingTestCases(TestCasesRoot root)
        {
            TestCasesRoot = root;
            CreatedTestCases = new List<TestCase>();

            CreateAllTestCases(TestCasesRoot.Conditions);
        }

        private void CreateAllTestCases(IList<ConditionObject> conditions)
        {
            int condIndex = 0;
            List<KeyValuePair<int, ConditionObject>> pairs = new List<KeyValuePair<int, ConditionObject>>();
            if (conditions.Count >= 1)
            {
                CreateOneTestCase(conditions, condIndex, pairs);
            }
        }

        private void CreateOneTestCase(IList<ConditionObject> conditions, int condIndex, List<KeyValuePair<int, ConditionObject>> pairs)
        {
            if (conditions.Count > condIndex)
            {
                ConditionObject condition = conditions[condIndex];
                if (condition.ValidEnumValueIndexes.Count == 0)
                {
                    if (condition.EnumValues.Count == 0)
                    {
                        KeyValuePair<int, ConditionObject> pair = new KeyValuePair<int, ConditionObject>(-1, condition);
                        pairs.Add(pair);
                        CreateOneTestCase(conditions, condIndex + 1, pairs);
                        pairs.Remove(pair);
                    }
                    else
                    {
                        KeyValuePair<int, ConditionObject> pair = new KeyValuePair<int, ConditionObject>(0, condition);
                        pairs.Add(pair);
                        CreateOneTestCase(conditions, condIndex + 1, pairs);
                        pairs.Remove(pair);
                    }
                }
                else
                {
                    foreach (int enumIndex in condition.ValidEnumValueIndexes)
                    {
                        KeyValuePair<int, ConditionObject> pair = new KeyValuePair<int, ConditionObject>(enumIndex, condition);
                        pairs.Add(pair);
                        CreateOneTestCase(conditions, condIndex + 1, pairs);
                        pairs.Remove(pair);
                    }
                }
            }
            else
            {
                CreateNewTestCase(pairs);
            }
        }

        private void CreateNewTestCase(List<KeyValuePair<int, ConditionObject>> pairs)
        {
            TestCase newTestCase = new TestCase("new TestCase");
            foreach (KeyValuePair<int, ConditionObject> pair in pairs)
            {
                ValueObject value = null;
                if (pair.Key == -1)
                {
                    value = new ValueObject(pair.Value.EnumValues, pair.Key);
                }
                else
                {
                    value = new ValueObject(pair.Value.EnumValues, pair.Key);
                }
                value.ConditionOrActionParent = pair.Value;
                newTestCase.Conditions.Add(value);
            }
            CreatedTestCases.Add(newTestCase);
        }
    }
}

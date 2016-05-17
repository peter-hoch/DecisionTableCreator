using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.TestCases;
using NUnit.Framework;

namespace UnitTests2
{

    public class SelectedValuesByTestCases
    {
        public List<SelectedValues> TestCaseConditionValues { get; set; }

        public List<SelectedValues> TestCaseActionValues { get; set; }

        internal void CollectValues(TestCasesRoot testCasesRoot)
        {
            TestCaseConditionValues = new List<SelectedValues>();
            TestCaseActionValues = new List<SelectedValues>();
            foreach (TestCase testCase in testCasesRoot.TestCases)
            {
                var selValues = new SelectedValues();
                selValues.Collect(testCase.Conditions);
                TestCaseConditionValues.Add(selValues);

                selValues = new SelectedValues();
                selValues.Collect(testCase.Actions);
                TestCaseActionValues.Add(selValues);
            }
        }

        internal void Check(TestCasesRoot testCasesRoot)
        {
            Assert.That(testCasesRoot.TestCases.Count == TestCaseConditionValues.Count);
            Assert.That(testCasesRoot.TestCases.Count == TestCaseActionValues.Count);

            for (int idx = 0; idx < testCasesRoot.TestCases.Count; idx++)
            {
                TestCase tc = testCasesRoot.TestCases[idx];
                TestCaseConditionValues[idx].Check(tc.Conditions);
                TestCaseActionValues[idx].Check(tc.Actions);
            }
        }

        internal void AppendTestCase(TestCasesRoot testCasesRoot)
        {
            var selValues = new SelectedValues();
            selValues.Values = SelectedValue.CreateNew(testCasesRoot.Conditions.Count);
            TestCaseConditionValues.Add(selValues);
            selValues = new SelectedValues();
            selValues.Values = SelectedValue.CreateNew(testCasesRoot.Actions.Count);
            TestCaseActionValues.Add(selValues);
        }

        internal void InsertTestCase(AddRowsTest.InsertPosition insertPosition, TestCasesRoot testCasesRoot)
        {
            var selValues = new SelectedValues();
            selValues.Values = SelectedValue.CreateNew(testCasesRoot.Conditions.Count);
            switch (insertPosition)
            {
                case AddRowsTest.InsertPosition.First:
                    TestCaseConditionValues.Insert(0, selValues);
                    break;
                case AddRowsTest.InsertPosition.Second:
                    TestCaseConditionValues.Insert(1, selValues);
                    break;
                case AddRowsTest.InsertPosition.Last:
                    TestCaseConditionValues.Insert(TestCaseConditionValues.Count - 1, selValues);
                    break;
                case AddRowsTest.InsertPosition.AfterLast:
                    TestCaseConditionValues.Add(selValues);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(insertPosition), insertPosition, null);
            }
            selValues = new SelectedValues();
            selValues.Values = SelectedValue.CreateNew(testCasesRoot.Actions.Count);
            switch (insertPosition)
            {
                case AddRowsTest.InsertPosition.First:
                    TestCaseActionValues.Insert(0, selValues);
                    break;
                case AddRowsTest.InsertPosition.Second:
                    TestCaseActionValues.Insert(1, selValues);
                    break;
                case AddRowsTest.InsertPosition.Last:
                    TestCaseActionValues.Insert(TestCaseActionValues.Count - 1, selValues);
                    break;
                case AddRowsTest.InsertPosition.AfterLast:
                    TestCaseActionValues.Add(selValues);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(insertPosition), insertPosition, null);
            }
        }

        internal void DeleteTestCase(AddRowsTest.DeletePosition deletePosition, TestCasesRoot testCasesRoot)
        {
            switch (deletePosition)
            {
                case AddRowsTest.DeletePosition.First:
                    TestCaseConditionValues.RemoveAt(0);
                    TestCaseActionValues.RemoveAt(0);
                    break;
                case AddRowsTest.DeletePosition.Second:
                    TestCaseConditionValues.RemoveAt(1);
                    TestCaseActionValues.RemoveAt(1);
                    break;
                case AddRowsTest.DeletePosition.Last:
                    TestCaseConditionValues.RemoveAt(TestCaseConditionValues.Count - 1);
                    TestCaseActionValues.RemoveAt(TestCaseActionValues.Count - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deletePosition), deletePosition, null);
            }
        }
    }

    public class SelectedValuesByConditionsAndActions
    {
        public List<SelectedValues> ActionValues { get; set; }

        public List<SelectedValues> ConditionValues { get; set; }

        internal void CollectValues(TestCasesRoot testCasesRoot)
        {
            ActionValues = new List<SelectedValues>();
            ConditionValues = new List<SelectedValues>();
            foreach (ConditionObject condition in testCasesRoot.Conditions)
            {
                var selValues = new SelectedValues();
                selValues.Collect(condition);
                ConditionValues.Add(selValues);
            }
            foreach (ActionObject action in testCasesRoot.Actions)
            {
                var selValues = new SelectedValues();
                selValues.Collect(action);
                ActionValues.Add(selValues);
            }
        }

        internal void Check(TestCasesRoot testCasesRoot)
        {
            Assert.That(testCasesRoot.Conditions.Count == ConditionValues.Count);

            for (int idx = 0; idx < testCasesRoot.Conditions.Count; idx++)
            {
                ConditionObject co = testCasesRoot.Conditions[idx];
                ConditionValues[idx].Check(co);
            }

            Assert.That(testCasesRoot.Actions.Count == ActionValues.Count);

            for (int idx = 0; idx < testCasesRoot.Actions.Count; idx++)
            {
                ActionObject co = testCasesRoot.Actions[idx];
                ActionValues[idx].Check(co);
            }
        }

        internal void AppendCondition(int count)
        {
            var selValues = new SelectedValues();
            selValues.Values = SelectedValue.CreateNew(count);
            ConditionValues.Add(selValues);
        }

        internal void AppendAction(int count)
        {
            var selValues = new SelectedValues();
            selValues.Values = SelectedValue.CreateNew(count);
            ActionValues.Add(selValues);
        }


        internal void InsertCondition(AddRowsTest.InsertPosition insertPosition, int count)
        {
            var selValues = new SelectedValues();
            selValues.Values = SelectedValue.CreateNew(count);
            switch (insertPosition)
            {
                case AddRowsTest.InsertPosition.First:
                    ConditionValues.Insert(0, selValues);
                    break;
                case AddRowsTest.InsertPosition.Second:
                    ConditionValues.Insert(1, selValues);
                    break;
                case AddRowsTest.InsertPosition.Last:
                    ConditionValues.Insert(ConditionValues.Count-1, selValues);
                    break;
                case AddRowsTest.InsertPosition.AfterLast:
                    ConditionValues.Add(selValues);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(insertPosition), insertPosition, null);
            }
        }

        internal void InsertAction(AddRowsTest.InsertPosition insertPosition, int count)
        {
            var selValues = new SelectedValues();
            selValues.Values = SelectedValue.CreateNew(count);
            switch (insertPosition)
            {
                case AddRowsTest.InsertPosition.First:
                    ActionValues.Insert(0, selValues);
                    break;
                case AddRowsTest.InsertPosition.Second:
                    ActionValues.Insert(1, selValues);
                    break;
                case AddRowsTest.InsertPosition.Last:
                    ActionValues.Insert(ActionValues.Count - 1, selValues);
                    break;
                case AddRowsTest.InsertPosition.AfterLast:
                    ActionValues.Add(selValues);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(insertPosition), insertPosition, null);
            }
        }

        internal void DeleteCondition(AddRowsTest.DeletePosition deletePosition)
        {
            switch (deletePosition)
            {
                case AddRowsTest.DeletePosition.First:
                    ConditionValues.RemoveAt(0);
                    break;
                case AddRowsTest.DeletePosition.Second:
                    ConditionValues.RemoveAt(1);
                    break;
                case AddRowsTest.DeletePosition.Last:
                    ConditionValues.RemoveAt(ConditionValues.Count-1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deletePosition), deletePosition, null);
            }
        }

        internal void DeleteAction(AddRowsTest.DeletePosition deletePosition)
        {
            switch (deletePosition)
            {
                case AddRowsTest.DeletePosition.First:
                    ActionValues.RemoveAt(0);
                    break;
                case AddRowsTest.DeletePosition.Second:
                    ActionValues.RemoveAt(1);
                    break;
                case AddRowsTest.DeletePosition.Last:
                    ActionValues.RemoveAt(ActionValues.Count - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deletePosition), deletePosition, null);
            }
        }
    }


    public class SelectedValues
    {
        public List<SelectedValue> Values { get; set; }

        internal void Check(ConditionObject co)
        {
            Assert.That(co.TestValues.Count == Values.Count);

            for (int idx = 0; idx < Values.Count; idx++)
            {
                Values[idx].Check(co.TestValues[idx]);
            }
        }

        internal void Check(ObservableCollection<ValueObject> conditions)
        {
            Assert.That(conditions.Count == Values.Count);

            for (int idx = 0; idx < Values.Count; idx++)
            {
                Values[idx].Check(conditions[idx]);
            }
        }

        internal void Check(ActionObject ao)
        {
            Assert.That(ao.TestValues.Count == Values.Count);

            for (int idx = 0; idx < Values.Count; idx++)
            {
                Values[idx].Check(ao.TestValues[idx]);
            }
        }

        internal void Collect(ObservableCollection<ValueObject> conditions)
        {
            Values = new List<SelectedValue>();
            foreach (ValueObject value in conditions)
            {
                Assert.That(value.DataType == ValueDataType.Enumeration);
                Values.Add(new SelectedValue(value));
            }
        }

        internal void Collect(ConditionObject condition)
        {
            Assert.That(condition.DataType == ValueDataType.Enumeration);

            Values = new List<SelectedValue>();
            foreach (ValueObject value in condition.TestValues)
            {
                Values.Add(new SelectedValue(value));
            }
        }

        internal void Collect(ActionObject action)
        {
            Assert.That(action.DataType == ValueDataType.Enumeration);

            Values = new List<SelectedValue>();
            foreach (ValueObject value in action.TestValues)
            {
                Values.Add(new SelectedValue(value));
            }
        }
    }

    public class SelectedValue
    {
        public int SelectedItemIndex { get; set; }

        public bool Inserted { get; set; }

        public SelectedValue()
        {
            Inserted = true;
        }

        public SelectedValue(ValueObject vo)
        {
            Inserted = false;
            switch (vo.DataType)
            {
                case ValueDataType.Enumeration:
                    SelectedItemIndex = vo.SelectedItemIndex;
                    break;

                case ValueDataType.Text:
                case ValueDataType.Bool:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void Check(ValueObject valueObject)
        {
            if (Inserted)
            {
                switch (valueObject.DataType)
                {
                    case ValueDataType.Enumeration:
                        Assert.That(valueObject.SelectedItemIndex == CalculateSelectedItemIndex(valueObject.EnumValues));
                        break;

                    case ValueDataType.Text:
                    case ValueDataType.Bool:
                    default:
                        Assert.That(false);
                        break;
                }
            }
            else
            {
                switch (valueObject.DataType)
                {
                    case ValueDataType.Enumeration:
                        Assert.That(SelectedItemIndex == valueObject.SelectedItemIndex);
                        break;

                    case ValueDataType.Text:
                    case ValueDataType.Bool:
                    default:
                        Assert.That(false);
                        break;
                }
            }
        }

        int CalculateSelectedItemIndex(ObservableCollection<EnumValue> enumValues)
        {
            for (int idx = 0; idx < enumValues.Count; idx++)
            {
                if (enumValues[idx].IsDefault)
                {
                    return idx;
                }
            }
            return 0;
        }

        public static List<SelectedValue> CreateNew(int count)
        {
            var list = new List<SelectedValue>();
            for (int idx = 0; idx < count; idx++)
            {
                list.Add(new SelectedValue());
            }
            return list;
        }
    }
}

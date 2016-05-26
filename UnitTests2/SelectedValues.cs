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
using DecisionTableCreator.TestCases;
using NUnit.Framework;

namespace UnitTests2
{
    public class TestCaseContainer
    {
        public SelectedValues TestCaseConditionValues { get; set; }

        public SelectedValues TestCaseActionValues { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return String.Format("{0} cond {1}  act {2}", Name, TestCaseConditionValues, TestCaseActionValues);
        }
    }

    public class SelectedValuesByTestCases
    {
        public List<TestCaseContainer> TestCaseContainers { get; set; }

        internal void CollectValues(TestCasesRoot testCasesRoot)
        {
            TestCaseContainers = new List<TestCaseContainer>();
            foreach (TestCase testCase in testCasesRoot.TestCases)
            {
                TestCaseContainer container = new TestCaseContainer();
                container.Name = testCase.Name;

                var selValues = new SelectedValues();
                selValues.Collect(testCase.Conditions);
                container.TestCaseConditionValues = selValues;

                selValues = new SelectedValues();
                selValues.Collect(testCase.Actions);
                container.TestCaseActionValues = selValues;

                TestCaseContainers.Add(container);
            }
        }

        TestCaseContainer GetContainerByName(string name)
        {
            return TestCaseContainers.FirstOrDefault(c => c.Name.Equals(name));
        }
        internal void Check(TestCasesRoot testCasesRoot)
        {
            Assert.That(testCasesRoot.TestCases.Count == TestCaseContainers.Count);
            Assert.That(testCasesRoot.TestCases.Count == TestCaseContainers.Count);

            var orderedTestCases  = testCasesRoot.TestCases.OrderBy(tc=>tc.Name).ToArray();

            for (int idx = 0; idx < testCasesRoot.TestCases.Count; idx++)
            {
                TestCase tc = orderedTestCases[idx];
                GetContainerByName(tc.Name).TestCaseConditionValues.Check(tc.Conditions);
                GetContainerByName(tc.Name).TestCaseActionValues.Check(tc.Actions);
            }
        }

        internal void AppendTestCase(TestCase newTestCase, TestCasesRoot testCasesRoot)
        {
            TestCaseContainer cont = new TestCaseContainer();
            cont.Name = newTestCase.Name;
            var selValues = new SelectedValues();
            selValues.Values = SelectedValue.CreateNew(testCasesRoot.Conditions.Count);
            cont.TestCaseConditionValues = selValues;
            selValues = new SelectedValues();
            selValues.Values = SelectedValue.CreateNew(testCasesRoot.Actions.Count);
            cont.TestCaseActionValues = selValues;
            TestCaseContainers.Add(cont);
        }


        internal void DeleteTestCase(TestCase deletedTestCase, TestCasesRoot testCasesRoot)
        {
            TestCaseContainers.Remove(GetContainerByName(deletedTestCase.Name));
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (SelectedValue value in Values)
            {
                sb.AppendFormat("{0} ", value);
            }
            return sb.ToString();
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

        public override string ToString()
        {
            return String.Format("idx{0} {1}", SelectedItemIndex, Inserted ? "ins " : "");
        }
    }
}

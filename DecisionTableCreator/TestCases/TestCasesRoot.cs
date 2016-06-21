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
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Antlr4.StringTemplate;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    public delegate void ViewChangedDelegate();

    public delegate void StatisticsChangedDelegate();

    public partial class TestCasesRoot : INotifyDirtyChanged, ITestCasesRoot
    {
        public TestCasesRoot()
        {
            Init();
            CreateInfosForDatagrid();
            DirtyObserver = new DirtyObserver(this);
        }

        void Init()
        {
            ConditionTable = new DataTable();
            ActionTable = new DataTable();
            TestCases = new ObservableCollection<TestCase>();
            Conditions = new ObservableCollection<ConditionObject>();
            Actions = new ObservableCollection<ActionObject>();
        }

        public static readonly string ConditionsColumnHeaderName = "Conditions";
        public static readonly string ActionsColumnHeaderName = "Actions";

        private void CreateBasicColumnDescriptions()
        {
            ConditionTable = new DataTable();
            ConditionTable.Columns.Add(ConditionsColumnHeaderName, typeof(ConditionObject));

            ActionTable = new DataTable();
            ActionTable.Columns.Add(ActionsColumnHeaderName, typeof(ActionObject));
        }

        void AddColumnDescriptionsForTestCases()
        {
            foreach (TestCase testCase in TestCases)
            {
                ConditionTable.Columns.Add(testCase.Name, typeof(ValueObject));
                ActionTable.Columns.Add(testCase.Name, typeof(ValueObject));
            }
        }


        void PopulateRows<TType>(DataTable dataTable, IList<TType> list, IList<TestCase> testCases, TestCase.CollectionType colType)
        {
            switch (colType)
            {
                case TestCase.CollectionType.Conditions:
                    ConditionsBeginChange?.Invoke();
                    break;
                case TestCase.CollectionType.Actions:
                    ActionsBeginChange?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(colType), colType, null);
            }
            dataTable.Rows.Clear();
            int rowIndex = 0;
            foreach (TType conditionActionBase in list)
            {
                List<object> values = new List<object>();
                values.Add(conditionActionBase);
                int columnIndex = 1;
                foreach (TestCase testCase in testCases)
                {
                    var value = testCase.GetValueObject(colType, rowIndex);
                    values.Add(value);
                    columnIndex++;
                }
                rowIndex++;
                dataTable.Rows.Add(values.ToArray());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">-1 --> append</param>
        /// <returns></returns>
        public TestCase InsertTestCase(int index = -1)
        {
            var tc = AddTestCase(index);

            ConditionTable.Columns.Clear();
            ActionTable.Columns.Clear();
            CreateBasicColumnDescriptions();
            AddColumnDescriptionsForTestCases();

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            FireActionsChanged();
            ProcessConditionsChanged();
            return tc;
        }

        public TestCase DeleteTestCaseAt(int index)
        {
            TestCases = new ObservableCollection<TestCase>(TestCases.OrderBy(testcase => testcase.DisplayIndex));
            TestCase deleted = TestCases[index];
            if (!TestCases.Remove(deleted))
            {
                throw new Exception("test case " + deleted.Name + " not found");
            }
            UpdateDisplayIndex(TestCases);

            ConditionTable.Columns.Clear();
            ActionTable.Columns.Clear();
            CreateBasicColumnDescriptions();
            AddColumnDescriptionsForTestCases();

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            FireActionsChanged();
            ProcessConditionsChanged();
            return deleted;
        }


        int CalculateNextTestCaseId()
        {
            int nextId = 0;

            foreach (TestCase testCase in TestCases)
            {
                Regex regex = new Regex(@"^TC(?<value>\d+)$");
                var match = regex.Match(testCase.Name);
                if (match.Success)
                {
                    int id = int.Parse(match.Groups["value"].Value);
                    if (nextId < id)
                    {
                        nextId = id;
                    }
                }
                else
                {
                    throw new Exception("unexpected test case name " + testCase.Name);
                }
            }

            return ++nextId;
        }

        private TestCase AddTestCase(int indexWhereToInsert = -1)
        {
            int index = CalculateNextTestCaseId();
            var tc = new TestCase(String.Format("TC{0}", index));
            if (indexWhereToInsert == -1)
            {
                TestCases.Add(tc);
                tc.DisplayIndex = TestCases.Count; // displayindex includes first column action or condition
            }
            else
            {
                // sort test cases by display index
                TestCases = new ObservableCollection<TestCase>(TestCases.OrderBy(testcase => testcase.DisplayIndex));
                TestCases.Insert(indexWhereToInsert, tc);
                UpdateDisplayIndex(TestCases);
            }
            AddValueObjects(tc, Conditions, TestCase.CollectionType.Conditions);
            AddValueObjects(tc, Actions, TestCase.CollectionType.Actions);

            return tc;
        }

        private void UpdateDisplayIndex(ObservableCollection<TestCase> testCases)
        {
            for (int idx = 0; idx < testCases.Count; idx++)
            {
                testCases[idx].DisplayIndex = idx + 1;
            }
        }

        public void AppendAction(ActionObject actionObject)
        {
            Actions.Add(actionObject);
            AddToTestCases(actionObject);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            FireActionsChanged();
        }

        public void InsertAction(int index, ActionObject newAction)
        {
            Actions.Insert(index, newAction);
            AddToTestCases(newAction, index);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            FireActionsChanged();
        }

        public void DeleteActionAt(int index)
        {
            Actions.RemoveAt(index);
            RemoveActionFromTestCases(index);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            FireActionsChanged();
        }


        public void AppendCondition(ConditionObject conditionObject)
        {
            Conditions.Add(conditionObject);
            AddToTestCases(conditionObject);
            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            ProcessConditionsChanged();
        }

        public void InsertCondition(int index, ConditionObject newCondition)
        {
            Conditions.Insert(index, newCondition);
            AddToTestCases(newCondition, index);
            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            ProcessConditionsChanged();
        }

        public void DeleteConditionAt(int index)
        {
            Conditions.RemoveAt(index);
            RemoveConditionFromTestCases(index);
            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            ProcessConditionsChanged();
        }

        public void MoveConditionDown(int index)
        {
            if (Conditions.Count > 1 && index < Conditions.Count - 1)
            {
                ConditionObject co = Conditions[index];
                Conditions.RemoveAt(index);
                Conditions.Insert(index + 1, co);
                foreach (TestCase testCase in TestCases)
                {
                    var vo = testCase.Conditions[index];
                    testCase.Conditions.RemoveAt(index);
                    testCase.Conditions.Insert(index + 1, vo);
                }
                PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
                ProcessConditionsChanged();
            }
        }

        public void MoveConditionUp(int index)
        {
            if (Conditions.Count > 1 && index >= 1)
            {
                ConditionObject co = Conditions[index];
                Conditions.RemoveAt(index);
                Conditions.Insert(index - 1, co);
                foreach (TestCase testCase in TestCases)
                {
                    var vo = testCase.Conditions[index];
                    testCase.Conditions.RemoveAt(index);
                    testCase.Conditions.Insert(index - 1, vo);
                }
                PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
                ProcessConditionsChanged();
            }
        }

        public void MoveActionUp(int index)
        {
            if (Actions.Count > 1 && index >= 1)
            {
                ActionObject co = Actions[index];
                Actions.RemoveAt(index);
                Actions.Insert(index - 1, co);
                foreach (TestCase testCase in TestCases)
                {
                    var vo = testCase.Actions[index];
                    testCase.Actions.RemoveAt(index);
                    testCase.Actions.Insert(index - 1, vo);
                }
                PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
                FireActionsChanged();
            }
        }

        public void MoveActionDown(int index)
        {
            if (Actions.Count > 1 && index < Actions.Count - 1)
            {
                ActionObject co = Actions[index];
                Actions.RemoveAt(index);
                Actions.Insert(index + 1, co);
                foreach (TestCase testCase in TestCases)
                {
                    var vo = testCase.Actions[index];
                    testCase.Actions.RemoveAt(index);
                    testCase.Actions.Insert(index + 1, vo);
                }
                PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
                FireActionsChanged();
            }
        }


        public void ChangeCondition(int index, ConditionObject conditionClone)
        {
            List<int> savedSelectedItemIndexes = SaveSelectedConditionItemIndex(index);
            Conditions[index].Merge(conditionClone);
            RestoreSelectedConditionItemIndex(index, savedSelectedItemIndexes);
            RecalculateStatistics();
        }

        internal void ChangeAction(int index, ActionObject actionClone)
        {
            List<int> savedSelectedItemIndexes = SaveSelectedActionItemIndex(index);
            Actions[index].Merge(actionClone);
            RestoreSelectedActionItemIndex(index, savedSelectedItemIndexes);
        }


        public List<int> SaveSelectedConditionItemIndex(int conditionIndex)
        {
            List<int> selectedItemIndexes = new List<int>();

            foreach (TestCase testCase in TestCases)
            {
                selectedItemIndexes.Add(testCase.Conditions[conditionIndex].SelectedItemIndex);
            }

            return selectedItemIndexes;
        }

        public void RestoreSelectedConditionItemIndex(int conditionIndex, List<int> selectedItemIndexes)
        {
            if (TestCases.Count > 0)
            {
                int enumValueCount = TestCases[0].Conditions[conditionIndex].EnumValues.Count;
                for (int idx = 0; idx < TestCases.Count; idx++)
                {
                    if (enumValueCount <= selectedItemIndexes[idx])
                    {
                        TestCases[idx].Conditions[conditionIndex].CalculateAndSetDefaultIndex();
                    }
                    else
                    {
                        TestCases[idx].Conditions[conditionIndex].SelectedItemIndex = selectedItemIndexes[idx];
                    }
                }
            }
        }

        public List<int> SaveSelectedActionItemIndex(int actionIndex)
        {
            List<int> selectedItemIndexes = new List<int>();

            foreach (TestCase testCase in TestCases)
            {
                selectedItemIndexes.Add(testCase.Actions[actionIndex].SelectedItemIndex);
            }

            return selectedItemIndexes;
        }

        public void RestoreSelectedActionItemIndex(int actionIndex, List<int> selectedItemIndexes)
        {
            if (TestCases.Count > 0)
            {
                int enumValueCount = TestCases[0].Actions[actionIndex].EnumValues.Count;
                for (int idx = 0; idx < TestCases.Count; idx++)
                {
                    if (enumValueCount <= selectedItemIndexes[idx])
                    {
                        TestCases[idx].Actions[actionIndex].CalculateAndSetDefaultIndex();
                    }
                    else
                    {
                        TestCases[idx].Actions[actionIndex].SelectedItemIndex = selectedItemIndexes[idx];
                    }
                }
            }
        }


        private void AddToTestCases(ConditionObject conditionObject, int whereToInsert = -1)
        {
            foreach (TestCase testCase in TestCases)
            {
                if (whereToInsert < 0)
                {
                    testCase.AddValueObject(TestCase.CollectionType.Conditions, ValueObject.Create(conditionObject));
                }
                else
                {
                    testCase.InsertValueObject(TestCase.CollectionType.Conditions, whereToInsert, ValueObject.Create(conditionObject));
                }
            }
        }

        private void AddToTestCases(ActionObject actionObject, int whereToInsert = -1)
        {
            foreach (TestCase testCase in TestCases)
            {
                if (whereToInsert < 0)
                {
                    testCase.AddValueObject(TestCase.CollectionType.Actions, ValueObject.Create(actionObject));
                }
                else
                {
                    testCase.InsertValueObject(TestCase.CollectionType.Actions, whereToInsert, ValueObject.Create(actionObject));
                }
            }
        }


        private void RemoveActionFromTestCases(int whereToDelete)
        {
            foreach (TestCase testCase in TestCases)
            {
                testCase.Actions.RemoveAt(whereToDelete);
            }
        }

        private void RemoveConditionFromTestCases(int whereToDelete)
        {
            foreach (TestCase testCase in TestCases)
            {
                testCase.Conditions.RemoveAt(whereToDelete);
            }
        }


        public static void AddValueObjects<TType>(TestCase tc, ObservableCollection<TType> list, TestCase.CollectionType colType) where TType : IConditionAction
        {
            foreach (IConditionAction condition in list)
            {
                ValueObject vo = ValueObject.Create(condition);
                vo.TestProperty = tc.Name + " " + condition.Name;
                tc.AddValueObject(colType, vo);
            }
        }

        public void RecalculateStatistics()
        {
            FireStatisticsChanged();
        }

        private DirtyObserver _dirtyObserver;

        public DirtyObserver DirtyObserver
        {
            get { return _dirtyObserver; }
            set
            {
                _dirtyObserver = value;
                OnPropertyChanged("DirtyObserver");
            }
        }

        #region event

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(name);
                PropertyChanged(this, args);
            }
        }

        #endregion

        #region Dirty Support

        public event DirtyChangedDelegate DirtyChanged;

        public void FireDirtyChanged()
        {
            DirtyChanged?.Invoke();
        }

        public void ResetDirty()
        {
            if (DirtyObserver != null)
            {
                DirtyObserver.Reset();
            }
        }

        #endregion
    }
}

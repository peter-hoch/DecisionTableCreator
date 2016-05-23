using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DecisionTableCreator.DynamicTable;

namespace DecisionTableCreator.TestCases
{
    public delegate void ViewChangedDelegate();

    public delegate void StatisticsChangedDelegate();

    public partial class TestCasesRoot : INotifyPropertyChanged
    {
        public TestCasesRoot()
        {
            Init();
        }

        void Init()
        {
            ConditionTable = new DataTableView();
            ActionTable = new DataTableView();
            TestCases = new ObservableCollection<TestCase>();
            Conditions = new ObservableCollection<ConditionObject>();
            Actions = new ObservableCollection<ActionObject>();
        }

        public static readonly string ConditionsColumnHeaderName = "Conditions";
        public static readonly string ActionsColumnHeaderName = "Actions";

        private void CreateBasicColumnDescriptions()
        {
            ConditionTable = new DataTableView();
            ConditionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor(ConditionsColumnHeaderName, typeof(ConditionObject), null));

            ActionTable = new DataTableView();
            ActionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor(ActionsColumnHeaderName, typeof(ActionObject), null));
        }

        void AddColumnDescriptionsForTestCases()
        {
            foreach (TestCase testCase in TestCases)
            {
                ConditionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor(testCase.Name, typeof(TestCase), null));
                ActionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor(testCase.Name, typeof(TestCase), null));
            }
        }


        void PopulateRows<TType>(DataTableView dataTable, IList<TType> list, IList<TestCase> testCases, TestCase.CollectionType colType)
        {
            dataTable.Rows.Clear();
            int rowIndex = 0;
            foreach (TType conditionActionBase in list)
            {
                var row = dataTable.Rows.AddRow();
                dataTable.Columns[0].SetValue(row, conditionActionBase);
                int columnIndex = 1;
                foreach (TestCase testCase in testCases)
                {
                    var value = testCase.GetValueObject(colType, rowIndex);
                    dataTable.Columns[columnIndex].SetValue(row, value);
                    columnIndex++;
                }
                rowIndex++;
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

            CreateBasicColumnDescriptions();
            AddColumnDescriptionsForTestCases();

            ConditionTable.ResizeColumnCount(TestCases.Count + 1);
            ActionTable.ResizeColumnCount(TestCases.Count + 1);

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

            CreateBasicColumnDescriptions();
            AddColumnDescriptionsForTestCases();

            ConditionTable.ResizeColumnCount(TestCases.Count + 1);
            ActionTable.ResizeColumnCount(TestCases.Count + 1);

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
            List<int> savedSelectedItemIndexes = SaveSelectedItemIndex(index);
            Conditions[index].Merge(conditionClone);
            RestoreSelectedItemIndex(index, savedSelectedItemIndexes);
            RecalculateStatistics();
        }

        internal void ChangeAction(int index, ActionObject actionClone)
        {
            List<int> savedSelectedItemIndexes = SaveSelectedItemIndex(index);
            Actions[index].Merge(actionClone);
            RestoreSelectedItemIndex(index, savedSelectedItemIndexes);
        }


        public List<int> SaveSelectedItemIndex(int conditionIndex)
        {
            List<int> selectedItemIndexes = new List<int>();

            foreach (TestCase testCase in TestCases)
            {
                selectedItemIndexes.Add(testCase.Conditions[conditionIndex].SelectedItemIndex);
            }

            return selectedItemIndexes;
        }

        public void RestoreSelectedItemIndex(int conditionIndex, List<int> selectedItemIndexes)
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


        private void AddToTestCases(ConditionObject conditionObject, int whereToInsert=-1)
        {
            foreach (TestCase testCase in TestCases)
            {
                if (whereToInsert < 0)
                {
                    testCase.AddValueObject(TestCase.CollectionType.Conditions,  ValueObject.Create(conditionObject));
                }
                else
                {
                    testCase.InsertValueObject(TestCase.CollectionType.Conditions,  whereToInsert, ValueObject.Create(conditionObject));
                }
            }
        }

        private void AddToTestCases(ActionObject actionObject, int whereToInsert=-1)
        {
            foreach (TestCase testCase in TestCases)
            {
                if (whereToInsert < 0)
                {
                    testCase.AddValueObject(TestCase.CollectionType.Actions,  ValueObject.Create(actionObject));
                }
                else
                {
                    testCase.InsertValueObject(TestCase.CollectionType.Actions,  whereToInsert, ValueObject.Create(actionObject));
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

    }
}

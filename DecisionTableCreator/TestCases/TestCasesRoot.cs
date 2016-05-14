using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.DynamicTable;

namespace DecisionTableCreator.TestCases
{
    public delegate void ViewChangedDelegate();

    public partial class TestCasesRoot : INotifyPropertyChanged
    {
        public TestCasesRoot()
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
                AddColumnDescriptionForTestCase(testCase.Name);
            }
        }

        void AddColumnDescriptionForTestCase(string testCaseName)
        {
            ConditionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor(testCaseName, typeof(TestCase), null));
            ActionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor(testCaseName, typeof(TestCase), null));
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

        public void AppendTestCase()
        {
            AddTestCase();
            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            FireActionsChanged();
            FireConditionsChanged();
        }

        internal void InsertTestCase(int index)
        {
            AddTestCase(index);
            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            FireActionsChanged();
            FireConditionsChanged();
        }


        private void AddTestCase(int indexWhereToInsert = -1)
        {
            int index = TestCases.Count + 1;
            var tc = new TestCase(String.Format("TC{0}", index));
            if (indexWhereToInsert == -1)
            {
                TestCases.Add(tc);
            }
            else
            {
                TestCases.Insert(indexWhereToInsert, tc);
            }
            AddValueObjects(tc, Conditions, TestCase.CollectionType.Conditions);
            AddValueObjects(tc, Actions, TestCase.CollectionType.Actions);
            AddColumnDescriptionForTestCase(tc.Name);
            ConditionTable.ResizeColumnCount(TestCases.Count + 1);
            ActionTable.ResizeColumnCount(TestCases.Count + 1);
        }


        public void AppendAction(ActionObject actionObject)
        {
            Actions.Add(actionObject);
            AddToTestCases(actionObject);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            FireActionsChanged();
        }

        internal void InsertAction(int index, ActionObject newAction)
        {
            Actions.Insert(index, newAction);
            AddToTestCases(newAction, index);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            FireActionsChanged();
        }

        public void AppendCondition(ConditionObject conditionObject)
        {
            Conditions.Add(conditionObject);
            AddToTestCases(conditionObject);
            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            FireConditionsChanged();
        }

        internal void InsertCondition(int index, ConditionObject newCondition)
        {
            Conditions.Insert(index, newCondition);
            AddToTestCases(newCondition, index);
            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            FireConditionsChanged();
        }

        public void ChangeCondition(int index, ConditionObject conditionClone)
        {
            List<int> savedSelectedItemIndexes = SaveSelectedItemIndex(index);
            Conditions[index].Merge(conditionClone);
            RestoreSelectedItemIndex(index, savedSelectedItemIndexes);
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
                    testCase.Conditions.Add(ValueObject.Create(conditionObject));
                }
                else
                {
                    testCase.Conditions.Insert(whereToInsert, ValueObject.Create(conditionObject));
                }
            }
        }

        private void AddToTestCases(ActionObject actionObject, int whereToInsert=-1)
        {
            foreach (TestCase testCase in TestCases)
            {
                if (whereToInsert < 0)
                {
                    testCase.Actions.Add(ValueObject.Create(actionObject));
                }
                else
                {
                    testCase.Actions.Insert(whereToInsert, ValueObject.Create(actionObject));
                }
            }
        }


        public static void AddValueObjects<TType>(TestCase tc, ObservableCollection<TType> list, TestCase.CollectionType colType) where TType : IConditionAction
        {

            foreach (IConditionAction condition in list)
            {
                ValueObject vo = ValueObject.Create(condition);
                vo.TooltipText = tc.Name + " " + condition.Text;
                tc.AddValueObject(colType, vo);
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

    }
}

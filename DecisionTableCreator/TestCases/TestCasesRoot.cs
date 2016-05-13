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
        private DataTableView _conditionTable;

        public DataTableView ConditionTable
        {
            get { return _conditionTable; }
            set
            {
                _conditionTable = value;
                OnPropertyChanged("ConditionTable");
            }
        }

        private DataTableView _actionTable;

        public DataTableView ActionTable
        {
            get { return _actionTable; }
            set
            {
                _actionTable = value;
                OnPropertyChanged("ActionTable");
            }
        }

        private ObservableCollection<TestCase> _testCases;

        public ObservableCollection<TestCase> TestCases
        {
            get { return _testCases; }
            set
            {
                _testCases = value;
                OnPropertyChanged("TestCases");
            }
        }

        private ObservableCollection<ConditionObject> _conditions;

        public ObservableCollection<ConditionObject> Conditions
        {
            get { return _conditions; }
            set
            {
                _conditions = value;
                OnPropertyChanged("Conditions");
            }
        }

        private ObservableCollection<ActionObject> _actions;

        public ObservableCollection<ActionObject> Actions
        {
            get { return _actions; }
            set
            {
                _actions = value;
                OnPropertyChanged("Actions");
            }
        }

        public event ViewChangedDelegate ConditionsChanged;

        public event ViewChangedDelegate ActionsChanged;

        public TestCasesRoot()
        {
            ConditionTable = new DataTableView();
            ActionTable = new DataTableView();
            TestCases = new ObservableCollection<TestCase>();
            Conditions = new ObservableCollection<ConditionObject>();
            Actions = new ObservableCollection<ActionObject>();
        }

        public void FireConditionsChanged()
        {
            ConditionsChanged?.Invoke();
        }

        public void FireActionsChanged()
        {
            ActionsChanged?.Invoke();
        }

        public static TestCasesRoot CreateSampleTable()
        {
            TestCasesRoot testCasesRoot = new TestCasesRoot();
            testCasesRoot.CreateSampleTableInternal();

            return testCasesRoot;
        }

        public static TestCasesRoot CreateSimpleTable()
        {
            TestCasesRoot testCasesRoot = new TestCasesRoot();
            testCasesRoot.CreateSimpleTableInternal();

            return testCasesRoot;
        }

        private void CreateSampleTableInternal()
        {
            int testCasesCount = 20;
            int conditionCount = 5;
            int actionCount = 5;

            List<ObservableCollection<EnumValue>> lists = new List<ObservableCollection<EnumValue>>();
            for (int list = 0; list < (conditionCount + actionCount); list++)
            {
                var subList = new ObservableCollection<EnumValue>();
                lists.Add(subList);
                bool isInvalid = true;
                for (int idx = 1; idx < 10; idx++)
                {
                    subList.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", list, idx), isInvalid));
                    isInvalid = false;
                }
            }

            int listIndex = 0;
            Conditions.Add(ConditionObject.Create(String.Format("Condition {0}", 0), ValueDataType.Bool));
            for (int idx = 1; idx < conditionCount; idx++)
            {
                Conditions.Add(ConditionObject.Create(String.Format("Condition {0}", idx), lists[listIndex++]));
            }

            Actions.Add(ActionObject.Create(String.Format("Action1{0}", 0), ValueDataType.Bool));
            for (int idx = 1; idx < actionCount; idx++)
            {
                Actions.Add(ActionObject.Create(String.Format("Action1{0}", idx), lists[listIndex++]));
            }

            CreateBasicColumnDescriptions();

            for (int idx = 0; idx < testCasesCount; idx++)
            {
                AddTestCase();
            }

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
        }

        private void CreateSimpleTableInternal()
        {
            int testCasesCount = 10;
            int conditionCount = 5;
            int actionCount = 5;

            List<ObservableCollection<EnumValue>> lists = new List<ObservableCollection<EnumValue>>();
            int listNumber = 0;
            int enumIdx = 0;
            var subListSample = new ObservableCollection<EnumValue>();
            lists.Add(subListSample);
            subListSample.Add(new EnumValue(String.Format("{0}-Invalid-{1}", listNumber, enumIdx++), true, false, true));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-Don' t care-{1}", listNumber++, enumIdx++), false, true, false));
            enumIdx = 0;
            subListSample = new ObservableCollection<EnumValue>();
            lists.Add(subListSample);
            subListSample.Add(new EnumValue(String.Format("{0}-Invalid-{1}", listNumber, enumIdx++), true, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, true));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-Don' t care-{1}", listNumber++, enumIdx++), false, true, false));
            enumIdx = 0;
            subListSample = new ObservableCollection<EnumValue>();
            lists.Add(subListSample);
            subListSample.Add(new EnumValue(String.Format("{0}-Invalid-{1}", listNumber, enumIdx++), true, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-Don' t care-{1}", listNumber++, enumIdx++), false, true, true));

            for (int list = listNumber; list < 10; list++)
            {
                var subList = new ObservableCollection<EnumValue>();
                lists.Add(subList);
                bool isInvalid = true;
                for (int idx = 1; idx < 10; idx++)
                {
                    subList.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", list, idx), isInvalid));
                    isInvalid = false;
                }
            }

            int listIndex = 0;
            //Conditions.Add(new ConditionObject(String.Format("Condition {0}", 0), ConditionActionBase.ConditionActionType.Bool));
            for (int idx = 1; idx < conditionCount; idx++)
            {
                Conditions.Add(ConditionObject.Create(String.Format("Condition {0}", idx), lists[listIndex++]));
            }

            for (int idx = 1; idx < actionCount; idx++)
            {
                Actions.Add(ActionObject.Create(String.Format("Action1{0}", idx), lists[listIndex++]));
            }

            CreateBasicColumnDescriptions();

            for (int idx = 0; idx < testCasesCount; idx++)
            {
                AddTestCase();
            }

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
        }

        public static TestCasesRoot CreatePrinterTrubbleshootingSample()
        {
            TestCasesRoot testCasesRoot = new TestCasesRoot();
            testCasesRoot.CreatePrinterTrubbleshootingSampleInternal();
            return testCasesRoot;
        }
        private void CreatePrinterTrubbleshootingSampleInternal()
        {
            var printerEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue("Yes"), new EnumValue("No"), };
            Conditions.Add(ConditionObject.Create(String.Format("Printer does not print"), printerEnum));

            var ledEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue("Yes"), new EnumValue("No"), };
            Conditions.Add(ConditionObject.Create("An error led is flashing", ledEnum));

            var unrecognizedEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue("Yes"), new EnumValue("No"), };
            Conditions.Add(ConditionObject.Create("Printer is unrecognized", unrecognizedEnum));

            var checkPowerEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue(""), new EnumValue("X") };
            Actions.Add(ActionObject.Create(String.Format("Check the power cable"), checkPowerEnum));

            var checkPinterCableEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(ActionObject.Create(String.Format("Check the printer-computer cable"), checkPinterCableEnum));

            var softwareInstalledEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(ActionObject.Create(String.Format("Enshure printer software is installed"), softwareInstalledEnum));

            var checkInkEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(ActionObject.Create(String.Format("Check/replace ink"), checkInkEnum));

            var checkPaperJamEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(ActionObject.Create(String.Format("Check for paper jam"), checkPaperJamEnum));

            CreateBasicColumnDescriptions();

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
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

        //private void UpdateTestCases(int index, ConditionObject condition)
        //{
        //    int colIdx = 1;
        //    foreach (TestCase testCase in TestCases)
        //    {
        //        var rowView = ConditionTable.Rows[index];
        //        rowView.SetValue(colIdx, null);
        //        ValueObject vo = testCase.Conditions[index];
        //        vo.ConditionOrActionParent = condition;
        //        rowView.SetValue(colIdx, vo);
        //        colIdx++;
        //    }
        //}

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

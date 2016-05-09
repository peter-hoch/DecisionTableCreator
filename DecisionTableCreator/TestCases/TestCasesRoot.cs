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

        private ObservableCollection<ConditionActionBase> _conditions;

        public ObservableCollection<ConditionActionBase> Conditions
        {
            get { return _conditions; }
            set
            {
                _conditions = value;
                OnPropertyChanged("Conditions");
            }
        }

        private ObservableCollection<ConditionActionBase> _actions;

        public ObservableCollection<ConditionActionBase> Actions
        {
            get { return _actions; }
            set
            {
                _actions = value;
                OnPropertyChanged("Actions");
            }
        }

        public TestCasesRoot()
        {
            ConditionTable = new DataTableView();
            ActionTable = new DataTableView();
            TestCases = new ObservableCollection<TestCase>();
            Conditions = new ObservableCollection<ConditionActionBase>();
            Actions = new ObservableCollection<ConditionActionBase>();
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
            Conditions.Add(new ConditionObject(String.Format("Condition {0}", 0), ValueDataType.Bool));
            for (int idx = 1; idx < conditionCount; idx++)
            {
                Conditions.Add(new ConditionObject(String.Format("Condition {0}", idx), lists[listIndex++]));
            }

            Actions.Add(new ActionObject(String.Format("Action1{0}", 0), ValueDataType.Bool));
            for (int idx = 1; idx < actionCount; idx++)
            {
                Actions.Add(new ActionObject(String.Format("Action1{0}", idx), lists[listIndex++]));
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
                Conditions.Add(new ConditionObject(String.Format("Condition {0}", idx), lists[listIndex++]));
            }

            Actions.Add(new ActionObject(String.Format("Action1{0}", 0), ValueDataType.Bool));
            for (int idx = 1; idx < actionCount; idx++)
            {
                Actions.Add(new ActionObject(String.Format("Action1{0}", idx), lists[listIndex++]));
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
            var printerEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue("Yes"), new EnumValue("No"),};
            Conditions.Add(new ConditionObject(String.Format("Printer does not print"), printerEnum));

            var ledEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue("Yes"), new EnumValue("No"), };
            Conditions.Add(new ConditionObject("An error led is flashing", ledEnum));

            var unrecognizedEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue("Yes"), new EnumValue("No"), };
            Conditions.Add(new ConditionObject("Printer is unrecognized", unrecognizedEnum));

            var checkPowerEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue(""), new EnumValue("X") };
            Actions.Add(new ActionObject(String.Format("Check the power cable"), checkPowerEnum));

            var checkPinterCableEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(new ActionObject(String.Format("Check the printer-computer cable"), checkPinterCableEnum));

            var softwareInstalledEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(new ActionObject(String.Format("Enshure printer software is installed"), softwareInstalledEnum));

            var checkInkEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(new ActionObject(String.Format("Check/replace ink"), checkInkEnum));

            var checkPaperJamEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(new ActionObject(String.Format("Check for paper jam"), checkPaperJamEnum));

            CreateBasicColumnDescriptions();

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
        }

        private void CreateBasicColumnDescriptions()
        {
            ConditionTable = new DataTableView();
            ConditionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor("Condition", typeof(ConditionObject), null));

            ActionTable = new DataTableView();
            ActionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor("Action", typeof(ActionObject), null));
        }

        void AddColumnDescriptionForTestCase(string testCaseName)
        {
            ConditionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor(testCaseName, typeof(TestCase), null));
            ActionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor(testCaseName, typeof(TestCase), null));
        }

        void PopulateRows(DataTableView dataTable, IList<ConditionActionBase> list, IList<TestCase> testCases, TestCase.CollectionType colType)
        {
            dataTable.Rows.Clear();
            int rowIndex = 0;
            foreach (ConditionActionBase conditionActionBase in list)
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
        }

        private void AddTestCase()
        {
            int index = TestCases.Count+1;
            var tc = new TestCase(String.Format("TC{0}", index));
            TestCases.Add(tc);
            AddValueObjects(tc, Conditions, TestCase.CollectionType.Conditions);
            AddValueObjects(tc, Actions, TestCase.CollectionType.Actions);
            AddColumnDescriptionForTestCase(tc.Name);
            ConditionTable.ResizeColumnCount(TestCases.Count + 1);
            ActionTable.ResizeColumnCount(TestCases.Count + 1);
        }

        public static void AddValueObjects(TestCase tc, ObservableCollection<ConditionActionBase> list, TestCase.CollectionType colType)
        {

            foreach (ConditionActionBase condition in list)
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

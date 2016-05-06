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

    public class TestCasesRoot : INotifyPropertyChanged
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

        private void CreateSampleTableInternal()
        {
            int testCasesCount = 2;
            var list = new ObservableCollection<EnumValue>() { new EnumValue("EnumValue1"), new EnumValue("EnumValue2"), new EnumValue("EnumValue3") };

            Conditions.Add(new ConditionObject("Condition1", ConditionActionBase.ConditionActionType.Text));
            Conditions.Add(new ConditionObject("Condition2", ConditionActionBase.ConditionActionType.Text));
            Conditions.Add(new ConditionObject("Condition3", list));
            Conditions.Add(new ConditionObject("Condition4", ConditionActionBase.ConditionActionType.Bool));
            Actions.Add(new ActionObject("Action1", ConditionActionBase.ConditionActionType.Text));
            Actions.Add(new ActionObject("Action2", ConditionActionBase.ConditionActionType.Text));

            CreateColumnDescriptions(testCasesCount);

            AddTestCase();
            AddTestCase();

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
        }

        private void CreateColumnDescriptions(int testCasesCount)
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
                ValueObject vo;
                switch (condition.Type)
                {
                    case ConditionActionBase.ConditionActionType.Text:
                        vo = new ValueObject("defaultText");
                        break;
                    case ConditionActionBase.ConditionActionType.Enum:
                        vo = new ValueObject(condition.EnumValues);
                        break;
                    case ConditionActionBase.ConditionActionType.Bool:
                        vo = new ValueObject(false, "boolText");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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

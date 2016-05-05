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
            int testCasesCount = 2;
            TestCasesRoot testCasesRoot = new TestCasesRoot();

            testCasesRoot.CreateColumnDescriptions(testCasesCount);


            var list = new ObservableCollection<Item>() {new Item("Value1"), new Item("Value2"), new Item("Value3")};
            ValueObject enum1 = new ValueObject(list);
            ValueObject enum2 = new ValueObject(list);

            testCasesRoot.Conditions.Add(new ConditionObject("Condition1"));
            testCasesRoot.Conditions.Add(new ConditionObject("Condition2"));
            testCasesRoot.Conditions.Add(new ConditionObject("Condition3"));
            testCasesRoot.Conditions.Add(new ConditionObject("Condition4"));
            testCasesRoot.Actions.Add(new ActionObject("Action1"));
            testCasesRoot.Actions.Add(new ActionObject("Action2"));

            testCasesRoot.TestCases.Add(new TestCase("TC1",
                new ValueObject[] {new ValueObject("c1-TC1"), new ValueObject("c2-TC1"), enum1, new ValueObject(true, "Cond4")},
                new ValueObject[] {new ValueObject("a1-TC1"), new ValueObject("a2-TC1"),}));
            testCasesRoot.TestCases.Add(new TestCase("TC2",
                new ValueObject[] {new ValueObject("c1-TC2"), new ValueObject("c2-TC2"), enum2, new ValueObject(true, "Cond4")},
                new ValueObject[] {new ValueObject("a1-TC2"), new ValueObject("a2-TC2"),}));

            testCasesRoot.PopulateRows(testCasesRoot.ConditionTable, testCasesRoot.Conditions, testCasesRoot.TestCases, TestCase.CollectionType.Conditions);

            testCasesRoot.PopulateRows(testCasesRoot.ActionTable, testCasesRoot.Actions, testCasesRoot.TestCases, TestCase.CollectionType.Actions);

            return testCasesRoot;
        }

        private void CreateColumnDescriptions(int testCasesCount)
        {
            ConditionTable = new DataTableView();
            ConditionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor("Condition", typeof(ConditionObject), null));

            ActionTable = new DataTableView();
            ActionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor("Action", typeof(ActionObject), null));

            for (int idx = 0; idx < testCasesCount; idx++)
            {
                string testCaseName = String.Format("TC{0}", idx);
                ConditionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor(testCaseName, typeof(TestCase), null));
                ActionTable.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor(testCaseName, typeof(TestCase), null));
            }
        }

        void PopulateRows(DataTableView dataTable, IList<ConditionActionBase> list, IList<TestCase> testCases, TestCase.CollectionType colType)
        {
            int rowIndex = 0;
            foreach (ConditionActionBase conditionActionBase in list)
            {
                var row = dataTable.Rows.AddRow();
                dataTable.Columns[0].SetValue(row, conditionActionBase);
                int columnIndex = 1;
                foreach (TestCase testCase in testCases)
                {
                    dataTable.Columns[columnIndex].SetValue(row, testCase.GetValueObject(colType, rowIndex));
                    columnIndex++;
                }
                rowIndex++;
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

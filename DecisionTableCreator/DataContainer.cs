using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.DynamicTable;
using DecisionTableCreator.TestCases;

namespace DecisionTableCreator
{
    public class DataContainer : INotifyPropertyChanged
    {
        public DataContainer()
        {
            CreateSampleTables();
        }

        public void CreateSampleTables()
        {
            Conditions = new DataTableView();
            Conditions.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor("Condition", typeof(ConditionObject), null));
            Conditions.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor("TC1", typeof(TestCase), null));
            Conditions.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor("TC2", typeof(TestCase), null));

            var list = new ObservableCollection<Item>() {new Item("Value1"), new Item("Value2"), new Item("Value3")};
            ValueObject enum1 = new ValueObject(list);
            ValueObject enum2 = new ValueObject(list);
            ValueObject co1 = new ValueObject("Condition1");
            ValueObject co2 = new ValueObject("Condition2");
            ValueObject co3 = new ValueObject("Condition3");
            ValueObject co4 = new ValueObject("Condition4");
            ValueObject ac1 = new ValueObject("Action1");
            ValueObject ac2 = new ValueObject("Action2");
            TestCase tc1 = new TestCase("TC1", new ValueObject[] { new ValueObject("c1-TC1"), new ValueObject("c2-TC1"), enum1, new ValueObject(true, "Cond4") }, new ValueObject[] { new ValueObject("a1-TC1"), new ValueObject("a2-TC1"), });
            TestCase tc2 = new TestCase("TC2", new ValueObject[] { new ValueObject("c1-TC2"), new ValueObject("c2-TC2"), enum2, new ValueObject(true, "Cond4") }, new ValueObject[] { new ValueObject("a1-TC2"), new ValueObject("a2-TC2"), });

            var row = Conditions.Rows.AddRow();
            Conditions.Columns[0].SetValue(row, co1);
            Conditions.Columns[1].SetValue(row, tc1.Conditions[0]);
            Conditions.Columns[2].SetValue(row, tc2.Conditions[0]);

            row = Conditions.Rows.AddRow();
            Conditions.Columns[0].SetValue(row, co2);
            Conditions.Columns[1].SetValue(row, tc1.Conditions[1]);
            Conditions.Columns[2].SetValue(row, tc2.Conditions[1]);

            row = Conditions.Rows.AddRow();
            Conditions.Columns[0].SetValue(row, co3);
            Conditions.Columns[1].SetValue(row, tc1.Conditions[2]);
            Conditions.Columns[2].SetValue(row, tc2.Conditions[2]);

            row = Conditions.Rows.AddRow();
            Conditions.Columns[0].SetValue(row, co4);
            Conditions.Columns[1].SetValue(row, tc1.Conditions[3]);
            Conditions.Columns[2].SetValue(row, tc2.Conditions[3]);

            Actions = new DataTableView();
            Actions.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor("Action", typeof(ActionObject), null));
            Actions.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor("TC1", typeof(TestCase), null));
            Actions.ColumnPropDescColl.AddDescription(new ColumnPropertyDescriptor("TC2", typeof(TestCase), null));

            row = Actions.Rows.AddRow();
            Actions.Columns[0].SetValue(row, ac1);
            Actions.Columns[1].SetValue(row, tc1.Actions[0]);
            Actions.Columns[2].SetValue(row, tc2.Actions[0]);

            row = Actions.Rows.AddRow();
            Actions.Columns[0].SetValue(row, ac2);
            Actions.Columns[1].SetValue(row, tc1.Actions[1]);
            Actions.Columns[2].SetValue(row, tc2.Actions[1]);

        }

        private DataTableView _conditions;

        public DataTableView Conditions
        {
            get { return _conditions; }
            set
            {
                _conditions = value;
                OnPropertyChanged("Conditions");
            }
        }


        private DataTableView _actions;

        public DataTableView Actions
        {
            get { return _actions; }
            set
            {
                _actions = value;
                OnPropertyChanged("Actions");
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

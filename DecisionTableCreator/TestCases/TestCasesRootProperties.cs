using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.DynamicTable;

namespace DecisionTableCreator.TestCases
{
    public partial class TestCasesRoot
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
                if (_conditions != null)
                {
                    _conditions.CollectionChanged -= OnCollectionChanged;                   
                }
                _conditions = value;
                OnPropertyChanged("Conditions");
                if (_conditions != null)
                {
                    _conditions.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            if (eventArgs.Action == NotifyCollectionChangedAction.Add || eventArgs.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (ConditionActionBase newItem in eventArgs.NewItems)
                {
                    newItem.TestCasesRoot = this;
                }
            }
        }

        private ObservableCollection<ActionObject> _actions;

        public ObservableCollection<ActionObject> Actions
        {
            get { return _actions; }
            set
            {
                if (_actions != null)
                {
                    _actions.CollectionChanged -= OnCollectionChanged;
                }
                _actions = value;
                OnPropertyChanged("Actions");
                if (_actions != null)
                {
                    _actions.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        public event ViewChangedDelegate ConditionsChanged;

        public event ViewChangedDelegate ActionsChanged;

        public event StatisticsChangedDelegate StatisticsChanged;

        private void ProcessConditionsChanged()
        {
            RecalculateStatistics();

            ConditionsChanged?.Invoke();
        }


        private void FireActionsChanged()
        {
            ActionsChanged?.Invoke();
        }

        public void FireStatisticsChanged()
        {
            StatisticsChanged?.Invoke();
        }



    }
}

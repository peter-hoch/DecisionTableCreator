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
            TestCasesRoot = TestCasesRoot.CreateSimpleTable();
            //TestCasesRoot = TestCasesRoot.CreateSampleTable();
            //TestCasesRoot = TestCasesRoot.CreatePrinterTrubbleshootingSample();
            Conditions = TestCasesRoot.ConditionTable;
            Actions = TestCasesRoot.ActionTable;
            OnStatisticsChanged();
        }

        private TestCasesRoot _testCasesRoot;

        public TestCasesRoot TestCasesRoot
        {
            get { return _testCasesRoot; }
            set
            {
                if (_testCasesRoot != null)
                {
                    _testCasesRoot.ConditionsChanged -= OnConditionsChanged;
                    _testCasesRoot.ActionsChanged -= OnActionsChanged;
                    _testCasesRoot.StatisticsChanged -= OnStatisticsChanged;
                }
                _testCasesRoot = value;
                OnPropertyChanged("TestCasesRoot");
                if (_testCasesRoot != null)
                {
                    _testCasesRoot.ConditionsChanged += OnConditionsChanged;
                    _testCasesRoot.ActionsChanged += OnActionsChanged;
                    _testCasesRoot.StatisticsChanged += OnStatisticsChanged;
                }
            }
        }

        private void OnStatisticsChanged()
        {
            PossibleCombinations = TestCasesRoot.CalculatePossibleCombinations();
            CoveredTestCases = TestCasesRoot.CalculateNumberOfUniqueCoveredTestCases();
            CoveredTestCases = TestCasesRoot.CalculateCoverage();
        }

        private void OnActionsChanged()
        {
            Actions = null;
            Actions = TestCasesRoot.ActionTable;
        }

        private void OnConditionsChanged()
        {
            Conditions = null;
            Conditions = TestCasesRoot.ConditionTable;
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

        private int _possibleCombinations;

        public int PossibleCombinations
        {
            get { return _possibleCombinations; }
            set
            {
                _possibleCombinations = value;
                OnPropertyChanged("PossibleCombinations");
            }
        }

        private double _coveredTestCases;

        public double CoveredTestCases
        {
            get { return _coveredTestCases; }
            set
            {
                _coveredTestCases = value;
                OnPropertyChanged("CoveredTestCases");
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

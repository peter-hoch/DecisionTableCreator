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
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    public partial class TestCasesRoot
    {
        private DataTable _conditionTable;

        public DataTable ConditionTable
        {
            get { return _conditionTable; }
            set
            {
                _conditionTable = value;
                OnPropertyChanged("ConditionTable");
            }
        }

        private DataTable _actionTable;

        public DataTable ActionTable
        {
            get { return _actionTable; }
            set
            {
                _actionTable = value;
                OnPropertyChanged("ActionTable");
            }
        }

        private ObservableCollection<TestCase> _testCases;

        [ObserveForDirty]
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

        [ObserveForDirty]
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

        [ObserveForDirty]
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

        public event ViewChangedDelegate ConditionsBeginChange;
        public event ViewChangedDelegate ConditionsEndChange;

        public event ViewChangedDelegate ActionsBeginChange;
        public event ViewChangedDelegate ActionsEndChange;

        public event StatisticsChangedDelegate StatisticsChanged;

        private void ProcessConditionsChanged()
        {
            RecalculateStatistics();

            ConditionsEndChange?.Invoke();
        }


        private void FireActionsChanged()
        {
            ActionsEndChange?.Invoke();
        }

        public void FireStatisticsChanged()
        {
            StatisticsChanged?.Invoke();
        }



    }
}

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
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DecisionTableCreator.TestCases;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator
{
    public delegate void QuitEditModeDelegate();

    public class DataContainer : INotifyDirtyChanged
    {
        public DataContainer()
        {
            TestCasesRoot = new TestCasesRoot();
            //TestCasesRoot.CreateTestProject();
            Conditions = TestCasesRoot.ConditionTable;
            Actions = TestCasesRoot.ActionTable;
            OnStatisticsChanged();
            DirtyObserver = new DirtyObserver(this);
            UpdateTitle();
            DirtyChanged += OnDirtyChanged;
        }

        public event QuitEditModeDelegate QuitEditMode;

        private void OnDirtyChanged()
        {
            UpdateTitle();
        }


        const string TitleFormat = "{0} Decision Table creator - {1}";

        public void UpdateTitle()
        {
            string projectName = "New Project";
            if (!String.IsNullOrEmpty(ProjectPath))
            {
                projectName = Path.GetFileNameWithoutExtension(ProjectPath);
            }
            Title = string.Format(TitleFormat, DirtyObserver.Dirty ? "*" : "", projectName);
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _projectPath = "New Project";

        public string ProjectPath
        {
            get { return _projectPath; }
            set
            {
                _projectPath = value;
                OnPropertyChanged("ProjectPath");
                UpdateTitle();
            }
        }



        private TestCasesRoot _testCasesRoot;

        [ObserveForDirty]
        public TestCasesRoot TestCasesRoot
        {
            get { return _testCasesRoot; }
            set
            {
                if (_testCasesRoot != null)
                {
                    _testCasesRoot.ConditionsBeginChange -= OnConditionsBeginChange;
                    _testCasesRoot.ConditionsEndChange -= OnConditionsEndChange;
                    _testCasesRoot.ActionsBeginChange -= OnActionsBeginChange;
                    _testCasesRoot.ActionsEndChange -= OnActionsEndChange;
                    _testCasesRoot.StatisticsChanged -= OnStatisticsChanged;
                }
                _testCasesRoot = value;
                OnPropertyChanged("TestCasesRoot");
                if (_testCasesRoot != null)
                {
                    _testCasesRoot.ConditionsBeginChange += OnConditionsBeginChange;
                    _testCasesRoot.ConditionsEndChange += OnConditionsEndChange;
                    _testCasesRoot.ActionsBeginChange += OnActionsBeginChange;
                    _testCasesRoot.ActionsEndChange += OnActionsEndChange;
                    _testCasesRoot.StatisticsChanged += OnStatisticsChanged;
                }
            }
        }

        private void OnStatisticsChanged()
        {
            Statistics stat = TestCasesRoot.CalculateStatistics();
            PossibleCombinations = stat.PossibleCombinations;
            CoveredTestCases = stat.CoveredTestCases;
            Coverage = stat.Coverage;
        }

        private void OnActionsBeginChange()
        {
            QuitEditMode?.Invoke();
            Actions = null;
        }

        private void OnActionsEndChange()
        {
            Actions = TestCasesRoot.ActionTable;
        }

        private void OnConditionsBeginChange()
        {
            QuitEditMode?.Invoke();
            Conditions = null;
        }

        private void OnConditionsEndChange()
        {
            Conditions = TestCasesRoot.ConditionTable;
        }


        private DataTable _conditions;

        public DataTable Conditions
        {
            get { return _conditions; }
            set
            {
                _conditions = value;
                OnPropertyChanged("Conditions");
            }
        }


        private DataTable _actions;

        public DataTable Actions
        {
            get { return _actions; }
            set
            {
                _actions = value;
                OnPropertyChanged("Actions");
            }
        }

        private long _possibleCombinations;

        public long PossibleCombinations
        {
            get { return _possibleCombinations; }
            set
            {
                _possibleCombinations = value;
                OnPropertyChanged("PossibleCombinations");
            }
        }

        private long _coveredTestCases;

        public long CoveredTestCases
        {
            get { return _coveredTestCases; }
            set
            {
                _coveredTestCases = value;
                OnPropertyChanged("CoveredTestCases");
            }
        }

        private double _coverage;

        public double Coverage
        {
            get { return _coverage; }
            set
            {
                _coverage = value;
                OnPropertyChanged("Coverage");
            }
        }


        private ObservableCollection<MenuItem> _exportToFileItem = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> ExportToFileItem
        {
            get { return _exportToFileItem; }
            set
            {
                _exportToFileItem = value;
                OnPropertyChanged("ExportToFileItem");
            }
        }

        private ObservableCollection<MenuItem> _exportToClipboardItem = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> ExportToClipboardItem
        {
            get { return _exportToClipboardItem; }
            set
            {
                _exportToClipboardItem = value;
                OnPropertyChanged("ExportToClipboardItem");
            }
        }


        private DirtyObserver _dirtyObserver;

        public DirtyObserver DirtyObserver
        {
            get { return _dirtyObserver; }
            set
            {
                _dirtyObserver = value;
                OnPropertyChanged("DirtyObserver");
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

        #region Dirty Support

        public event DirtyChangedDelegate DirtyChanged;

        public void FireDirtyChanged()
        {
            DirtyChanged?.Invoke();
        }

        public void ResetDirty()
        {
            if (DirtyObserver != null)
            {
                DirtyObserver.Reset();
            }
        }

        #endregion

    }
}

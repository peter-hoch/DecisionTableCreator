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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{

    public class TestCase : IEqualTestSetting<TestCase>, INotifyDirtyChanged, ITestCase
    {
        public enum CollectionType
        {
            Conditions,
            Actions
        }

        public TestCase(string name, ValueObject[] conditions, ValueObject[] actions)
        {
            DirtyObserver = new DirtyObserver(this);
            TestCaseIsUnique = true;
            Name = name;
            Conditions = new ObservableCollection<ValueObject>(conditions);
            Actions = new ObservableCollection<ValueObject>(actions);
        }

        public TestCase(string name, List<ValueObject> conditions, List<ValueObject> actions)
        {
            DirtyObserver = new DirtyObserver(this);
            TestCaseIsUnique = true;
            Name = name;
            Conditions = new ObservableCollection<ValueObject>(conditions);
            Actions = new ObservableCollection<ValueObject>(actions);
        }

        public TestCase(string name)
        {
            DirtyObserver = new DirtyObserver(this);
            TestCaseIsUnique = true;
            Name = name;
            Conditions = new ObservableCollection<ValueObject>();
            Actions = new ObservableCollection<ValueObject>();
        }

        private String _name;

        [ObserveForDirty]
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }


        public ValueObject GetValueObject(CollectionType type, int index)
        {
            switch (type)
            {
                case CollectionType.Conditions:
                    return Conditions[index];

                case CollectionType.Actions:
                    return Actions[index];

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void AddValueObject(CollectionType type, ValueObject value)
        {
            switch (type)
            {
                case CollectionType.Conditions:
                    Conditions.Add(value);
                    break;
                case CollectionType.Actions:
                    Actions.Add(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void InsertValueObject(CollectionType type, int whereToInsert, ValueObject value)
        {
            switch (type)
            {
                case CollectionType.Conditions:
                    Conditions.Insert(whereToInsert, value);
                    break;
                case CollectionType.Actions:
                    Actions.Insert(whereToInsert, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private ObservableCollection<ValueObject> _conditions;

        [ObserveForDirty]
        public ObservableCollection<ValueObject> Conditions
        {
            get { return _conditions; }
            set
            {
                _conditions = value;
                OnPropertyChanged("Conditions");
            }
        }

        private ObservableCollection<ValueObject> _actions;

        [ObserveForDirty]
        public ObservableCollection<ValueObject> Actions
        {
            get { return _actions; }
            set
            {
                _actions = value;
                OnPropertyChanged("Actions");
            }
        }

        private string _testProperty;

        /// <summary>
        /// for unit testing only
        /// </summary>
        public string TestProperty
        {
            get { return _testProperty; }
            set
            {
                _testProperty = value;
                OnPropertyChanged("TestProperty");
            }
        }

        private int _displayIndex;

        [ObserveForDirty]
        public int DisplayIndex
        {
            get { return _displayIndex; }
            set
            {
                _displayIndex = value;
                OnPropertyChanged("DisplayIndex");
            }
        }

        /// <summary>
        /// only for rtf generation 
        /// value is not saved
        /// is set during preparation for output generation
        /// </summary>
        public int RtfCellOffset { get; set; }

        #region test case calculation

        public bool TestSettingIsEqual(TestCase other)
        {
            return TestSettingIsEqual(this, other);
        }

        public static void UpdateUniqueness(IList<TestCase> testCases)
        {
            foreach (TestCase testCase in testCases)
            {
                testCase.TestCaseIsUnique = true;
            }
            for (int outerIdx = 0; outerIdx < testCases.Count; outerIdx++)
            {
                for (int innerIdx = outerIdx + 1; innerIdx < testCases.Count; innerIdx++)
                {
                    UpdateUniqueness(testCases[outerIdx], testCases[innerIdx]);
                }
            }
        }

        /// <summary>
        /// update the uniqueness flag of the testcases
        /// a test case with an invalid value ist not unique
        /// </summary>
        /// <param name="tc1"></param>
        /// <param name="tc2"></param>
        /// <returns>true if both test cases are unique</returns>
        public static bool UpdateUniqueness(TestCase tc1, TestCase tc2)
        {
            if (tc1.Conditions.Count != tc2.Conditions.Count)
            {
                throw new ArgumentOutOfRangeException("compare operation with two diffrent test case settings");
            }
            if (tc1.ContainsInvalid)
            {
                tc1.TestCaseIsUnique = false;
            }
            if (tc2.ContainsInvalid)
            {
                tc2.TestCaseIsUnique = false;
            }
            if (TestSettingIsEqual(tc1, tc2))
            {
                tc2.TestCaseIsUnique = false;
            }
            else
            {
                if (tc1.ContainsDontCare || tc2.ContainsDontCare)
                {
                    if (IsOneNonDontCareSettingDiffrent(tc1, tc2))
                    {
                    }
                    else
                    {
                        int c1 = GetDontCareCount(tc1);
                        int c2 = GetDontCareCount(tc2);
                        if (c1 > c2)
                        {
                            tc2.TestCaseIsUnique = false;
                        }
                        else
                        {
                            tc1.TestCaseIsUnique = false;
                        }
                    }
                }
            }
            return tc1.TestCaseIsUnique && tc2.TestCaseIsUnique;
        }

        /// <summary>
        /// both test settings are equal
        /// invalid and DontCare are ignored
        /// </summary>
        /// <param name="tc1"></param>
        /// <param name="tc2"></param>
        /// <returns></returns>
        static bool TestSettingIsEqual(TestCase tc1, TestCase tc2)
        {
            if (tc1.Conditions.Count != tc2.Conditions.Count)
            {
                throw new ArgumentOutOfRangeException("compare operation with two diffrent test case settings");
            }
            for (int idx = 0; idx < tc1.Conditions.Count; idx++)
            {
                if (!tc1.Conditions[idx].TestSettingIsEqual(tc2.Conditions[idx]))
                {
                    return false;
                }
            }
            return true;
        }

        static int GetDontCareCount(TestCase tc)
        {
            int count = 0;
            for (int idx = 0; idx < tc.Conditions.Count; idx++)
            {
                if (tc.Conditions[idx].SelectedValue.DontCare)
                {
                    count++;
                }
            }

            return count;
        }

        static bool IsOneNonDontCareSettingDiffrent(TestCase tc1, TestCase tc2)
        {
            for (int idx = 0; idx < tc1.Conditions.Count; idx++)
            {
                if (!tc1.Conditions[idx].SelectedValue.DontCare && !tc2.Conditions[idx].SelectedValue.DontCare)
                {
                    if (tc1.Conditions[idx].SelectedItemIndex != tc2.Conditions[idx].SelectedItemIndex)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        public bool ContainsDontCare
        {
            get { return TestSettingContainsDontCare(this); }
        }

        public bool ContainsInvalid
        {
            get
            {
                return TestSettingContainsInvalid(this);
            }
        }


        static bool TestSettingContainsDontCare(TestCase tc)
        {
            foreach (ValueObject val in tc.Conditions)
            {
                if (val.SelectedValue.DontCare)
                {
                    return true;
                }
            }
            return false;
        }


        static bool TestSettingContainsInvalid(TestCase tc)
        {
            for (int idx = 0; idx < tc.Conditions.Count; idx++)
            {
                if (tc.Conditions[idx].SelectedValue.IsInvalid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// this property indicates that there an an other test case with the same setting
        /// </summary>
        public bool TestCaseIsUnique { get; set; }


        /// <summary>
        /// without DontCare the count == 1
        /// With one DontCare count == EnumValue.Count
        /// With two and more DontCares count == EnumValue.Count * EnumValue.Count * ...
        /// </summary>
        /// <returns></returns>
        public int CalculateNumberOfCoveredTestCases()
        {
            int count = 0;
            foreach (ValueObject vo in Conditions)
            {
                if (vo.SelectedValue.DontCare)
                {
                    if (count == 0)
                    {
                        count = vo.PossibleValues;
                    }
                    else
                    {
                        count *= vo.PossibleValues;
                    }
                }
            }
            return count == 0 ? 1 : count;
        }

        #endregion


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("TestCase {0} dispIdx={1}  unique={2} {3}cond:", Name, DisplayIndex, TestCaseIsUnique, ContainsInvalid ? "invalid " : "");
            foreach (ValueObject vo in Conditions)
            {
                sb.Append(vo.ToTestString() + " ");
            }
            return sb.ToString();
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

        /// <summary>
        /// for unit tests only
        /// </summary>
        /// <param name="root"></param>
        /// <param name="conditionValues"></param>
        /// <param name="actionValues"></param>
        public void SetValues(TestCasesRoot root, List<ValueObject> conditionValues, List<ValueObject> actionValues)
        {
            if (conditionValues.Count != root.Conditions.Count)
            {
                throw new Exception("internal error");
            }
            for (int idx = 0; idx < root.Conditions.Count; idx++)
            {
                Conditions[idx].SelectedItemIndex = conditionValues[idx].SelectedItemIndex;
            }
            for (int idx = 0; idx < root.Actions.Count; idx++)
            {
                Actions[idx].SelectedItemIndex = actionValues[idx].SelectedItemIndex;
            }
        }

        #endregion


    }
}

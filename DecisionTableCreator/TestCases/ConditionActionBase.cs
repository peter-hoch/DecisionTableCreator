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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    /// <summary>
    /// base class for condition and action
    /// </summary>
    public class ConditionActionBase : IConditionAction, INotifyDirtyChanged, IConditionActionObject
    {

        protected ConditionActionBase()
        {
            DirtyObserver = new DirtyObserver(this);
        }

        protected ConditionActionBase(string text, ValueDataType type) : this()
        {
            Name = text;
            DataType = type;
        }

        protected ConditionActionBase(string text, ObservableCollection<EnumValue> enums) : this(text, ValueDataType.Enumeration)
        {
            EnumValues = enums;
        }

        public void Merge(ConditionActionBase clone)
        {
            if (Name != clone.Name)
            {
                Name = clone.Name;
            }
            if (DataType != clone.DataType)
            {
                DataType = clone.DataType;
            }
            if (DefaultText != clone.DefaultText)
            {
                DefaultText = clone.DefaultText;
            }
            DefaultBool = clone.DefaultBool;
            Background = clone.Background;

            //EnumValues.Clear();
            while (EnumValues.Count != 0)
            {
              EnumValues.RemoveAt(0);  
            }
            foreach (EnumValue value in clone.EnumValues)
            {
                EnumValues.Add(value);
            }
        }

        /// <summary>
        /// clone the object and all its content
        /// </summary>
        /// <param name="clone"></param>
        protected void Clone(IConditionAction clone)
        {
            ConditionActionBase co = (ConditionActionBase)clone;
            co.Name = Name;
            co.DataType = DataType;
            co.DefaultBool = DefaultBool;
            co.DefaultText = DefaultText;
            co.Background = Background;
            co.EnumValues = new ObservableCollection<EnumValue>();
            foreach (EnumValue value in EnumValues)
            {
                co.EnumValues.Add(value.Clone());
            }
        }

        public IList<ValueObject> TestValues
        {
            get
            {
                var result = new List<ValueObject>();
                if (TestCasesRoot != null && TestCasesRoot.TestCases != null && TestCasesRoot.TestCases.Count != 0)
                {
                    if (this is ConditionObject)
                    {
                        GetTestValues(result, TestCase.CollectionType.Conditions);
                    }
                    else
                    {
                        GetTestValues(result, TestCase.CollectionType.Actions);
                    }
                }
                return result;
            }
        }

        private void GetTestValues(List<ValueObject> result, TestCase.CollectionType collectionType)
        {
            int index = CalculateIndexOfConditionOrAction(collectionType);
            foreach (TestCase testCase in TestCasesRoot.TestCases)
            {
                result.Add(testCase.GetValueObject(collectionType, index));
            }
        }

        int CalculateIndexOfConditionOrAction(TestCase.CollectionType collectionType)
        {
            IList<ValueObject> values;
            if (collectionType == TestCase.CollectionType.Conditions)
            {
                values = TestCasesRoot.TestCases[0].Conditions;
            }
            else
            {
                values = TestCasesRoot.TestCases[0].Actions;
            }
            ValueObject value = values.FirstOrDefault(vo => vo.ConditionOrActionParent.Equals(this));
            if (value != null)
            {
                int index =  values.IndexOf(value);
                return index;
            }
            return -1;
        }

        #region properties

        public TestCasesRoot TestCasesRoot { get; set; }

        public string EditBoxName { get; protected set; }

        /// <summary>
        /// only valid during load and save!!
        /// </summary>
        public int LoadSaveId { get; set; }

        private ValueDataType _dataType;

        [ObserveForDirty]
        public ValueDataType DataType
        {
            get { return _dataType; }
            protected set
            {
                _dataType = value;
                OnPropertyChanged("DataType");
            }
        }

        private string _name;
        [ObserveForDirty]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private bool _defaultBool;

        [ObserveForDirty]
        public bool DefaultBool
        {
            get { return _defaultBool; }
            set
            {
                _defaultBool = value;
                OnPropertyChanged("DefaultBool");
            }
        }

        private string _defaultText;

        [ObserveForDirty]
        public string DefaultText
        {
            get { return _defaultText; }
            set
            {
                _defaultText = value;
                OnPropertyChanged("DefaultText");
            }
        }

        private ObservableCollection<EnumValue> _enumValues = new ObservableCollection<EnumValue>();

        [ObserveForDirty]
        public ObservableCollection<EnumValue> EnumValues
        {
            get { return _enumValues; }
            set
            {
                _enumValues = value;
                OnPropertyChanged("EnumValues");
            }
        }

        private string _tooltipText;

        [ObserveForDirty]
        public string TooltipText
        {
            get { return _tooltipText; }
            set
            {
                _tooltipText = value;
                OnPropertyChanged("TooltipText");
            }
        }

        private Background _background = new Background(BackgroundColor.White);

        public Background Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged("Background");
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

        /// <summary>
        /// only for rtf generation 
        /// value is not saved
        /// is set during preparation for output generation
        /// </summary>
        public int RtfCellOffset { get; set; }


        #endregion

        public override string ToString()
        {
            return this.GetType().Name + " " + Name;
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

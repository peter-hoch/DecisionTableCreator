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
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    public enum ValueObjectSettingOption
    {
        Normal,
        IsInvalid,
        DontCare
    }

    public interface IEqualTestSetting<TType>
    {
        bool TestSettingIsEqual(TType other);
    }

    public class ValueObject : INotifyDirtyChanged, IEqualTestSetting<ValueObject>, IValueObject
    {

        public static ValueObject Create(IConditionAction conditionOrAction)
        {
            ValueObject vo;
            switch (conditionOrAction.DataType)
            {
                case ValueDataType.Text:
                    vo = new ValueObject(conditionOrAction.DefaultText, ValueDataType.Text);
                    break;
                case ValueDataType.Enumeration:
                    vo = CreateEnumValueObject(conditionOrAction);
                    break;
                case ValueDataType.Bool:
                    vo = new ValueObject(conditionOrAction.DefaultText, ValueDataType.Bool);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            vo.ConditionOrActionParent = conditionOrAction;
            return vo;
        }

        private static ValueObject CreateEnumValueObject(IConditionAction conditionOrAction)
        {
            var defaultIndex = CalculateDefaultIndex(conditionOrAction);
            ValueObject vo;
            vo = new ValueObject(conditionOrAction.EnumValues);
            vo.SelectedItemIndex = defaultIndex;
            return vo;
        }

        public void CalculateAndSetDefaultIndex()
        {
            SelectedItemIndex = CalculateDefaultIndex(ConditionOrActionParent);
        }

        private static int CalculateDefaultIndex(IConditionAction conditionOrAction)
        {
            int defaultIndex = 0;
            int idx = 0;
            foreach (EnumValue value in conditionOrAction.EnumValues)
            {
                if (value.IsDefault)
                {
                    defaultIndex = idx;
                    break;
                }
                idx++;
            }
            return defaultIndex;
        }

        public ValueObject(string text, ValueDataType type)
        {
            DirtyObserver = new DirtyObserver(this);
            EnumValues = new ObservableCollection<EnumValue>();
            DataType = type;
            Text = text;
        }


        public ValueObject(ObservableCollection<EnumValue> items)
        {
            DirtyObserver = new DirtyObserver(this);
            DataType = ValueDataType.Enumeration;
            EnumValues = items;
        }

        public ValueObject(ObservableCollection<EnumValue> items, int selectedItem)
        {
            DirtyObserver = new DirtyObserver(this);
            DataType = ValueDataType.Enumeration;
            EnumValues = items;
            if (selectedItem < 0 || selectedItem >= EnumValues.Count)
            {
                throw new ArgumentOutOfRangeException("selectedItem index out of range");
            }
            SelectedItemIndex = selectedItem;
        }

        private IConditionAction _conditionOrActionParent;
        public IConditionAction ConditionOrActionParent
        {
            get
            {
                return _conditionOrActionParent;
            }
            set
            {
                _conditionOrActionParent = value;
                if (DataType != value.DataType)
                {
                    BoolValue = value.DefaultBool;
                    Text = value.DefaultText;
                }
                DataType = value.DataType;
                if (EnumValues.Count > 0)
                {
                    if (SelectedItemIndex >= value.EnumValues.Count)
                    {
                        SelectedItemIndex = CalculateDefaultIndex(value);
                    }
                }
                EnumValues = value.EnumValues;
            }
        }

        private ValueDataType _dataType;

        [ObserveForDirty]
        public ValueDataType DataType
        {
            get { return _dataType; }
            private set
            {
                _dataType = value;
                OnPropertyChanged("DataType");

                switch (value)
                {
                    case ValueDataType.Text:
                        TextBoxVisibility = Visibility.Visible;
                        ComboBoxVisibility = Visibility.Collapsed;
                        CheckboxVisibility = Visibility.Collapsed;
                        break;
                    case ValueDataType.Enumeration:
                        TextBoxVisibility = Visibility.Collapsed;
                        ComboBoxVisibility = Visibility.Visible;
                        CheckboxVisibility = Visibility.Collapsed;
                        break;
                    case ValueDataType.Bool:
                        TextBoxVisibility = Visibility.Collapsed;
                        ComboBoxVisibility = Visibility.Collapsed;
                        CheckboxVisibility = Visibility.Visible;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private string _text;

        [ObserveForDirty]
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        public object Value
        {
            get
            {
                switch (DataType)
                {
                    case ValueDataType.Text:
                        return Text;

                    case ValueDataType.Enumeration:
                        if (SelectedItemIndex < 0 || SelectedItemIndex >= EnumValues.Count)
                        {
                            return "";
                        }
                        return EnumValues[SelectedItemIndex];

                    case ValueDataType.Bool:
                        return BoolValue;

                    default:
                        return "error " + DataType.ToString();
                }
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

        private Visibility _textBoxVisibility;

        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
            private set
            {
                _textBoxVisibility = value;
                OnPropertyChanged("TextBoxVisibility");
            }
        }

        private Visibility _comboBoxVisibility;

        public Visibility ComboBoxVisibility
        {
            get { return _comboBoxVisibility; }
            private set
            {
                _comboBoxVisibility = value;
                OnPropertyChanged("ComboBoxVisibility");
            }
        }


        private bool _boolValue;

        [ObserveForDirty]
        public bool BoolValue
        {
            get { return _boolValue; }
            set
            {
                _boolValue = value;
                OnPropertyChanged("BoolValue");
            }
        }


        private Visibility _checkboxVisibility;

        public Visibility CheckboxVisibility
        {
            get { return _checkboxVisibility; }
            set
            {
                _checkboxVisibility = value;
                OnPropertyChanged("CheckboxVisibility");
            }
        }


        private ObservableCollection<EnumValue> _enumValues;

        [ObserveForDirty]
        public ObservableCollection<EnumValue> EnumValues
        {
            get { return _enumValues; }
            set
            {
                _enumValues = value;
                OnPropertyChanged("EnumValues");
                SetBackground();
            }
        }


        private int _selectedItemIndex;

        [ObserveForDirty]
        public int SelectedItemIndex
        {
            get { return _selectedItemIndex; }
            set
            {
                _selectedItemIndex = value;
                OnPropertyChanged("SelectedItemIndex");
                SetBackground();
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

        private void SetBackground()
        {
            Brush brush = Brushes.White;

            if (DataType == ValueDataType.Enumeration)
            {
                EnumValue ev = Value as EnumValue;
                if (ev != null)
                {
                    if (ev.IsInvalid)
                    {
                        Background.BackgroundColor = BackgroundColor.Red;
                    }
                    else if (ev.DontCare)
                    {
                        Background.BackgroundColor = BackgroundColor.Aqua;
                    }
                    else
                    {
                        Background.BackgroundColor = BackgroundColor.White;
                    }
                }
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


        #region test case calculation

        public ValueObjectSettingOption SettingOption { get; private set; }

        public bool TestSettingIsEqual(ValueObject other)
        {
            //TODO
            //if (this.EnumValues.Equals(other.EnumValues))
            //{
            //    throw new ArgumentException("the two ValueObjects do not have the same enumeration");
            //}
            return SettingOption == other.SettingOption && SelectedItemIndex == other.SelectedItemIndex;
        }

        public bool IsInvalid
        {
            get { return EnumValues[SelectedItemIndex].IsInvalid; }
        }

        public EnumValue SelectedValue
        {
            get { return EnumValues[SelectedItemIndex]; }
        }

        public int PossibleValues
        {
            get
            {
                int count = 0;
                foreach (EnumValue value in EnumValues)
                {
                    if (!value.IsInvalid && !value.DontCare)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        #endregion


        public override string ToString()
        {
            return Value.ToString();
        }

        public string ToTestString()
        {
            if (Value is EnumValue)
            {
                return ((EnumValue)Value).ToTestString();
            }
            return Value.ToString();
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

        protected void OnPropertyChanged(string name)
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

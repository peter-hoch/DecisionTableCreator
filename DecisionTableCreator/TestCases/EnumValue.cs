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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    /// <summary>
    /// this clas represents a possible value for a condition or action
    /// DontCare==true and IsInvalid==true is not a possible combination 
    /// </summary>
    public class EnumValue : INotifyDirtyChanged, IEnumValue
    {
        public EnumValue(string name, string value, bool isInavlid = false, bool dontCare = false, bool isDefault = false, string tooltipText = null)
        {
            DirtyObserver = new DirtyObserver(this);
            Name = name;
            Background = new Background(BackgroundColor.White);
            IsInvalid = isInavlid;
            DontCare = dontCare;
            IsDefault = isDefault;
            Value = value;
            TooltipText = tooltipText;
        }

        public EnumValue(string name, bool isInavlid = false, bool dontCare = false, bool isDefault = false, string tooltipText = null) : this(name, "value-" + name, isInavlid, dontCare, isDefault, tooltipText)
        {
        }

        private bool _isDefault;

        [ObserveForDirty]
        public bool IsDefault
        {
            get { return _isDefault; }
            set
            {
                _isDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }


        private bool _isInvalid;

        [ObserveForDirty]
        public bool IsInvalid
        {
            get { return _isInvalid; }
            set
            {
                _isInvalid = value;
                OnPropertyChanged("IsInvalid");
                if (_isInvalid && _dontCare)
                {
                    _dontCare = false;
                    OnPropertyChanged("DontCare");
                }
                SetBackground();
            }
        }

        private bool _dontCare;

        [ObserveForDirty]
        public bool DontCare
        {
            get { return _dontCare; }
            set
            {
                _dontCare = value;
                OnPropertyChanged("DontCare");
                if (_isInvalid && _dontCare)
                {
                    _isInvalid = false;
                    OnPropertyChanged("IsInvalid");
                }
                SetBackground();
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

        private String _value;

        [ObserveForDirty]
        public String Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
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

        public string Comment { get { return _tooltipText; } }


        public override string ToString()
        {
            return Name;
        }

        public string ToTestString()
        {
            return string.Format("{0} {1} {2}", Name, IsInvalid ? "I" : " ", DontCare ? "D" : " ");
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

        private void SetBackground()
        {
            if (IsInvalid)
            {
                Background.BackgroundColor = BackgroundColor.Red;
            }
            else if (DontCare)
            {
                Background.BackgroundColor = BackgroundColor.Aqua;
            }
            else
            {
                Background.BackgroundColor = BackgroundColor.White;
            }
        }


        private Background _background;

        public Background Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged("Background");
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


        public EnumValue Clone()
        {
            return new EnumValue(Name, Value, IsInvalid, DontCare, IsDefault, TooltipText);
        }
    }
}

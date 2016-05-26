using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    /// <summary>
    /// this clas represents a possible value for a condition or action
    /// DontCare==true and IsInvalid==true is not a possible combination 
    /// </summary>
    public class EnumValue : INotifyDirtyChanged
    {
        public EnumValue(string name, string value, bool isInavlid = false, bool dontCare = false, bool isDefault = false)
        {
            DirtyObserver = new DirtyObserver(this);
            Name = name;
            IsInvalid = isInavlid;
            DontCare = dontCare;
            IsDefault = isDefault;
            Value = value;
        }

        public EnumValue(string name, bool isInavlid = false, bool dontCare = false, bool isDefault = false) : this(name, "value-"+name, isInavlid, dontCare, isDefault)
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
            return new EnumValue(Name, Value, IsInvalid, DontCare, IsDefault);
        }
    }
}

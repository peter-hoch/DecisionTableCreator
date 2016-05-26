using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.Utils;

namespace UnitTests2.DirtyObserverTestFiles
{
    public class DataContainerTest1 : INotifyDirtyChanged
    {
        private int _value1;

        public int Value1
        {
            get { return _value1; }
            set
            {
                _value1 = value;
                OnPropertyChanged("Value1");
            }
        }


        private int _value2;

        [ObserveForDirty]
        public int Value2
        {
            get { return _value2; }
            set
            {
                _value2 = value;
                OnPropertyChanged("Value2");
            }
        }

        private object _testObject;

        [ObserveForDirty]
        public object TestObject
        {
            get { return _testObject; }
            set
            {
                _testObject = value;
                OnPropertyChanged("TestObject");
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

        public void ResetDirty()
        {
            throw new NotImplementedException();
        }

        public void FireDirtyChanged()
        {
            
        }

        #endregion
    }
}

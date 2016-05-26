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
    public class DataContainerTest2 : INotifyDirtyChanged
    {
        public DataContainerTest2()
        {
            DirtyObserver = new DirtyObserver(this);
        }


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

        private ObservableCollection<DataContainerTest2> _children;

        [ObserveForDirty]
        public ObservableCollection<DataContainerTest2> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                OnPropertyChanged("Children");
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


        private DataContainerTest2 _container1;

        [ObserveForDirty]
        public DataContainerTest2 Container1
        {
            get { return _container1; }
            set
            {
                _container1 = value;
                OnPropertyChanged("Container1");
            }
        }


        private DataContainerTest2 _container2;

        [ObserveForDirty]
        public DataContainerTest2 Container2
        {
            get { return _container2; }
            set
            {
                _container2 = value;
                OnPropertyChanged("Container2");
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

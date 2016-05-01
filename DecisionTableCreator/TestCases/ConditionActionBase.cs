using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    /// <summary>
    /// base class for condition and action
    /// </summary>
    public class ConditionActionBase : INotifyPropertyChanged
    {
        private String _name;

        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
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

    public class ConditionObject : ConditionActionBase
    {
    }

    public class ActionObject : ConditionActionBase
    {
    }

}

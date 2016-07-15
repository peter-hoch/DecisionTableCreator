using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator
{
    public class AboutBoxDataContainer : INotifyPropertyChanged
    {
        public AboutBoxDataContainer()
        {
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            _version = String.Format("Version {0}.{1}", ver.Major, ver.Minor);
        }

        private string _version;

        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChanged("Version");
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
}

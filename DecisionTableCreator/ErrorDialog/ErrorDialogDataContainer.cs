using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.StringTemplate.Misc;
using DecisionTableCreator.TestCases;

namespace DecisionTableCreator.ErrorDialog
{
    public class ErrorDialogDataContainer : INotifyPropertyChanged
    {
        private int _errorCount;

        public int ErrorCount
        {
            get { return _errorCount; }
            set
            {
                _errorCount = value;
                OnPropertyChanged("ErrorCount");
            }
        }


        private ObservableCollection<TemplateMessageWrapper> _errorMessages;

        public ObservableCollection<TemplateMessageWrapper> ErrorMessages
        {
            get { return _errorMessages; }
            set
            {
                _errorMessages = value;
                OnPropertyChanged("ErrorMessages");
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

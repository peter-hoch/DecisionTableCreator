using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator
{
    public class DataGridHeader : INotifyPropertyChanged
    {
        public DataGridHeader(string name, int index)
        {
            Name = name;
            ColumnIndex = index;
        }

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

        private int _columnIndex;

        public int ColumnIndex
        {
            get { return _columnIndex; }
            set
            {
                _columnIndex = value;
                OnPropertyChanged("ColumnIndex");
            }
        }


        public override string ToString()
        {
            return Name;
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

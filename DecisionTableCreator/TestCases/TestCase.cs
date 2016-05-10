using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{

    public class TestCase
    {
        public enum CollectionType
        {
            Conditions,
            Actions    
        }

        public TestCase(string name, ValueObject[] conditions, ValueObject[] actions)
        {
            Name = name;
            Conditions = new ObservableCollection<ValueObject>(conditions);
            Actions = new ObservableCollection<ValueObject>(actions);
        }

        public TestCase(string name)
        {
            Name = name;
            Conditions = new ObservableCollection<ValueObject>();
            Actions = new ObservableCollection<ValueObject>();
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


        public ValueObject GetValueObject(CollectionType type, int index)
        {
            switch (type)
            {
                case CollectionType.Conditions:
                    return Conditions[index];
                    
                case CollectionType.Actions:
                    return Actions[index];

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void AddValueObject(CollectionType type, ValueObject value)
        {
            switch (type)
            {
                case CollectionType.Conditions:
                    Conditions.Add(value);
                    break;
                case CollectionType.Actions:
                    Actions.Add(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private ObservableCollection<ValueObject> _conditions;

        public ObservableCollection<ValueObject> Conditions
        {
            get { return _conditions; }
            set
            {
                _conditions = value;
                OnPropertyChanged("ConditionTable");
            }
        }

        private ObservableCollection<ValueObject> _actions;

        public ObservableCollection<ValueObject> Actions
        {
            get { return _actions; }
            set
            {
                _actions = value;
                OnPropertyChanged("ActionTable");
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

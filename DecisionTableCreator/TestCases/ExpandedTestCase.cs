using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    public class ExpandedTestCase : ITestCase
    {
        public string Name { get; private set; }

        public ObservableCollection<ValueObject> Conditions { get; private set; }
        public ObservableCollection<ValueObject> Actions { get; private set; }

        public ExpandedTestCase(string name)
        {
            Name = name;
            Conditions = new ObservableCollection<ValueObject>();
            Actions = new ObservableCollection<ValueObject>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0,10} C", Name);
            foreach (ValueObject condition in Conditions)
            {
                sb.AppendFormat(" {0,4}", condition.Value);
            }
            sb.Append(" - A");
            foreach (ValueObject action in Actions)
            {
                sb.AppendFormat(" {0,4}", action.Value);
            }
            return sb.ToString();
        }
    }
}

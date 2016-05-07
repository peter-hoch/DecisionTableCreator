using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DecisionTableCreator
{
    public class WpfCommands
    {
        public static readonly RoutedUICommand AppendColumn;
        public static readonly RoutedUICommand EditCondition;

        static WpfCommands()
        {
            AppendColumn = new RoutedUICommand("AppendColumn", "AppendColumn", typeof(WpfCommands));
            EditCondition = new RoutedUICommand("EditCondition", "EditCondition", typeof(WpfCommands));
        }
    }
}

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
        public static readonly RoutedUICommand EditAction;
        public static readonly RoutedUICommand Save;

        static WpfCommands()
        {
            AppendColumn = new RoutedUICommand("Append test case", "AppendColumn", typeof(WpfCommands));
            EditCondition = new RoutedUICommand("Edit condition", "EditCondition", typeof(WpfCommands));
            EditAction = new RoutedUICommand("Edit action", "EditAction", typeof(WpfCommands));
            Save = new RoutedUICommand("Save", "Save", typeof(WpfCommands));
        }
    }
}

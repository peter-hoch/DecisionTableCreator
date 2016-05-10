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
        public static readonly RoutedUICommand Open;
        public static readonly RoutedUICommand NewDocument;
        public static readonly RoutedUICommand AppendCondition;
        public static readonly RoutedUICommand AppendAction;
        public static readonly RoutedUICommand AppendEnumValue;
        public static readonly RoutedUICommand DeleteEnumValue;

        static WpfCommands()
        {
            AppendColumn = new RoutedUICommand("Append test case", "AppendColumn", typeof(WpfCommands));
            EditCondition = new RoutedUICommand("Edit condition", "EditCondition", typeof(WpfCommands));
            EditAction = new RoutedUICommand("Edit action", "EditAction", typeof(WpfCommands));
            Save = new RoutedUICommand("Save", "Save", typeof(WpfCommands));
            Open = new RoutedUICommand("Open", "Open", typeof(WpfCommands));
            NewDocument = new RoutedUICommand("New", "NewDocument", typeof(WpfCommands));
            AppendCondition = new RoutedUICommand("Append condition", "AppendCondition", typeof(WpfCommands));
            AppendAction = new RoutedUICommand("Append action", "AppendAction", typeof(WpfCommands));
            AppendEnumValue = new RoutedUICommand("Append enum value", "AppendEnumValue", typeof(WpfCommands));
            DeleteEnumValue = new RoutedUICommand("Delete enum value", "DeleteEnumValue", typeof(WpfCommands));
        }
    }
}

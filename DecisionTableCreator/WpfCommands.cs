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
        public static readonly RoutedUICommand AppendTestCase;
        public static readonly RoutedUICommand InsertTestCase;
        public static readonly RoutedUICommand EditCondition;
        public static readonly RoutedUICommand EditAction;
        public static readonly RoutedUICommand Save;
        public static readonly RoutedUICommand Open;
        public static readonly RoutedUICommand NewDocument;
        public static readonly RoutedUICommand AppendCondition;
        public static readonly RoutedUICommand InsertCondition;
        public static readonly RoutedUICommand AppendAction;
        public static readonly RoutedUICommand InsertAction;
        public static readonly RoutedUICommand AppendEnumValue;
        public static readonly RoutedUICommand DeleteEnumValue;

        static WpfCommands()
        {
            AppendTestCase = new RoutedUICommand("Append test case", "AppendTestCase", typeof(WpfCommands));
            InsertTestCase = new RoutedUICommand("Insert test case", "InsertTestCase", typeof(WpfCommands));
            EditCondition = new RoutedUICommand("Edit condition", "EditCondition", typeof(WpfCommands));
            EditAction = new RoutedUICommand("Edit action", "EditAction", typeof(WpfCommands));
            Save = new RoutedUICommand("Save", "Save", typeof(WpfCommands));
            Open = new RoutedUICommand("Open", "Open", typeof(WpfCommands));
            NewDocument = new RoutedUICommand("New", "NewDocument", typeof(WpfCommands));
            AppendCondition = new RoutedUICommand("Append condition", "AppendCondition", typeof(WpfCommands));
            InsertCondition = new RoutedUICommand("Insert condition", "InsertCondition", typeof(WpfCommands));
            AppendAction = new RoutedUICommand("Append action", "AppendAction", typeof(WpfCommands));
            InsertAction = new RoutedUICommand("Insert action", "InsertAction", typeof(WpfCommands));
            AppendEnumValue = new RoutedUICommand("Append enum value", "AppendEnumValue", typeof(WpfCommands));
            DeleteEnumValue = new RoutedUICommand("Delete enum value", "DeleteEnumValue", typeof(WpfCommands));
        }
    }
}

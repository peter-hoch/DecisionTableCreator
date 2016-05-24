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
        public static readonly RoutedUICommand DeleteCondition;
        public static readonly RoutedUICommand EditAction;
        public static readonly RoutedUICommand EditConditionOrAction;
        public static readonly RoutedUICommand DeleteAction;
        public static readonly RoutedUICommand Save;
        public static readonly RoutedUICommand Open;
        public static readonly RoutedUICommand NewDocument;
        public static readonly RoutedUICommand AppendCondition;
        public static readonly RoutedUICommand InsertCondition;
        public static readonly RoutedUICommand AppendAction;
        public static readonly RoutedUICommand InsertAction;
        public static readonly RoutedUICommand AppendEnumValue;
        public static readonly RoutedUICommand DeleteEnumValue;
        public static readonly RoutedUICommand ExportHtmlToClipboard;
        public static readonly RoutedUICommand MoveConditionOrActionUp;
        public static readonly RoutedUICommand MoveConditionOrActionDown;
        public static readonly RoutedUICommand ExportToFileWithExternalTemplate;

        static WpfCommands()
        {
            KeyGesture gestureAltC = new KeyGesture(Key.C, ModifierKeys.Alt);
            KeyGesture gestureAltA = new KeyGesture(Key.A, ModifierKeys.Alt);
            KeyGesture gestureAltT = new KeyGesture(Key.T, ModifierKeys.Alt);
            AppendTestCase = new RoutedUICommand("Append test case", "AppendTestCase", typeof(WpfCommands), new InputGestureCollection() { gestureAltT });
            InsertTestCase = new RoutedUICommand("Insert test case", "InsertTestCase", typeof(WpfCommands));
            EditCondition = new RoutedUICommand("Edit condition", "EditCondition", typeof(WpfCommands));
            EditConditionOrAction = new RoutedUICommand("Edit condition or action", "EditConditionOrAction", typeof(WpfCommands));
            DeleteCondition = new RoutedUICommand("Delete condition", "DeleteCondition", typeof(WpfCommands));
            EditAction = new RoutedUICommand("Edit action", "EditAction", typeof(WpfCommands));
            DeleteAction = new RoutedUICommand("Delete action", "DeleteAction", typeof(WpfCommands));
            Save = new RoutedUICommand("Save", "Save", typeof(WpfCommands));
            Open = new RoutedUICommand("Open", "Open", typeof(WpfCommands));
            NewDocument = new RoutedUICommand("New", "NewDocument", typeof(WpfCommands));
            AppendCondition = new RoutedUICommand("Append condition", "AppendCondition", typeof(WpfCommands), new InputGestureCollection() { gestureAltC});
            InsertCondition = new RoutedUICommand("Insert condition", "InsertCondition", typeof(WpfCommands));
            AppendAction = new RoutedUICommand("Append action", "AppendAction", typeof(WpfCommands), new InputGestureCollection() { gestureAltA });
            InsertAction = new RoutedUICommand("Insert action", "InsertAction", typeof(WpfCommands));
            AppendEnumValue = new RoutedUICommand("Append enum value", "AppendEnumValue", typeof(WpfCommands));
            DeleteEnumValue = new RoutedUICommand("Delete enum value", "DeleteEnumValue", typeof(WpfCommands));
            ExportHtmlToClipboard = new RoutedUICommand("Export HTML to clipboard", "ExportHtmlToClipboard", typeof(WpfCommands));
            MoveConditionOrActionUp = new RoutedUICommand("Move up", "MoveConditionOrActionUp", typeof(WpfCommands));
            MoveConditionOrActionDown = new RoutedUICommand("Move down", "MoveConditionOrActionDown", typeof(WpfCommands));
            ExportToFileWithExternalTemplate = new RoutedUICommand("Export to file with external template", "ExportToFileWithExternalTemplate", typeof(WpfCommands));
        }
    }
}

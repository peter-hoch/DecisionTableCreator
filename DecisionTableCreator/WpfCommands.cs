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
        public static readonly RoutedUICommand InsertColumn;

        static WpfCommands()
        {
            InsertColumn = new RoutedUICommand("InsertColumn", "InsertColumn", typeof(WpfCommands));
        }
    }
}

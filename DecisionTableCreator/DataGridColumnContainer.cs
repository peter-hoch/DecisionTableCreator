using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DecisionTableCreator
{
    public class DataGridColumnContainer
    {
        public DataGridColumnContainer(DataGrid dataGrid, DataGridColumn column)
        {
            DataGrid = dataGrid;
            Column = column;
        }

        public DataGrid DataGrid { get; private set; }

        public DataGridColumn Column { get; private set; }
    }
}

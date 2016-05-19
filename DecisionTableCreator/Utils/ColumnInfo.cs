using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.Utils
{
    /// <summary>
    /// container for column header and display index for reordering
    /// </summary>
    public class ColumnInfo
    {
        public int DisplayIndex { get; private set; }

        public object Header { get; set; }

        public ColumnInfo(int displayIndex, object header)
        {
            DisplayIndex = displayIndex;
            Header = header;
        }
    }
}

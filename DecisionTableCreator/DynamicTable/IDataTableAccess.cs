using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.DynamicTable
{
    public interface IDataTableAccess
    {
        DataRowViewCollection Rows { get; }

        ColumnPropertyDescriptionCollection ColumnPropDescColl { get; }
    }
}

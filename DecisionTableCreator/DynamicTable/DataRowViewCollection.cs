using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DecisionTableCreator.DynamicTable
{

    /// <summary>
    /// this class represents a collection of rows.
    /// </summary>
    public class DataRowViewCollection
        : List<DataRowView>, ITypedList
    {
        private readonly DataTableView _parent;

        public DataRowViewCollection(DataTableView parent)
            : base()
        {
            _parent = parent;
        }

        public DataTableView ParentTable { get { return _parent; } }

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            return _parent.Columns;
        }

        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            return null;
        }

        public DataRowView AddRow()
        {
            DataRowView row = new DataRowView(this, _parent.Columns.Count);
            Add(row);
            return row;
        }

        internal void ResizeColumnCount(int columnCount)
        {
            foreach (DataRowView view in this)
            {
                view.ResizeColumnCount(columnCount);
            }
        }



    }
}

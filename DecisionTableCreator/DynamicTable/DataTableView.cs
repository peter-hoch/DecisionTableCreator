using System.Collections.Generic;
using System.ComponentModel;

namespace DecisionTableCreator.DynamicTable
{
    /// <summary>
    /// this class represents the whole table for the DataGrid (conditions or actions)
    /// </summary>
    public class DataTableView : IListSource, IDataTableAccess
    {
        private List<DataRowView> _rows = new List<DataRowView>();
        private readonly DataRowViewCollection _dataRows;
        private readonly ColumnPropertyDescriptionCollection _columns = new ColumnPropertyDescriptionCollection();

        public DataTableView()
        {
            _dataRows = new DataRowViewCollection(this);
        }


        public DataRowViewCollection Rows { get { return _dataRows; } }

        public PropertyDescriptorCollection Columns { get { return _columns.DescriptorCollection; } }

        public ColumnPropertyDescriptionCollection ColumnPropDescColl { get {return _columns;} }

        /// <summary>
        /// Returns the underlying collection of elements for this source.
        /// </summary>
        System.Collections.IList IListSource.GetList()
        {
            return _dataRows;
        }

        bool IListSource.ContainsListCollection
        {
            get { return false; }
        }
    }
}

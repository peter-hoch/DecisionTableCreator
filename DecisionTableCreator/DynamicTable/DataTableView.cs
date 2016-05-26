/*
 * [The "BSD license"]
 * Copyright (c) 2016 Peter Hoch
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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

        internal void ResizeColumnCount(int columnCount)
        {
            _dataRows.ResizeColumnCount(columnCount);
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

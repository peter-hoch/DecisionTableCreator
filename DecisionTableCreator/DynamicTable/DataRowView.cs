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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DecisionTableCreator.DynamicTable
{
    /// <summary>
    /// this class repesents a row with all properties descripted by ColumnPropertyDescriptor
    /// </summary>
    public class DataRowView : ICustomTypeDescriptor, INotifyPropertyChanged
    {
        private readonly DataRowViewCollection _parentCollection;
        private object[] _columns;

        internal DataRowView(DataRowViewCollection parentCollection, int columnCount)
        {
            _columns = new object[columnCount];
            _parentCollection = parentCollection;
        }

        internal void ResizeColumnCount(int columnCount)
        {
            _columns = new object[columnCount];
        }

        public int ColumnCount { get { return _columns.Length; } }

        public object this[string name]
        {
            get { return _columns[ColumnIndexFromName(name)]; }
            set
            {
                _columns[ColumnIndexFromName(name)] = value;
                OnPropertyChanged(name);
            }
        }

        // only for testing purposes
        public object this[int index]
        {
            get { return _columns[index]; }
        }

        public void SetValue(int index, object value)
        {
            _columns[index] = value;
        }

        private int ColumnIndexFromName(string name)
        {
            PropertyDescriptor pd = _parentCollection.ParentTable.Columns[name];
            if (pd != null)
            {
                return _parentCollection.ParentTable.Columns.IndexOf(pd);
            }
            return -1;
        }

        internal void AppendColumn(object defaultValue)
        {
            List<object> newColumns = new List<object>(_columns);
            newColumns.Add(defaultValue);
            _columns = newColumns.ToArray();
        }


        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return typeof (DataRowView).FullName;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return typeof (DataRowView).Name;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return null;
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return EventDescriptorCollection.Empty;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return ((ITypedList) _parentCollection).GetItemProperties(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ITypedList) _parentCollection).GetItemProperties(null);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #region event

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(name);
                PropertyChanged(this, args);
            }
        }

        #endregion

    }
}


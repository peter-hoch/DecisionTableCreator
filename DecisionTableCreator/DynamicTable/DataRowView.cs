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

        public object this[string name]
        {
            get { return _columns[ColumnIndexFromName(name)]; }
            set
            {
                _columns[ColumnIndexFromName(name)] = value;
                OnPropertyChanged(name);
            }
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


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.DynamicTable
{
    /// <summary>
    /// this class represents a collection of property desciptions
    /// </summary>
    public class ColumnPropertyDescriptionCollection
    {
        private List<PropertyDescriptor> _collection = new List<PropertyDescriptor>();

        public ColumnPropertyDescriptionCollection()
        {
            DescriptorCollection = new PropertyDescriptorCollection(null);
        }

        public PropertyDescriptorCollection DescriptorCollection { get; private set; }

        public int Count
        {
            get { return DescriptorCollection.Count; }
        }

        internal void AddDescription(ColumnPropertyDescriptor columnPropertyDescriptor)
        {
            PropertyDescriptor propDesc = _collection.FirstOrDefault(pd=>pd.Name.Equals(columnPropertyDescriptor.Name));
            if (propDesc != null)
            {
                throw new Exception("property name is not unique");
            }
            _collection.Add(columnPropertyDescriptor);
            DescriptorCollection = new PropertyDescriptorCollection(_collection.ToArray(), true);
        }

    }
}


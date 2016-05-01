using System;
using System.ComponentModel;

namespace DecisionTableCreator.DynamicTable
{
    /// <summary>
    /// The class that represents the property description of a column
    /// </summary>
    public class ColumnPropertyDescriptor : PropertyDescriptor
    {
        private readonly Type _propertyType;

        public ColumnPropertyDescriptor(string name, Type type, Attribute[] attrs) : base(name, attrs)
        {
            _propertyType = type;
        }

        public override object GetValue(object component)
        {
            return ((DataRowView)component)[this.Name];
        }

        public override void SetValue(object component, object value)
        {
            ((DataRowView)component)[this.Name] = value;
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return _propertyType; }
        }

        public override Type ComponentType
        {
            get { return typeof(DataRowView); }
        }

        public override void ResetValue(object component)
        {
            throw new NotSupportedException();
        }

        public override bool CanResetValue(object component) => false;

        public override bool ShouldSerializeValue(object component) => true;
    }

}


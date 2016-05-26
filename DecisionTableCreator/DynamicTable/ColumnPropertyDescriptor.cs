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


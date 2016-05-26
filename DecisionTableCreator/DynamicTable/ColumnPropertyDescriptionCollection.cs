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
            PropertyDescriptor propDesc = _collection.FirstOrDefault(pd => pd.Name.Equals(columnPropertyDescriptor.Name));
            if (propDesc != null)
            {
                throw new Exception("property name is not unique");
            }
            _collection.Add(columnPropertyDescriptor);
            DescriptorCollection = new PropertyDescriptorCollection(_collection.ToArray(), true);
        }

        /// <summary>
        /// remove the test case property description
        /// first column is condition or action column
        /// --> index+1 == index for test case
        /// </summary>
        /// <param name="testCaseIndex"></param>
        internal void RemoveDescription(int testCaseIndex)
        { 
            _collection.RemoveAt(testCaseIndex + 1);
            DescriptorCollection = new PropertyDescriptorCollection(_collection.ToArray(), true);
        }

    }
}


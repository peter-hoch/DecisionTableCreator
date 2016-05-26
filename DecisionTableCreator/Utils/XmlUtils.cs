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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DecisionTableCreator.Utils
{
    public enum XmlElementOption
    {
        Optional,
        MustExist,
        MustHaveValue
    }
    public static class XmlUtils
    {
        public static bool MustExist(XmlElementOption option)
        {
            return option == XmlElementOption.MustExist || option == XmlElementOption.MustHaveValue;
        }

        public static bool MustHaveValue(XmlElementOption option)
        {
            return option == XmlElementOption.MustHaveValue;
        }


        public static XmlElement AddElement(this XmlElement me, string name)
        {
            var child = me.OwnerDocument.CreateElement(name);
            me.AppendChild(child);
            return child;
        }

        /// <summary>
        /// add attribute if value is not null or empty
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlElement AddAttribute(this XmlElement parent, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var attrib = parent.OwnerDocument.CreateAttribute(name);
                attrib.Value = value;
                parent.Attributes.Append(attrib);
            }
            return parent;
        }

        /// <summary>
        /// add attribute
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static XmlElement AddAttribute(this XmlElement parent, string name, Enum enumValue)
        {
            var attrib = parent.OwnerDocument.CreateAttribute(name);
            attrib.Value = enumValue.ToString();
            parent.Attributes.Append(attrib);
            return parent;
        }

        /// <summary>
        /// add attribute 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlElement AddAttribute(this XmlElement parent, string name, bool value)
        {
            var attrib = parent.OwnerDocument.CreateAttribute(name);
            attrib.Value = value.ToString(CultureInfo.InvariantCulture);
            parent.Attributes.Append(attrib);
            return parent;
        }


        /// <summary>
        /// add attribute 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlElement AddAttribute(this XmlElement parent, string name, int value)
        {
            var attrib = parent.OwnerDocument.CreateAttribute(name);
            attrib.Value = value.ToString(CultureInfo.InvariantCulture);
            parent.Attributes.Append(attrib);
            return parent;
        }


        public static TEnumType GetAttributeEnumValue<TEnumType>(this XmlElement me, string name)
        {
            string value = me.GetAttribute(name);
            TEnumType convertedValue;
            try
            {
                convertedValue = (TEnumType)Enum.Parse(typeof(TEnumType), value);
            }
            catch (Exception ex)
            {
                throw new InvalidAttributeValueException(me, name, ex);
            }
            if (!Enum.IsDefined(typeof(TEnumType), convertedValue))
            {
                throw new InvalidAttributeValueException(me, name);
            }
            return convertedValue;
        }

        public static string GetAttributeStringValue(this XmlElement me, string name, XmlElementOption option = XmlElementOption.MustHaveValue)
        {
            XmlAttribute attrib = me.GetAttributeNode(name);
            if (attrib == null)
            {
                if (MustExist(option))
                {
                    throw new MissingAttributeException(me, name);
                }
                return null;
            }
            if (MustHaveValue(option) && string.IsNullOrEmpty(attrib.Value))
            {
                throw new InvalidAttributeValueException(me, name);
            }
            return attrib.Value;
        }

        public static int GetAttributeIntValue(this XmlElement me, string name, XmlElementOption option = XmlElementOption.MustHaveValue, int defaultValue = 0)
        {
            XmlAttribute attrib = me.GetAttributeNode(name);
            if (attrib == null)
            {
                if (MustExist(option))
                {
                    throw new MissingAttributeException(me, name);
                }
                return defaultValue;
            }
            if (MustHaveValue(option) && string.IsNullOrEmpty(attrib.Value))
            {
                throw new InvalidAttributeValueException(me, name);
            }
            if (string.IsNullOrEmpty(attrib.Value))
            {
                return defaultValue;
            }
            int value;
            if (!int.TryParse(attrib.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
            {
                throw new InvalidAttributeValueException(me, name);
            }
            return value;
        }

        public static bool GetAttributeBoolValue(this XmlElement me, string name, XmlElementOption option = XmlElementOption.MustHaveValue, bool defaultValue = false)
        {
            XmlAttribute attrib = me.GetAttributeNode(name);
            if (attrib == null)
            {
                if (MustExist(option))
                {
                    throw new MissingAttributeException(me, name);
                }
                return defaultValue;
            }
            if (MustHaveValue(option) && string.IsNullOrEmpty(attrib.Value))
            {
                throw new InvalidAttributeValueException(me, name);
            }
            if (string.IsNullOrEmpty(attrib.Value))
            {
                return defaultValue;
            }
            try
            {
                return bool.Parse(attrib.Value);
            }
            catch (Exception ex)
            {
                throw new InvalidAttributeValueException(me, name, ex);
            }
        }


    }

    public class XmlElementNotFoundException : Exception
    {
        public XmlElementNotFoundException(string name, Exception inner= null) 
            : base(String.Format("xml element {0} not found ", name ?? "null"), inner)
        {
        }
    }

    public class InvalidAttributeValueException : Exception
    {
        public InvalidAttributeValueException(XmlElement element, string attributeName, Exception inner = null)
            : base(String.Format("xml attribute {0} in element {1} is wrong", attributeName, element != null ? element.Name : "null"), inner)
        {
        }
    }

    public class MissingAttributeException : Exception
    {
        public MissingAttributeException(XmlElement element, string attributeName, Exception inner = null)
            : base(String.Format("xml attribute {0} in element {1} is missing", attributeName, element != null ? element.Name : "null"), inner)
        {
        }
    }

    public class InvalidObjectIdReferenceException : Exception
    {
        public InvalidObjectIdReferenceException(XmlElement element, string attributeName, int id, Exception inner = null)
            : base(String.Format("object id reference {0} in xml attribute {1} in element {2} is invalid", id, attributeName, element != null ? element.Name : "null"), inner)
        {
        }
    }


}

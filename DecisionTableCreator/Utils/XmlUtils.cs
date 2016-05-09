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
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlElement AddAttribute(this XmlElement parent, string name, bool value)
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
            : base(String.Format("xml attribute {0} in element {1} is wrong", element != null ? element.Name : "null", attributeName), inner)
        {
        }
    }

    public class MissingAttributeException : Exception
    {
        public MissingAttributeException(XmlElement element, string attributeName, Exception inner = null)
            : base(String.Format("xml attribute {0} in element {1} is missing", element != null ? element.Name : "null", attributeName), inner)
        {
        }
    }


}

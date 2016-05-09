using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DecisionTableCreator.TestCases;
using DecisionTableCreator.Utils;
using NUnit.Framework;

namespace UnitTests1
{
    [TestFixture]
    public class Conversions
    {
        [TestCase("<Test value='Text'/>", ValueDataType.Text)]
        [TestCase("<Test value='Enumeration'/>", ValueDataType.Enumeration)]
        public void ConvertEnum(string xml, ValueDataType expectedResult)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var result = doc.DocumentElement.GetAttributeEnumValue<ValueDataType>("value");
            Assert.That(result == expectedResult);
        }

        [TestCase("<Test value='10'/>")]
        [TestCase("<Test value='wrong'/>")]
        [TestCase("<Test />")]
        public void ConvertEnumNegative(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.That(() =>
            {
                var result = doc.DocumentElement.GetAttributeEnumValue<ValueDataType>("value");
            },
            Throws.TypeOf<InvalidAttributeValueException>());
        }

        [TestCase("<Test value='test string'/>", XmlElementOption.MustHaveValue, "test string")]
        [TestCase("<Test value='test string'/>", XmlElementOption.MustExist, "test string")]
        [TestCase("<Test value='test string'/>", XmlElementOption.Optional, "test string")]
        [TestCase("<Test value=''/>", XmlElementOption.MustExist, "")]
        [TestCase("<Test value=''/>", XmlElementOption.Optional, "")]
        [TestCase("<Test />", XmlElementOption.Optional, null)]
        public void GetAttributeStringValue(string xml, XmlElementOption option, string expectedResult)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string name = doc.DocumentElement.GetAttributeStringValue("value", option);
            if (expectedResult != null)
            {
                Assert.That(name.Equals(expectedResult));
            }
            else
            {
                Assert.That(name == null);
            }
        }

        [TestCase("<Test value=''/>", XmlElementOption.MustHaveValue, typeof(InvalidAttributeValueException))]
        [TestCase("<Test />", XmlElementOption.MustExist, typeof(MissingAttributeException))]
        public void GetAttributeStringValueNegative(string xml, XmlElementOption option, Type expectedException)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.That(() =>
            {
                string name = doc.DocumentElement.GetAttributeStringValue("value");
            },
            Throws.TypeOf(expectedException));
        }


        [TestCase("<Test value='true'/>", XmlElementOption.MustHaveValue, true)]
        [TestCase("<Test value='false'/>", XmlElementOption.MustExist, false)]
        [TestCase("<Test value='FALSE'/>", XmlElementOption.Optional, false)]
        [TestCase("<Test value=''/>", XmlElementOption.MustExist, false)]
        [TestCase("<Test value=''/>", XmlElementOption.Optional, false)]
        [TestCase("<Test />", XmlElementOption.Optional, false)]
        public void GetAttributeBoolValue(string xml, XmlElementOption option, bool expectedResult)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            bool value = doc.DocumentElement.GetAttributeBoolValue("value", option);
            Assert.That(value == expectedResult);
        }

        [TestCase("<Test value=''/>", XmlElementOption.MustHaveValue, typeof(InvalidAttributeValueException))]
        [TestCase("<Test />", XmlElementOption.MustExist, typeof(MissingAttributeException))]
        public void GetAttributeBoolValueNegative(string xml, XmlElementOption option, Type expectedException)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.That(() =>
            {
                bool value = doc.DocumentElement.GetAttributeBoolValue("value", option);
            },
            Throws.TypeOf(expectedException));
        }

    }

}

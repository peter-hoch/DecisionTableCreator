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

        [TestCase("<Test value='0'/>", XmlElementOption.MustHaveValue, 0, 0)]
        [TestCase("<Test value='-1'/>", XmlElementOption.MustExist, 0, -1)]
        [TestCase("<Test value='1'/>", XmlElementOption.Optional, 0, 1)]
        [TestCase("<Test value=''/>", XmlElementOption.MustExist, 0, 0)]
        [TestCase("<Test value=''/>", XmlElementOption.Optional, 0, 0)]
        [TestCase("<Test />", XmlElementOption.Optional, 0, 0)]
        [TestCase("<Test />", XmlElementOption.Optional, 5, 5)]
        public void GetAttributeIntValue(string xml, XmlElementOption option, int defaultValue, int expectedResult)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            int value = doc.DocumentElement.GetAttributeIntValue("value", option, defaultValue);
            Assert.That(value == expectedResult);
        }

        [TestCase("<Test value=''/>", XmlElementOption.MustHaveValue, typeof(InvalidAttributeValueException))]
        [TestCase("<Test />", XmlElementOption.MustExist, typeof(MissingAttributeException))]
        public void GetAttributeIntValueNegative(string xml, XmlElementOption option, Type expectedException)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.That(() =>
            {
                int value = doc.DocumentElement.GetAttributeIntValue("value", option);
            },
            Throws.TypeOf(expectedException));
        }

    }

}

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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    public class LoadSaveVisitor
    {
        private Dictionary<int, ConditionActionBase> ConditionActionIdDictionary { get; set; }

        public string FilePath { get; private set; }

        public LoadSaveVisitor(string filePath)
        {
            FilePath = filePath;
        }

        public void Save(TestCasesRoot testCasesRoot)
        {
            ConditionActionIdDictionary = new Dictionary<int, ConditionActionBase>();
            BuildIdDictionary(testCasesRoot);

            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
            var root = doc.CreateElement(XmlNames.DecisionTableRootName);
            doc.AppendChild(root);

            var xmlConditions = root.AddElement(XmlNames.ConditionsName);
            foreach (ConditionObject condition in testCasesRoot.Conditions)
            {
                Save(xmlConditions, condition);
            }

            var xmlActions = root.AddElement(XmlNames.ActionsName);
            foreach (ActionObject action in testCasesRoot.Actions)
            {
                Save(xmlActions, action);
            }

            var xmlTestCases = root.AddElement(XmlNames.TestCasesName);
            foreach (TestCase testCase in testCasesRoot.TestCases.OrderBy(tc=>tc.DisplayIndex))
            {
                Save(xmlTestCases, testCase);
            }

            doc.Save(FilePath);

        }

        private void BuildIdDictionary(TestCasesRoot testCasesRoot)
        {
            int id = 1;
            foreach (ConditionObject condition in testCasesRoot.Conditions)
            {
                condition.LoadSaveId = id;
                ConditionActionIdDictionary.Add(id++, condition);
            }

            if (id < 1000)
            {
                id = 1000;
            }

            foreach (ActionObject action in testCasesRoot.Actions)
            {
                action.LoadSaveId = id;
                ConditionActionIdDictionary.Add(id++, action);
            }
        }

        int GetId(ConditionActionBase obj)
        {
            var result = ConditionActionIdDictionary.Where(val => val.Value.Equals(obj));
            if (result.Count() != 1)
            {
                throw new Exception("internal error");
            }
            return result.First().Key;
        }

        private void Save(XmlElement parent, ConditionObject condition)
        {
            var xmlCondition = parent.AddElement(XmlNames.ConditionName).AddAttribute(XmlNames.NameAttributeName, condition.Name).AddAttribute(XmlNames.TypeAttributeName, condition.DataType.ToString());
            xmlCondition.AddAttribute(XmlNames.IdAttributeName, condition.LoadSaveId);
            xmlCondition.AddAttribute(XmlNames.DefaultTextAttributeName, condition.DefaultText);
            xmlCondition.AddAttribute(XmlNames.DefaultBoolAttributeName, condition.DefaultBool);
            if (condition.EnumValues != null && condition.EnumValues.Count != 0)
            {
                var xmlEnumValues = xmlCondition.AddElement(XmlNames.EnumValuesName);
                foreach (EnumValue value in condition.EnumValues)
                {
                    Save(xmlEnumValues, value);
                }
            }
        }

        private void Save(XmlElement xmlTestCases, TestCase testCase)
        {
            var xmlTestCase = xmlTestCases.AddElement(XmlNames.TestCaseName);
            xmlTestCase.AddAttribute(XmlNames.NameAttributeName, testCase.Name);

            var xmlConditions = xmlTestCase.AddElement(XmlNames.TestCaseConditionsName);
            foreach (ValueObject condValueObject in testCase.Conditions)
            {
               Save(xmlConditions, condValueObject);
            }

            var xmlActions = xmlTestCase.AddElement(XmlNames.TestCaseActionsName);
            foreach (ValueObject actionValueObject in testCase.Actions)
            {
               Save(xmlActions, actionValueObject);
            }
        }

        private void Save(XmlElement xmlParent, ValueObject valueObject)
        {
            var xmlValueElement = xmlParent.AddElement(XmlNames.TestCaseValueName);
            xmlValueElement.AddAttribute(XmlNames.IdAttributeName, valueObject.ConditionOrActionParent.LoadSaveId);
            xmlValueElement.AddAttribute(XmlNames.TypeAttributeName, valueObject.DataType);
            xmlValueElement.AddAttribute(XmlNames.BoolAttributeName, valueObject.BoolValue);
            xmlValueElement.AddAttribute(XmlNames.SelectedItemIndexAttributeName, valueObject.SelectedItemIndex);
            xmlValueElement.AddAttribute(XmlNames.TextAttributeName, valueObject.Text);
        }

        private void Save(XmlElement parent, ActionObject action)
        {
            var xmlCondition = parent.AddElement(XmlNames.ActionName);
            xmlCondition.AddAttribute(XmlNames.IdAttributeName, action.LoadSaveId);
            xmlCondition.AddAttribute(XmlNames.NameAttributeName, action.Name);
            xmlCondition.AddAttribute(XmlNames.TypeAttributeName, action.DataType.ToString());
            xmlCondition.AddAttribute(XmlNames.DefaultTextAttributeName, action.DefaultText);
            xmlCondition.AddAttribute(XmlNames.DefaultBoolAttributeName, action.DefaultBool);
            if (action.EnumValues != null && action.EnumValues.Count != 0)
            {
                var xmlEnumValues = xmlCondition.AddElement(XmlNames.EnumValuesName);
                foreach (EnumValue value in action.EnumValues)
                {
                    Save(xmlEnumValues, value);
                }
            }
        }

        private void Save(XmlElement parent, EnumValue value)
        {
            var xmlEnumValue = parent.AddElement(XmlNames.EnumValueName);
            xmlEnumValue.AddAttribute(XmlNames.NameAttributeName, value.Name);
            xmlEnumValue.AddAttribute(XmlNames.ValueAttributeName, value.Value);
            xmlEnumValue.AddAttribute(XmlNames.IsDefaultAttributeName, value.IsDefault);
            xmlEnumValue.AddAttribute(XmlNames.IsInvalidAttributeName, value.IsInvalid);
            xmlEnumValue.AddAttribute(XmlNames.DontCareAttributeName, value.DontCare);
        }


        public void Load(TestCasesRoot testCasesRoot)
        {
            ConditionActionIdDictionary = new Dictionary<int, ConditionActionBase>();
            XmlDocument doc = new XmlDocument();
            doc.Load(FilePath);
            XmlElement root = doc.DocumentElement;
            if (root.Name.Equals(XmlNames.DecisionTableRootName))
            {
                var xmlConditions = root.GetElementsByTagName(XmlNames.ConditionsName);
                var element = xmlConditions[0] as XmlElement;
                foreach (XmlElement item in element.GetElementsByTagName(XmlNames.ConditionName))
                {
                    ConditionObject condition;
                    Load(item, out condition);
                    testCasesRoot.Conditions.Add(condition);
                    ConditionActionIdDictionary.Add(condition.LoadSaveId, condition);
                }

                var xmlActions = root.GetElementsByTagName(XmlNames.ActionsName);
                element = xmlActions[0] as XmlElement;
                foreach (XmlElement item in element.GetElementsByTagName(XmlNames.ActionName))
                {
                    ActionObject action;
                    Load(item, out action);
                    testCasesRoot.Actions.Add(action);
                    ConditionActionIdDictionary.Add(action.LoadSaveId, action);
                }

                var xmlTestCases = root.GetElementsByTagName(XmlNames.TestCasesName);
                element = xmlTestCases[0] as XmlElement;
                int displayIndex = 1;
                foreach (XmlElement item in element.GetElementsByTagName(XmlNames.TestCaseName))
                {
                    TestCase testCase;
                    Load(item, out testCase);
                    testCase.DisplayIndex = displayIndex++;
                    testCasesRoot.TestCases.Add(testCase);
                }
            }
            else
            {
                throw new XmlElementNotFoundException(XmlNames.DecisionTableRootName);
            }

        }

        private void Load(XmlElement element, out TestCase testCase)
        {
            string name = element.GetAttributeStringValue(XmlNames.NameAttributeName);
            testCase = new TestCase(name);

            var xmlValueObjectColl = element.GetElementsByTagName(XmlNames.TestCaseConditionsName);
            var xmlValueObject = xmlValueObjectColl[0] as XmlElement;
            foreach (XmlElement xmlEnumValue in xmlValueObject.GetElementsByTagName(XmlNames.TestCaseValueName))
            {
                ValueObject valueObject;
                Load(xmlEnumValue, out valueObject);
                testCase.Conditions.Add(valueObject);
            }

            xmlValueObjectColl = element.GetElementsByTagName(XmlNames.TestCaseActionsName);
            xmlValueObject = xmlValueObjectColl[0] as XmlElement;
            foreach (XmlElement xmlEnumValue in xmlValueObject.GetElementsByTagName(XmlNames.TestCaseValueName))
            {
                ValueObject valueObject;
                Load(xmlEnumValue, out valueObject);
                testCase.Actions.Add(valueObject);
            }
        }

        private void Load(XmlElement xmlEnumValue, out ValueObject valueObject)
        {
            int id = xmlEnumValue.GetAttributeIntValue(XmlNames.IdAttributeName);
            if (!ConditionActionIdDictionary.ContainsKey(id))
            {
                throw new InvalidObjectIdReferenceException(xmlEnumValue, XmlNames.IdAttributeName, id);
            }
            valueObject = ValueObject.Create(ConditionActionIdDictionary[id]);
            valueObject.Text = xmlEnumValue.GetAttributeStringValue(XmlNames.TextAttributeName, XmlElementOption.Optional);
            valueObject.BoolValue = xmlEnumValue.GetAttributeBoolValue(XmlNames.BoolAttributeName);
            valueObject.SelectedItemIndex = xmlEnumValue.GetAttributeIntValue(XmlNames.SelectedItemIndexAttributeName);
        }

        private void Load(XmlElement element, out ConditionObject condition)
        {
            string name = element.GetAttributeStringValue(XmlNames.NameAttributeName);
            // only enums are supported by conditions ValueDataType type = element.GetAttributeEnumValue<ValueDataType>(XmlNames.TypeAttributeName);            
            condition = ConditionObject.Create(name);
            condition.LoadSaveId = element.GetAttributeIntValue(XmlNames.IdAttributeName, XmlElementOption.MustHaveValue);
            condition.DefaultText = element.GetAttributeStringValue(XmlNames.DefaultTextAttributeName, XmlElementOption.Optional);
            condition.DefaultBool = element.GetAttributeBoolValue(XmlNames.DefaultBoolAttributeName);

            LoadEnumValues(element, condition);
        }

        private void Load(XmlElement element, out ActionObject action)
        {
            string name = element.GetAttributeStringValue(XmlNames.NameAttributeName);
            ValueDataType type = element.GetAttributeEnumValue<ValueDataType>(XmlNames.TypeAttributeName);
            action = ActionObject.Create(name, type);
            action.LoadSaveId = element.GetAttributeIntValue(XmlNames.IdAttributeName, XmlElementOption.MustHaveValue);
            action.DefaultText = element.GetAttributeStringValue(XmlNames.DefaultTextAttributeName, XmlElementOption.Optional);
            action.DefaultBool = element.GetAttributeBoolValue(XmlNames.DefaultBoolAttributeName);
            LoadEnumValues(element, action);
        }

        private void LoadEnumValues(XmlElement element, ConditionActionBase co)
        {
            if (co.DataType == ValueDataType.Enumeration)
            {
                var xmlEnumValuesColl = element.GetElementsByTagName(XmlNames.EnumValuesName);
                var xmlEnumValues = xmlEnumValuesColl[0] as XmlElement;
                foreach (XmlElement xmlEnumValue in xmlEnumValues.GetElementsByTagName(XmlNames.EnumValueName))
                {
                    EnumValue ev;
                    Load(xmlEnumValue, out ev);
                    co.EnumValues.Add(ev);
                }
            }
        }

        private void Load(XmlElement element, out EnumValue ev)
        {
            ev = new EnumValue(
                element.GetAttributeStringValue(XmlNames.NameAttributeName),
                element.GetAttributeStringValue(XmlNames.ValueAttributeName, XmlElementOption.Optional),
                element.GetAttributeBoolValue(XmlNames.IsInvalidAttributeName),
                element.GetAttributeBoolValue(XmlNames.DontCareAttributeName),
                element.GetAttributeBoolValue(XmlNames.IsDefaultAttributeName)
                );
        }

    }
}

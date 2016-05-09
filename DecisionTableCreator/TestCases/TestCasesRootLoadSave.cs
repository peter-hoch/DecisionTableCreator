using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    public partial class TestCasesRoot
    {
        public void Save(string savePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
            var root = doc.CreateElement(XmlNames.DecisionTableRootName);
            doc.AppendChild(root);

            var xmlConditions = root.AddElement(XmlNames.ConditionsName);
            foreach (ConditionObject condition in Conditions)
            {
                condition.Save(xmlConditions);
            }

            var xmlActions = root.AddElement(XmlNames.ActionsName);
            foreach (ActionObject action in Actions)
            {
                action.Save(xmlActions);
            }

            doc.Save(savePath);
        }

        public static TestCasesRoot Load(string filePath)
        {
            var testCasesRoot = new TestCasesRoot();
            testCasesRoot.LoadInternal(filePath);
            return testCasesRoot;
        }

        private void LoadInternal(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlElement root = doc.DocumentElement;
            if (root.Name.Equals(XmlNames.DecisionTableRootName))
            {
                var xmlConditions = root.GetElementsByTagName(XmlNames.ConditionsName);
                var element = xmlConditions[0] as XmlElement;
                foreach (XmlElement item in element.GetElementsByTagName(XmlNames.ConditionName))
                {
                    Conditions.Add(ConditionObject.Load(item));
                }

                var xmlActions = root.GetElementsByTagName(XmlNames.ActionsName);
                element = xmlActions[0] as XmlElement;
                foreach (XmlElement item in element.GetElementsByTagName(XmlNames.ActionName))
                {
                    Actions.Add(ActionObject.Load(item));
                }
            }
            else
            {
                throw new XmlElementNotFoundException(XmlNames.DecisionTableRootName);
            }

            CreateBasicColumnDescriptions();


            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);

        }

    }
}


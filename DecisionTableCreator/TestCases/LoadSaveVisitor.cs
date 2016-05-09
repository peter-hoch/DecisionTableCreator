using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DecisionTableCreator.Utils;

namespace DecisionTableCreator.TestCases
{
    
    public class LoadSaveVisitor
    {
        public String SavePath { get; private set; }

        public LoadSaveVisitor(string path)
        {
            SavePath = path;
        }

        public void Save(TestCasesRoot testCasesRoot)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
            var root = doc.CreateElement("DecisionTableRoot");
            doc.AppendChild(root);

            Save("Conditions", testCasesRoot.Conditions, root);

            doc.Save(SavePath);
        }

        private void Save(string name, ObservableCollection<ConditionActionBase> conditions, XmlElement parent)
        {
            var collectionParent = parent.AddElement(name);
            foreach (ConditionActionBase child in conditions)
            {
                Save("Element", child, collectionParent);
            }
        }

        private void Save(string name, ConditionActionBase child, XmlElement collectionParent)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace DecisionTableCreator.TestCases
{
    public partial class TestCasesRoot
    {
        public void Save(string savePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
            var root = doc.CreateElement("DecisionTableRoot");
            doc.AppendChild(root);

            doc.Save(savePath);
        }

    }
}

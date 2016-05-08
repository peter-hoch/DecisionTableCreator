using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DecisionTableCreator.Utils
{
    public static class XmlUtils
    {
        public static XmlElement AddElement(this XmlElement me, string name)
        {
            var child = me.OwnerDocument.CreateElement(name);
            me.AppendChild(child);
            return child;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.StringTemplate.Misc;

namespace DecisionTableCreator.ErrorDialog
{
    public class TemplateMessageWrapper
    {
        public TemplateMessageWrapper(TemplateMessage msg)
        {
            Message = msg;
        }

        public TemplateMessage Message { get; private set; }

        public String Text { get { return Message.ToString(); } }
    }
}

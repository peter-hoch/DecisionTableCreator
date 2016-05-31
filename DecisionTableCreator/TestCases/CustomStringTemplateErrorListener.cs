using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Misc;

namespace DecisionTableCreator.TestCases
{
    public class CustomStringTemplateErrorListener : ITemplateErrorListener
    {
        public CustomStringTemplateErrorListener()
        {
            ErrorMessages = new ObservableCollection<TemplateMessage>();
        }

        public bool ErrorReported { get { return ErrorCount != 0; } }

        public int ErrorCount { get { return ErrorMessages.Count; } }

        public ObservableCollection<TemplateMessage> ErrorMessages { get; private set; }

        public void CompiletimeError(TemplateMessage msg)
        {
            Trace.WriteLine("StringTemplate CompileTimeError" + msg.ToString());
            ErrorMessages.Add(msg);
        }

        public void RuntimeError(TemplateMessage msg)
        {
            Trace.WriteLine("StringTemplate RuntimeError" + msg.ToString());
            ErrorMessages.Add(msg);
        }

        public void IOError(TemplateMessage msg)
        {
            Trace.WriteLine("StringTemplate IOError" + msg.ToString());
            ErrorMessages.Add(msg);
        }

        public void InternalError(TemplateMessage msg)
        {
            Trace.WriteLine("StringTemplate InternalError" + msg.ToString());
            ErrorMessages.Add(msg);
        }
    }
}

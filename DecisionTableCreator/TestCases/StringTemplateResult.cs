using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.StringTemplate.Misc;

namespace DecisionTableCreator.TestCases
{
    public class StringTemplateResult
    {
        public CustomStringTemplateErrorListener ErrorListener { get; private set; }

        public string GeneratedContent { get; private set; }

        public string GetErrorList()
        {
            StringBuilder sb = new StringBuilder();

            foreach (TemplateMessage msg in ErrorListener.ErrorMessages)
            {
                sb.AppendLine(msg.ToString());
            }

            return sb.ToString();
        }

        public StringTemplateResult(CustomStringTemplateErrorListener errorListener, string generatedContent)
        {
            ErrorListener = errorListener;
            GeneratedContent = generatedContent;
        }
    }
}

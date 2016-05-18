using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.StringTemplate;

namespace DecisionTableCreator.TestCases
{
    public partial class TestCasesRoot
    {
        public string GenerateFromtemplate(string groupFilePath)
        {
            TemplateGroup group = new TemplateGroupFile(groupFilePath);
            Template templ = group.GetInstanceOf("TestCasesRoot");
            templ.Add("root", this);
            return templ.Render();
        }
        public string GenerateFromTemplateString(string groupFileContent)
        {
            TemplateGroup group = new TemplateGroupString(groupFileContent);
            Template templ = group.GetInstanceOf("TestCasesRoot");
            templ.Add("root", this);
            return templ.Render();
        }
    }
}

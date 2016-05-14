using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.TestCases;
using NUnit.Framework;
using UnitTestSupport;

namespace UnitTests1
{
    [TestFixture]
    public class StringTemplateTests
    {
        [Test]
        public void FirstTemplateTest()
        {
            string templPath = Path.Combine(TestSupport.TestFilesDirectory, "template.stg");
            string resultPath = Path.Combine(TestSupport.CreatedFilesDirectory, "output.txt");
            TestCasesRoot tcRoot = TestCasesRoot.CreateSimpleTable();
            tcRoot.Test = "test text";
            string text = tcRoot.GenerateFromtemplate(templPath);
            File.WriteAllText(resultPath, text);
        }
    }
}

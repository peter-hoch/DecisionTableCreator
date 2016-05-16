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
        [SetUp]
        public void Setup()
        {
            TestSupport.ClearCreatedFiles();
            TestSupport.DiffAction = new InvokeWinMerge();
        }

        [Test]
        public void FirstTemplateTest()
        {
            string templPath = Path.Combine(TestSupport.TestFilesDirectory, "template.stg");
            string resultPath = Path.Combine(TestSupport.CreatedFilesDirectory, "output.txt");
            TestCasesRoot tcRoot = TestCasesRoot.CreateSimpleTable();
            string text = tcRoot.GenerateFromtemplate(templPath);
            File.WriteAllText(resultPath, text);
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(resultPath)));
        }

        [Test]
        public void CreateHtmlTableTest()
        {
            string templPath = Path.Combine(TestSupport.TestFilesDirectory, "HtmlTemplate.stg");
            string resultPath = Path.Combine(TestSupport.CreatedFilesDirectory, "output.htm");
            TestCasesRoot tcRoot = TestCasesRoot.CreateSimpleTable();
            string text = tcRoot.GenerateFromtemplate(templPath);
            File.WriteAllText(resultPath, text);
            Assert.That( TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(resultPath)));
        }
    }
}

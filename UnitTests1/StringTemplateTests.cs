using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.TestCases;
using DecisionTableCreator.Utils;
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

        [Test]
        public void FirstHtmlContentFromTemplateResource()
        {
            string resultPath = Path.Combine(TestSupport.CreatedFilesDirectory, "output.html");
            TestCasesRoot tcRoot = TestCasesRoot.CreateSimpleTable();
            string text = tcRoot.GenerateFromTemplateString(DecisionTableCreator.Templates.Resources.HtmlTemplate);
            File.WriteAllText(resultPath, text);
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(resultPath)));
        }

        [Test]
        public void FirstHtmlContentFromTemplateResourceEmptyProject()
        {
            string resultPath = Path.Combine(TestSupport.CreatedFilesDirectory, "output.html");
            TestCasesRoot tcRoot = new TestCasesRoot();
            string text = tcRoot.GenerateFromTemplateString(DecisionTableCreator.Templates.Resources.HtmlTemplate);
            File.WriteAllText(resultPath, text);
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(resultPath)));
        }


        [Test]
        public void PrepareHtmlForClipboard()
        {
            string resultPath = Path.Combine(TestSupport.CreatedFilesDirectory, "output.txt");
            string resultPathHtml = Path.Combine(TestSupport.CreatedFilesDirectory, "html.txt");
            string resultPathFragment = Path.Combine(TestSupport.CreatedFilesDirectory, "fragment.txt");
            TestCasesRoot tcRoot = TestCasesRoot.CreateSimpleTable();
            string html = tcRoot.GenerateFromTemplateString(DecisionTableCreator.Templates.Resources.HtmlTemplate);

            PrepareForClipboard prepare = new PrepareForClipboard();
            string result = prepare.Prepare(html);

            File.WriteAllText(resultPath, result);
            File.WriteAllText(resultPathHtml, result.Substring(prepare.StartHtml, prepare.EndHtml - prepare.StartHtml));
            File.WriteAllText(resultPathFragment, result.Substring(prepare.StartFragment, prepare.EndFragment - prepare.StartFragment));
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(resultPathHtml)));
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(resultPathFragment)));
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(resultPath)));
        }

    }
}

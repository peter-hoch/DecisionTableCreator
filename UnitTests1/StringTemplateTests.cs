/*
 * [The "BSD license"]
 * Copyright (c) 2016 Peter Hoch
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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

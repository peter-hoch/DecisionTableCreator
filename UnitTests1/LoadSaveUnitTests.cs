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
using System.Diagnostics;
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
    public class LoadSaveUnitTests
    {
        [SetUp]
        public void Setup()
        {
            TestSupport.ClearCreatedFiles();
            TestSupport.DiffAction = new InvokeWinMerge();
        }

        [Test]
        public void Save100()
        {

            string savePath = Path.Combine(TestSupport.CreatedFilesDirectory, "Save.dtc");
            TestCasesRoot root = TestCasesRoot.CreateSimpleTable();
            root.Save(savePath);

            root.New();

            root.Load(savePath);

            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(savePath)));
            //ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files (x86)\Notepad++\notepad++.exe", savePath);
            //Process.Start(info);
        }

        [TestCase(0, "PrinterTestV1.0.dtc")]
        public void LoadOldProjectFiles(int idx, string fileName)
        {
            string loadPath = Path.Combine(TestSupport.TestFilesDirectory, fileName);
            string savePath = Path.Combine(TestSupport.CreatedFilesDirectory, fileName);
            TestCasesRoot root = new TestCasesRoot();
            root.Load(loadPath);

            root.Save(savePath);
            // only to view the diffrence
            TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.TestFilesDirectory, Path.GetFileName(savePath));
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(savePath)));
            //ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files (x86)\Notepad++\notepad++.exe", savePath);
            //Process.Start(info);
        }
    }
}

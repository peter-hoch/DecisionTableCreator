/*
 * [The "BSD license"]
 * Copyright (c) 2017 Peter Hoch
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

namespace UnitTests2
{
    [TestFixture]
    public class CopyTestCaseTest
    {
        [SetUp]
        public void Setup()
        {
            TestSupport.ClearCreatedFiles();
            TestSupport.DiffAction = new InvokeWinMerge();
        }

        [TestCase(10, 5, 5, 12)]
        [TestCase(12, 0, 5, 12)]
        [TestCase(14, 5, 0, 12)]
        public void AppendCopyOfTestCaseTest(int idx, int conditionCount, int actionCount, int testCasesCount)
        {
            TestCasesRoot tcr = TestCasesRoot.CreateSimpleTable2(conditionCount, actionCount, testCasesCount);
            TestUtils.CheckTestCasesAndConditionsAndActions(tcr);

            for (int tcIdx = 0; tcIdx < testCasesCount; tcIdx++)
            {
                TestCase newTestCase = tcr.InsertTestCase();
                TestCase templateTestCase = tcr.TestCases[tcIdx];
                tcr.CopyTestCaseSettings(templateTestCase, newTestCase);
            }

            // create and save the sample project
            string savePath = Path.Combine(TestSupport.CreatedFilesDirectory, idx + "SimpleProject.dtc");
            tcr.Save(savePath);
            //ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files (x86)\Notepad++\notepad++.exe", savePath);
            //Process.Start(info);

            for (int tcIdx = 0; tcIdx < testCasesCount; tcIdx++)
            {
                CompareTestCases(tcr, tcIdx, tcIdx + testCasesCount);
            }

        }

        [Test]
        public void AppendCopyOfTestCaseSampleProjectTest()
        {
            TestCasesRoot tcr = new TestCasesRoot();
            tcr.CreateSampleProject();
            TestUtils.CheckTestCasesAndConditionsAndActions(tcr);
            int testCasesCount = tcr.TestCases.Count;

            for (int tcIdx = 0; tcIdx < testCasesCount; tcIdx++)
            {
                TestCase newTestCase = tcr.InsertTestCase();
                TestCase templateTestCase = tcr.TestCases[tcIdx];
                tcr.CopyTestCaseSettings(templateTestCase, newTestCase);
            }

            // create and save the sample project
            string savePath = Path.Combine(TestSupport.CreatedFilesDirectory, "SimpleProject.dtc");
            tcr.Save(savePath);
            ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files (x86)\Notepad++\notepad++.exe", savePath);
            Process.Start(info);

            for (int tcIdx = 0; tcIdx < testCasesCount; tcIdx++)
            {
                CompareTestCases(tcr, tcIdx, tcIdx + testCasesCount);
            }

        }

        private void CompareTestCases(TestCasesRoot tcr, int sourceIndex, int targetIndex)
        {
            TestCase sourceTestCase = tcr.TestCases[sourceIndex];
            TestCase targetTestCase = tcr.TestCases[targetIndex];

            Assert.That(sourceTestCase.Description.Equals(targetTestCase.Description));

            Assert.That(sourceTestCase.Conditions.Count == targetTestCase.Conditions.Count);
            for (int idx = 0; idx < sourceTestCase.Conditions.Count; idx++)
            {
                Assert.That(sourceTestCase.Conditions[idx].SelectedItemIndex == targetTestCase.Conditions[idx].SelectedItemIndex);
            }

            Assert.That(sourceTestCase.Actions.Count == targetTestCase.Actions.Count);
            for (int idx = 0; idx < sourceTestCase.Actions.Count; idx++)
            {
                Assert.That(sourceTestCase.Actions[idx].SelectedItemIndex == targetTestCase.Actions[idx].SelectedItemIndex);
            }
        }
    }
}

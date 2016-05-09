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
        }

        [Test]
        public void Save100()
        {

            string savePath = Path.Combine(TestSupport.CreatedFilesDirectory, "Save.dtc");
            TestCasesRoot root = TestCasesRoot.CreateSimpleTable();
            root.Save(savePath);

            root = TestCasesRoot.Load(savePath);

            ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files (x86)\Notepad++\notepad++.exe", savePath);
            Process.Start(info);
        }
    }
}

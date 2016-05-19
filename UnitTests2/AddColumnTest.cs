using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.StringTemplate;
using DecisionTableCreator.TestCases;
using NUnit.Framework;
using UnitTestSupport;

namespace UnitTests2
{
    [TestFixture]
    public class AddColumnTest
    {
        [SetUp]
        public void Setup()
        {
            TestSupport.DiffAction = new InvokeWinMerge();
        }

        void SaveTestCasesDisplayOrder(TestCasesRootContainer tcrc, string path)
        {
            StringBuilder sb = new StringBuilder();
            foreach (TestCase testCase in tcrc.TestCasesRoot.TestCases.OrderBy(tc=>tc.DisplayIndex))
            {
                sb.AppendFormat("{0} di={1}", testCase.Name, testCase.DisplayIndex).AppendLine();
            }
            File.WriteAllText(path, sb.ToString());
        }

        [Test]
        public void TestAddTestCase()
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();

            tcrc.TestCasesRoot.TestCases[0].DisplayIndex = 3;
            tcrc.TestCasesRoot.TestCases[1].DisplayIndex = 4;
            tcrc.TestCasesRoot.TestCases[2].DisplayIndex = 6;
            tcrc.TestCasesRoot.TestCases[3].DisplayIndex = 1;
            tcrc.TestCasesRoot.TestCases[4].DisplayIndex = 5;
            tcrc.TestCasesRoot.TestCases[5].DisplayIndex = 2;

            SaveTestCasesDisplayOrder(tcrc, firstPath);

            SelectedValuesByTestCases selValues = new SelectedValuesByTestCases();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            int testCaseCount = tcrc.TestCasesRoot.TestCases.Count;
            TestCase newTestCase = tcrc.TestCasesRoot.InsertTestCase(-1);
            string testCaseId = "new test case " + DateTime.Now.ToString("F");
            newTestCase.TestProperty = testCaseId;

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(tcrc.TestCasesRoot.TestCases.Count == testCaseCount + 1);
            Assert.That(tcrc.TestCasesRoot.TestCases.Last().TestProperty.Equals(testCaseId));
            Assert.That(tcrc.ConditionChangeCount == 1);
            Assert.That(tcrc.ActionChangeCount == 1);
            selValues.AppendTestCase(newTestCase, tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            SaveTestCasesDisplayOrder(tcrc, secondPath);

            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(firstPath)));
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(secondPath)));
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }


        [TestCase(AddRowsTest.InsertPosition.First)]
        [TestCase(AddRowsTest.InsertPosition.Second)]
        [TestCase(AddRowsTest.InsertPosition.Last)]
        [TestCase(AddRowsTest.InsertPosition.AfterLast)]
        public void TestInsertTestCase(AddRowsTest.InsertPosition insertPosition)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, insertPosition+"_first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, insertPosition + "_second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();

            tcrc.TestCasesRoot.TestCases[0].DisplayIndex = 3;
            tcrc.TestCasesRoot.TestCases[1].DisplayIndex = 4;
            tcrc.TestCasesRoot.TestCases[2].DisplayIndex = 6;
            tcrc.TestCasesRoot.TestCases[3].DisplayIndex = 1;
            tcrc.TestCasesRoot.TestCases[4].DisplayIndex = 5;
            tcrc.TestCasesRoot.TestCases[5].DisplayIndex = 2;

            SaveTestCasesDisplayOrder(tcrc, firstPath);
            SelectedValuesByTestCases selValues = new SelectedValuesByTestCases();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            int indexWhereToInsert = TestUtils.CalculateIndex(tcrc.TestCasesRoot.TestCases, insertPosition);

            int testCaseCount = tcrc.TestCasesRoot.TestCases.Count;
            TestCase newTestCase = tcrc.TestCasesRoot.InsertTestCase(indexWhereToInsert);
            string testCaseId = "new test case " + DateTime.Now.ToString("F");
            newTestCase.TestProperty = testCaseId;

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(tcrc.TestCasesRoot.TestCases.Count == testCaseCount + 1);
            Assert.That(tcrc.TestCasesRoot.TestCases[indexWhereToInsert].TestProperty.Equals(testCaseId));
            Assert.That(tcrc.ConditionChangeCount == 1);
            Assert.That(tcrc.ActionChangeCount == 1);
            selValues.AppendTestCase(newTestCase, tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            SaveTestCasesDisplayOrder(tcrc, secondPath);

            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(firstPath)));
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(secondPath)));
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }

        [TestCase(AddRowsTest.DeletePosition.First)]
        [TestCase(AddRowsTest.DeletePosition.Second)]
        [TestCase(AddRowsTest.DeletePosition.Last)]
        public void TestDeleteTestCase(AddRowsTest.DeletePosition deletePosition)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, deletePosition+"_first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, deletePosition+"_second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();

            tcrc.TestCasesRoot.TestCases[0].DisplayIndex = 3;
            tcrc.TestCasesRoot.TestCases[1].DisplayIndex = 4;
            tcrc.TestCasesRoot.TestCases[2].DisplayIndex = 6;
            tcrc.TestCasesRoot.TestCases[3].DisplayIndex = 1;
            tcrc.TestCasesRoot.TestCases[4].DisplayIndex = 5;
            tcrc.TestCasesRoot.TestCases[5].DisplayIndex = 2;

            SaveTestCasesDisplayOrder(tcrc, firstPath);
            SelectedValuesByTestCases selValues = new SelectedValuesByTestCases();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            int indexWhereToDelete = TestUtils.CalculateIndex(tcrc.TestCasesRoot.TestCases, deletePosition);

            int testCaseCount = tcrc.TestCasesRoot.TestCases.Count;
            TestCase deletedTestCase = tcrc.TestCasesRoot.DeleteTestCaseAt(indexWhereToDelete);

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(tcrc.TestCasesRoot.TestCases.Count == testCaseCount - 1);
            Assert.That(tcrc.ConditionChangeCount == 1);
            Assert.That(tcrc.ActionChangeCount == 1);
            selValues.DeleteTestCase(deletedTestCase, tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            SaveTestCasesDisplayOrder(tcrc, secondPath);

            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(firstPath)));
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.ReferenceFilesDirectory, Path.GetFileName(secondPath)));
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }

        [TestCase(AddRowsTest.DeletePosition.First, 1, AddRowsTest.InsertPosition.AfterLast, 1)]
        [TestCase(AddRowsTest.DeletePosition.First, 4, AddRowsTest.InsertPosition.AfterLast, 6)]
        [TestCase(AddRowsTest.DeletePosition.First, 6, AddRowsTest.InsertPosition.AfterLast, 6)]
        [TestCase(AddRowsTest.DeletePosition.First, 3, AddRowsTest.InsertPosition.AfterLast, 60)]
        public void TestDeleteAndAddTestCase(AddRowsTest.DeletePosition deletePosition, int deleteCount, AddRowsTest.InsertPosition insertPosition, int addCount)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();
            tcrc.TestCasesRoot.Save(firstPath);
            SelectedValuesByTestCases selValues = new SelectedValuesByTestCases();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            int testCaseCount = tcrc.TestCasesRoot.TestCases.Count;

            for (int idx = 0; idx < deleteCount; idx++)
            {
                int indexWhereToDelete = TestUtils.CalculateIndex(tcrc.TestCasesRoot.TestCases, deletePosition);
                TestCase deletedTestCase = tcrc.TestCasesRoot.TestCases[indexWhereToDelete];
                tcrc.TestCasesRoot.DeleteTestCaseAt(indexWhereToDelete);
                selValues.DeleteTestCase(deletedTestCase, tcrc.TestCasesRoot);
            }

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(tcrc.TestCasesRoot.TestCases.Count == testCaseCount - deleteCount);
            Assert.That(tcrc.ConditionChangeCount == deleteCount);
            Assert.That(tcrc.ActionChangeCount == deleteCount);
            selValues.Check(tcrc.TestCasesRoot);

            testCaseCount = tcrc.TestCasesRoot.TestCases.Count;
            for (int idx = 0; idx < addCount; idx++)
            {
                int indexWhereToInsert = TestUtils.CalculateIndex(tcrc.TestCasesRoot.TestCases, insertPosition);
                TestCase newTestCase = tcrc.TestCasesRoot.InsertTestCase(indexWhereToInsert);
                string testCaseId = "new test case " + DateTime.Now.ToString("F");
                newTestCase.TestProperty = testCaseId;
                selValues.AppendTestCase(newTestCase, tcrc.TestCasesRoot);
                Assert.That(tcrc.TestCasesRoot.TestCases[indexWhereToInsert].TestProperty.Equals(testCaseId));
            }

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(tcrc.TestCasesRoot.TestCases.Count == testCaseCount + addCount);
            Assert.That(tcrc.ConditionChangeCount == (deleteCount + addCount));
            Assert.That(tcrc.ActionChangeCount == (deleteCount + addCount));
            selValues.Check(tcrc.TestCasesRoot);

            tcrc.TestCasesRoot.Save(secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);

        }


    }
}

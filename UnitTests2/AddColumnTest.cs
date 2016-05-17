using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class AddColumnTest
    {
        [SetUp]
        public void Setup()
        {
            TestSupport.DiffAction = new InvokeWinMerge();
        }


        [Test]
        public void TestAddTestCase()
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();

            tcrc.TestCasesRoot.Save(firstPath);
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
            selValues.AppendTestCase(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);


            tcrc.TestCasesRoot.Save(secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }


        [TestCase(AddRowsTest.InsertPosition.First)]
        [TestCase(AddRowsTest.InsertPosition.Second)]
        [TestCase(AddRowsTest.InsertPosition.Last)]
        [TestCase(AddRowsTest.InsertPosition.AfterLast)]
        public void TestInsertTestCase(AddRowsTest.InsertPosition insertPosition)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();
            tcrc.TestCasesRoot.Save(firstPath);
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
            selValues.InsertTestCase(insertPosition, tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);


            tcrc.TestCasesRoot.Save(secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }

        [TestCase(AddRowsTest.DeletePosition.First)]
        [TestCase(AddRowsTest.DeletePosition.Second)]
        [TestCase(AddRowsTest.DeletePosition.Last)]
        public void TestDeletetTestCase(AddRowsTest.DeletePosition deletePosition)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();
            tcrc.TestCasesRoot.Save(firstPath);
            SelectedValuesByTestCases selValues = new SelectedValuesByTestCases();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            int indexWhereToDelete = TestUtils.CalculateIndex(tcrc.TestCasesRoot.TestCases, deletePosition);

            int testCaseCount = tcrc.TestCasesRoot.TestCases.Count;
            tcrc.TestCasesRoot.DeleteTestCaseAt(indexWhereToDelete);

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(tcrc.TestCasesRoot.TestCases.Count == testCaseCount - 1);
            Assert.That(tcrc.ConditionChangeCount == 1);
            Assert.That(tcrc.ActionChangeCount == 1);
            selValues.DeleteTestCase(deletePosition, tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);


            tcrc.TestCasesRoot.Save(secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }


    }
}

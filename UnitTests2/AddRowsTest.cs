using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.DynamicTable;
using DecisionTableCreator.TestCases;
using Microsoft.Win32;
using NUnit.Framework;
using UnitTestSupport;

namespace UnitTests2
{
    [TestFixture]
    public class AddRowsTest
    {
        public enum InsertPosition
        {
            First,
            Second,
            Last,
            AfterLast
        }

        public enum DeletePosition
        {
            First,
            Second,
            Last,
        }


        [SetUp]
        public void Setup()
        {
            TestSupport.DiffAction = new InvokeWinMerge();
        }


        [Test]
        public void TestAddCondition()
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();

            tcrc.TestCasesRoot.Save(firstPath);
            SelectedValuesByConditionsAndActions selValues = new SelectedValuesByConditionsAndActions();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            string newConditionName = "new condition " + DateTime.Now.ToString("F");
            int conditionCount = tcrc.TestCasesRoot.Conditions.Count;
            tcrc.TestCasesRoot.AppendCondition(ConditionObject.Create(newConditionName, new ObservableCollection<EnumValue>() { new EnumValue("new test", "new value") }));

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(tcrc.TestCasesRoot.Conditions.Count == conditionCount + 1);
            Assert.That(tcrc.TestCasesRoot.Conditions.Last().Name.Equals(newConditionName));
            Assert.That(tcrc.ConditionChangeCount == 1);
            Assert.That(tcrc.ActionChangeCount == 0);
            selValues.AppendCondition(tcrc.TestCasesRoot.TestCases.Count);
            selValues.Check(tcrc.TestCasesRoot);


            tcrc.TestCasesRoot.Save(secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }


        [TestCase(InsertPosition.First)]
        [TestCase(InsertPosition.Second)]
        [TestCase(InsertPosition.Last)]
        [TestCase(InsertPosition.AfterLast)]
        public void TestInsertCondition(InsertPosition insertPosition)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();
            tcrc.TestCasesRoot.Save(firstPath);
            SelectedValuesByConditionsAndActions selValues = new SelectedValuesByConditionsAndActions();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            int indexWhereToInsert = TestUtils.CalculateIndex(tcrc.TestCasesRoot.Conditions, insertPosition);
            string newConditionName = "new condition " + DateTime.Now.ToString("F");
            int conditionCount = tcrc.TestCasesRoot.Conditions.Count;
            tcrc.TestCasesRoot.InsertCondition(indexWhereToInsert, ConditionObject.Create(newConditionName, new ObservableCollection<EnumValue>() { new EnumValue("new test", "new value") }));

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(conditionCount == 3); // adjust last testcase on failue
            Assert.That(tcrc.TestCasesRoot.Conditions.Count == conditionCount + 1);
            Assert.That(tcrc.TestCasesRoot.Conditions[indexWhereToInsert].Name.Equals(newConditionName));
            Assert.That(tcrc.ConditionChangeCount == 1);
            Assert.That(tcrc.ActionChangeCount == 0);
            selValues.InsertCondition(insertPosition, tcrc.TestCasesRoot.TestCases.Count);
            selValues.Check(tcrc.TestCasesRoot);

            tcrc.TestCasesRoot.Save(secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }

        [TestCase(DeletePosition.First)]
        [TestCase(DeletePosition.Second)]
        [TestCase(DeletePosition.Last)]
        public void TestDeleteCondition(DeletePosition deletePosition)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();
            tcrc.TestCasesRoot.Save(firstPath);
            SelectedValuesByConditionsAndActions selValues = new SelectedValuesByConditionsAndActions();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            int indexWhereToDelete = TestUtils.CalculateIndex(tcrc.TestCasesRoot.Conditions, deletePosition);
            int conditionCount = tcrc.TestCasesRoot.Conditions.Count;
            tcrc.TestCasesRoot.DeleteConditionAt(indexWhereToDelete);

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(conditionCount == 3); // adjust last testcase on failue
            Assert.That(tcrc.TestCasesRoot.Conditions.Count == conditionCount - 1);
            Assert.That(tcrc.ConditionChangeCount == 1);
            Assert.That(tcrc.ActionChangeCount == 0);
            selValues.DeleteCondition(deletePosition);
            selValues.Check(tcrc.TestCasesRoot);

            tcrc.TestCasesRoot.Save(secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }

        [Test]
        public void TestAddAction()
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();
            tcrc.TestCasesRoot.Save(firstPath);
            SelectedValuesByConditionsAndActions selValues = new SelectedValuesByConditionsAndActions();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            string newActionName = "new action" + DateTime.Now.ToString("F");
            int actionCount = tcrc.TestCasesRoot.Actions.Count;
            tcrc.TestCasesRoot.AppendAction(ActionObject.Create(newActionName, new ObservableCollection<EnumValue>() {new EnumValue("new test", "new value")}));

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(tcrc.TestCasesRoot.Actions.Count == actionCount + 1);
            Assert.That(tcrc.TestCasesRoot.Actions.Last().Name.Equals(newActionName));
            Assert.That(tcrc.ConditionChangeCount == 0);
            Assert.That(tcrc.ActionChangeCount == 1);
            selValues.AppendAction(tcrc.TestCasesRoot.TestCases.Count);
            selValues.Check(tcrc.TestCasesRoot);

            tcrc.TestCasesRoot.Save(secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }

        [TestCase(InsertPosition.First)]
        [TestCase(InsertPosition.Second)]
        [TestCase(InsertPosition.Last)]
        [TestCase(InsertPosition.AfterLast)]
        public void TestInsertAction(InsertPosition insertPosition)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();
            tcrc.TestCasesRoot.Save(firstPath);
            SelectedValuesByConditionsAndActions selValues = new SelectedValuesByConditionsAndActions();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            int indexWhereToInsert = TestUtils.CalculateIndex(tcrc.TestCasesRoot.Conditions, insertPosition);
            string newActionName = "new action" + DateTime.Now.ToString("F");
            int actionCount = tcrc.TestCasesRoot.Actions.Count;
            tcrc.TestCasesRoot.InsertAction(indexWhereToInsert, ActionObject.Create(newActionName, new ObservableCollection<EnumValue>() {new EnumValue("new test", "new value")}));

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(actionCount == 3); // adjust last testcase on failue
            Assert.That(tcrc.TestCasesRoot.Actions.Count == actionCount + 1);
            Assert.That(tcrc.TestCasesRoot.Actions[indexWhereToInsert].Name.Equals(newActionName));
            Assert.That(tcrc.ConditionChangeCount == 0);
            Assert.That(tcrc.ActionChangeCount == 1);
            selValues.InsertAction(insertPosition, tcrc.TestCasesRoot.TestCases.Count);
            selValues.Check(tcrc.TestCasesRoot);

            tcrc.TestCasesRoot.Save(secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }

        [TestCase(DeletePosition.First)]
        [TestCase(DeletePosition.Second)]
        [TestCase(DeletePosition.Last)]
        public void TestDeleteAction(DeletePosition deletePosition)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();
            tcrc.TestCasesRoot.Save(firstPath);
            SelectedValuesByConditionsAndActions selValues = new SelectedValuesByConditionsAndActions();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            int indexWhereToDelete = TestUtils.CalculateIndex(tcrc.TestCasesRoot.Actions, deletePosition);
            int actionCount = tcrc.TestCasesRoot.Actions.Count;
            tcrc.TestCasesRoot.DeleteActionAt(indexWhereToDelete);

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(actionCount == 3); // adjust last testcase on failue
            Assert.That(tcrc.TestCasesRoot.Actions.Count == actionCount - 1);
            Assert.That(tcrc.ConditionChangeCount == 0);
            Assert.That(tcrc.ActionChangeCount == 1);
            selValues.DeleteAction(deletePosition);
            selValues.Check(tcrc.TestCasesRoot);

            tcrc.TestCasesRoot.Save(secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }



    }
}

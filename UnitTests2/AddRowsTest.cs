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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr.Runtime;
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

        public enum Move
        {
            Up,
            Down
        }

        void SaveConditionsOrActions<TType>(ObservableCollection<TType> list , string path)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendFormat("{0}", item).AppendLine();
            }
            File.WriteAllText(path, sb.ToString());
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

        [Test]
        public void TestAddAndEditCondition()
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

            int index = conditionCount;
            ConditionObject original = tcrc.TestCasesRoot.Conditions[index];
            ConditionObject coClone = original.Clone();
            coClone.EnumValues.Add(new EnumValue("test1", false, true));
            tcrc.TestCasesRoot.ChangeCondition(index, coClone);

            tcrc.TestCasesRoot.CalculateStatistics();

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


        [TestCase(Move.Up, 1)]
        [TestCase(Move.Up, 2)]
        [TestCase(Move.Down, 1)]
        [TestCase(Move.Down, 0)]
        public void TestConditionMove(Move move, int index)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, move + "_" + index + "_first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, move + "_" + index + "_second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();
            SaveConditionsOrActions(tcrc.TestCasesRoot.Conditions, firstPath);
            SelectedValuesByConditionsAndActions selValues = new SelectedValuesByConditionsAndActions();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            if (move == Move.Up)
            {
                tcrc.TestCasesRoot.MoveConditionUp(index);
            }
            else
            {
                tcrc.TestCasesRoot.MoveConditionDown(index);
            }

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(tcrc.TestCasesRoot.Actions.Count == 3);
            Assert.That(tcrc.TestCasesRoot.Conditions.Count == 3);
            Assert.That(tcrc.ConditionChangeCount == 1);
            Assert.That(tcrc.ActionChangeCount == 0);
            selValues.Check(tcrc.TestCasesRoot);

            SaveConditionsOrActions(tcrc.TestCasesRoot.Conditions, secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }

        [TestCase(Move.Up, 1)]
        [TestCase(Move.Up, 2)]
        [TestCase(Move.Down, 1)]
        [TestCase(Move.Down, 0)]
        public void TestActionMove(Move move, int index)
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, move + "_" + index + "_first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, move + "_" + index + "_second.xml");
            TestCasesRootContainer tcrc = new TestCasesRootContainer();
            SaveConditionsOrActions(tcrc.TestCasesRoot.Actions, firstPath);
            SelectedValuesByConditionsAndActions selValues = new SelectedValuesByConditionsAndActions();
            selValues.CollectValues(tcrc.TestCasesRoot);
            selValues.Check(tcrc.TestCasesRoot);

            if (move == Move.Up)
            {
                tcrc.TestCasesRoot.MoveActionUp(index);
            }
            else
            {
                tcrc.TestCasesRoot.MoveActionDown(index);
            }

            TestUtils.CheckTestCasesAndConditionsAndActions(tcrc.TestCasesRoot);
            Assert.That(tcrc.TestCasesRoot.Actions.Count == 3);
            Assert.That(tcrc.TestCasesRoot.Conditions.Count == 3);
            Assert.That(tcrc.ConditionChangeCount == 0);
            Assert.That(tcrc.ActionChangeCount == 1);
            selValues.Check(tcrc.TestCasesRoot);

            SaveConditionsOrActions(tcrc.TestCasesRoot.Actions, secondPath);
            // only for manual check of testcase
            //TestSupport.CompareFile(firstPath, secondPath);
        }



    }
}

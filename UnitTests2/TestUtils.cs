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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.TestCases;
using NUnit.Framework;

namespace UnitTests2
{
    public class TestUtils
    {
        public static int CalculateIndex<TType>(IList<TType> conditionOrAction, AddRowsTest.InsertPosition indexWhereToInsert)
        {
            switch (indexWhereToInsert)
            {
                case AddRowsTest.InsertPosition.First:
                    return 0;
                case AddRowsTest.InsertPosition.Second:
                    return 1;
                case AddRowsTest.InsertPosition.Last:
                    return conditionOrAction.Count - 1;
                case AddRowsTest.InsertPosition.AfterLast:
                    return conditionOrAction.Count;
                default:
                    throw new ArgumentOutOfRangeException(nameof(indexWhereToInsert), indexWhereToInsert, null);
            }
        }

        public static  int CalculateIndex<TType>(IList<TType> conditionOrAction, AddRowsTest.DeletePosition deletePosition)
        {
            switch (deletePosition)
            {
                case AddRowsTest.DeletePosition.First:
                    return 0;
                case AddRowsTest.DeletePosition.Second:
                    return 1;
                case AddRowsTest.DeletePosition.Last:
                    return conditionOrAction.Count - 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deletePosition), deletePosition, null);
            }
        }

        public static void CheckTestCasesAndConditionsAndActions(TestCasesRoot tcr)
        {
            int testCasesCount = tcr.TestCases.Count;
            int conditionsCount = tcr.Conditions.Count;
            int actionsCount = tcr.Actions.Count;

            for (int testCaseIndex = 0; testCaseIndex < tcr.TestCases.Count; testCaseIndex++)
            {
                TestCase tc = tcr.TestCases[testCaseIndex];
                Assert.That(tc.Conditions.Count == conditionsCount);
                Assert.That(tc.Actions.Count == actionsCount);
                var actionCol = tcr.ActionTable.Columns[testCaseIndex + 1];
                Assert.That(tc.Name.Equals(actionCol.Caption));
                var conditionCol = tcr.ConditionTable.Columns[testCaseIndex + 1];
                Assert.That(tc.Name.Equals(conditionCol.Caption));

                for (int conditionIndex = 0; conditionIndex < tcr.Conditions.Count; conditionIndex++)
                {
                    ConditionObject co = tcr.Conditions[conditionIndex];
                    Assert.That(co.TestValues.Count == testCasesCount);

                    Assert.That(tc.Conditions[conditionIndex].ConditionOrActionParent.Equals(co));

                    ValueObject tcValueObject = tc.Conditions[conditionIndex];
                    ValueObject condValueObject = tcr.Conditions[conditionIndex].TestValues[testCaseIndex];
                    Assert.That(tcValueObject == condValueObject);
                    Assert.That(tcValueObject.EnumValues == condValueObject.EnumValues);
                }

                for (int actionIndex = 0; actionIndex < tcr.Actions.Count; actionIndex++)
                {
                    ActionObject co = tcr.Actions[actionIndex];
                    Assert.That(co.TestValues.Count == testCasesCount);

                    Assert.That(tc.Actions[actionIndex].ConditionOrActionParent.Equals(co));

                    ValueObject tcValueObject = tc.Actions[actionIndex];
                    ValueObject actionValueObject = tcr.Actions[actionIndex].TestValues[testCaseIndex];
                    Assert.That(tcValueObject == actionValueObject);
                    Assert.That(tcValueObject.EnumValues == actionValueObject.EnumValues);
                }
            }

            Assert.That(tcr.ConditionTable.Columns.Count == tcr.TestCases.Count + 1);
            Assert.That(tcr.ConditionTable.Columns.Count == tcr.TestCases.Count + 1);
            Assert.That(tcr.ActionTable.Columns.Count == tcr.TestCases.Count + 1);
            Assert.That(tcr.ActionTable.Columns.Count == tcr.TestCases.Count + 1);

            for (int rowIndex = 0; rowIndex < tcr.ConditionTable.Rows.Count; rowIndex++)
            {
                DataRow rowView = tcr.ConditionTable.Rows[rowIndex];
                Assert.That(rowView.ItemArray.Length == tcr.TestCases.Count + 1);

                Assert.That(rowView[0] is ConditionObject);
                Assert.That(rowView[0] == tcr.Conditions[rowIndex]);
                for (int colIndex = 1; colIndex < tcr.ConditionTable.Columns.Count; colIndex++)
                {
                    ValueObject vo = rowView[colIndex] as ValueObject;
                    TestCase tc = tcr.TestCases[colIndex - 1];
                    Assert.That(vo == tc.Conditions[rowIndex]);
                }
            }

            for (int rowIndex = 0; rowIndex < tcr.ActionTable.Rows.Count; rowIndex++)
            {
                DataRow rowView = tcr.ActionTable.Rows[rowIndex];
                Assert.That(rowView.ItemArray.Length == tcr.TestCases.Count + 1);

                Assert.That(rowView[0] is ActionObject);
                Assert.That(rowView[0] == tcr.Actions[rowIndex]);
                for (int colIndex = 1; colIndex < tcr.ActionTable.Columns.Count; colIndex++)
                {
                    ValueObject vo = rowView[colIndex] as ValueObject;
                    TestCase tc = tcr.TestCases[colIndex - 1];
                    Assert.That(vo == tc.Actions[rowIndex]);
                }
            }
        }

        public static string TestCaseToString(TestCase tc)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ValueObject cond in tc.Conditions)
            {
                sb.AppendFormat("{1}({0})  ", cond.Value, cond.ConditionOrActionParent.Name);
            }
            foreach (ValueObject cond in tc.Actions)
            {
                sb.AppendFormat("{1}({0})  ", cond.Value, cond.ConditionOrActionParent.Name);
            }
            return sb.ToString();
        }
    }
}

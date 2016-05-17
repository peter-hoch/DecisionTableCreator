using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.DynamicTable;
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
                Assert.That(tc.Name.Equals(tcr.ActionTable.Columns[testCaseIndex + 1].Name));
                Assert.That(tc.Name.Equals(tcr.ActionTable.Columns[testCaseIndex + 1].DisplayName));
                Assert.That(tc.Name.Equals(tcr.ConditionTable.Columns[testCaseIndex + 1].Name));
                Assert.That(tc.Name.Equals(tcr.ConditionTable.Columns[testCaseIndex + 1].DisplayName));

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
            Assert.That(tcr.ConditionTable.ColumnPropDescColl.Count == tcr.TestCases.Count + 1);
            Assert.That(tcr.ActionTable.Columns.Count == tcr.TestCases.Count + 1);
            Assert.That(tcr.ActionTable.ColumnPropDescColl.Count == tcr.TestCases.Count + 1);

            for (int rowIndex = 0; rowIndex < tcr.ConditionTable.Rows.Count; rowIndex++)
            {
                DataRowView rowView = tcr.ConditionTable.Rows[rowIndex];
                Assert.That(rowView.ColumnCount == tcr.TestCases.Count + 1);

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
                DataRowView rowView = tcr.ActionTable.Rows[rowIndex];
                Assert.That(rowView.ColumnCount == tcr.TestCases.Count + 1);

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
    }
}

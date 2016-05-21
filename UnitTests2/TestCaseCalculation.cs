using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTableCreator.TestCases;
using NUnit.Framework;
using UnitTestSupport;

namespace UnitTests2
{
    [TestFixture]
    public class TestCaseCalculation
    {
        [SetUp]
        public void Setup()
        {
            TestSupport.DiffAction = new InvokeWinMerge();
        }


        [TestCase(2, -1, -1, -1, 2)]
        [TestCase(1, -1, -1, -1, 1)]
        [TestCase(1, 0, -1, -1, 1)]
        [TestCase(1, 0, -1, 0, 0)]
        [TestCase(1, 0, 0, -1, 0)]
        [TestCase(1, 0, 0, 0, 0)]
        [TestCase(5, 0, 0, 0, 4)]
        [TestCase(5, 0, 0, 1, 3)]
        [TestCase(0, 0, 0, 0, 0)]
        public void TestCalculationEnumValues(int count, int defaultIndex, int invalidIndex, int dontCareIndex, int expectedResult)
        {
            var condition = ConditionObject.Create("name", TestCasesRoot.CreateSampleEnum("name", count, defaultIndex, invalidIndex, dontCareIndex));
            int result = TestCasesRoot.CalculateEnumValuesWithoutDontCareAndInvalid(condition);
            Assert.That(result == expectedResult);
        }

        [Test]
        public void TestCalculationConditions()
        {
            TestCasesRoot tcr = new TestCasesRoot();
            int expectedResult = 0;
            int result = tcr.CalculatePossibleCombinations();
            Assert.That(result == expectedResult);

            expectedResult = 4;
            tcr.Conditions.Add(ConditionObject.Create("name", TestCasesRoot.CreateSampleEnum("name", 4, 0, 0, 3))); 
            tcr.Conditions.Add(ConditionObject.Create("name", TestCasesRoot.CreateSampleEnum("name", 4, 0, 0, 3)));
            result = tcr.CalculatePossibleCombinations();
            Assert.That(result == expectedResult);

            // 2 invalids
            var cond = ConditionObject.Create("name", TestCasesRoot.CreateSampleEnum("name", 6, 0, 0, 3));
            cond.EnumValues[5].IsInvalid = true;
            tcr.Conditions.Add(cond);
            expectedResult *= 3;
            result = tcr.CalculatePossibleCombinations();
            Assert.That(result == expectedResult);

            cond = ConditionObject.Create("name", TestCasesRoot.CreateSampleEnum("name", 2, 0, 0, 1));
            tcr.Conditions.Add(cond);
            result = tcr.CalculatePossibleCombinations();
            Assert.That(result == expectedResult);

            cond = ConditionObject.Create("name", TestCasesRoot.CreateSampleEnum("name", 1, 0, 0, 0));
            tcr.Conditions.Add(cond);
            result = tcr.CalculatePossibleCombinations();
            Assert.That(result == expectedResult);

            cond = ConditionObject.Create("name", TestCasesRoot.CreateSampleEnum("name", 0, 0, 0, 0));
            tcr.Conditions.Add(cond);
            result = tcr.CalculatePossibleCombinations();
            Assert.That(result == expectedResult);

            cond = ConditionObject.Create("name", TestCasesRoot.CreateSampleEnum("name", 2, 0, -1, -1));
            tcr.Conditions.Add(cond);
            expectedResult *= 2;
            result = tcr.CalculatePossibleCombinations();
            Assert.That(result == expectedResult);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    public class TestCaseCalcContainer
    {
        public ITestCase TestCase { get; private set; }

        public int CoveredCount { get; set; }

        public TestCaseCalcContainer(ITestCase testCase, int coveredCount)
        {
            TestCase = testCase;
            CoveredCount = coveredCount;
        }
    }
}

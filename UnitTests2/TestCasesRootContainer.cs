using DecisionTableCreator.TestCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests2
{
    public class TestCasesRootContainer : IDisposable
    {
        public TestCasesRoot TestCasesRoot { get; set; }

        public int ConditionChangeCount { get; set; }

        public int ActionChangeCount { get; set; }
    

        public TestCasesRootContainer()
        {
            TestCasesRoot = TestCasesRoot.CreateSimpleTable();
            TestCasesRoot.ActionsChanged += TestCasesRootOnActionsChanged;
            TestCasesRoot.ConditionsChanged += TestCasesRootOnConditionsChanged;
        }

        private void TestCasesRootOnConditionsChanged()
        {
            ConditionChangeCount++;
        }

        private void TestCasesRootOnActionsChanged()
        {
            ActionChangeCount++;
        }

        public void Dispose()
        {
            TestCasesRoot.ActionsChanged -= TestCasesRootOnActionsChanged;
            TestCasesRoot.ConditionsChanged -= TestCasesRootOnConditionsChanged;
        }
    }
}

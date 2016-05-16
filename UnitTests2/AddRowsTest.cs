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
    public class AddRowsTest
    {
        [SetUp]
        public void Setup()
        {
            //TestSupport.DiffAction = new InvokeWinMerge();     
        }

        [Test]
        public void TestAddCondition()
        {
            string firstPath = Path.Combine(TestSupport.CreatedFilesDirectory, "first.xml");
            string secondPath = Path.Combine(TestSupport.CreatedFilesDirectory, "second.xml");
            TestCasesRoot tcr = TestCasesRoot.CreateSimpleTable();
            tcr.Save(firstPath);

            tcr.AppendCondition(ConditionObject.Create("new condition", new ObservableCollection<EnumValue>() {new EnumValue("new test", "new value")}));

            tcr.Save(secondPath);

            TestSupport.CompareFile(firstPath, secondPath);
        }
    }
}

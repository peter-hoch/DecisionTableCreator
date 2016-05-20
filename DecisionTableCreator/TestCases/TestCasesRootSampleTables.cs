using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    public partial class TestCasesRoot
    {
        public static TestCasesRoot CreateSampleTable()
        {
            TestCasesRoot testCasesRoot = new TestCasesRoot();
            testCasesRoot.CreateSampleTableInternal();

            return testCasesRoot;
        }

        /// <summary>
        /// used for unit tests
        /// </summary>
        /// <returns></returns>
        public static TestCasesRoot CreateSimpleTable()
        {
            TestCasesRoot testCasesRoot = new TestCasesRoot();
            testCasesRoot.CreateSimpleTableInternal();

            return testCasesRoot;
        }

        public static TestCasesRoot CreatePrinterTrubbleshootingSample()
        {
            TestCasesRoot testCasesRoot = new TestCasesRoot();
            testCasesRoot.CreatePrinterTrubbleshootingSampleInternal();
            return testCasesRoot;
        }

        private void CreateSampleTableInternal()
        {
            int testCasesCount = 20;
            int conditionCount = 5;
            int actionCount = 5;

            List<ObservableCollection<EnumValue>> lists = new List<ObservableCollection<EnumValue>>();
            for (int list = 0; list < (conditionCount + actionCount); list++)
            {
                var subList = new ObservableCollection<EnumValue>();
                lists.Add(subList);
                bool isInvalid = true;
                for (int idx = 1; idx < 10; idx++)
                {
                    subList.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", list, idx), isInvalid));
                    isInvalid = false;
                }
            }

            int listIndex = 0;
            for (int idx = 1; idx < conditionCount; idx++)
            {
                Conditions.Add(ConditionObject.Create(String.Format("Condition {0}", idx), lists[listIndex++]));
            }

            Actions.Add(ActionObject.Create(String.Format("Action1{0}", 0), ValueDataType.Bool));
            for (int idx = 1; idx < actionCount; idx++)
            {
                Actions.Add(ActionObject.Create(String.Format("Action1{0}", idx), lists[listIndex++]));
            }

            CreateBasicColumnDescriptions();

            for (int idx = 0; idx < testCasesCount; idx++)
            {
                AddTestCase();
            }

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
        }

        private void CreateSimpleTableInternal()
        {
            int testCasesCount = 6;
            int conditionCount = 3;
            int actionCount = 3;

            List<ObservableCollection<EnumValue>> lists = new List<ObservableCollection<EnumValue>>();
            int listNumber = 0;
            int enumIdx = 0;
            var subListSample = new ObservableCollection<EnumValue>();
            lists.Add(subListSample);
            subListSample.Add(new EnumValue(String.Format("{0}-Invalid-{1}-Default", listNumber, enumIdx++), true, false, true));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-Don' t care-{1}", listNumber++, enumIdx++), false, true, false));
            enumIdx = 0;
            subListSample = new ObservableCollection<EnumValue>();
            lists.Add(subListSample);
            subListSample.Add(new EnumValue(String.Format("{0}-Invalid-{1}", listNumber, enumIdx++), true, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}-Default", listNumber, enumIdx++), false, false, true));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-Don' t care-{1}", listNumber++, enumIdx++), false, true, false));
            enumIdx = 0;
            subListSample = new ObservableCollection<EnumValue>();
            lists.Add(subListSample);
            subListSample.Add(new EnumValue(String.Format("{0}-Invalid-{1}", listNumber, enumIdx++), true, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-Don' t care-{1}-Default", listNumber++, enumIdx++), false, true, true));
            enumIdx = 0;
            subListSample = new ObservableCollection<EnumValue>();
            lists.Add(subListSample);
            subListSample.Add(new EnumValue(String.Format("{0}-Invalid-{1}-Default", listNumber, enumIdx++), true, false, true));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-Don' t care-{1}", listNumber++, enumIdx++), false, true, false));
            enumIdx = 0;
            subListSample = new ObservableCollection<EnumValue>();
            lists.Add(subListSample);
            subListSample.Add(new EnumValue(String.Format("{0}-Invalid-{1}", listNumber, enumIdx++), true, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}-Default", listNumber, enumIdx++), false, false, true));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-Don' t care-{1}", listNumber++, enumIdx++), false, true, false));
            enumIdx = 0;
            subListSample = new ObservableCollection<EnumValue>();
            lists.Add(subListSample);
            subListSample.Add(new EnumValue(String.Format("{0}-Invalid-{1}", listNumber, enumIdx++), true, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-EnumValue-{1}", listNumber, enumIdx++), false, false, false));
            subListSample.Add(new EnumValue(String.Format("{0}-Don' t care-{1}-Default", listNumber++, enumIdx++), false, true, true));

            int listIndex = 0;
            for (int idx = 1; idx <= conditionCount; idx++)
            {
                Conditions.Add(ConditionObject.Create(String.Format("Condition {0}", idx), lists[listIndex++]));
            }

            for (int idx = 1; idx <= actionCount; idx++)
            {
                Actions.Add(ActionObject.Create(String.Format("Action1{0}", idx), lists[listIndex++]));
            }

            CreateBasicColumnDescriptions();

            for (int idx = 0; idx < testCasesCount; idx++)
            {
                TestCase tc = AddTestCase();
                tc.DisplayIndex = idx + 1;
                foreach (ValueObject value in tc.Conditions)
                {
                    value.SelectedItemIndex = idx % value.EnumValues.Count;
                }
                foreach (ValueObject value in tc.Actions)
                {
                    value.SelectedItemIndex = idx % value.EnumValues.Count;
                }
            }

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
        }

        private void CreatePrinterTrubbleshootingSampleInternal()
        {
            var printerEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue("Yes"), new EnumValue("No"), };
            Conditions.Add(ConditionObject.Create(String.Format("Printer does not print"), printerEnum));

            var ledEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue("Yes"), new EnumValue("No"), };
            Conditions.Add(ConditionObject.Create("An error led is flashing", ledEnum));

            var unrecognizedEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue("Yes"), new EnumValue("No"), };
            Conditions.Add(ConditionObject.Create("Printer is unrecognized", unrecognizedEnum));

            var checkPowerEnum = new ObservableCollection<EnumValue>() { new EnumValue("", true), new EnumValue(""), new EnumValue("X") };
            Actions.Add(ActionObject.Create(String.Format("Check the power cable"), checkPowerEnum));

            var checkPinterCableEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(ActionObject.Create(String.Format("Check the printer-computer cable"), checkPinterCableEnum));

            var softwareInstalledEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(ActionObject.Create(String.Format("Enshure printer software is installed"), softwareInstalledEnum));

            var checkInkEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(ActionObject.Create(String.Format("Check/replace ink"), checkInkEnum));

            var checkPaperJamEnum = new ObservableCollection<EnumValue>() { new EnumValue(""), new EnumValue("X") };
            Actions.Add(ActionObject.Create(String.Format("Check for paper jam"), checkPaperJamEnum));

            CreateBasicColumnDescriptions();

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
        }

    }
}

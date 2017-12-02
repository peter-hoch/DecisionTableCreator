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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    public partial class TestCasesRoot
    {

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

        public void CreateSampleProject()
        {
            Init();
            CreateSampleProjectInternal();
            CreateInfosForDatagrid();
            FireActionsChanged();
            ProcessConditionsChanged();
            FireStatisticsChanged();
            FireDirtyChanged();
        }

        public void CreateTestProject()
        {
            Init();
            CreateTestProjectInternal();
            CreateInfosForDatagrid();
            FireActionsChanged();
            ProcessConditionsChanged();
            FireStatisticsChanged();
            FireDirtyChanged();
        }


        public static ObservableCollection<EnumValue> CreateSampleEnum(string name, int count, int defaultIndex, int invalidIndex, int dontCareIndex)
        {
            ObservableCollection<EnumValue> list = new ObservableCollection<EnumValue>();

            for (int idx = 0; idx < count; idx++)
            {
                EnumValue ev = new EnumValue(name + "-" + idx, idx == invalidIndex, idx == dontCareIndex, idx == defaultIndex);
                list.Add(ev);
            }

            return list;
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

            CreateBasicColumnDescriptions();
            AddColumnDescriptionsForTestCases();
            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            RecalculateStatistics();
        }

        private void CreateSampleProjectInternal()
        {
            var enumList = new ObservableCollection<EnumValue>();
            enumList.Add(new EnumValue("invalid condition", true, false, true, "this is an invalid condition please choose an other value"));
            enumList.Add(new EnumValue("Printer is printing", false, false, false, "everything is ok"));
            enumList.Add(new EnumValue("Printer is not printing", false, false, false, "problems with printing or printing quality"));
            enumList.Add(new EnumValue("DC", false, true, false, "don't care this condition in this case"));

            Conditions.Add(ConditionObject.Create("Basic Printer status", enumList));

            enumList = new ObservableCollection<EnumValue>();
            enumList.Add(new EnumValue("invalid condition", true, false, true, "this is an invalid condition please choose an other value"));
            enumList.Add(new EnumValue("LED is on", false, false, false, "Power LED is on"));
            enumList.Add(new EnumValue("LED is flashing", false, false, false, "Power LED is flashing"));
            enumList.Add(new EnumValue("LED is off", false, false, false, "Power LED is off"));
            enumList.Add(new EnumValue("DC", false, true, false, "don't care this condition in this case"));

            Conditions.Add(ConditionObject.Create("Green LED", enumList));

            enumList = new ObservableCollection<EnumValue>();
            enumList.Add(new EnumValue("invalid condition", true, false, true, "this is an invalid condition please choose an other value"));
            enumList.Add(new EnumValue("Blank sheet is ejected", false, false, false, "Printer ejects only blank sheets"));
            enumList.Add(new EnumValue("Print quality is bad", false, false, false, "Printer output has bad quality"));
            enumList.Add(new EnumValue("Nothing is ejected", false, false, false, "Printer don't eject a sheet"));
            enumList.Add(new EnumValue("DC", false, true, false, "don't care this condition in this case"));

            Conditions.Add(ConditionObject.Create("Paper", enumList));


            enumList = new ObservableCollection<EnumValue>();
            enumList.Add(new EnumValue("", false, false, true, "nothing to do"));
            enumList.Add(new EnumValue("Check connection", false, false, false, "Check the printer power connection"));

            Actions.Add(ActionObject.Create("Power", enumList));

            enumList = new ObservableCollection<EnumValue>();
            enumList.Add(new EnumValue("", false, false, true, "nothing to do"));
            enumList.Add(new EnumValue("Check filling level", false, false, false, "Check the fill level of the ink cartridges"));

            Actions.Add(ActionObject.Create("Ink catridge", enumList));

            enumList = new ObservableCollection<EnumValue>();
            enumList.Add(new EnumValue("", false, false, true, "nothing to do"));
            enumList.Add(new EnumValue("Check connection", false, false, false, "Check the printer computer connection"));

            Actions.Add(ActionObject.Create("PC connection", enumList));

            enumList = new ObservableCollection<EnumValue>();
            enumList.Add(new EnumValue("", false, false, true, "nothing to do"));
            enumList.Add(new EnumValue("Check paper filling level", false, false, false, "Does the printer contain enough paper?"));
            enumList.Add(new EnumValue("Check for clean (unused) paper", false, false, false, "Does the printer contain blank paper?"));

            Actions.Add(ActionObject.Create("Paper", enumList));

            int idx = 1;
            TestCase tc = AddTestCase();
            tc.DisplayIndex = idx++;
            SetSelectedItemIndex(tc, new int[] { 1, 4, 4 }, new int[0]);
            tc.Description = "This is the description of TestCase 1";

            tc = AddTestCase();
            tc.DisplayIndex = idx++;
            SetSelectedItemIndex(tc, new int[] { 2, 3, 4 }, new int[] { 1 });
            tc.Description = "This is the description of TestCase 2";

            tc = AddTestCase();
            tc.DisplayIndex = idx++;
            SetSelectedItemIndex(tc, new int[] { 2, 2, 4 }, new int[] { 0, 1, 0, 1 });
            tc.Description = "This is the description of TestCase 3";

            tc = AddTestCase();
            tc.DisplayIndex = idx++;
            SetSelectedItemIndex(tc, new int[] { 2, 1, 1 }, new int[] { 0, 1, 0, 0 });
            tc.Description = "This is the description of TestCase 4";

            tc = AddTestCase();
            tc.DisplayIndex = idx++;
            SetSelectedItemIndex(tc, new int[] { 2, 1, 2 }, new int[] { 0, 1, 0, 2 });
            tc.Description = "This is the description of TestCase 5";

            tc = AddTestCase();
            tc.DisplayIndex = idx++;
            SetSelectedItemIndex(tc, new int[] { 2, 1, 3 }, new int[] { 0, 0, 1, 1 });
            tc.Description = "This is the description of TestCase 6";

            this.Description = "This is the description of the sample project";
        }

        void SetSelectedItemIndex(TestCase tc, int[] conditions, int[] actions)
        {
            int idx = 0;
            foreach (int condition in conditions)
            {
                tc.Conditions[idx++].SelectedItemIndex = condition;
            }
            idx = 0;
            foreach (int action in actions)
            {
                tc.Actions[idx++].SelectedItemIndex = action;
            }
        }

        private void CreateTestProjectInternal()
        {
            int testCasesCount = 20;
            int conditionCount = 8;
            int actionCount = 8;

            List<ObservableCollection<EnumValue>> lists = new List<ObservableCollection<EnumValue>>();

            for (int idx = 0; idx < conditionCount; idx++)
            {
                CreateConditionEnum(lists, conditionCount, testCasesCount, "Cond");
            }
            for (int idx = 0; idx < actionCount; idx++)
            {
                CreateConditionEnum(lists, actionCount, testCasesCount, "Action");
            }

            int listIndex = 0;
            for (int idx = 1; idx <= conditionCount; idx++)
            {
                Conditions.Add(ConditionObject.Create(String.Format("Condition {0}", idx), lists[listIndex++]));
            }

            for (int idx = 1; idx <= actionCount; idx++)
            {
                Actions.Add(ActionObject.Create(String.Format("Action {0}", idx), lists[listIndex++]));
            }

            CreateBasicColumnDescriptions();

            for (int idx = 0; idx < testCasesCount; idx++)
            {
                TestCase tc = AddTestCase();
                tc.DisplayIndex = idx + 1;
                for (int condIdx = 0; condIdx < conditionCount; condIdx++)
                {
                    tc.Conditions[condIdx].SelectedItemIndex = idx*conditionCount+condIdx+1;
                }
                for (int actionIdx = 0; actionIdx < actionCount; actionIdx++)
                {
                    tc.Actions[actionIdx].SelectedItemIndex = idx * actionCount + actionIdx + 1;
                }
            }

            PopulateRows(ConditionTable, Conditions, TestCases, TestCase.CollectionType.Conditions);
            PopulateRows(ActionTable, Actions, TestCases, TestCase.CollectionType.Actions);
            RecalculateStatistics();

        }

        private static void CreateConditionEnum(List<ObservableCollection<EnumValue>> lists, int conditionCount, int testCasesCount, string prefix)
        {
            ObservableCollection<EnumValue> subListSample = new ObservableCollection<EnumValue>();
            lists.Add(subListSample);
            subListSample.Add(new EnumValue(String.Format("Invalid-Default"), true, false, true));
            for (int tcIdx = 1; tcIdx <= testCasesCount; tcIdx++)
            {
                for (int idx = 1; idx < conditionCount + 1; idx++)
                {
                    subListSample.Add(new EnumValue(String.Format("{0}-{1}-TestCase-{2}", prefix, idx, tcIdx), false, false, false));
                }
            }
            subListSample.Add(new EnumValue(String.Format("Don' t care"), false, true, false));
        }
    }
}

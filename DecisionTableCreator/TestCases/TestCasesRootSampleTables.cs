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


        public static ObservableCollection<EnumValue> CreateSampleEnum(string name, int count, int defaultIndex, int invalidIndex, int dontCareIndex)
        {
            ObservableCollection<EnumValue> list = new ObservableCollection<EnumValue>();

            for (int idx = 0; idx < count; idx++)
            {
                EnumValue ev = new EnumValue(name+"-"+idx, idx==invalidIndex, idx==dontCareIndex, idx==defaultIndex);
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
            RecalculateStatistics();
        }


    }
}

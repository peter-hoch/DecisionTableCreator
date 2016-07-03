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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTableCreator.TestCases
{
    public class XmlNames
    {
        public static readonly string DecisionTableRootName = "DecisionTableRoot";
        public static readonly string ConditionsName = "Conditions";
        public static readonly string ActionsName = "Actions";
        public static readonly string ConditionName = "Condition";
        public static readonly string ActionName = "Action";
        public static readonly string EnumValuesName = "EnumValues";
        public static readonly string EnumValueName = "EnumValue";
        public static readonly string TestCaseName = "TestCase";
        public static readonly string TestCasesName = "TestCases";
        public static readonly string TestCaseConditionsName = "TestCaseConditions";
        public static readonly string TestCaseActionsName = "TestCaseActions";
        public static readonly string TestCaseValueName = "TestCaseValue";

        public static readonly string TextAttributeName = "Text";
        public static readonly string TypeAttributeName = "Type";
        public static readonly string DefaultTextAttributeName = "DefaultText";
        public static readonly string DefaultBoolAttributeName = "DefaultBool";
        public static readonly string TooltipTextAttributeName = "TooltipText";
        public static readonly string BoolAttributeName = "Bool";
        public static readonly string SelectedItemIndexAttributeName = "SelectedItemIndex";

        public static readonly string NameAttributeName = "Name";
        public static readonly string ValueAttributeName = "Value";
        public static readonly string IsDefaultAttributeName = "IsDefault";
        public static readonly string IsInvalidAttributeName = "IsInvalid";
        public static readonly string DontCareAttributeName = "DontCare";
        public static readonly string IdAttributeName= "LoadSaveId";
        //public static readonly string AttributeName = "Name";
    }
}

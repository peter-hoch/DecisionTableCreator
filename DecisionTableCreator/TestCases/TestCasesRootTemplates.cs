﻿/*
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
using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Misc;

namespace DecisionTableCreator.TestCases
{
    public partial class TestCasesRoot
    {
        public StringTemplateResult GenerateFromtemplate(string groupFilePath)
        {
            InitData();
            TemplateGroup group = new TemplateGroupFile(groupFilePath);
            var errorListener = new CustomStringTemplateErrorListener();
            group.ErrorManager = new ErrorManager(errorListener);
            Template templ = group.GetInstanceOf("TestCasesRoot");
            templ.Add("root", this);
            return new StringTemplateResult(errorListener, templ.Render());
        }

        public StringTemplateResult GenerateFromTemplateString(string groupFileContent)
        {
            InitData();
            TemplateGroup group = new TemplateGroupString(groupFileContent);
            var errorListener = new CustomStringTemplateErrorListener();
            group.ErrorManager = new ErrorManager(errorListener);
            Template templ = group.GetInstanceOf("TestCasesRoot");
            templ.Add("root", this);
            return new StringTemplateResult(errorListener, templ.Render());
        }

       
        void InitData(int cellWidth = 3000)
        {
            RtfCellOffset = cellWidth;
            int offset = cellWidth;
            foreach (TestCase testCase in TestCases)
            {
                offset += cellWidth;
                testCase.RtfCellOffset = offset;
            }

            foreach (ConditionObject condition in Conditions)
            {
                condition.RtfCellOffset = offset = cellWidth;
                foreach (ValueObject value in condition.TestValues)
                {
                    offset += cellWidth;
                    value.RtfCellOffset = offset;
                }
            }
            foreach (ActionObject action in Actions)
            {
                action.RtfCellOffset = offset = cellWidth;
                foreach (ValueObject value in action.TestValues)
                {
                    offset += cellWidth;
                    value.RtfCellOffset = offset;
                }
            }
        }
    }
}

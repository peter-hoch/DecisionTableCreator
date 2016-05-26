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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestSupport
{
    public class InvokeWinMerge : IInvokeDiffAction
    {
        public string ToolPath { get; private set; }

        public InvokeWinMerge(string toolPath = @"C:\Program Files (x86)\WinMerge\WinMergeU.exe")
        {
            ToolPath = toolPath;
        }

        /// <summary>
        /// start the diff tool if the tools is available
        /// if one file does not exits - create an empty file in order to make the first compare possible
        /// </summary>
        /// <param name="filePath1"></param>
        /// <param name="filePath2"></param>
        public void ExecuteDiffAction(string filePath1, string filePath2)
        {
            if (!File.Exists(ToolPath))
            {
                throw new FileNotFoundException("diff tool not found", ToolPath);
            }

            if (!File.Exists(filePath1))
            {
                File.WriteAllText(filePath1, "");
            }
            if (!File.Exists(filePath2))
            {
                File.WriteAllText(filePath2, "");
            }
            Process process = Process.Start(ToolPath, string.Format("\"{0}\" \"{1}\"", filePath1, filePath2));
        }
    }
}

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

        public void ExecuteDiffAction(string filePath1, string filePath2)
        {
            if (!File.Exists(ToolPath))
            {
                throw new FileNotFoundException("diff tool not found", ToolPath);
            }
            Process process = Process.Start(ToolPath, string.Format("\"{0}\" \"{1}\"", filePath1, filePath2));
        }
    }
}

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

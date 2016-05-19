using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTestSupport
{
    public class TestSupport
    {
        private const string ReferenceFilesDirectoryName = "ReferenceFiles";
        private const string TestFilesDirectoryName = "TestFiles";
        private const string CreatedFilesDirectoryName = "CreatedFiles";

        public static IInvokeDiffAction DiffAction { get; set; }

        public static String WorkDirectory
        {
            get
            {
                return TestContext.CurrentContext.WorkDirectory;
            }
        }

        /// <summary>
        ///  assume the name of the project directory is the same name as the namespace name
        /// </summary>
        public static string ProjectDir
        {
            get
            {
                string className = TestContext.CurrentContext.Test.ClassName;
                string namespaceName = className.Substring(0, className.IndexOf(".", StringComparison.InvariantCulture));
                string testDir = TestContext.CurrentContext.TestDirectory;
                string path = testDir.Substring(0, testDir.LastIndexOf(namespaceName, StringComparison.InvariantCultureIgnoreCase) + namespaceName.Length);
                return path;
            }
        }


        /// <summary>
        /// the reference directory is in the project directory
        /// create the directory if it does not exist
        /// </summary>
        public static string ReferenceFilesDirectory
        {
            get
            {
                string dir = Path.Combine(ProjectDir, ReferenceFilesDirectoryName, TestContext.CurrentContext.Test.MethodName);
                Directory.CreateDirectory(dir);
                return dir;
            }
        }


        /// <summary>
        /// the test files directory is in the project directory
        /// create the directory if it does not exist
        /// </summary>
        public static string TestFilesDirectory
        {
            get
            {
                string dir = Path.Combine(ProjectDir, TestFilesDirectoryName, TestContext.CurrentContext.Test.MethodName);
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

        /// <summary>
        /// the test files directory is in the project directory
        /// create the directory if it does not exist
        /// </summary>
        public static string TestFilesBaseDirectory
        {
            get
            {
                string dir = Path.Combine(ProjectDir, TestFilesDirectoryName);
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

        /// <summary>
        /// the created files directory is in the project directory
        /// create the directory if it does not exist
        /// </summary>
        public static string CreatedFilesDirectory
        {
            get
            {
                string dir = Path.Combine(ProjectDir, CreatedFilesDirectoryName, TestContext.CurrentContext.Test.MethodName);
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

        /// <summary>
        /// the created files directory is in the project directory
        /// create the directory if it does not exist
        /// </summary>
        public static string CreatedFilesBaseDirectory
        {
            get
            {
                string dir = Path.Combine(ProjectDir, CreatedFilesDirectoryName);
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

        /// <summary>
        /// the reference directory is in the project directory
        /// </summary>
        public static void ClearCreatedFiles()
        {
            RemoveAllFiles(CreatedFilesBaseDirectory);
            RemoveDirectoriesRecursive(CreatedFilesBaseDirectory);
        }

        private static void RemoveAllFiles(string directory)
        {
            foreach (string file in Directory.EnumerateFiles(directory, "*").ToArray())
            {
                File.Delete(file);
            }
        }

        private static void RemoveDirectoriesRecursive(string directory)
        {
            foreach (string dir in Directory.EnumerateDirectories(directory).ToArray())
            {
                RemoveAllFiles(dir);
                RemoveDirectoriesRecursive(dir);
                Directory.Delete(dir);
            }
        }

        /// <summary>
        /// compare two files
        /// </summary>
        /// <param name="createdFilesDir">directory of created file</param>
        /// <param name="referenceFilesDir">directory of reference file</param>
        /// <param name="file">the file name</param>
        /// <returns></returns>
        public static bool CompareFile(string createdFilesDir, string referenceFilesDir, string file)
        {
            return CompareFile(Path.Combine(createdFilesDir, file), Path.Combine(referenceFilesDir, file));
        }


        public static bool CompareFile(string createdFilePath, string referenceFilePath)
        {
            try
            {
                CompareFileInternal(createdFilePath, referenceFilePath);
                return true;
            }
            catch (FileCompareException ex)
            {
                Console.WriteLine(ex.Message);
                Trace.WriteLine(ex.Message);
                RunDiffTool(createdFilePath, referenceFilePath);
            }
            return false;
        }

        internal static void CompareFileInternal(string createdFilePath, string referenceFilePath)
        {
            FileInfo fi1 = new FileInfo(createdFilePath);
            FileInfo fi2 = new FileInfo(referenceFilePath);

            if (!fi1.Exists)
            {
                throw new FileCompareException("the file " + fi1.FullName + " does not exist");
            }
            if (!fi2.Exists)
            {
                throw new FileCompareException("the file " + fi2.FullName + " does not exist");
            }

            if (fi1.Length != fi2.Length)
            {
                throw new FileCompareException("the files " + fi1.FullName + " and " + fi2.FullName + " differ in length");
            }

            using (FileStream fs1 = new FileStream(createdFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader br1 = new BinaryReader(fs1))
                {
                    using (FileStream fs2 = new FileStream(referenceFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (BinaryReader br2 = new BinaryReader(fs2))
                        {
                            for (int idx = 0; idx < fs1.Length; idx++)
                            {
                                if (br1.ReadByte() != br2.ReadByte())
                                {
                                    throw new FileCompareException("the files " + fi1.FullName + " and " + fi2.FullName + " differ at position " + idx);
                                }
                            }
                        }
                    }

                }
            }
        }

        private static void RunDiffTool(string createdFilePath, string referenceFilePath)
        {
            DiffAction?.ExecuteDiffAction(createdFilePath, referenceFilePath);
        }
    }

}

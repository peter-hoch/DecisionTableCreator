using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTestSupport
{
    [TestFixture]
    public class TestSupportUnitTests
    {
        [SetUp]
        public void Setup()
        {
            TestSupport.DiffAction = null;
        }

        [Test, Property("MyTestProperty", "StringValue")]
        public void Test100()
        {

            var prop = TestContext.CurrentContext.Test.Properties["MyTestProperty"];
            Debug.WriteLine(TestContext.CurrentContext.Test.Properties["MyTestProperty"]);

        }


        [TestCase(-1, -1, "does not exist")]
        [TestCase(0, -1, "does not exist")]
        [TestCase(-1, 0, "does not exist")]
        [TestCase(1, 0, "differ in length")]
        [TestCase(0, 1, "differ in length")]
        [TestCase(10, 11, "differ in length")]
        [TestCase(11, 10, "differ in length")]
        public void TestFileCompare(int filesize1, int filesize2, string expectedExceptionMessageContainment)
        {
            string name = "TestFile.txt";
            string filePath1 = Path.Combine(TestSupport.TestFilesDirectory, name);
            string filePath2 = Path.Combine(TestSupport.CreatedFilesDirectory, name);
            File.Delete(filePath1);
            File.Delete(filePath2);
            CreateFile(filePath1, filesize1);
            CreateFile(filePath2, filesize2);

            TestSupport.DiffAction = null;
            Assert.That(() => { TestSupport.CompareFileInternal(filePath1, filePath2); }, Throws.TypeOf<FileCompareException>().With.Message.Contains(expectedExceptionMessageContainment));
            Assert.That(TestSupport.CompareFile(filePath1, filePath2) == false);
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.TestFilesDirectory, name) == false);
        }

        [TestCase("0123456789", "x123456789", "differ at position 0")]
        [TestCase("x123456789", "0123456789", "differ at position 0")]
        [TestCase("0123456789", "0x23456789", "differ at position 1")]
        [TestCase("0x23456789", "0123456789", "differ at position 1")]
        [TestCase("012345678x", "0123456789", "differ at position 9")]
        [TestCase("0123456789", "012345678x", "differ at position 9")]
        [TestCase("012345678x0123456789", "01234567890123456789", "differ at position 9")]
        [TestCase("01234567890123456789", "012345678x0123456789", "differ at position 9")]
        public void TestFileCompareDifferInContent(string content1, string content2, string expectedExceptionMessageContainment)
        {
            string name = "TestFile.txt";
            string filePath1 = Path.Combine(TestSupport.TestFilesDirectory, name);
            string filePath2 = Path.Combine(TestSupport.CreatedFilesDirectory, name);
            File.Delete(filePath1);
            File.Delete(filePath2);
            File.WriteAllText(filePath1, content1);
            File.WriteAllText(filePath2, content2);

            Assert.That(() => { TestSupport.CompareFileInternal(filePath1, filePath2); }, Throws.TypeOf<FileCompareException>().With.Message.Contains(expectedExceptionMessageContainment));
            Assert.That(TestSupport.CompareFile(filePath1, filePath2) == false);
            Assert.That(TestSupport.CompareFile(TestSupport.CreatedFilesDirectory, TestSupport.TestFilesDirectory, name) == false);
        }

        [TestCase("01234567890123456789", "01234567890123456789")]
        public void TestFileCompare(string content1, string content2)
        {
            string filePath1 = Path.Combine(TestSupport.CreatedFilesDirectory, "TestFile1.txt");
            string filePath2 = Path.Combine(TestSupport.CreatedFilesDirectory, "TestFile2.txt");
            File.Delete(filePath1);
            File.Delete(filePath2);
            File.WriteAllText(filePath1, content1);
            File.WriteAllText(filePath2, content2);
            Assert.That(TestSupport.CompareFile(filePath1, filePath2));
        }

        [Ignore("execute manually - diff toll is started")]
        [TestCase("01234567890123456789", "01234567890123456789x", Description = "WinDiff should open")]
        public void TestFileCompareWithDifftool(string content1, string content2)
        {
            TestSupport.DiffAction = new InvokeWinMerge();

            string filePath1 = Path.Combine(TestSupport.CreatedFilesDirectory, "TestFile1.txt");
            string filePath2 = Path.Combine(TestSupport.CreatedFilesDirectory, "TestFile2.txt");
            File.Delete(filePath1);
            File.Delete(filePath2);
            File.WriteAllText(filePath1, content1);
            File.WriteAllText(filePath2, content2);
            Assert.That(TestSupport.CompareFile(filePath1, filePath2) == false);

        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(121)]
        public void TestCreateString(int size)
        {
            Assert.That(CreateString(size).Length == size);
        }

        [TestCase(-1, 0, false)]
        [TestCase(0, 0, true)]
        [TestCase(1, 1, true)]
        [TestCase(101, 101, true)]
        public void TestCreateFile(int size, int expectedLength, bool fileExists)
        {
            string path = Path.Combine(TestSupport.CreatedFilesDirectory, "TestFile1.txt");
            if(File.Exists(path))
            {
                File.Delete(path);
            }
            CreateFile(path, size);
            FileInfo fi = new FileInfo(path);
            Assert.That(fi.Exists == fileExists);
            if (fileExists)
            {
                Assert.That(fi.Length == expectedLength);
            }
        }

        public static void CreateFile(string path, int size)
        {
            if (size >= 0)
            {
                File.WriteAllText(path, CreateString(size));
            }
        }


        public static string CreateString(int size)
        {
            string text = "";
            if (size > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int idx = 0; idx <= (size / 10); idx++)
                {
                    sb.Append("0123456789");
                }
                return sb.ToString().Substring(0, size);
            }
            return text;
        }

        [Test]
        public void VerifyDirectories()
        {
            var project = TestSupport.ProjectDir;
            var refDir = TestSupport.ReferenceFilesDirectory;
            var createdDir = TestSupport.CreatedFilesDirectory;

            // check if this is the project directory
            Assert.That(File.Exists(Path.Combine(project, "UnitTestSupport.csproj")));

            Assert.That(Directory.Exists(project));
            Assert.That(Directory.Exists(refDir));
            Assert.That(Directory.Exists(createdDir));
        }


        [Test]
        public void CreateAndDeleteTestFiles()
        {
            CreateTestFiles(TestSupport.CreatedFilesDirectory, 0);
            Assert.That(Directory.EnumerateFiles(TestSupport.CreatedFilesDirectory).Count() > 9);
            TestSupport.ClearCreatedFiles();
            int count = Directory.EnumerateDirectories(TestSupport.CreatedFilesBaseDirectory).Count();
            Assert.That(Directory.EnumerateDirectories(TestSupport.CreatedFilesBaseDirectory).Count() == 0);

            CreateTestFiles(TestSupport.CreatedFilesDirectory, 3);
            int files = Directory.EnumerateDirectories(TestSupport.CreatedFilesDirectory).Count();
            Assert.That(files > 0);
            TestSupport.ClearCreatedFiles();
            Assert.That(Directory.EnumerateDirectories(TestSupport.CreatedFilesBaseDirectory).Count() == 0);
        }

        void CreateTestFiles(string dir, int levels)
        {
            if (levels > 0)
            {
                string subDir = Path.Combine(dir, "Subdir");
                Directory.CreateDirectory(subDir);
                CreateTestFiles(subDir, levels-1);
            }
            for (int idx = 0; idx < 10; idx++)
            {
                File.WriteAllText(Path.Combine(dir, "file" + idx + ".txt"), "this is a test");
            }

        }
    }
}

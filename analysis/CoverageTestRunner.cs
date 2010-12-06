using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using FaultLocalization;

namespace FaultLocalization
{
    public class CoverageTestRunner : AbstractTestRunner
    {
        private const string visualStudioKey = "\\SOFTWARE\\Microsoft\\VisualStudio";
        private static string[] visualStudioVersions = { "10.0", "9.0", "8.0", "7.1" };
        private const string installDirKey = "InstallDir";

        private const string allTestsFilename = "allTests.bat";
        private const string individualTestsFilename = "individualTests.bat";

        private const string allTestsCommand = "mstest /testcontainer:\"{0}\" /runconfig:\"{1}\" /resultsfile:\"{2}\"";
        private const string individualTestCommand = "mstest /testcontainer:\"{0}\" /runconfig:\"{1}\" /test:\"{2}\"";

        private static string vsInstallDir;

        public CoverageTestRunner(TestSuite testSuite)
            :base(testSuite)
        {

        }

        /// <summary>
        /// Decides which tests need to be run based on the change date for the test dlls compared to the test results files
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, bool> DetermineWhichTestsNeedToBeRun()
        {
            var needsToRun = new Dictionary<string, bool>();
            foreach (string testDllPath in tests.TestDllPaths)
            {
                var coveredDlls = tests.CoveredDllPaths(testDllPath);
                var coveredDllModDates = from coveredDll in coveredDlls
                                         select File.GetLastWriteTimeUtc(coveredDll);
                DateTime testDllModDate = File.GetLastWriteTimeUtc(testDllPath);
                DateTime allTestResultsModDate = File.GetLastWriteTimeUtc(tests.AllTestsResultsFile(testDllPath));
                DateTime individualTestsModDate = File.GetLastWriteTimeUtc(tests.TestResultsDirectory(testDllPath));
                
                if (testDllModDate > allTestResultsModDate)
                {
                    needsToRun.Add(testDllPath, true);
                }
                else/* if (coveredDllModDates.Any(date => date > individualTestsModDate))*/
                {
                    needsToRun.Add(testDllPath, false);
                }
            }
            return needsToRun;
        }

        public override IEnumerable<TestResult> RunTests()
        {
            var testDllsToRun = DetermineWhichTestsNeedToBeRun();
            GetVisualStudioPathFromRegistry();
            
            foreach (string testDllPath in testDllsToRun.Keys)
            {
                string AllTestsResultsPath = tests.AllTestsResultsFile(testDllPath);
                string projectName = Path.GetFileNameWithoutExtension(testDllPath); 
                if (testDllsToRun[testDllPath] && File.Exists(AllTestsResultsPath))
                {                    
                    Console.Out.WriteLine("Tests have changed for project " + projectName + ".  All tests must be re-run");
                    File.Delete(AllTestsResultsPath);
                }
                
                if (!File.Exists(AllTestsResultsPath))
                {
                    RunAllTests(testDllPath);
                }
               
                Console.Out.WriteLine("The software under test for project " + projectName + " has changed.  Individual tests must be re-run");
                    
                ClearTestResultsDirectory(testDllPath);
                RunIndividualTests(testDllPath);
            }
			return tests.TestResults.Select<ExecutedTest, TestResult>(e => e);
        }

        private void ClearTestResultsDirectory(string testDllPath)
        {
            var testResultsDirectory = tests.TestResultsDirectory(testDllPath);
            if (Directory.Exists(testResultsDirectory))
            {
                Directory.Delete(testResultsDirectory, true);
            }
            Directory.CreateDirectory(testResultsDirectory);
        }

        private static void GetVisualStudioPathFromRegistry()
        {
            Console.Out.WriteLine("Searching for Visual Studio installation...");
            RegistryKey software = Registry.LocalMachine.OpenSubKey("SOFTWARE");
            RegistryKey microsoft = software.OpenSubKey("Microsoft");
            RegistryKey vs = microsoft.OpenSubKey("VisualStudio");
            
            if (vs != null)
            {
                foreach (string versionString in visualStudioVersions)
                {
                    RegistryKey key = vs.OpenSubKey(versionString);
                    if (key != null)
                    {
                        vsInstallDir = (string)key.GetValue(installDirKey);
                        if (vsInstallDir != null)
                        {
                            Console.Out.WriteLine("Visual Studio " + versionString + " installation found at " + vsInstallDir);
                            return;
                        }
                    }
                }
            }
            throw new FileNotFoundException("Visual Studio installation not found");
        }

        private void RunAllTests(String testDllPath)
        {
            Console.WriteLine("Starting initial run of all tests in " + testDllPath + "...");
            string AllTestsPath = Path.Combine(tests.SolutionDirectory, allTestsFilename);

            File.Delete(AllTestsPath);
            FileStream batFile = File.Open(AllTestsPath,FileMode.OpenOrCreate,FileAccess.Write);
            TextWriter batFileWriter = new StreamWriter(batFile);
            
            String dllFileName = Path.GetFileName(testDllPath) + ".trx";
            string command = String.Format(allTestsCommand, testDllPath, tests.TestRunConfigPath, dllFileName);
            batFileWriter.WriteLine(command);
            
            batFileWriter.Close();

            int ExitCode = RunBatchScriptSynchronous(AllTestsPath, tests.TestResultsBase(testDllPath));
            if (ExitCode > 1)
            {
                throw new ApplicationException("mstest exited with status " + ExitCode + " while running command:" + allTestsFilename);
            }

            File.Delete(AllTestsPath);
        }

        private void RunIndividualTests(string TestDllPath)
        {
            Console.Out.WriteLine("Generating individual test case coverage for tests in " + TestDllPath + "...");
            string AllTestsResultPath = tests.AllTestsResultsFile(TestDllPath);
            String text = File.ReadAllText(AllTestsResultPath);
            text = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(text));
            text = new string(text.Where(t => t != 4).ToArray());
            XDocument xDoc = XDocument.Parse(text);
            XNamespace ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
            var unitTests = from unitTest in xDoc.Descendants(ns + "UnitTest")
                            select new
                            {
                                Title = unitTest.Attribute("name").Value,
                            };

            Console.Out.WriteLine(unitTests.Count() + " tests discovered");

            string IndividualTestsPath = Path.Combine(tests.SolutionDirectory, individualTestsFilename);
            FileStream batFile = File.OpenWrite(IndividualTestsPath);
            TextWriter batFileWriter = new StreamWriter(batFile);
            foreach (var unitTest in unitTests)
            {
                batFileWriter.WriteLine(String.Format(individualTestCommand, TestDllPath, tests.TestRunConfigPath, unitTest.Title));
            }
            batFileWriter.Close();
            Console.Out.Flush();
            int ExitCode = RunBatchScriptSynchronous(IndividualTestsPath, tests.TestResultsBase(TestDllPath));

            if (ExitCode > 1)
            {
                throw new ApplicationException("mstest exited with status " + ExitCode + " while running individual tests for:" + AllTestsResultPath);
            }

            File.Delete(IndividualTestsPath);
        }

        private int RunBatchScriptSynchronous(string command, string workingDirectory)
        {
            Process proc = new Process();
            String file = Path.GetFileNameWithoutExtension(command);
            proc.StartInfo.FileName = command;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.WorkingDirectory = workingDirectory;
            proc.StartInfo.EnvironmentVariables["PATH"] += ";" + vsInstallDir;
            proc.StartInfo.UseShellExecute = false;
            //proc.StartInfo.RedirectStandardOutput = true;
            
            proc.Start();
            proc.WaitForExit();
            return proc.ExitCode;
        }

		public override TestResult RunTest(string TestName)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<string> TestNames
		{
			get { return tests.TestResults.Select(e => e.Name); }
		}
	}
}

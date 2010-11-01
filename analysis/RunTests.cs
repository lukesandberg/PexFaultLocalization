using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace FaultLocalization
{
    class TestRunner
    {
        private const string visualStudioKey = "\\SOFTWARE\\Microsoft\\VisualStudio";
        private static string[] visualStudioVersions = { "10.0", "9.0", "8.0", "7.1" };
        private const string installDirKey = "InstallDir";

        private const string allTestsFilename = "allTests.bat";
        private const string individualTestsFilename = "individualTests.bat";

        private const string allTestsCommand = "mstest /testcontainer:\"{0}\" /runconfig:\"{1}\" /resultsfile:\"{2}\"";
        private const string individualTestCommand = "mstest /testcontainer:\"{0}\" /runconfig:\"{1}\" /test:\"{2}\" /resultsfile:\"{2}\".trx";

        private static string vsInstallDir;

        
        private TestSuite tests;
        

        public TestRunner(TestSuite testSuite)
        {
            tests = testSuite;
        }

        

        public void RunTests()
        {
            GetVisualStudioPathFromRegistry();
            CreateTestResultsDirectory();
            foreach (string testDllPath in tests.TestDllPaths)
            {
                string AllTestsResultsPath = getResultPathFromDllName(testDllPath);
                if (!File.Exists(AllTestsResultsPath))
                {
                    RunAllTests(testDllPath);
                }
                RunIndividualTests(testDllPath);
            }
        }

        private string getResultPathFromDllName(string testDllPath)
        {
            string allTestResultsFilename = Path.GetFileName(testDllPath) + ".trx";
            string AllTestsResultsPath = Path.Combine(tests.TestResultsDirectory, allTestResultsFilename);
            return AllTestsResultsPath;
        }

        private void CreateTestResultsDirectory()
        {
            if (!File.Exists(tests.TestResultsDirectory))
            {
                Directory.CreateDirectory(tests.TestResultsDirectory);
            }
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

            int ExitCode = RunBatchScriptSynchronous(AllTestsPath);
            if (ExitCode != 0)
            {
                throw new ApplicationException("mstest exited with status " + ExitCode + " while running command:" + allTestsFilename);
            }

            File.Delete(AllTestsPath);
        }

        private void RunIndividualTests(string TestDllPath)
        {
            Console.Out.WriteLine("Generating individual test case coverage for tests in " + TestDllPath + "...");
            string AllTestsResultPath = getResultPathFromDllName(TestDllPath);
            XDocument xDoc = XDocument.Load(AllTestsResultPath);
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
            int ExitCode = RunBatchScriptSynchronous(IndividualTestsPath);

            if (ExitCode != 0)
            {
                throw new ApplicationException("mstest exited with status " + ExitCode + " while running individual tests for:" + AllTestsResultPath);
            }

            File.Delete(individualTestsFilename);
        }

        private int RunBatchScriptSynchronous(string command)
        {
            Process proc = new Process();
            String file = Path.GetFileNameWithoutExtension(command);
            proc.StartInfo.FileName = command + "> " + file + ".log";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.WorkingDirectory = tests.TestResultsDirectory;
            proc.StartInfo.EnvironmentVariables["PATH"] += ";" + vsInstallDir;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.WaitForExit();
            return proc.ExitCode;
        }




    }
}

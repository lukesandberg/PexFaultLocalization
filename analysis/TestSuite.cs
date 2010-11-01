using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace FaultLocalization
{
	public class TestSuite
    {
#region "Constants"
        private const string testFrameworkName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
#endregion


        #region "Constructor"

        public TestSuite(String dir)
        {
            SolutionDirectory = Path.GetFullPath(dir);
        }

        #endregion


        #region "Properties"

        public String SolutionDirectory { get; private set; }

        private string testRunConfigPath;
        public string TestRunConfigPath
        {
            get
            {
                if (testRunConfigPath == null)
                {
                    testRunConfigPath = GetPathToFileType("testrunconfig");
                    Console.Out.WriteLine("Using " + TestRunConfigPath + " for test run configuration");
                }
                
                return testRunConfigPath;
            }
        }
        private string testMetadataPath;
        public string TestMetadataPath
        {
            get
            {
                if (testMetadataPath == null)
                {
                    testMetadataPath = GetPathToFileType("vsmdi");
                    Console.Out.WriteLine("Using " + testMetadataPath + " for test metadata");
                }
                return testMetadataPath;
            }
        }

        private string solutionPath;
        public string SolutionPath
        {
            get
            {
                if (solutionPath == null)
                {
                    solutionPath = GetPathToFileType("sln");
                    Console.Out.WriteLine("Using " + solutionPath + " for solution");
                }
                return solutionPath;
            }
        }

        private List<string> projectFiles;
        public IEnumerable<string> ProjectFilePaths {
            get {
                if (projectFiles == null)
                {
                    projectFiles = new List<string>();
                    using (TextReader reader = new StreamReader(SolutionPath))
                    {
                        var projectLines = from line in reader.Lines()
                                           where line.Contains(".csproj")
                                           select line;
                        foreach (string projectLine in projectLines)
                        {
                            string[] tokens = projectLine.Split(",".ToCharArray());
                            string projectFile = tokens[1];
                            projectFile = projectFile.Replace("\"", "");
                            projectFile = projectFile.Trim();
                            projectFiles.Add(Path.Combine(SolutionDirectory, projectFile));
                        }
                    }
                }
                return projectFiles;
            }
        }

        public List<string> testDllPaths;
        public IEnumerable<string> TestDllPaths
        {
            get
            {
                if (testDllPaths == null)
                {
                    testDllPaths = new List<string>();
                    foreach (string projectFile in ProjectFilePaths)
                    {
                        XDocument projectDoc = XDocument.Load(projectFile);
                        XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
                        var testFrameworkReferences = from reference in projectDoc.Descendants(ns + "Reference")
                                                      where reference.Attribute("Include") != null && reference.Attribute("Include").Value.Contains( testFrameworkName)
                                                      select reference;
                        if (testFrameworkReferences.Count() != 0)
                        {
                            IEnumerable<string> OutputDirs = from propertyGroup in projectDoc.Descendants(ns + "PropertyGroup")
                                                             where propertyGroup.Element(ns + "DebugSymbols") != null && 
                                                             propertyGroup.Element(ns + "DebugSymbols").Value == "true"
                                                             select propertyGroup.Element(ns + "OutputPath").Value;
                            string OutputDir = OutputDirs.FirstOrDefault();
                            string AssemblyName = projectDoc.Descendants(ns + "AssemblyName").FirstOrDefault().Value;
                            string dllPath = Path.Combine(OutputDir, AssemblyName + ".dll");
                            string projectFolder = Path.GetDirectoryName(projectFile);
                            testDllPaths.Add(Path.Combine(projectFolder, dllPath));
                        }
                    }
                }
                return testDllPaths;
            }
        }


        private string GetPathToFileType(string fileType)
        {
            Console.Out.WriteLine("Scanning for ." + fileType + " files...");
            string[] files = Directory.GetFiles(SolutionDirectory, "*." + fileType);
            if (files.Length == 0)
            {
                Console.Out.WriteLine("Could not find ." + fileType + " in project directory " + SolutionDirectory);
                throw new FileNotFoundException("Could not find ." + fileType + " in project directory " + SolutionDirectory);
            }
            else
            {
                return Path.Combine(SolutionDirectory, files.First());
            }
        }

        public String TestResultsDirectory
        {
            get
            {
                return Path.Combine(SolutionDirectory, "TestResults");
            }
        }

        #endregion

        private IChain<ExecutedTest> cached = null;
		public IEnumerable<ExecutedTest> TestResults
		{
			get
			{
				if(cached == null)
				{

					var query = from file in Directory.GetFiles(TestResultsDirectory, "*.trx")
								select ExecutedTest.CreateTest(file);

					cached = Chains.CreateLazy(query);
				}
				return cached;
			}
		}

	}
}

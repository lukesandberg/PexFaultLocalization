using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using FaultLocalization.Util;

namespace FaultLocalization
{
	public class TestSuite
	{
		#region "Constants"
		private const string testFrameworkName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
		private const string testProjectGuid = "3AC096D0-A1C2-E12C-1390-A8335801FDAB";
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
				if(testRunConfigPath == null)
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
				if(testMetadataPath == null)
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
				if(solutionPath == null)
				{
					solutionPath = GetPathToFileType("sln");
					Console.Out.WriteLine("Using " + solutionPath + " for solution");
				}
				return solutionPath;
			}
		}

		private List<string> projectFiles;
		public IEnumerable<string> ProjectFilePaths
		{
			get
			{
				if(projectFiles == null)
				{
					using(TextReader reader = new StreamReader(SolutionPath))
					{
						projectFiles = (from line in reader.Lines()
										where line.Contains(".csproj")
										let projectFile = line.Split(",".ToCharArray())[1]
																.Replace("\"", "")
																.Trim()
										select Path.Combine(SolutionDirectory, projectFile)).ToList();

					}
				}
				return projectFiles;
			}
		}

		/// <summary>
		/// The path to each test dll in this solution
		/// </summary>
		public IEnumerable<string> TestDllPaths
		{
			get
			{
				if(testProjectGuids == null)
				{
					initializeDllPaths();
				}
				return dllPaths.Where(dll => testProjectGuids.Contains(dll.Key)).Select(dll => dll.Value);
			}
		}

		/// <summary>
		/// Returns the paths to dlls from projects that are covered by the tests at the dll path provided.
		/// </summary>
		/// <param name="testDllPath">The full path to a test dll</param>
		/// <returns>the paths to dlls from projects that are covered by the tests at the dll path provided.</returns>
		public IEnumerable<string> CoveredDllPaths(string testDllPath)
		{
			if(coveredProjectGuids == null)
			{
				initializeDllPaths();
			}
			string testDllGuid = dllPaths.Where(dll => dll.Value == testDllPath).First().Key;
			IEnumerable<string> coveredGuids = coveredProjectGuids[testDllGuid];
			return dllPaths.Where(dll => coveredGuids.Contains(dll.Key)).Select(dll => dll.Value);
		}

		private Dictionary<string, string> dllPaths;
		private IDictionary<string, IEnumerable<string>> coveredProjectGuids;
		private List<string> testProjectGuids;

		public List<string> TestProjectGuids
		{
			get
			{
				if(testProjectGuids == null)
				{
					initializeDllPaths();
				}
				return testProjectGuids;
			}
		}

		private void initializeDllPaths()
		{
			testProjectGuids = new List<string>();
			dllPaths = new Dictionary<string, string>();
			coveredProjectGuids = new Dictionary<string, IEnumerable<string>>();
			foreach(string projectFile in ProjectFilePaths)
			{
				XDocument projectDoc = XDocument.Load(projectFile);
				XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
				IEnumerable<string> OutputDirs = from propertyGroup in projectDoc.Descendants(ns + "PropertyGroup")
												 where propertyGroup.Element(ns + "DebugSymbols") != null &&
												 propertyGroup.Element(ns + "DebugSymbols").Value == "true"
												 select propertyGroup.Element(ns + "OutputPath").Value;

				string OutputDir = OutputDirs.FirstOrDefault();
				string AssemblyName = projectDoc.Descendants(ns + "AssemblyName").FirstOrDefault().Value;
				string ProjectGuid = projectDoc.Descendants(ns + "ProjectGuid").FirstOrDefault().Value;
				string dllPath = Path.Combine(OutputDir, AssemblyName + ".dll");
				string projectFolder = Path.GetDirectoryName(projectFile);
				dllPaths.Add(ProjectGuid, Path.Combine(projectFolder, dllPath));

				var ProjectTypeNode = projectDoc.Descendants(ns + "ProjectTypeGuids").FirstOrDefault();
				string ProjectTypeGuid;
				if(ProjectTypeNode != null)
				{
					ProjectTypeGuid = ProjectTypeNode.Value;
				}
				else
				{
					ProjectTypeGuid = string.Empty;
				}

				if(ProjectTypeGuid.Contains(testProjectGuid))
				{
					var CoveredProjects = from projectRef in projectDoc.Descendants(ns + "ProjectReference")
										  select projectRef.Element(ns + "Project").Value;
					coveredProjectGuids.Add(ProjectGuid, CoveredProjects);

					testProjectGuids.Add(ProjectGuid);
				}
			}
		}


		private string GetPathToFileType(string fileType)
		{
			Console.Out.WriteLine("Scanning for ." + fileType + " files...");
			string[] files = Directory.GetFiles(SolutionDirectory, "*." + fileType);
			if(files.Length == 0)
			{
				Console.Out.WriteLine("Could not find ." + fileType + " in project directory " + SolutionDirectory);
				throw new FileNotFoundException("Could not find ." + fileType + " in project directory " + SolutionDirectory);
			}
			else
			{
				return Path.Combine(SolutionDirectory, files.First());
			}
		}

		public String TestResultsBase(string testDllPath)
		{
			string testDllFileName = Path.GetFileNameWithoutExtension(testDllPath);
			return Path.Combine(SolutionDirectory, "TestResults-" + testDllFileName);
		}

		public String TestResultsDirectory(string testDllPath)
		{
			return Path.Combine(TestResultsBase(testDllPath), "TestResults");
		}

		public string AllTestsResultsFile(string testDllPath)
		{
			string allTestResultsFilename = Path.GetFileName(testDllPath) + ".trx";
			string AllTestsResultsPath = Path.Combine(TestResultsBase(testDllPath), allTestResultsFilename);
			return AllTestsResultsPath;
		}

		#endregion

		private IChain<ExecutedTest> cached = null;
		public IEnumerable<ExecutedTest> TestResults
		{
			get
			{
				if(cached == null)
				{
					var files = new List<string>();
					foreach(string testDll in TestDllPaths)
					{
						var resultsDir = TestResultsDirectory(testDll);
						var trxFiles = Directory.GetFiles(resultsDir, "*.trx");
						files.AddRange(trxFiles);
					}

					var query = from file in files
								select ExecutedTest.CreateTest(file);

					cached = Chains.CreateLazy(query);
				}
				return cached;
			}
		}
	}
}

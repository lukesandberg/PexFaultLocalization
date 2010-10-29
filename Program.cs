using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Coverage.Analysis;
using System.IO;
using System.Xml.Linq;

namespace FaultLocalization
{
	class UnitTest
	{
		public String Name { get; set; }
		public Guid ID { get; set; }
		public bool Result { get; set; }
		public String ResultsFile { get; set; }

	}

	class Program
	{
		static void Main(string[] args)
		{
			if(args.Length < 1)
			{
				Console.WriteLine("Usage: FaultLocalization.exe <TestResultsDir>");
				return;
			}
			String TestResultsPath = args[0];
			Console.WriteLine("Searching " + TestResultsPath + "...");
			foreach(var file in Directory.GetFiles(TestResultsPath, "*.trx"))
			{
				Console.WriteLine("Parsing: " + file + "...");
				XDocument trx = XDocument.Load(file);
				var rf_path = (from res in trx.Descendants("ResultFile")
	  						select res.Attribute("path").Value).First();

				var test = trx.Descendants("UnitTest")
					.Select(x => new UnitTest
									{
										Name = x.Attribute("Name").Value,
										ID = Guid.Parse(x.Attribute("id").Value)
									}).First();
				test.ResultsFile = rf_path;
				test.Result = trx.Descendants("UnitTestResult")
					.Where(r => r.Attribute("testId").Value.Equals(test.ID.ToString()))
					.Select(r => r.Attribute("outcome").Equals("Passed")).First();
				String dir_name = Path.GetFileNameWithoutExtension(file);
				ProcessUnitTest(Path.Combine(TestResultsPath, dir_name), test);
			}
		}

		private static void ProcessUnitTest(String dir, UnitTest test)
		{
			String cov_file = Path.Combine(dir, "In", test.ResultsFile);
			String assemblies = Path.Combine(dir, "Out");
			var coverage = CoverageInfo.CreateFromFile(cov_file, new string[] { assemblies }, new string[] { assemblies });
			var data = coverage.BuildDataSet(null);//parses the file
			foreach(var c in data.Class)
			{
				Console.WriteLine(c.NamespaceTableRow.ModuleRow.ModuleName + "::" + c.NamespaceTableRow.NamespaceName + "::" + c.ClassName);
				foreach(var m in c.GetMethodRows())
				{
					Console.WriteLine(m.MethodFullName + " (" + m.LinesCovered + "/" + (m.LinesCovered + m.LinesNotCovered + m.LinesPartiallyCovered));
				}
			}
		}
	}
}

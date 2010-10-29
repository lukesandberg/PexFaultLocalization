using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Coverage.Analysis;
using System.IO;
using System.Xml.Linq;

namespace FaultLocalization
{
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

			var tests = from file in Directory.GetFiles(TestResultsPath, "*.trx")
						let trx = XDocument.Load(file)
						let xmlns = XNamespace.Get(trx.Elements().First().Attribute("xmlns").Value)
						let ut_el = trx.Descendants(xmlns + "UnitTest").First()
						let id = Guid.Parse(ut_el.Attribute("id").Value)
						let rf_path = trx.Descendants(xmlns + "ResultFile").First().Attribute("path").Value
						let dir_name = Path.Combine(TestResultsPath, Path.GetFileNameWithoutExtension(file))

						select new ExecutedTest(id, ut_el.Attribute("name").Value, trx.Descendants(xmlns + "UnitTestResult")
									.Where(r => r.Attribute("testId").Value.Equals(id.ToString()))
									.First().Attribute("outcome").Value.Equals("Passed"), Path.Combine(dir_name, "In", rf_path), Path.Combine(dir_name, "Out"));

			foreach(var test in tests)
			{
				Console.WriteLine(test);
			}
		
			Console.Read();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace FaultLocalization
{
	public class TestSuite
	{
		public String TestResultDirectory { get; private set; }
		public TestSuite(String dir)
		{
			TestResultDirectory = dir;
		}

		private IChain<ExecutedTest> cached = null;
		public IEnumerable<ExecutedTest> Tests
		{
			get
			{
				if(cached == null)
				{
					var query = from file in Directory.GetFiles(TestResultDirectory, "*.trx")
								let trx_doc = XDocument.Load(file)
								let xmlns = XNamespace.Get(trx_doc.Elements().First().Attribute("xmlns").Value)
								let unitTestElement = trx_doc.Descendants(xmlns + "UnitTest").First()
								let id = Guid.Parse(unitTestElement.Attribute("id").Value)
								let resultDirName = Path.Combine(TestResultDirectory, Path.GetFileNameWithoutExtension(file))
								let testName = unitTestElement.Attribute("name").Value
								let result = trx_doc.Descendants(xmlns + "UnitTestResult")
											.Where(r => r.Attribute("testId").Value.Equals(id.ToString()))
											.First().Attribute("outcome").Value.Equals("Passed")
								let coverageFileLocation = Path.Combine(resultDirName, "In", trx_doc.Descendants(xmlns + "ResultFile").First().Attribute("path").Value)
								let assemblyLocation = Path.Combine(resultDirName, "Out")
								select new ExecutedTest(id, testName, result, coverageFileLocation, assemblyLocation);
					cached = LazyChain<ExecutedTest>.Create(query);
				}
				return cached;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Coverage.Analysis;
using System.Xml.Linq;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace FaultLocalization
{
	public class ExecutedTest
	{
		public static ExecutedTest CreateTest(String trxFileLocation)
		{
			var trx_doc = XDocument.Load(trxFileLocation);
			var xmlns = XNamespace.Get(trx_doc.Elements().First().Attribute("xmlns").Value);
			var unitTestElement = trx_doc.Descendants(xmlns + "UnitTest").First();
			var id = Guid.Parse(unitTestElement.Attribute("id").Value);
			var resultDirName = Path.Combine(Path.GetDirectoryName(trxFileLocation), Path.GetFileNameWithoutExtension(trxFileLocation));
			var testName = unitTestElement.Attribute("name").Value;
			var unitTestResultElement = trx_doc.Descendants(xmlns + "UnitTestResult")
											.Where(r => r.Attribute("testId").Value.Equals(id.ToString())).First();
			var passed = unitTestResultElement.Attribute("outcome").Value.Equals("Passed");

			var coverageFileLocation = Path.Combine(resultDirName, "In", trx_doc.Descendants(xmlns + "ResultFile").First().Attribute("path").Value);
			var assemblyLocation = Path.Combine(resultDirName, "Out");
			ExecutedTest rval = new ExecutedTest();
			rval.Result = passed;
			rval.ID = id;
			rval.Name = testName;
			rval.CoverageFileLocation = coverageFileLocation;
			rval.AssembliesLocation = assemblyLocation;

			if(!passed)
			{
				var errorInfo = unitTestResultElement.Descendants(xmlns + "ErrorInfo").First();
				rval.ErrorMessage = errorInfo.Descendants(xmlns + "Message").Select(n => n.Value).First();
				rval.stackTraceString = errorInfo.Descendants(xmlns + "StackTrace").Select(n => n.Value).First();
	
			}
			return rval;
		}

		public String Name { get; private set; }
		public Guid ID { get; private set; }
		public String CoverageFileLocation { get; private set; }//"IN/HCU-..../data.coverage
		public String AssembliesLocation { get; private set; }//out/

		public bool Result { get; private set; }

		private CoverageInfo _info = null;
		public CoverageInfo CoverageInfo
		{
			get
			{
				if(_info == null)
				{
					_info = CoverageInfo.CreateFromFile(CoverageFileLocation, new string[] { AssembliesLocation }, new string[] { AssembliesLocation });
				}
				return _info;
			}
		}

		private CoverageDS _data;
		public CoverageDS CoverageData
		{
			get
			{
				if(_data == null)
				{
					_data = CoverageInfo.BuildDataSet();
				}
				return _data;
			}
		}
		protected ExecutedTest() { }

		public IEnumerable<CoverageDSPriv.ClassRow> Classes
		{
			get
			{
				return CoverageData.Class.Where(c => c.BlocksCovered > 0);
			}
		}

		public IEnumerable<CoverageDSPriv.MethodRow> Methods
		{
			get
			{
				return CoverageData.Method.Where(m => m.BlocksCovered > 0);
			}
		}

		public IEnumerable<CoverageDSPriv.LinesRow> Lines
		{
			get
			{
				return CoverageData.Lines.Where(l => (CoverageStatus) l.Coverage == CoverageStatus.Covered || (CoverageStatus) l.Coverage == CoverageStatus.PartiallyCovered);
			}
		}

		public String ErrorMessage { get; private set; }

		private IChain<StackTraceElement> cached;
		private String stackTraceString;
		public IEnumerable<StackTraceElement> StackTrace
		{
			get
			{
				if(cached == null)
				{
					StringReader reader = new StringReader(stackTraceString);
					var query = from line in reader.Lines()
								select new StackTraceElement(line, CoverageData);
					cached = Chains.CreateLazy(query);
				}
				return cached;
			}
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("Test: " + Name + " " + (Result ? "Passed" : "Failed"));

			var data = CoverageData.Class.Where(c => c.BlocksCovered > 0)
				.Select(c => c.GetMethodRows().Where(m => m.BlocksCovered > 0)
							.Aggregate(c.NamespaceTableRow.NamespaceName + "::" + c.ClassName + "\n"
							, (p, meth) => p + "\t" + meth.MethodName + "(" + meth.BlocksCovered + "/" + (meth.BlocksCovered + meth.BlocksNotCovered) + ")\n"));
			foreach(var line in data)
				str.AppendLine(line);
			return str.ToString();
		}
	}
}

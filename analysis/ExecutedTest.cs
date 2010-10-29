using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace FaultLocalization
{
	class ExecutedTest
	{
		public String Name { get; private set; }
		public Guid ID { get; private set; }
		public bool Result { get; private set; }
		public String CoverageFileLocation { get; private set; }
		public String AssembliesLocation { get; private set; }

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
		
		
		public ExecutedTest(Guid id, String name, bool res, String CovLoc, String AssembLoc)
		{
			ID = id;
			Name = name;
			Result = res;
			CoverageFileLocation = CovLoc;
			AssembliesLocation = AssembLoc;
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

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
								select ExecutedTest.CreateTest(file);

					cached = Chains.CreateLazy(query);
				}
				return cached;
			}
		}

	}
}

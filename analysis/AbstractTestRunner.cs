using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace FaultLocalization
{
    public abstract class AbstractTestRunner : ITestRunner
    {
        protected TestSuite tests;

        public AbstractTestRunner(TestSuite testSuite)
        {
            // <pex>
            Debug.Assert(testSuite != (TestSuite)null, "testSuite");
            // </pex>
            tests = testSuite;
        }

		public abstract IEnumerable<TestResult> RunTests();
		public abstract TestResult RunTest(string TestName);
		public abstract IEnumerable<string> TestNames { get; }
	}
}

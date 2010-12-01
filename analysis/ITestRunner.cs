using System;
using System.Collections.Generic;
namespace FaultLocalization
{
	public sealed class TestResult 
	{
		private readonly String name;
		public String TestName { get {return name;}}

		private readonly bool passed;
		public bool Passed { get { return passed; } }

		public TestResult(String n, bool b)
		{
			name = n;
			passed = b;
		}
	}
	
    public interface ITestRunner
    {
		IEnumerable<String> TestNames { get; }
        IEnumerable<TestResult> RunTests();
		TestResult RunTest(String TestName);
    }
}

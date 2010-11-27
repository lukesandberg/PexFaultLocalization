using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using UTCore;

namespace FaultLocalization
{
    public class ReflectionTestRunner : AbstractTestRunner
    {
		private bool Initialized = false;
        private UTCore.TestRunner TestRunner { get; set; }

        public ReflectionTestRunner(TestSuite testSuite)
            : base(testSuite)
        {
            TestRunner = new UTCore.TestRunner();
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        }

        public override IEnumerable<TestResult> RunTests()
        {
			EnsureInitialized();
			List<TestResult> results = DoRunTests();
			return results;
        }

		private List<TestResult> DoRunTests()
		{
			List<TestResult> results = new List<TestResult>();
			TestNotificationDelegate handler = delegate(UTCore.TestAttribute ta)
			{
				bool res = false;
				switch(ta.State)
				{
					case TestAttribute.TestState.Fail:
						res = false;
						break;
					case TestAttribute.TestState.Pass:
						res = true;
						break;
					default:
						return;
				}
				var name = GetTestName(ta);
				TestResult tr = new TestResult(name, res);
				results.Add(tr);
			};
			TestRunner.testNotificationEvent += handler;
			TestRunner.RunTests();
			TestRunner.testNotificationEvent -= handler;
			return results;
		}

		private void EnsureInitialized()
		{
			if(!Initialized)
			{
				foreach(string testDllPath in tests.TestDllPaths)
				{
					TestRunner.LoadAssembly(testDllPath);
				}
				TestRunner.ParseAssemblies();
				Initialized = true;
			}
		}


		private static string GetTestName(TestAttribute ta)
		{
			return ta.TestClass.FullName + ":" + ta.TestMethod.ToString();
		}

		public override TestResult RunTest(string TestName)
		{
			EnsureInitialized();
			EnableSingleTest(TestName);

			var res = DoRunTests();
			EnableAllTests();
			return res.First();
		}
		
		private void EnableAllTests()
		{
			foreach(var t in TestRunner.Tests)
			{
				t.TestMethod.Ignore = true;
			}
		}

		private void EnableSingleTest(string TestName)
		{
			foreach(var t in TestRunner.Tests)
			{
				if(GetTestName(t).Equals(TestName))
				{
					t.TestMethod.Ignore = false;
				}
				else
				{
					t.TestMethod.Ignore = true;
				}
			}
		}

		public override IEnumerable<string> TestNames
		{
			get 
			{ 
				EnsureInitialized(); 
				return TestRunner.Tests.Select(ta => GetTestName(ta)); 
			}
		}
	}
}

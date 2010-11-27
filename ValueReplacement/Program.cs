using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FaultLocalization;
using ValueInjector;

namespace ValueReplacement
{
	class Program
	{
		static void Main(string[] args)
		{
			string sln = @"D:\Documents and Settings\212059614\Desktop\PexFaultLocalization\strings\Strings.sln";
			string test_proj_name = "Strings.Tests";
			CodeRewriter rewriter = new CodeRewriter(sln, test_proj_name);
			rewriter.Rewrite();

			TestSuite tests = new TestSuite(Path.GetDirectoryName(sln));
			ReflectionTestRunner runner = new ReflectionTestRunner(tests);
			RunFaultLocalization(runner);
			Console.ReadLine();
		}

		
		static void RunFaultLocalization(ReflectionTestRunner runner)
		{
			var Tests = runner.TestNames.ToList();
			Instrumenter.IsSaving = true;
			List<String> FailingTests = new List<String>();
			foreach(var test in Tests)
			{
				Instrumenter.CurrentTestName = test;
				var res = runner.RunTest(test);
				if(!res.Passed)
				{
					FailingTests.Add(test);
				}
			}
			ValueInjector.Instrumenter.IsSaving = false;
			List<IVMP> ivmps = new List<IVMP>();
			foreach(var f in FailingTests)
			{
				Instrumenter.CurrentTestName = f;
				foreach(var statement in Instrumenter.CurrentTestStatements)
				{
					var alternatives = Instrumenter.GetAlternateMappings(statement).ToList();
					foreach(var alt in alternatives)
					{
						Instrumenter.ApplyMapping(statement, alt);
						if(runner.RunTest(f).Passed)
						{
							//we have found an IVMP
							ivmps.Add(new IVMP(statement, alt, Instrumenter.GetCurrentMapping(statement)));
							break;
						}
					}
				}
			}

			foreach(var stmt in ivmps.GroupBy(i => i.Location).OrderByDescending(g => g.Count()).Select(g => g.Key))
			{
				Console.Out.WriteLine(stmt.ToString());
			}
		}
	}
}

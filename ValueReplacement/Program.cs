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
			//String sln = @"D:\Documents and Settings\212059614\Desktop\PexFaultLocalization\SiemensSuite\TotInfo\TotInfo.sln";
			//string test_proj_name = "TotInfo.Tests";
			string sln = args[0];
			string test_proj_name = args[1];
			CodeRewriter rewriter = new CodeRewriter(sln, test_proj_name);
			rewriter.Rewrite();

			TestSuite tests = new TestSuite(Path.GetDirectoryName(sln));
			ReflectionTestRunner runner = new ReflectionTestRunner(tests);
			RunFaultLocalization(runner);
			//Console.Read();
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
			        foreach(var alt in Instrumenter.GetAlternateMappings(statement))
			        {
			            Instrumenter.ApplyMapping(statement, alt);
			            if(runner.RunTest(f).Passed)
			            {
			                //we have found an ivmp
			                ivmps.Add(new IVMP(statement, alt, Instrumenter.GetCurrentMapping(statement)));
			                break;
			            }
			        }
			    }
			}
			if(File.Exists("results.csv"))
				File.Delete("results.csv");
			using(var writer = new StreamWriter(File.OpenWrite("result.csv")))
			{
				writer.WriteLine("FileName, Start Line, End Line, Start Column, End Column, #IVMPs");
				var groups = ivmps.Select(i => i.Location).Concat(Instrumenter.AllStatements).GroupBy(i => i).OrderByDescending(g => g.Count());
				foreach(var g in groups)
				{
					var line = g.Key;
					writer.Write(line.FileName + ",");
					writer.Write(line.StartLine + ",");
					writer.Write(line.EndLine + ",");
					writer.Write(line.StartColumn+ ",");
					writer.Write(line.EndColumn + ",");
					writer.WriteLine(g.Count() -1);
				}
			}
		}
	}
}

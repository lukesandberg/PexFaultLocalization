using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Coverage.Analysis;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using ValueInjector;

namespace FaultLocalization
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if(args == (string[]) null || args.Length < 1)
			{
				Console.WriteLine("Usage: FaultLocalization.exe <SolutionDir>");
				Console.WriteLine("<SolutionDir> = full path to solution folder containing the .testconfig file");
				return;
			}

			String TestResultsPath = args[0];
			Console.WriteLine("Searching " + TestResultsPath + "...");

			String exePath = Path.Combine(TestResultsPath, "exes");

			foreach(String exe in Directory.GetFiles(exePath))
			{
                TestSuite tests;
                try
                {
                    tests = new TestSuite(TestResultsPath);
                }
                catch (Exception ex)
                {
                    Die(ex);
                    return;
                }

				String projectName = Path.GetFileNameWithoutExtension(TestResultsPath);
				String solutionOutput = Path.Combine(TestResultsPath, projectName + ".Tests", "bin", "Debug", projectName + ".exe");

				Console.WriteLine("Copying " + exe + " to " + solutionOutput);
				File.Copy(exe, solutionOutput, true);

				try
				{
					var testRunner = new CoverageTestRunner(tests);
					testRunner.RunTests();
				}
				catch(Exception ex)
				{
					Die(ex);
					return;
				}

				var testResults = tests.TestResults;

				var ratedLines = BuildDiagnosisMatrix(testResults);

				var dbbs = GetDynamicBasicBlocks(ratedLines);

				foreach(Type SuspicousnessRaterType in typeof(ISuspiciousnessRater).TypesImplementingInterface(true))
				{
					ISuspiciousnessRater rater = (ISuspiciousnessRater) SuspicousnessRaterType.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
					rater.RateLines(ratedLines, testResults);
				}
				OutputResults(ratedLines, Path.Combine(TestResultsPath, Path.GetFileNameWithoutExtension(exe)));
			}
			Console.Read();
		}

		private static IEnumerable<DynamicBasicBlock> GetDynamicBasicBlocks(IEnumerable<StatementSuspiciousnessInfo> ratedLines)
		{
			var TestComparer = new EqualityComparer<ExecutedTest>(
				(left, right) => left.Name == right.Name,
				test => test.Name.GetHashCode());

			var comparer = new EqualityComparer<IEnumerable<ExecutedTest>>(
				(left, right) => left.SequenceEqual(right, TestComparer),
				e => e.SumNoOverflow(t => TestComparer.GetHashCode(t)));

			IDictionary<IEnumerable<ExecutedTest>, DynamicBasicBlock> dbbs = new Dictionary<IEnumerable<ExecutedTest>, DynamicBasicBlock>(comparer);
			foreach(StatementSuspiciousnessInfo ratedLine in ratedLines)
			{
				DynamicBasicBlock block;
				if(!dbbs.TryGetValue(ratedLine.Tests, out block))
				{
					block = new DynamicBasicBlock(ratedLine.Tests);
					dbbs.Add(ratedLine.Tests, block);
				}

				block.Lines.Add(ratedLine);
				ratedLine.DBB = block;
			}
			return dbbs.Values;
		}

		public static IEnumerable<StatementSuspiciousnessInfo> BuildDiagnosisMatrix(IEnumerable<ExecutedTest> tests)
		{
			var testedLines = new Dictionary<SourceCodeLocation, StatementSuspiciousnessInfo>();
			uint passed = 0;
			uint failed = 0;
			foreach(var test in tests)
			{
				Console.WriteLine(test);

				if(test.Result)
					passed++;
				else
					failed++;

				CoverageDS data = test.CoverageData;
				foreach(var line in test.Lines)
				{
					var location = new SourceCodeLocation(data.SourceFileNames.Where(s => s.SourceFileID == line.SourceFileID).Select(s => s.SourceFileName).First(), (int) line.LnStart, (int) line.LnEnd, (int) line.ColStart, (int) line.ColEnd);
					StatementSuspiciousnessInfo currentLine;
					if(!testedLines.TryGetValue(location, out currentLine))
					{
						currentLine = new StatementSuspiciousnessInfo(location);
						testedLines.Add(location, currentLine);
					}
					currentLine.Tests.Add(test);
				}

			}

			return testedLines.Values;
		}

		public static void OutputResults(IEnumerable<StatementSuspiciousnessInfo> ratedLines, String prefix)
		{
			Debug.Assert(ratedLines.Count() != 0);

			string output = prefix + "results.csv";
			string delim = ",";

			StringBuilder sb = new StringBuilder();
			sb.Append("File" + delim + "Start Line #" + delim + "End Line #" + delim + "Start Column #" + delim + "End Column #" + delim);
			foreach(ISuspiciousnessRater rater in ratedLines.First().SuspiciousnessRatings.Keys)
			{
				sb.Append(rater.GetType().Name + delim);
			}
			sb.AppendLine("DBB Size");

			//TODO: what's our expected output for value replacement?
			foreach(var line in ratedLines)
			{
				sb.Append(line.Id.FileName + delim);
				sb.Append(line.Id.StartLine + delim);
				sb.Append(line.Id.EndLine + delim);
				sb.Append(line.Id.StartColumn+ delim);
				sb.Append(line.Id.EndColumn + delim);
				foreach(var rating in line.SuspiciousnessRatings)
				{
					sb.Append(rating.Value + delim);
				}
				sb.Append(line.DBB.Lines.Count);
				sb.AppendLine();
				File.WriteAllText(output, sb.ToString());
			}
			Console.WriteLine("Results stored in " + Path.GetFullPath(output));
		}

		private static void Die(Exception ex)
		{
			Console.WriteLine(ex.Message);
			Console.Read();
			Environment.Exit(1);
		}
	}
}

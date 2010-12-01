using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Coverage.Analysis;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;

namespace FaultLocalization
{
    public class Program
    {
        public static void Main(string[] args)
		{
			if(args == (string[])null || args.Length < 1)
			{
				Console.WriteLine("Usage: FaultLocalization.exe <SolutionDir>");
                Console.WriteLine("<SolutionDir> = full path to solution folder containing the .testconfig file");
				return;
			}

			String TestResultsPath = args[0];
			Console.WriteLine("Searching " + TestResultsPath + "...");
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

			try
            {
				var testRunner = new ReflectionTestRunner(tests);
				testRunner.RunTests();
            }
            catch (Exception ex)
            {
                Die(ex);
                return;
            }

            var testResults = tests.TestResults;
             
            var ratedLines = BuildDiagnosisMatrix(testResults);

            var dbbs = GetDynamicBasicBlocks(ratedLines);

            foreach (Type SuspicousnessRaterType in typeof(ISuspiciousnessRater).TypesImplementingInterface(true))
            {
                ISuspiciousnessRater rater = (ISuspiciousnessRater)SuspicousnessRaterType.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
                rater.RateLines(ratedLines, testResults);
            }
            OutputResults(ratedLines);
            Console.Read();
		}

        private static IEnumerable<DynamicBasicBlock> GetDynamicBasicBlocks(IEnumerable<Line> ratedLines)
        {
            var TestComparer = new EqualityComparer<ExecutedTest>(
                (left, right) => left.Name == right.Name, 
                test => test.Name.GetHashCode());

            var comparer = new EqualityComparer<IEnumerable<ExecutedTest>>(
                (left, right) => left.SequenceEqual(right, TestComparer),
                e => e.SumNoOverflow(t => TestComparer.GetHashCode(t)));

            IDictionary<IEnumerable<ExecutedTest>, DynamicBasicBlock> dbbs = new Dictionary<IEnumerable<ExecutedTest>, DynamicBasicBlock>(comparer);
            foreach (Line ratedLine in ratedLines)
            {
                DynamicBasicBlock block;
                if (!dbbs.TryGetValue(ratedLine.Tests, out block))
                {
                    block = new DynamicBasicBlock(ratedLine.Tests);
                    dbbs.Add(ratedLine.Tests, block);
                }

                block.Lines.Add(ratedLine);
                ratedLine.DBB = block;
            }
            return dbbs.Values;
        }

        public static IEnumerable<Line> BuildDiagnosisMatrix(IEnumerable<ExecutedTest> tests)
        {
            var testedLines = new Dictionary<LineIdentifier, Line>();
            uint passed = 0;
            uint failed = 0;
            foreach (var test in tests)
            {
                Console.WriteLine(test);

                if (test.Result)
                    passed++;
                else
                    failed++;

                CoverageDS data = test.CoverageData;
                foreach (var line in test.Lines)
                {
                    for (uint i = line.LnStart; i <= line.LnEnd; i++)
                    {
                        LineIdentifier id = new LineIdentifier(data.SourceFileNames.Where(s => s.SourceFileID == line.SourceFileID).Select(s => s.SourceFileName).First(), i);
                        Line currentLine;
                        if (!testedLines.TryGetValue(id, out currentLine))
                        {
                            currentLine = new Line(id);
                            testedLines.Add(id, currentLine);
                        }
                        currentLine.Tests.Add(test);
                    }
                }
            }

            return testedLines.Values;
        }

        public static void OutputResults(IEnumerable<Line> ratedLines)
        {
            Debug.Assert(ratedLines.Count() != 0);

            string output = "results.csv";
            string delim = ",";

            StringBuilder sb = new StringBuilder();
            sb.Append("File" + delim + "Line #" + delim);
            foreach (ISuspiciousnessRater rater in ratedLines.First().SuspiciousnessRatings.Keys)
            {
                sb.Append(rater.GetType().Name + delim);
            }
            sb.AppendLine("DBB Size");

            //TODO: what's our expected output for value replacement?
            foreach (var line in ratedLines)
            {
                sb.Append(line.Id.FileName + delim);
                sb.Append(line.Id.LineNo + delim);
                foreach (var rating in line.SuspiciousnessRatings)
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

    public class LineIdentifier
    {

       public uint LineNo { get; private set; }
       public String FileName { get; private set; }

        public LineIdentifier(String s, uint l)
        {
            FileName = s;
            LineNo = l;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is LineIdentifier)) return false;
            LineIdentifier other = (LineIdentifier)obj;
            return LineNo == other.LineNo && String.Equals(FileName, other.FileName);
        }
        public override int GetHashCode()
        {
            // <pex>
            Debug.Assert(this.FileName != (string)null, "this.FileName");
            // </pex>
            return (int)(FileName.GetHashCode() * 1021 ^ LineNo);

        }
        public override string ToString()
        {
            return FileName + ":" + LineNo;
        }
    }

    public class Line
    {
        public LineIdentifier Id { get; private set; }

        public int Passed
        {
            get
            {
                return Tests.Where(t => t.Result).Count();
            }
        }

        public int Failed
        {
            get
            {
                return Tests.Where(t => !t.Result).Count();
            }

        }

        public IList<ExecutedTest> Tests { get; private set; }

        public IDictionary<ISuspiciousnessRater, float> SuspiciousnessRatings
        {
            get;
            private set;
        }

        public DynamicBasicBlock DBB { get; set; }

        public Line(LineIdentifier id)
        {
            this.Id = id;
            Tests = new List<ExecutedTest>();
            SuspiciousnessRatings = new Dictionary<ISuspiciousnessRater, float>();
        }


        
    }
}

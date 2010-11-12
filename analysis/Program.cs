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
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
                return;
            }

            try
            {
				var testRunner = new CoverageTestRunner(tests);
				testRunner.RunTests();   
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Dictionary<LineIdentifier, Line> testedLines = new Dictionary<LineIdentifier, Line>();
            uint passed = 0;
            uint failed = 0;
			foreach(var test in tests.TestResults)
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

                        if(test.Result)
                            currentLine.pass();
                        else
                            currentLine.fail();
                    }
                }
			}
            var ratedLines = testedLines.Select(kvp => SuspiciousnessRater.applyOchiai(kvp.Value, passed, failed))
                                .OrderByDescending(l => l.Rating);

            
            foreach (var line in ratedLines)
            {
                Console.WriteLine("Line #" + line.Id + " had suspiciousness rating: " + line.Rating);
            }
            
			Console.Read();
		}
    }

    public class LineIdentifier
    {
        
        private readonly uint LineNo;
        private readonly String FileName;

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


        public int Passed { get; private set; }
        public int Failed { get; private set; }
        public float Rating { get; set; }

        public Line(LineIdentifier id)
        {
            this.Id = id;
            Passed = 0;
            Failed = 0;
            Rating = 0;
        }

        public void fail()
        {
            Failed++;
        }

        public void pass()
        {
            Passed++;
        }
    }
}

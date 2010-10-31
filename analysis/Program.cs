using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Coverage.Analysis;
using System.IO;
using System.Xml.Linq;

namespace FaultLocalization
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if(args.Length < 1)
			{
				Console.WriteLine("Usage: FaultLocalization.exe <TestResultsDir>");
				return;
			}

			String TestResultsPath = args[0];
			Console.WriteLine("Searching " + TestResultsPath + "...");

			var tests = new TestSuite(TestResultsPath);

			foreach(var test in tests.Tests)
			{
				Console.WriteLine(test);
			}
		
			Console.Read();
		}
	}
}

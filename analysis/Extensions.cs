using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Coverage.Analysis;
using System.Diagnostics;
using System.IO;
namespace FaultLocalization
{
	public static class Extensions
	{
		/// <summary>
		/// Generates a String consisting of two lines before and two lines after
		/// the given line in the source file.  All covered lines are proceeded
		/// with '>>'
		/// </summary>
		/// <param name="ds">The Coverage Data Set that this line is based in</param>
		/// <param name="line">The line we are interested in</param>
		/// <returns>The source code around the given line</returns>
		public static String GetCoveredLine(this CoverageDS ds, CoverageDS.LinesRow line)
		{
			uint file_id = line.SourceFileID;
			String filename = ds.SourceFileNames.Where(fn => fn.SourceFileID == file_id).Select(f => f.SourceFileName).First();
			using(var reader = File.OpenText(filename))
			{
				uint ln = 1;
				uint start = Math.Max(1, line.LnStart -2);
				StringBuilder rval = new StringBuilder();
				foreach(var code in reader.Lines())
				{
					if(ln >=start)
					{
						var output = code;
						if(ln >= line.LnStart && ln <= line.LnEnd)
						{
							if(output.StartsWith("  "))
								output = output.Substring(2);
							if(output.StartsWith("\t"))
								output = "  " + output.Substring(2);
							output = ">>" + output;
						}
						rval.AppendLine(output);
					}
					if(ln > line.LnEnd + 2)
					{
						break;
					}
					ln++;
				}
				return rval.ToString();
			}
		}
		/// <summary>
		/// Enumerates the lines of the given file
		/// </summary>
		/// <param name="reader">The Stream to read</param>
		/// <returns>The lines in the file or resource</returns>
		public static IEnumerable<String> Lines(this TextReader reader)
		{
			String line;
			while((line = reader.ReadLine()) != null)
			{
				yield return line;
			}
		}
	}
}

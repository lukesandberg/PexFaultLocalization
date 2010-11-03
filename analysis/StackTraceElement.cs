using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Coverage.Analysis;
using System.Text.RegularExpressions;

namespace FaultLocalization
{
	public class StackTraceElement
	{
		private string line;
		public StackTraceElement(string line, CoverageDS CoverageData)
		{
            // <pex>
            if (line == (string)null)
                throw new ArgumentNullException("line");
            if (CoverageData == (CoverageDS)null)
                throw new ArgumentNullException("CoverageData");
            // </pex>
			this.line = line;
			Regex r = new Regex("\\s+at\\s+(.+)\\.(.+\\))(\\s+in\\s+(.+):line\\s(\\d+))?");
			Match m = r.Match(line);
			if(m.Success)
			{
				Class = m.Groups[1].Value;
				Method = m.Groups[2].Value;
				if(m.Groups[3].Success)
				{
					FileName = m.Groups[4].Value;
					LineNumber = uint.Parse(m.Groups[5].Value);
					Lines = (from file in CoverageData.SourceFileNames
							 from ln in CoverageData.Lines
							 where file.SourceFileName.Equals(FileName)
									 && ln.LnStart <= LineNumber && ln.LnEnd >= LineNumber
									 && ln.SourceFileID == file.SourceFileID
							 select ln).ToList();

					IsCovered = Lines.Any();

				}
			}
			else
			{
				throw new ArgumentException("Not a valid stack trace line");
			}
		}

		public String Class { get; private set; }
		public String Method { get; private set; }
		public String FileName { get; private set; }
		public uint? LineNumber { get; private set; }
		public bool IsCovered { get; private set; }
		public IEnumerable<CoverageDSPriv.LinesRow> Lines { get; private set; }
		public override string ToString()
		{
			return line;
		}
	}
}

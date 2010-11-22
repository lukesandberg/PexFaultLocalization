using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AssemblyRewriter
{
	class Program
	{
		static void Main(string[] args)
		{
			CodeRewriter rewriter = new CodeRewriter(@"D:\Documents and Settings\212059614\Desktop\PexFaultLocalization\strings\Strings.sln", "Strings.Tests");
			rewriter.Rewrite( Path.Combine(Directory.GetCurrentDirectory(), "OutPut"));
		}
	}
}

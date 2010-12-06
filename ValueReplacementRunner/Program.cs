using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ValueReplacementRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			String exes = @"D:\Documents and Settings\212059614\Desktop\PexFaultLocalization\SiemensSuite\PrintTokens\exes";
			String sln = @"D:\Documents and Settings\212059614\Desktop\PexFaultLocalization\SiemensSuite\PrintTokens\PrintTokens.sln";
			String tp_name = @"PrintTokens.Tests";

			String vr_exe = @"D:\Documents and Settings\212059614\Desktop\PexFaultLocalization\ValueReplacement\bin\Debug\ValueReplacement.exe";
			
			var runs = Directory.EnumerateFiles(exes)
				.GroupBy(p => Path.GetFileNameWithoutExtension(p))
				.Select(g => new { exe = g.FirstOrDefault(s => Path.GetExtension(s).Equals(".exe")), pdb = g.FirstOrDefault(s => Path.GetExtension(s).Equals(".pdb")) })
				.Where(a => !String.IsNullOrEmpty(a.exe) && !String.IsNullOrEmpty(a.pdb)) ;
			foreach(var run in runs)
			{
				String targetName = Path.GetFileNameWithoutExtension(sln);
				String targetDir = Path.Combine(Path.GetDirectoryName(sln),targetName, "Bin", "Debug");
				File.Copy(run.exe, Path.Combine(targetDir, targetName + ".exe"), true);
				File.Copy(run.pdb, Path.Combine(targetDir, targetName + ".pdb"), true);
				Process proc = new Process();
				proc.StartInfo.FileName = vr_exe;
				proc.StartInfo.Arguments = "\"" + sln + "\" \"" + tp_name + "\"";
				//proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				proc.StartInfo.UseShellExecute = false;

				proc.Start();
				proc.WaitForExit();
				File.Copy("result.csv", Path.GetFileNameWithoutExtension(run.exe) + "_results.csv", true);
			}
		}
	}
}

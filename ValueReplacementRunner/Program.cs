using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using FaultLocalization.Util;

namespace ValueReplacementRunner
{
	class Program
	{
		static String exes = @"D:\Documents and Settings\212059614\Desktop\PexFaultLocalization\SiemensSuite\TotInfo\exes";
		static String sln = @"D:\Documents and Settings\212059614\Desktop\PexFaultLocalization\SiemensSuite\TotInfo\TotInfo.sln";
		static String tp_name = @"TotInfo.Tests";
		static String p_name = @"TotInfo";
		static String vr_exe = @"D:\Documents and Settings\212059614\Desktop\PexFaultLocalization\ValueReplacement\bin\Debug\ValueReplacement.exe";

		static void Main(string[] args)
		{
			
			var runs = Directory.EnumerateFiles(exes)
				.GroupBy(p => Path.GetFileNameWithoutExtension(p))
				.Select(g => new { exe = g.FirstOrDefault(s => Path.GetExtension(s).Equals(".exe")), pdb = g.FirstOrDefault(s => Path.GetExtension(s).Equals(".pdb")) })
				.Where(a => !String.IsNullOrEmpty(a.exe) && !String.IsNullOrEmpty(a.pdb));
			//foreach(var run in runs)
			//{
			//    try
			//    {
			//        build_and_copy(Path.GetFileNameWithoutExtension(run.exe));
			//    }
			//    catch(Exception e)
			//    {
			//    }
			//}
			foreach(var run in runs)
			{
				String targetName = Path.GetFileNameWithoutExtension(sln);
				String targetDir = Path.Combine(Path.GetDirectoryName(sln), targetName, "Bin", "Debug");
				CSSolution.Rebuild(sln);
				File.Copy(run.exe, Path.Combine(targetDir, targetName + ".exe"), true);
				File.Copy(run.pdb, Path.Combine(targetDir, targetName + ".pdb"), true);
				Process proc = new Process();
				proc.StartInfo.FileName = vr_exe;
				proc.StartInfo.Arguments = "\"" + sln + "\" \"" + tp_name + "\"";
				//proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				//proc.StartInfo.RedirectStandardOutput = true;
				proc.StartInfo.UseShellExecute = false;

				proc.Start();

				proc.WaitForExit();
				if(File.Exists("result.csv"))
				{
					File.Copy("result.csv", Path.GetFileNameWithoutExtension(run.exe) + "_results.csv", true);
					File.Delete("result.csv");
				}
				else
					Console.WriteLine("failed to gather results for " + run.exe);
			}
		}

		static void build_and_copy(String version)
		{
			Console.WriteLine(version);
			String proj_dir = Path.Combine(Path.GetDirectoryName(sln), p_name);
			String target_v = Path.Combine(proj_dir, version + ".cs");
			String fn = Path.Combine(proj_dir, "BaseVersion.cs");
			File.Copy(target_v, fn, true);
			CSSolution.Rebuild(sln);
			String targetDir = Path.Combine(Path.GetDirectoryName(sln), proj_dir, "Bin", "Debug");
			File.Copy(Path.Combine(targetDir, p_name + ".exe"), Path.Combine(exes, version + ".exe"), true);
			File.Copy(Path.Combine(targetDir, p_name + ".pdb"), Path.Combine(exes, version + ".pdb"), true);
		}
	}
}

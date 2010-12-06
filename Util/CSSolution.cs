using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;
using System.Runtime.InteropServices;
using System.Threading;

namespace FaultLocalization.Util
{
	public class CSSolution
	{
		public String SolutionPath { get; private set; }
		protected Solution2 Solution { get; private set; }

		private List<CSProject> _projects;
		public IEnumerable<CSProject> Projects { get { return _projects; } }

		public static void Rebuild(String sln)
		{
			System.Type type = System.Type.GetTypeFromProgID("VisualStudio.DTE.10.0");
			Object obj = System.Activator.CreateInstance(type, true);
			DTE2 dte2 = (DTE2) obj;
			var Solution = (Solution2) dte2.Solution;
			try
			{
				COMRetry(() => Solution.Open(sln));
				SolutionBuild2 build = (SolutionBuild2) Solution.SolutionBuild;
				COMRetry(() => build.Clean(true));
				COMRetry(() => build.Build(true));
			}
			finally
			{
				COMRetry(() =>
				{
					Solution.Close();
					dte2.Quit();
				});
			}
		}
		private static void COMRetry(Action a)
		{
			while(true)
			{
				try
				{
					a();
					break;
				}
				catch(COMException)
				{
					Thread.Sleep(500);
				}
			}
		}

		public CSSolution(String solutionPath)
		{
			SolutionPath = solutionPath;
			System.Type type = System.Type.GetTypeFromProgID("VisualStudio.DTE.10.0");
			Object obj = System.Activator.CreateInstance(type, true);
			DTE2 dte2 = (DTE2) obj;
			Solution = (Solution2) dte2.Solution;
			try
			{
				COMRetry(() => Solution.Open(solutionPath));
				COMRetry(() => _projects = Solution.Projects.Cast<dynamic>().Select(d => d.FullName).Cast<String>()
										.Where(n => !String.IsNullOrEmpty(n))
										.Select(n => new CSProject(n)).ToList());
			}
			finally
			{
				COMRetry(() =>
				{
					Solution.Close();
					dte2.Quit();
				});
			}
		}
	}
}

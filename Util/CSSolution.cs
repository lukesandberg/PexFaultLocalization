using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;

namespace FaultLocalization.Util
{
	public class CSSolution
	{
		public String SolutionPath { get; private set; }
		protected Solution2 Solution { get; private set; }

		private List<CSProject> _projects;
		public IEnumerable<CSProject> Projects { get { return _projects; } }

		public CSSolution(String solutionPath)
		{
			SolutionPath = solutionPath;
			System.Type type = System.Type.GetTypeFromProgID("VisualStudio.DTE.10.0");
			Object obj = System.Activator.CreateInstance(type, true);
			DTE2 dte2 = (DTE2) obj;
			Solution = (Solution2) dte2.Solution;
			try
			{
				Solution.Open(solutionPath);
				//SolutionBuild2 build = (SolutionBuild2) Solution.SolutionBuild;
				//build.Build(true);
				_projects = Solution.Projects.Cast<dynamic>().Select(d => d.FullName).Cast<String>()
								.Where(n => !String.IsNullOrEmpty(n))
								.Select(n => new CSProject(n)).ToList();
			}
			finally
			{
				Solution.Close();
				dte2.Quit();
			}
		}
	}
}

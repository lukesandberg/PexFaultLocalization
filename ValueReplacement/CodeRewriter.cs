using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Diagnostics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;
using FaultLocalization.Util;

namespace ValueReplacement
{
	public class CodeRewriter
	{
		private readonly CSSolution solution;
		private readonly String TestAssemblyName;
		public CodeRewriter(String solutionPath, String test_assembly_name)
		{
			solution = new CSSolution(solutionPath);
			this.TestAssemblyName = test_assembly_name;
		}
		/// <summary>
		/// Rewrites the provided project in place so that it still cooperates with our test running capabilities
		/// </summary>
		public void Rewrite()
		{
			var rewriters = solution.Projects.Where(p => !String.Equals(p.AssemblyName, TestAssemblyName)).Select(p => new ProjectRewriter(p));
			var test = solution.Projects.Where(p => String.Equals(p.AssemblyName, TestAssemblyName)).First();
			String dir = Path.GetDirectoryName(test.AssemblyLocation);
			foreach(var writer in rewriters)
			{
				writer.Rewrite();
				String newLoc = Path.Combine(dir, Path.GetFileName(writer.Proj.AssemblyLocation));
				File.Copy(writer.Proj.AssemblyLocation, newLoc, true);
			}
		}
	}
}

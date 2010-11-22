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

namespace AssemblyRewriter
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

		public void Rewrite(String toDir)
		{
			if(!Directory.Exists(toDir))
			{
				Directory.CreateDirectory(toDir);
			}
			var rewriters = solution.Projects.Where(p => !String.Equals(p.AssemblyName, TestAssemblyName)).Select(p => new ProjectRewriter(p, toDir));
			foreach(var writer in rewriters)
			{
				writer.Rewrite();
			}
			var testProject = solution.Projects.Where(p => String.Equals(p.AssemblyName, TestAssemblyName)).Single();
			File.Copy(testProject.AssemblyLocation, Path.Combine(toDir, Path.GetFileName(testProject.AssemblyLocation)));
		}
	}
}

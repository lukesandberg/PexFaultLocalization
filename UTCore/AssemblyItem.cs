using System;
using System.Diagnostics;
using System.Reflection;

using KALib;

namespace UTCore
{
	public class AssemblyItem
	{
		private Assembly assembly;
		private NamespaceCollection namespaceCollection;

		public NamespaceCollection NamespaceCollection
		{
			get
			{
				return namespaceCollection;
			}
		}

		public string FullName
		{
			get
			{
				return assembly.FullName;
			}
		}

		public AssemblyItem()
		{
		}

		public void Load(string assemblyName)
		{
			assembly=Assembly.LoadFrom(assemblyName);
		}

		public void LoadNamespaces()
		{
			namespaceCollection=GetNamespaceCollection();
			namespaceCollection.DumpKeys("Namespaces:");
		}

		public void LoadClasses()
		{
			namespaceCollection.LoadClasses();
		}

		public void LoadMethods()
		{
			foreach(NamespaceItem ni in namespaceCollection.Values)
			{
				ni.ClassCollection.LoadMethods();	
			}
		}

		private NamespaceCollection GetNamespaceCollection()
		{
			NamespaceCollection nc=new NamespaceCollection();
			Type[] types=assembly.GetTypes();
			foreach (Type type in types)
			{
				MethodInfo[] methods=type.GetMethods(Options.BindingFlags);
				foreach (MethodInfo methodInfo in methods)
				{
					string nameSpace=methodInfo.DeclaringType.Namespace;
					nc.Add(nameSpace, new NamespaceItem(assembly, nameSpace));
				}
			}
			return nc;
		}
	}
}

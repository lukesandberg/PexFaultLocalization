using System;
using System.Reflection;

using KALib;

namespace UTCore
{
	public class ClassItem
	{
		private Assembly assembly;
		private string namespaceName;
		private string className;
		private Type type;
		private object[] attributes;
		private MethodCollection methodCollection;

		public Assembly Assembly
		{
			get
			{
				return assembly;
			}
		}

		public object[] Attributes
		{
			get
			{
				return attributes;
			}
		}

		public MethodCollection MethodCollection
		{
			get
			{
				return methodCollection;
			}
		}

		public override string ToString()
		{
			return className;
		}

		public string Namespace
		{
			get
			{
				return namespaceName;
			}
		}

		public string FullName
		{
			get
			{
				return namespaceName+"."+className;
			}
		}

		public ClassItem(Assembly assembly, string namespaceName, string className, Type type)
		{
			this.assembly=assembly;
			this.namespaceName=namespaceName;
			this.className=className;
			this.type=type;
			attributes=type.GetCustomAttributes(true);
		}

		public void LoadMethods()
		{
			methodCollection=GetMethodCollection();
			methodCollection.DumpKeys("Namespace: "+namespaceName+", Class: "+className+", Methods:");
		}

		public object Create()
		{
			Type t=assembly.GetType(FullName, true);
			object obj=Activator.CreateInstance(t);
			return obj;
		}

		private MethodCollection GetMethodCollection()
		{
			MethodCollection methodCollection=new MethodCollection();
			Type[] types=assembly.GetTypes();
			foreach (Type type in types)
			{
				MethodInfo[] methods=type.GetMethods(Options.BindingFlags);
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.DeclaringType.Namespace==namespaceName)
					{
						string className=StringHelpers.RightOfRightmostOf(methodInfo.DeclaringType.FullName, '.');
						if (className==this.className)
						{
							string methodName=methodInfo.Name;
							methodCollection.Add(methodName, new MethodItem(assembly, namespaceName, className, methodName, methodInfo));
						}
					}
				}
			}
			return methodCollection;
		}
	}
}

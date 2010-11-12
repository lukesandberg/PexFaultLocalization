using System;
using System.Reflection;

using KALib;

namespace UTCore
{
	public class NamespaceItem
	{
		private Assembly assembly;
		private string namespaceName;
		private ClassCollection classCollection;

		public ClassCollection ClassCollection
		{
			get
			{
				return classCollection;
			}
		}

		public NamespaceItem(Assembly assembly, string namespaceName)
		{
			this.assembly=assembly;
			this.namespaceName=namespaceName;
		}

		public void LoadClasses()
		{
			classCollection=GetClassCollection();
			classCollection.DumpKeys("Namespace: "+namespaceName+", Classes:");
		}

		private ClassCollection GetClassCollection()
		{
			ClassCollection classCollection=new ClassCollection();
			Type[] types=assembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.IsClass)
				{
					MethodInfo[] methods=type.GetMethods(Options.BindingFlags);
					foreach (MethodInfo methodInfo in methods)
					{
						if (methodInfo.DeclaringType.Namespace==namespaceName)
						{
							string className=StringHelpers.RightOfRightmostOf(methodInfo.DeclaringType.FullName, '.');
							Type t=methodInfo.DeclaringType;
							classCollection.Add(className, new ClassItem(assembly, namespaceName, className, type));
						}
					}
				}
			}
			return classCollection;
		}
	}
}

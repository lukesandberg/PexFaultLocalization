using System;
using System.Reflection;

using KALib;

namespace UTCore
{
	public class MethodItem
	{
		private Assembly assembly;
		private string namespaceName;
		private string className;
		private string methodName;
		private object[] attributes;
		private MethodInfo methodInfo;
		private delegate void UnitTestDelegate();

		// test options
		private bool ignore=false;
		Attribute eea=null;

		public object[] Attributes
		{
			get
			{
				return attributes;
			}
		}

		public bool Ignore
		{
			get
			{
				return ignore;
			}
			set
			{
				ignore=value;
			}
		}

		public Attribute ExpectedException
		{
			get
			{
				return eea;
			}
			set
			{
				eea=value;
			}
		}

		public override string ToString()
		{
			return methodName;
		}

		public MethodItem(Assembly assembly, string namespaceName, string className, string methodName, MethodInfo methodInfo)
		{
			this.assembly=assembly;
			this.namespaceName=namespaceName;
			this.className=className;
			this.methodName=methodName;
			this.methodInfo=methodInfo;
			attributes=methodInfo.GetCustomAttributes(true);
		}

		public void Invoke(object classInstance)
		{
			// Delegates requires that methods have a specific signature and are public.
			// Delegates are faster than "methodInfo.Invoke".

			Type utdType=typeof(UnitTestDelegate);
			UnitTestDelegate utd=Delegate.CreateDelegate(utdType, classInstance, methodName) as UnitTestDelegate;
			try
			{
				utd();
			}
			catch(Exception e)
			{
				throw(e);
			}

// The invoke function allows us to call functions with different parameter lists, and ones that are not public.
// However, this changes how we handle exceptions

//			try
//			{
//				methodInfo.Invoke(classInstance, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Static, null, null, null);
//			}
//			catch(Exception e)
//			{
//				throw(e.InnerException);
//			}
		}
	}
}

using System;
using System.Reflection;

namespace UTCore
{
	public class Options
	{
		private static BindingFlags bindingFlags=BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

		public static BindingFlags BindingFlags
		{
			get
			{
				return bindingFlags;
			}
			set
			{
				bindingFlags=value;
			}
		}
	}
}

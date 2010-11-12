using System;
using KALib;

namespace UTCore
{
	public class ClassCollection : UniqueCollection
	{
		public ClassCollection()
		{
		}

		public void LoadMethods()
		{
			foreach (ClassItem ci in Values)
			{
				ci.LoadMethods();
			}
		}
	}
}

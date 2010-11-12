using System;
using KALib;

namespace UTCore
{
	public class NamespaceCollection : UniqueCollection
	{
		public NamespaceCollection()
		{
		}

		public void LoadClasses()
		{
			foreach (NamespaceItem ni in Values)
			{
				ni.LoadClasses();
			}
		}
	}
}

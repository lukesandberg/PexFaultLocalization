using System;
using System.Collections;
using System.Diagnostics;

namespace KALib
{
	public class UniqueCollection : Hashtable
	{
		public override void Add(object key, object val)
		{
			if (!this.Contains(key))
			{
				base.Add(key, val);
			}
		}

		public void DumpKeys(string listName)
		{
			Trace.WriteLine(listName);
			foreach(object obj in Keys)
			{
				Trace.WriteLine("     "+obj.ToString());
			}
		}
	}
}

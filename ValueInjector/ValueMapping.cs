using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueInjector
{
	public class ValueMapping
	{
		private Dictionary<int, Value> mapping = new Dictionary<int, Value>();
		public ValueMapping() { }
		internal void AddValue(int var_id, Value val)
		{
			mapping[var_id] = val;
		}
		internal bool TryGetValue(int var_id, out Value value)
		{
			return mapping.TryGetValue(var_id, out value);
		}
	}
}

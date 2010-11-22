using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueInjector
{
	class ValueProfile
	{
		private readonly Dictionary<String, ValueMapping> mappings = new Dictionary<string, ValueMapping>();
		public ValueProfile() { }

		public void AddValue(String TestName, int var, Value val)
		{
			ValueMapping mapping;
			if(!mappings.TryGetValue(TestName, out mapping))
			{
				mapping = new ValueMapping();
				mappings[TestName] = mapping;
			}
			mapping.AddValue(var, val);
		}
		public IEnumerable<ValueMapping> AlternateMappings(String TestName)
		{
			return mappings.Where(kvp => !kvp.Key.Equals(TestName)).Select(kvp => kvp.Value);
		}
	}
}

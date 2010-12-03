using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strings
{
	public class TestObject
	{
		private int field1;
		private String field2;

		public TestObject(int i, String s)
		{
			field1 = i;
			field2 = s;
		}

		public String Do()
		{
			if(field1 < 0)
				return field2;
			if(String.IsNullOrEmpty(field2))
				return "null" + field1;
			return field2 + field1;
		}
	}
}

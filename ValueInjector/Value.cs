﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueInjector
{
	public class Value
	{
		private Type _type;

		public Type Type
		{
			get { return _type; }
			set { _type = value; }
		}

		private object _value;
		public object Val
		{
			get { return _value; }
			set { _value = value; }
		}
		public override string ToString()
		{
			return "{" + Type + ": " + Val + "}";
		}
	}
}

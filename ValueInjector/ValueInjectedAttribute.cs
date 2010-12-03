using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueInjector
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class ValueInjectedAttribute : Attribute
	{
		private readonly byte[] _version;
		public byte[] Version { get { return _version; } }
		public ValueInjectedAttribute(byte[] v)
		{
			_version = v;
		}
	}
}

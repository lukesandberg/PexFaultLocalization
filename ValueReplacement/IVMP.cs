using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueInjector;

namespace ValueReplacement
{
	public class IVMP
	{
		private readonly SourceCodeLocation _location;
		public SourceCodeLocation Location
		{
			get { return _location; }
		}

		private readonly ValueMapping _failingProfile;
		public ValueMapping FailingProfile
		{
			get { return _failingProfile; }
		}
		private readonly ValueMapping _succeedingProfile;
		public ValueMapping SucceedingProfile
		{
			get { return _succeedingProfile; }
		}

		public IVMP(SourceCodeLocation loc, ValueMapping s, ValueMapping f)
		{
			_location = loc;
			_succeedingProfile = s;
			_failingProfile = f;
		}
	}
}

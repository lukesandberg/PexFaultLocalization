using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueInjector;

namespace FaultLocalization
{
	public class StatementSuspiciousnessInfo
	{
		public SourceCodeLocation Id { get; private set; }

		public int Passed
		{
			get
			{
				return Tests.Where(t => t.Result).Count();
			}
		}

		public int Failed
		{
			get
			{
				return Tests.Where(t => !t.Result).Count();
			}

		}

		public IList<ExecutedTest> Tests { get; private set; }

		public IDictionary<ISuspiciousnessRater, float> SuspiciousnessRatings
		{
			get;
			private set;
		}

		public DynamicBasicBlock DBB { get; set; }

		public StatementSuspiciousnessInfo(SourceCodeLocation id)
		{
			this.Id = id;
			Tests = new List<ExecutedTest>();
			SuspiciousnessRatings = new Dictionary<ISuspiciousnessRater, float>();
		}



	}
}

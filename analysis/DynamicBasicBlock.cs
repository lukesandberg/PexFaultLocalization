using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaultLocalization
{
    public class DynamicBasicBlock
    {
        public IEnumerable<ExecutedTest> Tests { get; private set; }
        public ISet<Line> Lines { get; private set; }

        public DynamicBasicBlock(IEnumerable<ExecutedTest> tests) 
        {
            Tests = tests;
            Lines = new HashSet<Line>();
        }
    }
}

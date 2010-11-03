using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FaultLocalization;

namespace FaultLocalization
{
    public class SuspiciousnessRater
    {
        public static Line applyTarantula(Line line, uint passed, uint failed)
        {
            // <pex>
            if (passed == 0uL)
                throw new ArgumentException("passed == 0uL", "passed");
            if (failed == 0uL)
                throw new ArgumentException("failed == 0uL", "failed");
            if (line == (Line)null)
                throw new ArgumentNullException("line");
            // </pex>
            float numerator = line.Failed / failed;
            float denominator = (line.Passed / passed) + (line.Failed / failed);
            line.Rating = numerator / denominator;
            return line;
        }

        public static Line applyOchiai(Line line, uint passed, uint failed)
        {
            throw new NotImplementedException();
        }
    }
}

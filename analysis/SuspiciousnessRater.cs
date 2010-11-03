using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

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
            float numerator = (float) line.Failed / (float) failed;
	    float denominator = ((float) line.Passed / (float) passed) + ((float) line.Failed / (float) failed);

            line.Rating = numerator / denominator;
	    Debug.Assert(!Double.IsNaN(line.Rating));
            return line;
        }

        public static Line applyOchiai(Line line, uint passed, uint failed)
        {
            throw new NotImplementedException();
        }
    }
}

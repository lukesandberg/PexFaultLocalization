using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaultLocalization
{
    class SuspiciousnessRater
    {
        public static Line applyTarantula(Line line, uint passed, uint failed)
        {
            float numerator = line.Failed / failed;
            float denominator = (line.Passed / passed) + (line.Failed / failed);
            line.Rating = numerator / denominator;
            return line;
        }

        public static Line applyOchiai(Line line, uint passed, uint failed)
        {
            double denominator = Math.Sqrt(failed + line.Failed + line.Passed);
            line.Rating = line.Failed / (float)denominator;
            return line;           
        }
    }
}

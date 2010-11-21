using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace FaultLocalization
{
    public interface ISuspiciousnessRater
    {
        void RateLines(IEnumerable<Line> ratedLines, IEnumerable<ExecutedTest> testResults);
    }

    public abstract class StatisticalRater : ISuspiciousnessRater {
        public void RateLines(IEnumerable<Line> testedLines, IEnumerable<ExecutedTest> testResults)
        {
            uint passed = (uint)testResults.Where(t=> t.Result).Count();
            uint failed = (uint)testResults.Count() - passed;
            foreach (Line l in testedLines)
            {
                applyRating(l, passed, failed);
            }
        }

        

        protected virtual Line applyRating(Line line, uint passed, uint failed)
        {
            // <pex>
            if (passed == 0uL)
                throw new ArgumentException("passed == 0uL", "passed");
            if (failed == 0uL)
                throw new ArgumentException("failed == 0uL", "failed");
            if (line == (Line)null)
                throw new ArgumentNullException("line");
            // </pex>

            return line;
        }
    }

    public class TarantulaRater : StatisticalRater
    {
        protected override Line applyRating(Line line, uint passed, uint failed)
        {
            base.applyRating(line, passed, failed);
            float numerator = (float)line.Failed / (float)failed;
            float denominator = ((float)line.Passed / (float)passed) + ((float)line.Failed / (float)failed);
            float rating = numerator / denominator;
            Debug.Assert(!Double.IsNaN(rating));
            line.SuspiciousnessRatings.Add(this, rating);
            return line;
        }
    }

    public class OchiaiRater : StatisticalRater
    {
        protected override Line applyRating(Line line, uint passed, uint failed)
        {
            base.applyRating(line, passed, failed);
            float denominator = (float)Math.Sqrt(failed * (line.Failed + line.Passed));
            float rating = line.Failed / denominator;
            Debug.Assert(!Double.IsNaN(rating));
            line.SuspiciousnessRatings.Add(this, rating);
            return line;
        }
    }
}

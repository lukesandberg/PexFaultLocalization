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
        void RateLines(IEnumerable<StatementSuspiciousnessInfo> ratedLines, IEnumerable<ExecutedTest> testResults);
    }

    public abstract class StatisticalRater : ISuspiciousnessRater {
        public void RateLines(IEnumerable<StatementSuspiciousnessInfo> testedLines, IEnumerable<ExecutedTest> testResults)
        {
            uint passed = (uint)testResults.Where(t=> t.Result).Count();
            uint failed = (uint)testResults.Count() - passed;
            foreach (StatementSuspiciousnessInfo l in testedLines)
            {
                applyRating(l, passed, failed);
            }
        }

        

        protected virtual StatementSuspiciousnessInfo applyRating(StatementSuspiciousnessInfo line, uint passed, uint failed)
        {
            // <pex>
            if (failed + passed == 0uL)
                throw new ArgumentException("failed + passed == 0uL", "passed");
            if (line == (StatementSuspiciousnessInfo)null)
                throw new ArgumentNullException("line");
            // </pex>

            return line;
        }
    }

    public class TarantulaRater : StatisticalRater
    {
        protected override StatementSuspiciousnessInfo applyRating(StatementSuspiciousnessInfo line, uint passed, uint failed)
        {
            base.applyRating(line, passed, failed);
            if ((float)failed == 0)
            {
                line.SuspiciousnessRatings.Add(this, 0);
                return line;
            }
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
        protected override StatementSuspiciousnessInfo applyRating(StatementSuspiciousnessInfo line, uint passed, uint failed)
        {
            base.applyRating(line, passed, failed);
            if ((float)failed == 0)
            {
                line.SuspiciousnessRatings.Add(this, 0);
                return line;
            }
            float denominator = (float)Math.Sqrt(failed * (line.Failed + line.Passed));
            float rating = line.Failed / denominator;
            Debug.Assert(!Double.IsNaN(rating));
            line.SuspiciousnessRatings.Add(this, rating);
            return line;
        }
    }

    public class IntensityRater : StatisticalRater
    {
        protected override StatementSuspiciousnessInfo applyRating(StatementSuspiciousnessInfo line, uint passed, uint failed)
        {
            base.applyRating(line, passed, failed);
            line.SuspiciousnessRatings.Add(this, Math.Max(line.Passed, line.Failed));
            return line;
        }
    }
}

using System;
namespace Edu.Nlu.Sir.Siemens.TotInfo
{
    public abstract class ITotInfo
    {
        public abstract double gcf(double a, double x);
		public abstract double gser(double a, double x);
		public abstract double InfoTbl(int r, int c, long[] f, out int pdf);
		public abstract double LGamma(double x);
		public abstract double QChiSq(double chisq, int df);
		public abstract double QGamma(double a, double x);
    }
}

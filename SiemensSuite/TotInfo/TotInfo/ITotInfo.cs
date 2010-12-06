using System;
namespace Edu.Nlu.Sir.Siemens.TotInfo
{
    public interface ITotInfo
    {
        double gcf(double a, double x);
        double gser(double a, double x);
        double InfoTbl(int r, int c, long[] f, out int pdf);
        double LGamma(double x);
        double QChiSq(double chisq, int df);
        double QGamma(double a, double x);
    }
}

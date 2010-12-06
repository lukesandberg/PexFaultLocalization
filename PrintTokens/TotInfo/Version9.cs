using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Edu.Nlu.Sir.Siemens.Shared;

namespace Edu.Nlu.Sir.Siemens.TotInfo
{
    public class Version9: FaultyVersion, ITotInfo
    {
        public int[] FaultLines { get { return new int[] { 123 }; } }
        public FaultType FaultType { get { return FaultType.OPERATOR_CHANGE; } }
        public const int MAXLINE = 256;
        public const int MAXTBL = 1000;
        public const char COMMENT = '#';
        public const int NULL = 0;
        public const int EXIT_FAILURE = -1;
        public const int EXIT_SUCCESS = 0;

        public const int ITMAX = 100;
        public const double EPS = 3.0e-7;

        /// <summary>
        /// row/column header input line */
        /// </summary>
        public char[] line = new char[MAXLINE];
        /// <summary>
        /// frequency tallies
        /// </summary>
        public long[] f = new long[MAXTBL];
        /// <summary>
        /// # of rows
        /// </summary>
        public int r;
        /// <summary>
        /// # of columns
        /// </summary>
        public int c;


        void Main(string[] args)
        {
            char p;		/* input line scan location */
            int i;		/* row index */
            int j;		/* column index */
            double info;		/* computed information measure */
            int infodf;		/* degrees of freedom for information */
            double totinfo = 0.0;	/* accumulated information */
            int totdf;	/* accumulated degrees of freedom */

            totdf = 0;
            string lineStr;
            while ((lineStr = Console.ReadLine()) != null)	/* start new table */
            {
                line = lineStr.ToCharArray();
                p = line[0];
                for (int pi = 1; p != '\0' && Char.IsWhiteSpace(p); ++pi)
                {
                    p = line[pi];
                }

                if (p == '\0')
                    continue;	/* skip blank line */

                if (p == COMMENT)
                {		/* copy comment through */
                    Console.WriteLine(line);
                    continue;
                }

                string[] tokens = p.ToString().Split(" ".ToCharArray());
                bool parsable = Int32.TryParse(tokens[0], out r) && Int32.TryParse(tokens[1], out c);
                if (!parsable)
                {
                    Console.WriteLine("* invalid row/column line *\n");
                    Environment.Exit(EXIT_FAILURE);
                }

                if (r * c > MAXTBL)
                {
                    Console.WriteLine("* table too large *\n");
                    Environment.Exit(EXIT_FAILURE);
                }

                /* input tallies */
                
                string valuesString = Console.In.ReadToEnd();
                string[] valuesLine = valuesString.Split("\n".ToCharArray());
                string[][] values = (string[][])valuesLine.Select(l => l.Split(" ".ToCharArray()));
                long value;
                bool parsed;
                for (i = 0; i < r; ++i)
                {
                    for (j = 0; j < c; ++j)
                    {
                        parsed = Int64.TryParse(values[i][j], out value);
                        if (parsed)
                        {
                            f[i * c + j] = value;
                        }
                        else
                        {
                            Console.Write("* EOF in table *\n");
                            Environment.Exit(EXIT_FAILURE);
                        }
                    }
                }

                /* compute statistic */
                info = InfoTbl(r, c, f,out infodf);

                /* print results */

                if (info >= 0.0)
                {
                    Console.Write(String.Format("2info = {0,000.00}\tdf = {1,##}\tq = {2,000.0000}%7.4f\n",
                        info, infodf,
                        QChiSq(info, infodf))
                        );
                    totinfo += info;
                    totdf = infodf; //FAULT: Removed the + before = 
                }
                else
                    Console.Write(info < -3.5 ? "out of memory\n"
                    : info < -2.5 ? "table too small\n"
                    : info < -1.5 ? "negative freq\n"
                    : "table all zeros\n");
            }

            if (totdf <= 0)
            {
                Console.Write("\n*** no information accumulated ***\n");
                Environment.Exit(EXIT_FAILURE);
            }

            Console.Write("\ntotal 2info = %5.2f\tdf = %2d\tq = %7.4f\n",
                totinfo, totdf,
                QChiSq(totinfo, totdf)
                );
            Environment.Exit(EXIT_SUCCESS);
        }

        public double LGamma(double x)
        {
            double[] cof =
	{
		76.18009173,	-86.50532033,	24.01409822,
		-1.231739516,	0.120858003e-2,	-0.536382e-5
	};
            double tmp, ser;
            int j;


            if (--x < 0.0)	/* use reflection formula for accuracy */
            {
                double pix = Math.PI * x;

                return Math.Log(pix / Math.Sin(pix)) - LGamma(1.0 - x);
            }

            tmp = x + 5.5;
            tmp -= (x + 0.5) * Math.Log(tmp);

            ser = 1.0;

            for (j = 0; j < 6; ++j)
                ser += cof[j] / ++x;

            return -tmp + Math.Log(2.50662827465 * ser);
        }



        public double
            gser(double a, double x)
        {
            double ap, del, sum;
            int n;


            if (x <= 0.0)
                return 0.0;

            del = sum = 1.0 / (ap = a);

            for (n = 1; n <= ITMAX; ++n)
            {
                sum += del *= x / ++ap;

                if (Math.Abs(del) < Math.Abs(sum) * EPS)
                    return sum * Math.Exp(-x + a * Math.Log(x) - LGamma(a));
            }

            throw new ApplicationException("Execution should not have reached this line");
        }

        public double
            gcf(double a, double x)
        {
            int n;
            double gold = 0.0, fac = 1.0, b1 = 1.0,
                b0 = 0.0, a0 = 1.0, a1 = x;

            for (n = 1; n <= ITMAX; ++n)
            {
                double anf;
                double an = (double)n;
                double ana = an - a;

                a0 = (a1 + a0 * ana) * fac;
                b0 = (b1 + b0 * ana) * fac;
                anf = an * fac;
                b1 = x * b0 + anf * b1;
                a1 = x * a0 + anf * a1;

                if (a1 != 0.0)
                {		/* renormalize */
                    double g = b1 * (fac = 1.0 / a1);

                    gold = g - gold;

                    if (Math.Abs(gold) < EPS * Math.Abs(g))
                        return Math.Exp(-x + a * Math.Log(x) - LGamma(a)) * g;

                    gold = g;
                }
            }
            throw new ApplicationException("Execution should not have reached this line");
        }

        public double
            QGamma(double a, double x)
        {

            return x < a + 1.0 ? 1.0 - gser(a, x) : gcf(a, x);
        }

        public double
            QChiSq(double chisq, int df)
        {
            return QGamma((double)df / 2.0, chisq / 2.0);
        }




        /// <summary>
        /// InfoTbl -- Kullback's information measure for a 2-way contingency table
        ///last edit:	88/09/19	D A Gwyn
        ///SCCS ID:	@(#)info.c	1.1 (edited for publication)
        ///Special return values:
        ///-1.0	entire table consisted of 0 entries
        ///-2.0	invalid table entry (frequency less than 0)
        ///-3.0	invalid table dimensions (r or c less than 2)
        ///-4.0	unable to allocate enough working storage
        /// </summary>
        /// <param name="r"># rows in table</param>
        /// <param name="c"># columns in table </param>
        /// <param name="f">r*c frequency tallies</param>
        /// <param name="pdf">return # d.f. for chi-square</param>
        /// <returns></returns>
        public double
            InfoTbl(int r, int c, long[] f, out int pdf)
        {
            int i;		/* row index */
            int j;		/* column index */
            double N;		/* (double)n */
            double info;		/* accumulates information measure */
            double[] xi;		/* row sums */
            double[] xj;		/* col sums */
            int rdf = r - 1;	/* row degrees of freedom */
            int cdf = c - 1;	/* column degrees of freedom */

            if (rdf <= 0 || cdf <= 0)
            {
                pdf = 0;
                info = -3.0;
                goto ret3;
            }

            pdf = rdf * cdf;		/* total degrees of freedom */
            try {
            xi = new double[r];
            }
            catch(OutOfMemoryException)
            {
                info = -4.0;
                goto ret3;
            }
            try {
            xj = new double[c];
            } 
            catch(OutOfMemoryException)
            {
                info = -4.0;
                goto ret2;
            }

            /* compute row sums and total */

            N = 0.0;

            for (i = 0; i < r; ++i)
            {
                double sum = 0.0;	/* accumulator */

                for (j = 0; j < c; ++j)
                {
                    long k = f[(i) * c + (j)];

                    if (k < 0L)
                    {
                        info = -2.0;
                        /* 				goto ret1; missing code */
                    }

                    sum += (double)k;
                }

                N += xi[i] = sum;
            }

            if (N <= 0.0)
            {
                info = -1.0;
                goto ret1;
            }

            /* compute column sums */

            for (j = 0; j < c; ++j)
            {
                double sum = 0.0;	/* accumulator */

                for (i = 0; i < r; ++i)
                    sum += (double)f[(i) * c + (j)];

                xj[j] = sum;
            }

            /* compute information measure (four parts) */

            info = N * Math.Log(N);					/* part 1 */

            for (i = 0; i < r; ++i)
            {
                double pi = xi[i];	/* row sum */

                if (pi > 0.0)
                    info -= pi * Math.Log(pi);			/* part 2 */

                for (j = 0; j < c; ++j)
                {
                    double pij = (double)f[(i) * c + (j)];

                    if (pij > 0.0)
                        info += pij * Math.Log(pij);	/* part 3 */
                }
            }

            for (j = 0; j < c; ++j)
            {
                double pj = xj[j];	/* column sum */

                if (pj > 0.0)
                    info -= pj * Math.Log(pj);			/* part 4 */
            }

            info *= 2.0;			/* for comparability with chi-square */

        ret1:
            xj = null;
        ret2:
            xi = null;
        ret3:
            return info;
        }
    }
}

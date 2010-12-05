// <copyright file="TotInfoVersionFactory.getTotInfoVersion()Test.cs" company="General Electric">Copyright © General Electric 2010</copyright>
using System;
using Edu.Nlu.Sir.Siemens.TotInfo;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace Edu.Nlu.Sir.Siemens.TotInfo
{
    /// <summary>This class contains parameterized unit tests for TotInfoVersionFactory.getTotInfoVersion()</summary>
    [PexClass(typeof(ITotInfo))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class BaseVersionTest
    {
        /// <summary>Test stub for InfoTbl(Int32, Int32, Int64[], Int32&amp;)</summary>
        [PexMethod(MaxConstraintSolverTime = 5, MaxRunsWithoutNewTests = 200)]
        public double InfoTbl(
            int r,
            int c,
            long[] f,
            out int pdf
        )
        {
            PexAssume.IsNotNull(f);
            PexAssume.IsTrue(r != int.MinValue);
            PexAssume.IsTrue(c != int.MinValue);
            PexAssume.IsTrue((r - 1) * (c) + c-1 < f.Length);
            PexAssume.TrueForAll(f, i => i > 0);
            double result = TotInfoVersionFactory.getTotInfoVersion().InfoTbl(r, c, f, out pdf);
            return result;
            // TODO: add assertions to method TotInfoVersionFactory.getTotInfoVersion()Test.InfoTbl(Int32, Int32, Int64[], Int32&)
        }

        /// <summary>Test stub for LGamma(Double)</summary>
        [PexMethod]
        public double LGamma(double x)
        {
            double result = TotInfoVersionFactory.getTotInfoVersion().LGamma(x);
            return result;
            // TODO: add assertions to method TotInfoVersionFactory.getTotInfoVersion()Test.LGamma(Double)
        }

        /// <summary>Test stub for QChiSq(Double, Int32)</summary>
        [PexMethod]
        public double QChiSq(double chisq, int df)
        {
            double result = TotInfoVersionFactory.getTotInfoVersion().QChiSq(chisq, df);
            return result;
            // TODO: add assertions to method TotInfoVersionFactory.getTotInfoVersion()Test.QChiSq(Double, Int32)
        }

        /// <summary>Test stub for QGamma(Double, Double)</summary>
        [PexMethod(MaxWorkingSet = 700)]
        public double QGamma(double a, double x)
        {
            double result = TotInfoVersionFactory.getTotInfoVersion().QGamma(a, x);
            return result;
            // TODO: add assertions to method TotInfoVersionFactory.getTotInfoVersion()Test.QGamma(Double, Double)
        }

        /// <summary>Test stub for gcf(Double, Double)</summary>
        [PexMethod]
        public double gcf(double a, double x)
        {
            double result = TotInfoVersionFactory.getTotInfoVersion().gcf(a, x);
            return result;
            // TODO: add assertions to method TotInfoVersionFactory.getTotInfoVersion()Test.gcf(Double, Double)
        }

        /// <summary>Test stub for gser(Double, Double)</summary>
        [PexMethod]
        public double gser(double a, double x)
        {
            double result = TotInfoVersionFactory.getTotInfoVersion().gser(a, x);
            return result;
            // TODO: add assertions to method TotInfoVersionFactory.getTotInfoVersion()Test.gser(Double, Double)
        }
        
    }
}

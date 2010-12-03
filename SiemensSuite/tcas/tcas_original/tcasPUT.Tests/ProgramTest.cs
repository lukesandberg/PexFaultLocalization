// <copyright file="ProgramTest.cs" company="Authorized User">Copyright © Authorized User 2010</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tcas;

namespace tcas
{
    /// <summary>This class contains parameterized unit tests for Program</summary>
    [PexClass(typeof(Program))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ProgramTest
    {
        /// <summary>Test stub for ALIM()</summary>
        [PexMethod]
        public int ALIM()
        {
            int result = Program.ALIM();
            return result;
            // TODO: add assertions to method ProgramTest.ALIM()
        }

        /// <summary>Test stub for Inhibit_Biased_Climb()</summary>
        [PexMethod]
        public int Inhibit_Biased_Climb()
        {
            int result = Program.Inhibit_Biased_Climb();
            return result;
            // TODO: add assertions to method ProgramTest.Inhibit_Biased_Climb()
        }

        /// <summary>Test stub for Main(String[])</summary>
        [PexMethod]
        public void Main(string[] args)
        {
            short x;
            PexAssume.IsNotNullOrEmpty(args);
            PexAssume.IsTrue(args.Length == 12);
            for (int i = 0; i < args.Length; i++)
            {
                PexAssume.IsNotNullOrEmpty(args[i]);
                PexAssume.IsTrue(Int16.TryParse(args[i], out x));
                if (i == 1 || i == 2)
                {
                    PexAssume.IsTrue(Int16.Parse(args[i]) == 1 || Int16.Parse(args[i]) == 0);
                }
                if (i == 6)
                {
                    PexAssume.IsTrue(Int16.Parse(args[i]) <= 3 && Int16.Parse(args[i]) >= 0);
                } 

            }

            Program.Main(args);
            // TODO: add assertions to method ProgramTest.Main(String[])
        }

        /// <summary>Test stub for Non_Crossing_Biased_Climb()</summary>
        [PexMethod]
        public bool Non_Crossing_Biased_Climb()
        {
            bool result = Program.Non_Crossing_Biased_Climb();
            return result;
            // TODO: add assertions to method ProgramTest.Non_Crossing_Biased_Climb()
        }

        /// <summary>Test stub for Non_Crossing_Biased_Descend()</summary>
        [PexMethod]
        public bool Non_Crossing_Biased_Descend()
        {
            bool result = Program.Non_Crossing_Biased_Descend();
            return result;
            // TODO: add assertions to method ProgramTest.Non_Crossing_Biased_Descend()
        }

        /// <summary>Test stub for Own_Above_Threat()</summary>
        [PexMethod]
        public bool Own_Above_Threat()
        {
            bool result = Program.Own_Above_Threat();
            return result;
            // TODO: add assertions to method ProgramTest.Own_Above_Threat()
        }

        /// <summary>Test stub for Own_Below_Threat()</summary>
        [PexMethod]
        public bool Own_Below_Threat()
        {
            bool result = Program.Own_Below_Threat();
            return result;
            // TODO: add assertions to method ProgramTest.Own_Below_Threat()
        }

        /// <summary>Test stub for alt_sep_test()</summary>
        [PexMethod]
        public int alt_sep_test()
        {
            int result = Program.alt_sep_test();
            return result;
            // TODO: add assertions to method ProgramTest.alt_sep_test()
        }

        /// <summary>Test stub for initialize()</summary>
        [PexMethod]
        public void initialize()
        {
            Program.initialize();
            // TODO: add assertions to method ProgramTest.initialize()
        }
    }
}

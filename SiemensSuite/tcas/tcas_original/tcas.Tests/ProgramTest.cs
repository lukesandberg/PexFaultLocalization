// <copyright file="ProgramTest.cs" company="Authorized User">Copyright © Authorized User 2010</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tcas;

namespace tcas
{
    [TestClass]
    [PexClass(typeof(Program))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ProgramTest
    {
        [PexMethod]
        public bool Own_Below_Threat()
        {
            bool result = Program.Own_Below_Threat();
            return result;
            // TODO: add assertions to method ProgramTest.Own_Below_Threat()
        }
        [PexMethod]
        public bool Own_Above_Threat()
        {
            bool result = Program.Own_Above_Threat();
            return result;
            // TODO: add assertions to method ProgramTest.Own_Above_Threat()
        }
        [PexMethod]
        public bool Non_Crossing_Biased_Descend()
        {
            bool result = Program.Non_Crossing_Biased_Descend();
            return result;
            // TODO: add assertions to method ProgramTest.Non_Crossing_Biased_Descend()
        }
        [PexMethod]
        public bool Non_Crossing_Biased_Climb()
        {
            bool result = Program.Non_Crossing_Biased_Climb();
            return result;
            // TODO: add assertions to method ProgramTest.Non_Crossing_Biased_Climb()
        }
        [PexMethod]
        public void Main(string[] args)
        {
            Program.Main(args);
            // TODO: add assertions to method ProgramTest.Main(String[])
        }
        [PexMethod]
        public void initialize()
        {
            Program.initialize();
            // TODO: add assertions to method ProgramTest.initialize()
        }
        [PexMethod]
        public int Inhibit_Biased_Climb()
        {
            int result = Program.Inhibit_Biased_Climb();
            return result;
            // TODO: add assertions to method ProgramTest.Inhibit_Biased_Climb()
        }
        [PexMethod]
        public int alt_sep_test()
        {
            int result = Program.alt_sep_test();
            return result;
            // TODO: add assertions to method ProgramTest.alt_sep_test()
        }
        [PexMethod]
        public int ALIM()
        {
            int result = Program.ALIM();
            return result;
            // TODO: add assertions to method ProgramTest.ALIM()
        }
    }
}

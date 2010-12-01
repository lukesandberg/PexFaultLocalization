// <copyright file="EleTest.cs" company="Authorized User">Copyright © Authorized User 2010</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using schedule;

namespace schedule
{
    /// <summary>This class contains parameterized unit tests for Ele</summary>
    [PexClass(typeof(Ele))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class EleTest
    {
        /// <summary>Test stub for .ctor(Int32)</summary>
        [PexMethod]
        public Ele Constructor(int num)
        {
            Ele target = new Ele(num);
            return target;
            // TODO: add assertions to method EleTest.Constructor(Int32)
        }
    }
}

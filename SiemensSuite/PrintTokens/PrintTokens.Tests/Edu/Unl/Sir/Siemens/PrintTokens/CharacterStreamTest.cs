// <copyright file="CharacterStreamTest.cs">Copyright ©  2010</copyright>
using System;
using Edu.Unl.Sir.Siemens.PrintTokens;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edu.Unl.Sir.Siemens.PrintTokens
{
    /// <summary>This class contains parameterized unit tests for CharacterStream</summary>
    [PexClass(typeof(CharacterStream))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class CharacterStreamTest
    {
        /// <summary>Test stub for .ctor()</summary>
        [PexMethod]
        public CharacterStream Constructor()
        {
            CharacterStream target = new CharacterStream();
            return target;
            // TODO: add assertions to method CharacterStreamTest.Constructor()
        }
    }
}

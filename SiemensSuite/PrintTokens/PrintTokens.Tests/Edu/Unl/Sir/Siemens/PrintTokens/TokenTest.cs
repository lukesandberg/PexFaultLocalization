// <copyright file="TokenTest.cs">Copyright ©  2010</copyright>
using System;
using Edu.Unl.Sir.Siemens.PrintTokens;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edu.Unl.Sir.Siemens.PrintTokens
{
    /// <summary>This class contains parameterized unit tests for Token</summary>
    [PexClass(typeof(Token))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class TokenTest
    {
        /// <summary>Test stub for .ctor()</summary>
        [PexMethod]
        public Token Constructor()
        {
            Token target = new Token();
            return target;
            // TODO: add assertions to method TokenTest.Constructor()
        }
    }
}

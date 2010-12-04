// <copyright file="TokenStreamTest.cs">Copyright ©  2010</copyright>
using System;
using Edu.Unl.Sir.Siemens.PrintTokens;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edu.Unl.Sir.Siemens.PrintTokens
{
    /// <summary>This class contains parameterized unit tests for TokenStream</summary>
    [PexClass(typeof(TokenStream))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class TokenStreamTest
    {
    }
}

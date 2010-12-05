// <copyright file="ProcessTest.cs" company="Authorized User">Copyright © Authorized User 2010</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using schedule2;

namespace schedule2
{
    /// <summary>This class contains parameterized unit tests for Process</summary>
    [PexClass(typeof(Process))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ProcessTest
    {
    }
}

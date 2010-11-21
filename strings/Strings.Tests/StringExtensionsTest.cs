// <copyright file="StringExtensionsTest.cs" company="MSIT">Copyright © MSIT 2008</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strings;

namespace Strings
{
    [TestClass]
    [PexClass(typeof(StringExtensions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class StringExtensionsTest
    {
        [PexMethod]
        public string Capitalize(string value)
        {
            string result = StringExtensions.Capitalize(value);
            PexAssert.IsTrue(!result.Contains("."));
            PexAssert.IsTrue(result == String.Empty || !Char.IsLetter(result[0]) || Char.IsUpper(result[0]));
            return result;
            // TODO: add assertions to method StringExtensionsTest.Capitalize(String)
           
        }
    }
}

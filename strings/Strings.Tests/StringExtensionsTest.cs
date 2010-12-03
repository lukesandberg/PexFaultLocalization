// <copyright file="StringExtensionsTest.cs" company="MSIT">Copyright © MSIT 2008</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strings;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Pex.Framework.Exceptions;

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
			PexAssume.IsNotNull(value);
			string result = StringExtensions.Capitalize(value);
			PexAssert.IsTrue(!result.Contains("."));
			PexAssert.ImpliesIsTrue(result.Length > 0 && Char.IsLetter(result[0]), () => Char.ToUpper(value[0]) == result[0]);
			PexAssert.ImpliesIsTrue(value.Equals(""), () => result.Equals(""));
			return result;
		}
	}
	[TestClass]
	[PexClass(typeof(TestObject))]
	public partial class TestObjectTest
	{
		[PexMethod]
		public string Do(int i, String s)
		{
			var obj = new TestObject(i, s);
			PexAssume.IsNotNull(obj);
			string result = obj.Do();
			PexAssert.IsNotNull(result);
			return result;
		}
	}
}

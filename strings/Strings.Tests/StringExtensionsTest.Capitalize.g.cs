// <copyright file="StringExtensionsTest.Capitalize.g.cs" company="MSIT">Copyright � MSIT 2008</copyright>
// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace Strings
{
    public partial class StringExtensionsTest
    {
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
[PexRaisedException(typeof(NullReferenceException))]
public void CapitalizeThrowsNullReferenceException195()
{
    string s;
    s = this.Capitalize((string)null);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize983()
{
    string s;
    s = this.Capitalize("");
    Assert.AreEqual<string>("", s);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize870()
{
    string s;
    s = this.Capitalize("\0");
    Assert.AreEqual<string>("", s);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize704()
{
    string s;
    s = this.Capitalize(".");
    Assert.AreEqual<string>("_", s);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize439()
{
    string s;
    s = this.Capitalize("a");
    Assert.AreEqual<string>("A", s);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
[PexRaisedException(typeof(FormatException))]
public void CapitalizeThrowsFormatException90()
{
    string s;
    s = this.Capitalize("jksdfhlasfhlskjfhsaljf");
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize582()
{
    string s;
    s = this.Capitalize("a\0");
    Assert.AreEqual<string>("A", s);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize322()
{
    string s;
    s = this.Capitalize("Pa");
    Assert.AreEqual<string>("Pa", s);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize67()
{
    string s;
    s = this.Capitalize("Aaa");
    Assert.AreEqual<string>("Aaa", s);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize896()
{
    string s;
    s = this.Capitalize(".\0");
    Assert.AreEqual<string>("_", s);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize817()
{
    string s;
    s = this.Capitalize("..");
    Assert.AreEqual<string>("__", s);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize825()
{
    string s;
    s = this.Capitalize("aaaaa.a");
    Assert.AreEqual<string>("Aaaaa_A", s);
}
[TestMethod]
[PexGeneratedBy(typeof(StringExtensionsTest))]
public void Capitalize948()
{
    string s;
    s = this.Capitalize("Aaaaa.aa.a.aaaaa..\0\0");
    Assert.AreEqual<string>("Aaaaa_Aa_A_Aaaaa__", s);
}
    }
}
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

namespace Edu.Unl.Sir.Siemens.PrintTokens2
{
    public partial class originalTest
    {
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void is_identifier820()
{
    int i;
    i = this.is_identifier("\0\0");
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void is_identifier878()
{
    int i;
    i = this.is_identifier("a\0");
    Assert.AreEqual<int>(1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void is_identifier40()
{
    int i;
    i = this.is_identifier("a\u0001\0\0\0\0");
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void is_identifier962()
{
    int i;
    i = this.is_identifier("a1\0\0");
    Assert.AreEqual<int>(1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void is_identifier7()
{
    int i;
    i = this.is_identifier("aa\0\0\0\0");
    Assert.AreEqual<int>(1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void is_identifier384()
{
    int i;
    i = this.is_identifier("a0\u0001\0\0\0");
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void is_identifier787()
{
    int i;
    i = this.is_identifier("a0a\0\0\0");
    Assert.AreEqual<int>(1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void is_identifier329()
{
    int i;
    i = this.is_identifier("aaa\0\0\0");
    Assert.AreEqual<int>(1, i);
}
    }
}

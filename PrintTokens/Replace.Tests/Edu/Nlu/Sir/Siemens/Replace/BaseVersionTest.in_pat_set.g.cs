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

namespace Edu.Nlu.Sir.Siemens.Replace
{
    public partial class BaseVersionTest
    {
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void in_pat_set98()
{
    bool b;
    b = this.in_pat_set(' ');
    Assert.AreEqual<bool>(false, b);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void in_pat_set562()
{
    bool b;
    b = this.in_pat_set('[');
    Assert.AreEqual<bool>(true, b);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void in_pat_set142()
{
    bool b;
    b = this.in_pat_set('!');
    Assert.AreEqual<bool>(true, b);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void in_pat_set948()
{
    bool b;
    b = this.in_pat_set('c');
    Assert.AreEqual<bool>(true, b);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void in_pat_set812()
{
    bool b;
    b = this.in_pat_set('?');
    Assert.AreEqual<bool>(true, b);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void in_pat_set281()
{
    bool b;
    b = this.in_pat_set('%');
    Assert.AreEqual<bool>(true, b);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void in_pat_set254()
{
    bool b;
    b = this.in_pat_set('$');
    Assert.AreEqual<bool>(true, b);
}
    }
}

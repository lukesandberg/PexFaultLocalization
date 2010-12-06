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

namespace Edu.Nlu.Sir.Siemens.TotInfo
{
    public partial class BaseVersionTest
    {
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void QChiSq936()
{
    double d;
    d = this.QChiSq(0, 0);
    Assert.AreEqual<double>(1, d);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void QChiSq817()
{
    double d;
    d = this.QChiSq(0.5, 1969057606);
    Assert.AreEqual<double>(1, d);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void QChiSq427()
{
    double d;
    d = this.QChiSq(2.2148835789167639E-251, -1);
    Assert.AreEqual<double>(double.NaN, d);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void QChiSq884()
{
    double d;
    d = this.QChiSq(4.77326203309412E+90, -1459709771);
    Assert.AreEqual<double>(0, d);
}
    }
}

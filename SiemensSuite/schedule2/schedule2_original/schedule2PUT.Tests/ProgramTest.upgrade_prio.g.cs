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

namespace schedule2
{
    public partial class ProgramTest
    {
[TestMethod]
[PexGeneratedBy(typeof(ProgramTest))]
public void upgrade_prio998()
{
    int i;
    i = this.upgrade_prio(0, (float)0);
    Assert.AreEqual<int>(-4, i);
}
[TestMethod]
[PexGeneratedBy(typeof(ProgramTest))]
public void upgrade_prio774()
{
    int i;
    i = this.upgrade_prio(1, (float)0);
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(ProgramTest))]
public void upgrade_prio751()
{
    int i;
    i = this.upgrade_prio(1, (float)(-1));
    Assert.AreEqual<int>(-5, i);
}
    }
}

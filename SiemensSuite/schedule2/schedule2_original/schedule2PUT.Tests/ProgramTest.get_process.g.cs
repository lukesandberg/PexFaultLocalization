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
public void get_process338()
{
    int i;
    Process process = (Process)null;
    i = this.get_process(0, (float)0, ref process);
    Assert.AreEqual<int>(1, i);
    Assert.IsNotNull((object)process);
    Assert.AreEqual<int>(0, process.pid);
    Assert.AreEqual<int>(0, process.priority);
}
[TestMethod]
[PexGeneratedBy(typeof(ProgramTest))]
public void get_process256()
{
    int i;
    Process process = (Process)null;
    i = this.get_process(2, (float)0, ref process);
    Assert.AreEqual<int>(0, i);
    Assert.IsNull((object)process);
}
[TestMethod]
[PexGeneratedBy(typeof(ProgramTest))]
public void get_process804()
{
    int i;
    Process process = (Process)null;
    i = this.get_process(0, (float)(-1), ref process);
    Assert.AreEqual<int>(-5, i);
    Assert.IsNull((object)process);
}
    }
}
// <copyright file="BinaryHeapTPriorityTValueTest.Constructor.g.cs" company="MSIT">Copyright © MSIT 2007</copyright>
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
using QuickGraph;
using Microsoft.Pex.Engine.Exceptions;

namespace QuickGraph.Collections
{
    public partial class BinaryHeapTPriorityTValueTest
    {
[TestMethod]
[PexGeneratedBy(typeof(BinaryHeapTPriorityTValueTest))]
public void Constructor703()
{
    this.Constructor<int, int>(0);
}
[TestMethod]
[PexGeneratedBy(typeof(BinaryHeapTPriorityTValueTest))]
[PexRaisedException(typeof(OverflowException))]
public void ConstructorThrowsOverflowException896()
{
    this.Constructor<int, int>(int.MinValue);
}
    }
}

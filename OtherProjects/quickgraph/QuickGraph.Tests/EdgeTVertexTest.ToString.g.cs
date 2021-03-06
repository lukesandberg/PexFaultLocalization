// <copyright file="EdgeTVertexTest.ToString.g.cs" company="MSIT">Copyright © MSIT 2007</copyright>
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

namespace QuickGraph
{
    public partial class EdgeTVertexTest
    {
[TestMethod]
[PexGeneratedBy(typeof(EdgeTVertexTest))]
public void ToString582()
{
    Edge<int> edge;
    string s;
    edge = EdgeFactory.Create(0, 0);
    s = this.ToString<int>(edge);
    Assert.AreEqual<string>("0->0", s);
    Assert.IsNotNull((object)edge);
    Assert.AreEqual<int>(0, edge.Source);
    Assert.AreEqual<int>(0, edge.Target);
}
[TestMethod]
[PexGeneratedBy(typeof(EdgeTVertexTest))]
public void ToString458()
{
    Edge<int> edge;
    string s;
    edge = EdgeFactory.Create(8, 0);
    s = this.ToString<int>(edge);
    Assert.AreEqual<string>("8->0", s);
    Assert.IsNotNull((object)edge);
    Assert.AreEqual<int>(8, edge.Source);
    Assert.AreEqual<int>(0, edge.Target);
}
    }
}

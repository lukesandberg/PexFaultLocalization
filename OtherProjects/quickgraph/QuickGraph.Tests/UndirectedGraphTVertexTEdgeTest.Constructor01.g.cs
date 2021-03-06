// <copyright file="UndirectedGraphTVertexTEdgeTest.Constructor01.g.cs" company="MSIT">Copyright � MSIT 2007</copyright>
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
    public partial class UndirectedGraphTVertexTEdgeTest
    {
[TestMethod]
[PexGeneratedBy(typeof(UndirectedGraphTVertexTEdgeTest))]
public void Constructor01157()
{
    UndirectedGraph<int, Edge<int>> undirectedGraph;
    undirectedGraph = this.Constructor01<int, Edge<int>>(false);
    Assert.IsNotNull((object)undirectedGraph);
    Assert.IsNotNull(undirectedGraph.EdgeEqualityComparer);
    Assert.AreEqual<int>(4, undirectedGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, undirectedGraph.IsDirected);
    Assert.AreEqual<bool>(false, undirectedGraph.AllowParallelEdges);
    Assert.AreEqual<int>(0, undirectedGraph.EdgeCount);
}
[TestMethod]
[PexGeneratedBy(typeof(UndirectedGraphTVertexTEdgeTest))]
public void Constructor0115701()
{
    UndirectedGraph<int, SEdge<int>> undirectedGraph;
    undirectedGraph = this.Constructor01<int, SEdge<int>>(false);
    Assert.IsNotNull((object)undirectedGraph);
    Assert.IsNotNull(undirectedGraph.EdgeEqualityComparer);
    Assert.AreEqual<int>(4, undirectedGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, undirectedGraph.IsDirected);
    Assert.AreEqual<bool>(false, undirectedGraph.AllowParallelEdges);
    Assert.AreEqual<int>(0, undirectedGraph.EdgeCount);
}
[TestMethod]
[PexGeneratedBy(typeof(UndirectedGraphTVertexTEdgeTest))]
public void Constructor397()
{
    UndirectedGraph<int, Edge<int>> undirectedGraph;
    undirectedGraph = this.Constructor<int, Edge<int>>();
    Assert.IsNotNull((object)undirectedGraph);
    Assert.IsNotNull(undirectedGraph.EdgeEqualityComparer);
    Assert.AreEqual<int>(4, undirectedGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, undirectedGraph.IsDirected);
    Assert.AreEqual<bool>(true, undirectedGraph.AllowParallelEdges);
    Assert.AreEqual<int>(0, undirectedGraph.EdgeCount);
}
[TestMethod]
[PexGeneratedBy(typeof(UndirectedGraphTVertexTEdgeTest))]
public void Constructor39701()
{
    UndirectedGraph<int, SEdge<int>> undirectedGraph;
    undirectedGraph = this.Constructor<int, SEdge<int>>();
    Assert.IsNotNull((object)undirectedGraph);
    Assert.IsNotNull(undirectedGraph.EdgeEqualityComparer);
    Assert.AreEqual<int>(4, undirectedGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, undirectedGraph.IsDirected);
    Assert.AreEqual<bool>(true, undirectedGraph.AllowParallelEdges);
    Assert.AreEqual<int>(0, undirectedGraph.EdgeCount);
}
    }
}

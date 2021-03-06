// <copyright file="UndirectedGraphTVertexTEdgeTest.AddEdgeRange.g.cs" company="MSIT">Copyright � MSIT 2007</copyright>
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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace QuickGraph
{
    public partial class UndirectedGraphTVertexTEdgeTest
    {
[TestMethod]
[PexGeneratedBy(typeof(UndirectedGraphTVertexTEdgeTest))]
public void AddEdgeRange327()
{
    UndirectedGraph<int, Edge<int>> undirectedGraph;
    int i;
    undirectedGraph = new UndirectedGraph<int, Edge<int>>
                          (false, (EdgeEqualityComparer<int, Edge<int>>)null);
    undirectedGraph.EdgeCapacity = 0;
    Edge<int>[] edges = new Edge<int>[0];
    i = this.AddEdgeRange<int, Edge<int>>
            (undirectedGraph, (IEnumerable<Edge<int>>)edges);
    Assert.AreEqual<int>(0, i);
    Assert.IsNotNull((object)undirectedGraph);
    Assert.IsNull(undirectedGraph.EdgeEqualityComparer);
    Assert.AreEqual<int>(0, undirectedGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, undirectedGraph.IsDirected);
    Assert.AreEqual<bool>(false, undirectedGraph.AllowParallelEdges);
    Assert.AreEqual<int>(0, undirectedGraph.EdgeCount);
}
    }
}

// <copyright file="UndirectedGraphTVertexTEdgeTest.RemoveEdges.g.cs" company="MSIT">Copyright � MSIT 2007</copyright>
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
public void RemoveEdges327()
{
    UndirectedGraph<int, Edge<int>> undirectedGraph;
    int i;
    undirectedGraph = new UndirectedGraph<int, Edge<int>>
                          (false, (EdgeEqualityComparer<int, Edge<int>>)null);
    undirectedGraph.EdgeCapacity = 0;
    Edge<int>[] edges = new Edge<int>[0];
    i = this.RemoveEdges<int, Edge<int>>
            (undirectedGraph, (IEnumerable<Edge<int>>)edges);
    Assert.AreEqual<int>(0, i);
    Assert.IsNotNull((object)undirectedGraph);
    Assert.IsNull(undirectedGraph.EdgeEqualityComparer);
    Assert.AreEqual<int>(0, undirectedGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, undirectedGraph.IsDirected);
    Assert.AreEqual<bool>(false, undirectedGraph.AllowParallelEdges);
    Assert.AreEqual<int>(0, undirectedGraph.EdgeCount);
}
[TestMethod]
[PexGeneratedBy(typeof(UndirectedGraphTVertexTEdgeTest))]
[PexRaisedException(typeof(KeyNotFoundException))]
public void RemoveEdgesThrowsKeyNotFoundException545()
{
    UndirectedGraph<int, Edge<int>> undirectedGraph;
    Edge<int> edge;
    int i;
    undirectedGraph = new UndirectedGraph<int, Edge<int>>
                          (false, (EdgeEqualityComparer<int, Edge<int>>)null);
    undirectedGraph.EdgeCapacity = 0;
    edge = EdgeFactory.Create(0, 0);
    Edge<int>[] edges = new Edge<int>[8];
    edges[0] = edge;
    edges[1] = edge;
    edges[2] = edge;
    edges[3] = edge;
    edges[4] = edge;
    edges[5] = edge;
    edges[6] = edge;
    edges[7] = edge;
    i = this.RemoveEdges<int, Edge<int>>
            (undirectedGraph, (IEnumerable<Edge<int>>)edges);
}
    }
}

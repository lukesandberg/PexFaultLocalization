// <copyright file="AdjacencyGraphTVertexTEdgeTest.AddVerticesAndEdgeRange.g.cs" company="MSIT">Copyright © MSIT 2007</copyright>
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
using Microsoft.Pex.Framework.Exceptions;
using System.Diagnostics.Contracts;

namespace QuickGraph
{
    public partial class AdjacencyGraphTVertexTEdgeTest
    {
[TestMethod]
[PexGeneratedBy(typeof(AdjacencyGraphTVertexTEdgeTest))]
public void AddVerticesAndEdgeRange749()
{
    AdjacencyGraph<int, Edge<int>> adjacencyGraph;
    int i;
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[1];
    KeyValuePair<int, int> s0 = new KeyValuePair<int, int>(256, 304);
    keyValuePairs[0] = s0;
    adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
    Edge<int>[] edges = new Edge<int>[0];
    i = this.AddVerticesAndEdgeRange<int, Edge<int>>(adjacencyGraph, edges);
    Assert.AreEqual<int>(0, i);
    Assert.IsNotNull((object)adjacencyGraph);
    Assert.AreEqual<bool>(true, adjacencyGraph.IsDirected);
    Assert.AreEqual<bool>(false, adjacencyGraph.AllowParallelEdges);
    Assert.AreEqual<int>(-1, adjacencyGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, adjacencyGraph.IsEdgesEmpty);
    Assert.AreEqual<int>(1, adjacencyGraph.EdgeCount);
}
[TestMethod]
[PexGeneratedBy(typeof(AdjacencyGraphTVertexTEdgeTest))]
public void AddVerticesAndEdgeRange734()
{
    AdjacencyGraph<int, Edge<int>> adjacencyGraph;
    Edge<int> edge;
    int i;
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[1];
    KeyValuePair<int, int> s0 = new KeyValuePair<int, int>(209, 134);
    keyValuePairs[0] = s0;
    adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
    edge = EdgeFactory.Create(0, 0);
    Edge<int>[] edges = new Edge<int>[1];
    edges[0] = edge;
    i = this.AddVerticesAndEdgeRange<int, Edge<int>>(adjacencyGraph, edges);
    Assert.AreEqual<int>(1, i);
    Assert.IsNotNull((object)adjacencyGraph);
    Assert.AreEqual<bool>(true, adjacencyGraph.IsDirected);
    Assert.AreEqual<bool>(false, adjacencyGraph.AllowParallelEdges);
    Assert.AreEqual<int>(-1, adjacencyGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, adjacencyGraph.IsEdgesEmpty);
    Assert.AreEqual<int>(2, adjacencyGraph.EdgeCount);
}
[TestMethod]
[PexGeneratedBy(typeof(AdjacencyGraphTVertexTEdgeTest))]
public void AddVerticesAndEdgeRange687()
{
    AdjacencyGraph<int, Edge<int>> adjacencyGraph;
    Edge<int> edge;
    int i;
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[4];
    adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
    edge = EdgeFactory.Create(0, 0);
    Edge<int>[] edges = new Edge<int>[4];
    edges[0] = edge;
    edges[1] = edge;
    edges[2] = edge;
    edges[3] = edge;
    i = this.AddVerticesAndEdgeRange<int, Edge<int>>(adjacencyGraph, edges);
    Assert.AreEqual<int>(1, i);
    Assert.IsNotNull((object)adjacencyGraph);
    Assert.AreEqual<bool>(true, adjacencyGraph.IsDirected);
    Assert.AreEqual<bool>(false, adjacencyGraph.AllowParallelEdges);
    Assert.AreEqual<int>(-1, adjacencyGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, adjacencyGraph.IsEdgesEmpty);
    Assert.AreEqual<int>(1, adjacencyGraph.EdgeCount);
}
[TestMethod]
[PexGeneratedBy(typeof(AdjacencyGraphTVertexTEdgeTest))]
public void AddVerticesAndEdgeRange478()
{
    AdjacencyGraph<int, Edge<int>> adjacencyGraph;
    Edge<int> edge;
    int i;
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[4];
    adjacencyGraph = AdjacencyGraphFactory.Create(true, keyValuePairs);
    edge = EdgeFactory.Create(0, 0);
    Edge<int>[] edges = new Edge<int>[4];
    edges[0] = edge;
    edges[1] = edge;
    edges[2] = edge;
    edges[3] = edge;
    i = this.AddVerticesAndEdgeRange<int, Edge<int>>(adjacencyGraph, edges);
    Assert.AreEqual<int>(4, i);
    Assert.IsNotNull((object)adjacencyGraph);
    Assert.AreEqual<bool>(true, adjacencyGraph.IsDirected);
    Assert.AreEqual<bool>(true, adjacencyGraph.AllowParallelEdges);
    Assert.AreEqual<int>(-1, adjacencyGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, adjacencyGraph.IsEdgesEmpty);
    Assert.AreEqual<int>(4, adjacencyGraph.EdgeCount);
}
[TestMethod]
[PexGeneratedBy(typeof(AdjacencyGraphTVertexTEdgeTest))]
public void AddVerticesAndEdgeRange187()
{
    AdjacencyGraph<int, Edge<int>> adjacencyGraph;
    Edge<int> edge;
    int i;
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[1];
    KeyValuePair<int, int> s0 = new KeyValuePair<int, int>(default(int), 930);
    keyValuePairs[0] = s0;
    adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
    edge = EdgeFactory.Create(0, 0);
    Edge<int>[] edges = new Edge<int>[1];
    edges[0] = edge;
    i = this.AddVerticesAndEdgeRange<int, Edge<int>>(adjacencyGraph, edges);
    Assert.AreEqual<int>(1, i);
    Assert.IsNotNull((object)adjacencyGraph);
    Assert.AreEqual<bool>(true, adjacencyGraph.IsDirected);
    Assert.AreEqual<bool>(false, adjacencyGraph.AllowParallelEdges);
    Assert.AreEqual<int>(-1, adjacencyGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, adjacencyGraph.IsEdgesEmpty);
    Assert.AreEqual<int>(2, adjacencyGraph.EdgeCount);
}
[TestMethod]
[PexGeneratedBy(typeof(AdjacencyGraphTVertexTEdgeTest))]
public void AddVerticesAndEdgeRange411()
{
    AdjacencyGraph<int, Edge<int>> adjacencyGraph;
    Edge<int> edge;
    int i;
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[1];
    KeyValuePair<int, int> s0 = new KeyValuePair<int, int>(int.MinValue, 817907712);
    keyValuePairs[0] = s0;
    adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
    edge = EdgeFactory.Create(10, 2);
    Edge<int>[] edges = new Edge<int>[1];
    edges[0] = edge;
    i = this.AddVerticesAndEdgeRange<int, Edge<int>>(adjacencyGraph, edges);
    Assert.AreEqual<int>(1, i);
    Assert.IsNotNull((object)adjacencyGraph);
    Assert.AreEqual<bool>(true, adjacencyGraph.IsDirected);
    Assert.AreEqual<bool>(false, adjacencyGraph.AllowParallelEdges);
    Assert.AreEqual<int>(-1, adjacencyGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, adjacencyGraph.IsEdgesEmpty);
    Assert.AreEqual<int>(2, adjacencyGraph.EdgeCount);
}
[TestMethod]
[PexGeneratedBy(typeof(AdjacencyGraphTVertexTEdgeTest))]
public void AddVerticesAndEdgeRange982()
{
    AdjacencyGraph<int, Edge<int>> adjacencyGraph;
    Edge<int> edge;
    Edge<int> edge1;
    int i;
    KeyValuePair<int, int>[] keyValuePairs = new KeyValuePair<int, int>[4];
    adjacencyGraph = AdjacencyGraphFactory.Create(false, keyValuePairs);
    edge = EdgeFactory.Create(-2147483632, -2147483632);
    edge1 = EdgeFactory.Create(0, 0);
    Edge<int>[] edges = new Edge<int>[4];
    edges[0] = edge1;
    edges[1] = edge;
    edges[2] = edge1;
    edges[3] = edge1;
    i = this.AddVerticesAndEdgeRange<int, Edge<int>>(adjacencyGraph, edges);
    Assert.AreEqual<int>(2, i);
    Assert.IsNotNull((object)adjacencyGraph);
    Assert.AreEqual<bool>(true, adjacencyGraph.IsDirected);
    Assert.AreEqual<bool>(false, adjacencyGraph.AllowParallelEdges);
    Assert.AreEqual<int>(-1, adjacencyGraph.EdgeCapacity);
    Assert.AreEqual<bool>(false, adjacencyGraph.IsEdgesEmpty);
    Assert.AreEqual<int>(2, adjacencyGraph.EdgeCount);
}
    }
}

// <copyright file="OrdSequenceTests.Test_ExtractAll.g.cs">Copyright �  2008</copyright>
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

namespace FPTests
{
	public partial class OrdSequenceTests
	{
[TestMethod]
[PexGeneratedBy(typeof(OrdSequenceTests))]
public void Test_ExtractAll607()
{
    int[] ints = new int[0];
    this.Test_ExtractAll<int>(ints, 0);
}
[TestMethod]
[PexGeneratedBy(typeof(OrdSequenceTests))]
public void Test_ExtractAll591()
{
    int[] ints = new int[1];
    ints[0] = 2;
    this.Test_ExtractAll<int>(ints, 512);
}
[TestMethod]
[PexGeneratedBy(typeof(OrdSequenceTests))]
public void Test_ExtractAll52()
{
    int[] ints = new int[1];
    ints[0] = -2147483136;
    this.Test_ExtractAll<int>(ints, -2147483647);
}
[TestMethod]
[PexGeneratedBy(typeof(OrdSequenceTests))]
public void Test_ExtractAll307()
{
    int[] ints = new int[1];
    ints[0] = -2147482640;
    this.Test_ExtractAll<int>(ints, -2147482640);
}
[TestMethod]
[PexGeneratedBy(typeof(OrdSequenceTests))]
public void Test_ExtractAll99()
{
    int[] ints = new int[2];
    ints[0] = -2147483444;
    ints[1] = -2147483572;
    this.Test_ExtractAll<int>(ints, -2147483572);
}
[TestMethod]
[PexGeneratedBy(typeof(OrdSequenceTests))]
public void Test_ExtractAll362()
{
    int[] ints = new int[2];
    ints[0] = -2147483642;
    ints[1] = -2147483642;
    this.Test_ExtractAll<int>(ints, -2147483642);
}
[TestMethod]
[PexGeneratedBy(typeof(OrdSequenceTests))]
public void Test_ExtractAll323()
{
    int[] ints = new int[6];
    ints[0] = 987;
    ints[1] = 485;
    ints[2] = 1;
    ints[3] = 987;
    ints[4] = 969;
    ints[5] = 768;
    this.Test_ExtractAll<int>(ints, 0);
}
	}
}
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

namespace Edu.Unl.Sir.Siemens.PrintTokens2.Orig
{
    public partial class originalTest
    {
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void is_token_end379()
{
    int i;
    i = this.is_token_end(0, ' ');
    Assert.AreEqual<int>(1, i);
}
    }
}
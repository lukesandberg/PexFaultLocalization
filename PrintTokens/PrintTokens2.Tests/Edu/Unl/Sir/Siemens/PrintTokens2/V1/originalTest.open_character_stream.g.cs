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
using Microsoft.Pex.Framework.Generated;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edu.Unl.Sir.Siemens.PrintTokens2.Orig
{
    public partial class originalTest
    {
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void open_character_stream417()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      Stream stream;
      stream = this.open_character_stream("\u0089");
      disposables.Add((IDisposable)stream);
      disposables.Dispose();
      Assert.IsNull((object)stream);
    }
}
    }
}

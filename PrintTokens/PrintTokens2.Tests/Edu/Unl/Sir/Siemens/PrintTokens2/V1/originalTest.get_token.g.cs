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
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace Edu.Unl.Sir.Siemens.PrintTokens2.Orig
{
    public partial class originalTest
    {
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
[PexRaisedException(typeof(NullReferenceException))]
public void get_tokenThrowsNullReferenceException565()
{
    string s;
    s = this.get_token((Stream)null);
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void get_token364()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      string s;
      byte[] bs = new byte[0];
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      s = this.get_token((Stream)memoryStream);
      disposables.Dispose();
      Assert.AreEqual<string>("System.Char[]", s);
    }
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void get_token36401()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      string s;
      byte[] bs = new byte[1];
      bs[0] = (byte)10;
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      s = this.get_token((Stream)memoryStream);
      disposables.Dispose();
      Assert.AreEqual<string>("System.Char[]", s);
    }
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void get_token36402()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      string s;
      byte[] bs = new byte[1];
      bs[0] = (byte)32;
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      s = this.get_token((Stream)memoryStream);
      disposables.Dispose();
      Assert.AreEqual<string>("System.Char[]", s);
    }
}
[TestMethod]
[PexGeneratedBy(typeof(originalTest))]
public void get_token36403()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      string s;
      byte[] bs = new byte[2];
      bs[0] = (byte)32;
      bs[1] = (byte)32;
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      s = this.get_token((Stream)memoryStream);
      disposables.Dispose();
      Assert.AreEqual<string>("System.Char[]", s);
    }
}
    }
}
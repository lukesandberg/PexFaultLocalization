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

namespace Edu.Nlu.Sir.Siemens.Replace
{
    public partial class BaseVersionTest
    {
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch438()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(0, i);
}

[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch44()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs1[0] = '\u0001';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: system environment exit")]
public void amatchThrowsExitException631()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs1[0] = '*';
    i = this.amatch(cs, 0, cs1, 0);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch232()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs1[0] = '*';
    cs1[1] = '*';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch163()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs1[0] = '*';
    cs1[1] = '!';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch38()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs1[0] = '*';
    cs1[1] = '$';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: system environment exit")]
public void amatchThrowsExitException566()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '*';
    i = this.amatch(cs, 0, cs1, 0);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch189()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs1[0] = '*';
    cs1[1] = '[';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch327()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '%';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch469()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '?';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch584()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\n';
    cs1[0] = '?';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch119()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[1] = '\u0001';
    cs1[0] = '%';
    i = this.amatch(cs, 1, cs1, 0);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch572()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = 'c';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch922()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = 'c';
    cs1[1] = '\u0001';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch849()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '$';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch700()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\n';
    cs1[0] = '$';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch406()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '!';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(1, i);
}

[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch650()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\n';
    cs1[0] = '!';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch305()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[33] = '!';
    cs1[34] = '\u0001';
    cs1[35] = '\u0001';
    i = this.amatch(cs, 0, cs1, 33);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch198()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '[';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch834()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[33] = '[';
    cs1[34] = '\u0001';
    cs1[35] = '\u0001';
    i = this.amatch(cs, 0, cs1, 33);
    Assert.AreEqual<int>(1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch951()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '*';
    cs1[1] = '!';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(1, i);
}

[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: path bounds exceeded")]
public void amatch126()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '*';
    cs1[1] = '%';
    i = this.amatch(cs, 0, cs1, 0);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch871()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '*';
    cs1[1] = '!';
    cs1[2] = '\a';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch254()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\n';
    cs1[0] = '*';
    cs1[1] = '!';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch22()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs[1] = '\u0001';
    cs1[3] = '*';
    cs1[4] = '!';
    i = this.amatch(cs, 0, cs1, 3);
    Assert.AreEqual<int>(2, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: system environment exit")]
public void amatchThrowsExitException508()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[63] = '\u0001';
    cs1[0] = '*';
    cs1[1] = '?';
    cs1[2] = '\u0002';
    i = this.amatch(cs, 63, cs1, 0);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch812()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[63] = '\u0001';
    cs[64] = '\u0001';
    cs1[62] = '*';
    cs1[63] = '!';
    cs1[65] = '!';
    i = this.amatch(cs, 63, cs1, 62);
    Assert.AreEqual<int>(65, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch588()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[63] = '\u0001';
    cs[64] = '\u0001';
    cs1[63] = '*';
    cs1[64] = '!';
    cs1[66] = '*';
    cs1[67] = '!';
    i = this.amatch(cs, 63, cs1, 63);
    Assert.AreEqual<int>(65, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch47()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[63] = '\u0001';
    cs1[0] = '*';
    cs1[1] = '?';
    cs1[2] = '?';
    i = this.amatch(cs, 63, cs1, 0);
    Assert.AreEqual<int>(64, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: system environment exit")]
public void amatchThrowsExitException447()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[62] = '\u0001';
    cs[63] = '\u0001';
    cs1[2] = '*';
    cs1[3] = '!';
    cs1[5] = '\u0001';
    i = this.amatch(cs, 62, cs1, 2);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: system environment exit")]
public void amatchThrowsExitException340()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[63] = ' ';
    cs[64] = '@';
    cs1[8] = '*';
    cs1[9] = '!';
    cs1[11] = '!';
    cs1[13] = '\u0080';
    i = this.amatch(cs, 63, cs1, 8);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: system environment exit")]
public void amatchThrowsExitException899()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[63] = '\u0001';
    cs[64] = '\u0001';
    cs1[63] = '*';
    cs1[64] = '!';
    cs1[66] = '*';
    i = this.amatch(cs, 63, cs1, 63);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: system environment exit")]
public void amatchThrowsExitException241()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '*';
    cs1[1] = '*';
    i = this.amatch(cs, 0, cs1, 0);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: system environment exit")]
public void amatchThrowsExitException541()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[62] = '\u0001';
    cs[63] = '\u0004';
    cs1[0] = '*';
    cs1[1] = '!';
    cs1[3] = '*';
    cs1[4] = '[';
    cs1[6] = '\u0001';
    i = this.amatch(cs, 62, cs1, 0);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch289()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[63] = '\u0001';
    cs[64] = '\u0001';
    cs1[6] = '*';
    cs1[7] = '!';
    cs1[9] = '*';
    cs1[10] = '[';
    cs1[12] = '[';
    i = this.amatch(cs, 63, cs1, 6);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: system environment exit")]
public void amatchThrowsExitException247()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs[99] = '\u0001';
    cs1[0] = '*';
    i = this.amatch(cs, 0, cs1, 0);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
[Ignore]
[PexDescription("the test state was: path bounds exceeded")]
public void amatch239()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\n';
    cs1[0] = '*';
    cs1[1] = '$';
    i = this.amatch(cs, 0, cs1, 0);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch226()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs[1] = '\u0001';
    cs1[0] = '*';
    cs1[1] = 'c';
    cs1[2] = '\u0001';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(2, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch557()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[62] = '\u0001';
    cs[63] = '\u0001';
    cs1[4] = '*';
    cs1[5] = '!';
    cs1[7] = '*';
    cs1[8] = '[';
    cs1[9] = '*';
    cs1[51] = '\u0001';
    cs1[52] = '[';
    i = this.amatch(cs, 62, cs1, 4);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch879()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[63] = '\u0002';
    cs[64] = '\u0003';
    cs1[0] = '*';
    cs1[1] = 'c';
    cs1[2] = '\u0002';
    cs1[3] = 'c';
    i = this.amatch(cs, 63, cs1, 0);
    Assert.AreEqual<int>(-1, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch455()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[0] = '\u0001';
    cs1[0] = '*';
    cs1[1] = '$';
    cs1[2] = '*';
    cs1[3] = '$';
    i = this.amatch(cs, 0, cs1, 0);
    Assert.AreEqual<int>(0, i);
}
[TestMethod]
[PexGeneratedBy(typeof(BaseVersionTest))]
public void amatch501()
{
    int i;
    char[] cs = new char[100];
    char[] cs1 = new char[100];
    cs[62] = '@';
    cs[63] = 'B';
    cs1[1] = '*';
    cs1[2] = '!';
    cs1[4] = '*';
    cs1[5] = '[';
    cs1[6] = '\u0006';
    cs1[8] = 'B';
    cs1[9] = '\u0002';
    cs1[10] = '\u0003';
    cs1[11] = '\u00c0';
    cs1[12] = '\u0001';
    cs1[13] = '%';
    i = this.amatch(cs, 62, cs1, 1);
    Assert.AreEqual<int>(-1, i);
}
    }
}

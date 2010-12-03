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
using System.IO;

namespace Edu.Unl.Sir.Siemens.PrintTokens.Orig
{
    public partial class OriginalTest
    {
[TestMethod]
[PexGeneratedBy(typeof(OriginalTest))]
[PexRaisedException(typeof(NullReferenceException))]
public void skipThrowsNullReferenceException57()
{
    this.skip((CharacterStream)null);
}
[TestMethod]
[PexGeneratedBy(typeof(OriginalTest))]
public void skip320()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      CharacterStream characterStream;
      byte[] bs = new byte[2];
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      byte[] bs1 = new byte[2];
      bs1[1] = (byte)127;
      characterStream = new CharacterStream();
      characterStream.fp = (Stream)memoryStream;
      characterStream.stream_ind = 0;
      characterStream.stream = bs1;
      this.skip(characterStream);
      disposables.Dispose();
    }
}
[TestMethod]
[PexGeneratedBy(typeof(OriginalTest))]
[Ignore]
[PexDescription("the test state was: path bounds exceeded")]
public void skip418()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      CharacterStream characterStream;
      byte[] bs = new byte[80];
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      byte[] bs1 = new byte[80];
      bs1[1] = (byte)127;
      bs1[2] = (byte)127;
      bs1[3] = (byte)127;
      bs1[4] = (byte)127;
      bs1[5] = (byte)127;
      bs1[6] = (byte)127;
      bs1[7] = (byte)127;
      bs1[8] = (byte)127;
      bs1[9] = (byte)127;
      bs1[10] = (byte)127;
      bs1[11] = (byte)127;
      bs1[12] = (byte)127;
      bs1[13] = (byte)127;
      bs1[14] = (byte)127;
      bs1[15] = (byte)127;
      bs1[16] = (byte)127;
      bs1[17] = (byte)127;
      bs1[18] = (byte)127;
      bs1[19] = (byte)127;
      bs1[20] = (byte)127;
      bs1[21] = (byte)127;
      bs1[22] = (byte)127;
      bs1[23] = (byte)127;
      bs1[24] = (byte)127;
      bs1[25] = (byte)127;
      bs1[26] = (byte)127;
      bs1[27] = (byte)127;
      bs1[28] = (byte)127;
      bs1[29] = (byte)127;
      bs1[30] = (byte)127;
      bs1[31] = (byte)127;
      bs1[32] = (byte)127;
      bs1[33] = (byte)127;
      bs1[34] = (byte)127;
      bs1[35] = (byte)127;
      bs1[36] = (byte)127;
      bs1[37] = (byte)127;
      bs1[38] = (byte)127;
      bs1[39] = (byte)127;
      bs1[40] = (byte)127;
      bs1[41] = (byte)127;
      bs1[42] = (byte)127;
      bs1[43] = (byte)127;
      bs1[44] = (byte)127;
      bs1[45] = (byte)127;
      bs1[46] = (byte)127;
      bs1[47] = (byte)127;
      bs1[48] = (byte)127;
      bs1[49] = (byte)127;
      bs1[50] = (byte)127;
      bs1[51] = (byte)127;
      bs1[52] = (byte)127;
      bs1[53] = (byte)127;
      bs1[54] = (byte)127;
      bs1[55] = (byte)127;
      bs1[56] = (byte)127;
      bs1[57] = (byte)127;
      bs1[58] = (byte)127;
      bs1[59] = (byte)127;
      bs1[60] = (byte)127;
      bs1[61] = (byte)127;
      bs1[62] = (byte)127;
      bs1[63] = (byte)127;
      bs1[64] = (byte)127;
      bs1[65] = (byte)127;
      bs1[66] = (byte)127;
      bs1[67] = (byte)127;
      bs1[68] = (byte)127;
      bs1[69] = (byte)127;
      bs1[70] = (byte)127;
      bs1[71] = (byte)127;
      bs1[72] = (byte)127;
      bs1[73] = (byte)127;
      bs1[74] = (byte)127;
      bs1[75] = (byte)127;
      bs1[76] = (byte)127;
      bs1[77] = (byte)127;
      bs1[78] = (byte)127;
      bs1[79] = (byte)127;
      characterStream = new CharacterStream();
      characterStream.fp = (Stream)memoryStream;
      characterStream.stream_ind = 0;
      characterStream.stream = bs1;
      this.skip(characterStream);
      disposables.Dispose();
    }
}
[TestMethod]
[PexGeneratedBy(typeof(OriginalTest))]
public void skip635()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      CharacterStream characterStream;
      byte[] bs = new byte[2];
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      byte[] bs1 = new byte[2];
      bs1[0] = (byte)1;
      bs1[1] = (byte)127;
      characterStream = new CharacterStream();
      characterStream.fp = (Stream)memoryStream;
      characterStream.stream_ind = 0;
      characterStream.stream = bs1;
      this.skip(characterStream);
      disposables.Dispose();
    }
}
[TestMethod]
[PexGeneratedBy(typeof(OriginalTest))]
[Ignore]
[PexDescription("the test state was: path bounds exceeded")]
public void skip142()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      CharacterStream characterStream;
      byte[] bs = new byte[81];
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      byte[] bs1 = new byte[81];
      bs1[1] = (byte)127;
      bs1[2] = (byte)127;
      bs1[3] = (byte)127;
      bs1[4] = (byte)127;
      bs1[5] = (byte)127;
      bs1[6] = (byte)127;
      bs1[7] = (byte)127;
      bs1[8] = (byte)127;
      bs1[9] = (byte)127;
      bs1[10] = (byte)127;
      bs1[11] = (byte)127;
      bs1[12] = (byte)127;
      bs1[13] = (byte)127;
      bs1[14] = (byte)127;
      bs1[15] = (byte)127;
      bs1[16] = (byte)127;
      bs1[17] = (byte)127;
      bs1[18] = (byte)127;
      bs1[19] = (byte)127;
      bs1[20] = (byte)127;
      bs1[21] = (byte)127;
      bs1[22] = (byte)127;
      bs1[23] = (byte)127;
      bs1[24] = (byte)127;
      bs1[25] = (byte)127;
      bs1[26] = (byte)127;
      bs1[27] = (byte)127;
      bs1[28] = (byte)127;
      bs1[29] = (byte)127;
      bs1[30] = (byte)127;
      bs1[31] = (byte)127;
      bs1[32] = (byte)127;
      bs1[33] = (byte)127;
      bs1[34] = (byte)127;
      bs1[35] = (byte)127;
      bs1[36] = (byte)127;
      bs1[37] = (byte)127;
      bs1[38] = (byte)127;
      bs1[39] = (byte)127;
      bs1[40] = (byte)127;
      bs1[41] = (byte)127;
      bs1[42] = (byte)127;
      bs1[43] = (byte)127;
      bs1[44] = (byte)127;
      bs1[45] = (byte)127;
      bs1[46] = (byte)127;
      bs1[47] = (byte)127;
      bs1[48] = (byte)127;
      bs1[49] = (byte)127;
      bs1[50] = (byte)127;
      bs1[51] = (byte)127;
      bs1[52] = (byte)127;
      bs1[53] = (byte)127;
      bs1[54] = (byte)127;
      bs1[55] = (byte)127;
      bs1[56] = (byte)127;
      bs1[57] = (byte)127;
      bs1[58] = (byte)127;
      bs1[59] = (byte)127;
      bs1[60] = (byte)127;
      bs1[61] = (byte)127;
      bs1[62] = (byte)127;
      bs1[63] = (byte)127;
      bs1[64] = (byte)127;
      bs1[65] = (byte)127;
      bs1[66] = (byte)127;
      bs1[67] = (byte)127;
      bs1[68] = (byte)127;
      bs1[69] = (byte)127;
      bs1[70] = (byte)127;
      bs1[71] = (byte)127;
      bs1[72] = (byte)127;
      bs1[73] = (byte)127;
      bs1[74] = (byte)127;
      bs1[75] = (byte)127;
      bs1[76] = (byte)127;
      bs1[77] = (byte)127;
      bs1[78] = (byte)127;
      bs1[79] = (byte)127;
      bs1[80] = (byte)127;
      characterStream = new CharacterStream();
      characterStream.fp = (Stream)memoryStream;
      characterStream.stream_ind = 0;
      characterStream.stream = bs1;
      this.skip(characterStream);
      disposables.Dispose();
    }
}
[TestMethod]
[PexGeneratedBy(typeof(OriginalTest))]
public void skip460()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      CharacterStream characterStream;
      byte[] bs = new byte[2];
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      byte[] bs1 = new byte[2];
      bs1[0] = (byte)10;
      bs1[1] = (byte)127;
      characterStream = new CharacterStream();
      characterStream.fp = (Stream)memoryStream;
      characterStream.stream_ind = 0;
      characterStream.stream = bs1;
      this.skip(characterStream);
      disposables.Dispose();
    }
}
[TestMethod]
[PexGeneratedBy(typeof(OriginalTest))]
[Ignore]
[PexDescription("the test state was: path bounds exceeded")]
public void skip648()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      CharacterStream characterStream;
      byte[] bs = new byte[2];
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      byte[] bs1 = new byte[80];
      bs1[79] = (byte)127;
      characterStream = new CharacterStream();
      characterStream.fp = (Stream)memoryStream;
      characterStream.stream_ind = 0;
      characterStream.stream = bs1;
      this.skip(characterStream);
      disposables.Dispose();
    }
}
[TestMethod]
[PexGeneratedBy(typeof(OriginalTest))]
[Ignore]
[PexDescription("the test state was: path bounds exceeded")]
public void skip308()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      CharacterStream characterStream;
      byte[] bs = new byte[192];
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      byte[] bs1 = new byte[81];
      bs1[2] = (byte)127;
      bs1[3] = (byte)127;
      bs1[4] = (byte)127;
      bs1[5] = (byte)127;
      bs1[6] = (byte)127;
      bs1[7] = (byte)127;
      bs1[8] = (byte)127;
      bs1[9] = (byte)127;
      bs1[10] = (byte)127;
      bs1[11] = (byte)127;
      bs1[12] = (byte)127;
      bs1[13] = (byte)127;
      bs1[14] = (byte)127;
      bs1[15] = (byte)127;
      bs1[16] = (byte)127;
      bs1[17] = (byte)127;
      bs1[18] = (byte)127;
      bs1[19] = (byte)127;
      bs1[20] = (byte)127;
      bs1[21] = (byte)127;
      bs1[22] = (byte)127;
      bs1[23] = (byte)127;
      bs1[24] = (byte)127;
      bs1[25] = (byte)127;
      bs1[26] = (byte)127;
      bs1[27] = (byte)127;
      bs1[28] = (byte)127;
      bs1[29] = (byte)127;
      bs1[30] = (byte)127;
      bs1[31] = (byte)127;
      bs1[32] = (byte)127;
      bs1[33] = (byte)127;
      bs1[34] = (byte)127;
      bs1[35] = (byte)127;
      bs1[36] = (byte)127;
      bs1[37] = (byte)127;
      bs1[38] = (byte)127;
      bs1[39] = (byte)127;
      bs1[40] = (byte)127;
      bs1[41] = (byte)127;
      bs1[42] = (byte)127;
      bs1[43] = (byte)127;
      bs1[44] = (byte)127;
      bs1[45] = (byte)127;
      bs1[46] = (byte)127;
      bs1[47] = (byte)127;
      bs1[48] = (byte)127;
      bs1[49] = (byte)127;
      bs1[50] = (byte)127;
      bs1[51] = (byte)127;
      bs1[52] = (byte)127;
      bs1[53] = (byte)127;
      bs1[54] = (byte)127;
      bs1[55] = (byte)127;
      bs1[56] = (byte)127;
      bs1[57] = (byte)127;
      bs1[58] = (byte)127;
      bs1[59] = (byte)127;
      bs1[60] = (byte)127;
      bs1[61] = (byte)127;
      bs1[62] = (byte)127;
      bs1[63] = (byte)127;
      bs1[64] = (byte)127;
      bs1[65] = (byte)127;
      bs1[66] = (byte)127;
      bs1[67] = (byte)127;
      bs1[68] = (byte)127;
      bs1[69] = (byte)127;
      bs1[70] = (byte)127;
      bs1[71] = (byte)127;
      bs1[72] = (byte)127;
      bs1[73] = (byte)127;
      bs1[74] = (byte)127;
      bs1[75] = (byte)127;
      bs1[76] = (byte)127;
      bs1[77] = (byte)127;
      bs1[78] = (byte)127;
      bs1[79] = (byte)127;
      bs1[80] = (byte)127;
      characterStream = new CharacterStream();
      characterStream.fp = (Stream)memoryStream;
      characterStream.stream_ind = 1;
      characterStream.stream = bs1;
      this.skip(characterStream);
      disposables.Dispose();
    }
}
[TestMethod]
[PexGeneratedBy(typeof(OriginalTest))]
[PexRaisedException(typeof(IndexOutOfRangeException))]
public void skipThrowsIndexOutOfRangeException308()
{
    using (PexDisposableContext disposables = PexDisposableContext.Create())
    {
      MemoryStream memoryStream;
      CharacterStream characterStream;
      byte[] bs = new byte[326];
      bs[0] = (byte)2;
      bs[1] = (byte)2;
      bs[2] = (byte)2;
      bs[3] = (byte)2;
      bs[4] = (byte)2;
      bs[5] = (byte)2;
      bs[6] = (byte)2;
      bs[7] = (byte)2;
      bs[8] = (byte)2;
      bs[9] = (byte)2;
      bs[10] = (byte)2;
      bs[11] = (byte)2;
      bs[12] = (byte)2;
      bs[13] = (byte)2;
      bs[14] = (byte)2;
      bs[15] = (byte)2;
      bs[16] = (byte)2;
      bs[17] = (byte)2;
      bs[18] = (byte)2;
      bs[19] = (byte)2;
      bs[20] = (byte)2;
      bs[21] = (byte)2;
      bs[22] = (byte)2;
      bs[23] = (byte)2;
      bs[24] = (byte)2;
      bs[25] = (byte)2;
      bs[26] = (byte)2;
      bs[27] = (byte)2;
      bs[28] = (byte)2;
      bs[29] = (byte)2;
      bs[30] = (byte)2;
      bs[31] = (byte)2;
      bs[32] = (byte)2;
      bs[33] = (byte)2;
      bs[34] = (byte)2;
      bs[35] = (byte)2;
      bs[36] = (byte)2;
      bs[37] = (byte)2;
      bs[38] = (byte)2;
      bs[39] = (byte)2;
      bs[40] = (byte)2;
      bs[41] = (byte)2;
      bs[42] = (byte)2;
      bs[43] = (byte)2;
      bs[44] = (byte)2;
      bs[45] = (byte)2;
      bs[46] = (byte)2;
      bs[47] = (byte)2;
      bs[48] = (byte)2;
      bs[49] = (byte)2;
      bs[50] = (byte)2;
      bs[51] = (byte)2;
      bs[52] = (byte)2;
      bs[53] = (byte)2;
      bs[54] = (byte)2;
      bs[55] = (byte)2;
      bs[56] = (byte)2;
      bs[57] = (byte)2;
      bs[58] = (byte)2;
      bs[59] = (byte)2;
      bs[60] = (byte)2;
      bs[61] = (byte)2;
      bs[62] = (byte)2;
      bs[63] = (byte)2;
      bs[64] = (byte)2;
      bs[65] = (byte)2;
      bs[66] = (byte)2;
      bs[67] = (byte)2;
      bs[68] = (byte)2;
      bs[69] = (byte)2;
      bs[70] = (byte)2;
      bs[71] = (byte)2;
      bs[72] = (byte)2;
      bs[73] = (byte)2;
      bs[74] = (byte)2;
      bs[75] = (byte)2;
      bs[76] = (byte)2;
      bs[77] = (byte)2;
      bs[78] = (byte)2;
      bs[79] = (byte)2;
      bs[80] = (byte)2;
      bs[81] = (byte)2;
      bs[82] = (byte)2;
      bs[83] = (byte)2;
      bs[84] = (byte)2;
      bs[85] = (byte)2;
      bs[86] = (byte)2;
      bs[87] = (byte)2;
      bs[88] = (byte)2;
      bs[89] = (byte)2;
      bs[90] = (byte)2;
      bs[91] = (byte)2;
      bs[92] = (byte)2;
      bs[93] = (byte)2;
      bs[94] = (byte)2;
      bs[95] = (byte)2;
      bs[96] = (byte)2;
      bs[97] = (byte)2;
      bs[98] = (byte)2;
      bs[99] = (byte)2;
      bs[100] = (byte)2;
      bs[101] = (byte)2;
      bs[102] = (byte)2;
      bs[103] = (byte)2;
      bs[104] = (byte)2;
      bs[105] = (byte)2;
      bs[106] = (byte)2;
      bs[107] = (byte)2;
      bs[108] = (byte)2;
      bs[109] = (byte)2;
      bs[110] = (byte)2;
      bs[111] = (byte)2;
      bs[112] = (byte)2;
      bs[113] = (byte)2;
      bs[114] = (byte)2;
      bs[115] = (byte)2;
      bs[116] = (byte)2;
      bs[117] = (byte)2;
      bs[118] = (byte)2;
      bs[119] = (byte)2;
      bs[120] = (byte)2;
      bs[121] = (byte)2;
      bs[122] = (byte)2;
      bs[123] = (byte)2;
      bs[124] = (byte)2;
      bs[125] = (byte)2;
      bs[126] = (byte)2;
      bs[sbyte.MaxValue] = (byte)2;
      bs[128] = (byte)2;
      bs[129] = (byte)2;
      bs[130] = (byte)2;
      bs[131] = (byte)2;
      bs[132] = (byte)2;
      bs[133] = (byte)2;
      bs[134] = (byte)2;
      bs[135] = (byte)2;
      bs[136] = (byte)2;
      bs[137] = (byte)2;
      bs[138] = (byte)2;
      bs[139] = (byte)2;
      bs[140] = (byte)2;
      bs[141] = (byte)2;
      bs[142] = (byte)2;
      bs[143] = (byte)2;
      bs[144] = (byte)2;
      bs[145] = (byte)2;
      bs[146] = (byte)2;
      bs[147] = (byte)2;
      bs[148] = (byte)2;
      bs[149] = (byte)2;
      bs[150] = (byte)2;
      bs[151] = (byte)2;
      bs[152] = (byte)2;
      bs[153] = (byte)2;
      bs[154] = (byte)2;
      bs[155] = (byte)2;
      bs[156] = (byte)2;
      bs[157] = (byte)2;
      bs[158] = (byte)2;
      bs[159] = (byte)2;
      bs[160] = (byte)2;
      bs[161] = (byte)2;
      bs[162] = (byte)2;
      bs[163] = (byte)2;
      bs[164] = (byte)2;
      bs[165] = (byte)2;
      bs[166] = (byte)2;
      bs[167] = (byte)2;
      bs[168] = (byte)2;
      bs[169] = (byte)2;
      bs[170] = (byte)2;
      bs[171] = (byte)2;
      bs[172] = (byte)2;
      bs[173] = (byte)2;
      bs[174] = (byte)2;
      bs[175] = (byte)2;
      bs[176] = (byte)2;
      bs[177] = (byte)2;
      bs[178] = (byte)2;
      bs[179] = (byte)2;
      bs[180] = (byte)2;
      bs[181] = (byte)2;
      bs[182] = (byte)2;
      bs[183] = (byte)2;
      bs[184] = (byte)2;
      bs[185] = (byte)2;
      bs[186] = (byte)2;
      bs[187] = (byte)2;
      bs[188] = (byte)2;
      bs[189] = (byte)2;
      bs[190] = (byte)2;
      bs[191] = (byte)2;
      bs[192] = (byte)2;
      bs[193] = (byte)2;
      bs[194] = (byte)2;
      bs[195] = (byte)2;
      bs[196] = (byte)2;
      bs[197] = (byte)2;
      bs[198] = (byte)2;
      bs[199] = (byte)2;
      bs[200] = (byte)2;
      bs[201] = (byte)2;
      bs[202] = (byte)2;
      bs[203] = (byte)2;
      bs[204] = (byte)2;
      bs[205] = (byte)2;
      bs[206] = (byte)2;
      bs[207] = (byte)2;
      bs[208] = (byte)2;
      bs[209] = (byte)2;
      bs[210] = (byte)2;
      bs[211] = (byte)2;
      bs[212] = (byte)2;
      bs[213] = (byte)2;
      bs[214] = (byte)2;
      bs[215] = (byte)2;
      bs[216] = (byte)2;
      bs[217] = (byte)2;
      bs[218] = (byte)2;
      bs[219] = (byte)2;
      bs[220] = (byte)2;
      bs[221] = (byte)2;
      bs[222] = (byte)2;
      bs[223] = (byte)2;
      bs[224] = (byte)2;
      bs[225] = (byte)2;
      bs[226] = (byte)2;
      bs[227] = (byte)2;
      bs[228] = (byte)2;
      bs[229] = (byte)2;
      bs[230] = (byte)2;
      bs[231] = (byte)2;
      bs[232] = (byte)2;
      bs[233] = (byte)2;
      bs[234] = (byte)2;
      bs[235] = (byte)2;
      bs[236] = (byte)2;
      bs[237] = (byte)2;
      bs[238] = (byte)2;
      bs[239] = (byte)2;
      bs[240] = (byte)2;
      bs[241] = (byte)2;
      bs[242] = (byte)2;
      bs[243] = (byte)2;
      bs[244] = (byte)2;
      bs[245] = (byte)2;
      bs[246] = (byte)2;
      bs[247] = (byte)2;
      bs[248] = (byte)2;
      bs[249] = (byte)2;
      bs[250] = (byte)2;
      bs[251] = (byte)2;
      bs[252] = (byte)2;
      bs[253] = (byte)2;
      bs[254] = (byte)2;
      bs[255] = (byte)2;
      bs[256] = (byte)2;
      bs[257] = (byte)2;
      bs[258] = (byte)2;
      bs[259] = (byte)2;
      bs[260] = (byte)2;
      bs[261] = (byte)2;
      bs[262] = (byte)2;
      bs[263] = (byte)2;
      bs[264] = (byte)2;
      bs[265] = (byte)2;
      bs[266] = (byte)2;
      bs[267] = (byte)2;
      bs[268] = (byte)2;
      bs[269] = (byte)2;
      bs[270] = (byte)2;
      bs[271] = (byte)2;
      bs[272] = (byte)2;
      bs[273] = (byte)2;
      bs[274] = (byte)2;
      bs[275] = (byte)2;
      bs[276] = (byte)2;
      bs[277] = (byte)2;
      bs[278] = (byte)2;
      bs[279] = (byte)2;
      bs[280] = (byte)2;
      bs[281] = (byte)2;
      bs[282] = (byte)2;
      bs[283] = (byte)2;
      bs[284] = (byte)2;
      bs[285] = (byte)2;
      bs[286] = (byte)2;
      bs[287] = (byte)2;
      bs[288] = (byte)2;
      bs[289] = (byte)2;
      bs[290] = (byte)2;
      bs[291] = (byte)2;
      bs[292] = (byte)2;
      bs[293] = (byte)2;
      bs[294] = (byte)2;
      bs[295] = (byte)2;
      bs[296] = (byte)2;
      bs[297] = (byte)2;
      bs[298] = (byte)2;
      bs[299] = (byte)2;
      bs[300] = (byte)2;
      bs[301] = (byte)2;
      bs[302] = (byte)2;
      bs[303] = (byte)2;
      bs[304] = (byte)2;
      bs[305] = (byte)2;
      bs[306] = (byte)2;
      bs[307] = (byte)2;
      bs[308] = (byte)2;
      bs[309] = (byte)2;
      bs[310] = (byte)2;
      bs[311] = (byte)2;
      bs[312] = (byte)2;
      bs[313] = (byte)2;
      bs[314] = (byte)2;
      bs[315] = (byte)2;
      bs[316] = (byte)2;
      bs[317] = (byte)2;
      bs[318] = (byte)2;
      bs[319] = (byte)2;
      bs[320] = (byte)2;
      bs[321] = (byte)10;
      memoryStream = new MemoryStream(bs, false);
      disposables.Add((IDisposable)memoryStream);
      byte[] bs1 = new byte[80];
      bs1[2] = (byte)10;
      bs1[3] = (byte)10;
      bs1[4] = (byte)10;
      bs1[5] = (byte)10;
      bs1[6] = (byte)10;
      bs1[7] = (byte)10;
      bs1[8] = (byte)10;
      bs1[9] = (byte)10;
      bs1[10] = (byte)10;
      bs1[11] = (byte)10;
      bs1[12] = (byte)10;
      bs1[13] = (byte)10;
      bs1[14] = (byte)10;
      bs1[15] = (byte)10;
      bs1[16] = (byte)10;
      bs1[17] = (byte)10;
      bs1[18] = (byte)10;
      bs1[19] = (byte)10;
      bs1[20] = (byte)10;
      bs1[21] = (byte)10;
      bs1[22] = (byte)10;
      bs1[23] = (byte)10;
      bs1[24] = (byte)10;
      bs1[25] = (byte)10;
      bs1[26] = (byte)10;
      bs1[27] = (byte)10;
      bs1[28] = (byte)10;
      bs1[29] = (byte)10;
      bs1[30] = (byte)10;
      bs1[31] = (byte)10;
      bs1[32] = (byte)10;
      bs1[33] = (byte)10;
      bs1[34] = (byte)10;
      bs1[35] = (byte)10;
      bs1[36] = (byte)10;
      bs1[37] = (byte)10;
      bs1[38] = (byte)10;
      bs1[39] = (byte)10;
      bs1[40] = (byte)10;
      bs1[41] = (byte)10;
      bs1[42] = (byte)10;
      bs1[43] = (byte)10;
      bs1[44] = (byte)10;
      bs1[45] = (byte)10;
      bs1[46] = (byte)10;
      bs1[47] = (byte)10;
      bs1[48] = (byte)10;
      bs1[49] = (byte)10;
      bs1[50] = (byte)10;
      bs1[51] = (byte)10;
      bs1[52] = (byte)10;
      bs1[53] = (byte)10;
      bs1[54] = (byte)10;
      bs1[55] = (byte)10;
      bs1[56] = (byte)10;
      bs1[57] = (byte)10;
      bs1[58] = (byte)10;
      bs1[59] = (byte)10;
      bs1[60] = (byte)10;
      bs1[61] = (byte)10;
      bs1[62] = (byte)10;
      bs1[63] = (byte)10;
      bs1[64] = (byte)10;
      bs1[65] = (byte)10;
      bs1[66] = (byte)10;
      bs1[67] = (byte)10;
      bs1[68] = (byte)10;
      bs1[69] = (byte)10;
      bs1[70] = (byte)10;
      bs1[71] = (byte)10;
      bs1[72] = (byte)10;
      bs1[73] = (byte)10;
      bs1[74] = (byte)10;
      bs1[75] = (byte)10;
      bs1[76] = (byte)10;
      bs1[77] = (byte)10;
      bs1[78] = (byte)10;
      bs1[79] = (byte)127;
      characterStream = new CharacterStream();
      characterStream.fp = (Stream)memoryStream;
      characterStream.stream_ind = 0;
      characterStream.stream = bs1;
      this.skip(characterStream);
      disposables.Dispose();
    }
}
    }
}
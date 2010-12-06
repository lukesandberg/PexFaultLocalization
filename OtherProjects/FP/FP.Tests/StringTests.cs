/*
* StringTests.cs is part of functional-dotnet project
* 
* Copyright (c) 2008 Alexey Romanov
* All rights reserved.
*
* This source file is available under The New BSD License.
* See license.txt file for more information.
* 
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
* CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
* INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
* MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
*/

using System.Collections.Generic;
using System.Linq;
using FP.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace FPTests {
	[TestClass]
    public class StringTests {
        [TestMethod]
        public void Test_EmptyStringContainsNoWords() {
			Assert.IsTrue(!"".Words().Any());
        }

		[TestMethod]
        public void Test_WordsShouldIncludePunctuationAndIgnoreExtraWhitespace() {
			Assert.IsTrue("This, \r\n  is  a \t test.".Words().SequenceEqual(
                            new[] {"This,", "is", "a", "test."}));
        }

		[TestMethod]
        public void Test_UnWordsAndUnlines_ShouldReturnEmptyStringsOnEmptySequences() {
			Assert.AreEqual((new List<string>()).UnWordsAsString(), "");
			Assert.AreEqual((new List<string>()).UnLinesAsString(), "");
        }

		[TestMethod]
        public void Test_Lines_ShouldWorkWithAllLineEndings() {
            Assert.IsTrue("This,   is \r\n a \t\n test.".Lines().
                            SequenceEqual(new[] {"This,   is ", " a \t", " test."}));
        }

		[TestMethod]
        public void Test_UnWords() {
            Assert.AreEqual(new[] {"This,", "is", "a", "test."}.UnWordsAsString(),
                         "This, is a test.");
        }

		[TestMethod]
        public void Test_UnLines() {
			Assert.AreEqual(new[] { "This,", "is", "a", "test." }.UnLinesAsString(),
                         @"This,
is
a
test.");
        }
    }
}
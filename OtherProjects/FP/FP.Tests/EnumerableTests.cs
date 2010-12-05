/*
* EnumerableTests.cs is part of functional-dotnet project
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

using System.Linq;
using FP.Core;
using FP.Text;
using FP.Validation;
using XunitExtensions;
using Microsoft.Pex.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FPTests {
	[TestClass]
    public partial class EnumerableTests {
        [TestMethod]
        public void TailShouldThrowOnEmptySeq() {
            var emptyList = Enumerable.Empty<int>();

            Assert2.Throws(typeof (EmptyEnumerableException), () => emptyList.Tail().FirstOrDefault());
        }

		[TestMethod]
        public void TailTest() {
            Assert2.SequenceEqual(Ints.Range(10, 100), Ints.Range(9, 100).Tail());
        }

		[TestMethod]
        public void IntersperseTest() {
            Assert.AreEqual("a,b,c,d,e", "abcde".Intersperse(',').AsString());
        }

		[TestMethod]
        public void IntercalateTest() {
			Assert.AreEqual("a,b,c,d,e",
                         new[] {"a", "b", "c", "d", "e"}.Intercalate(",").AsString());
        }

		[TestMethod]
        public void FoldRightTest1() {
			Assert.AreEqual(-2, new[] { 1, 2, 3, 4 }.FoldRight((x, y) => x - y)); //1 - (2 - (3 - 4))
        }

		[TestMethod]
        public void FoldRightTest2() {
			Assert.AreEqual(-2, new[] { 1, 2, 3, 4 }.FoldRight((x, y) => x - y, 0));
                //1 - (2 - (3 - (4 - 0))
        }

		[TestMethod]
        public void ScanLeftTest() {
            Assert2.SequenceEqual(new[] {1, 3, 6, 10}, new[] {1, 2, 3, 4}.ScanLeft((x, y) => x + y));
        }

        [PexMethod]
        public void SortAscendingTest([PexAssumeNotNull] int[] arr) {
            Assert2.SequenceEqual(arr.OrderBy(x => x), arr.Sort());
        }

        [PexMethod]
        public void SortDescendingTest([PexAssumeNotNull] int[] arr) {
            Assert2.SequenceEqual(arr.OrderByDescending(x => x), arr.SortDescending());
        }
    }
}
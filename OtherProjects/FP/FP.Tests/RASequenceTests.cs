/*
* RASequenceTests.cs is part of functional-dotnet project
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
using FP.Collections.Persistent;
using FP.Core;
using FP.HaskellNames;
using FP.Validation;
using Microsoft.Pex.Framework;
using XunitExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FPTests {
	[TestClass]
    public partial class RASequenceTests {
		[PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_IsEmptyWorksCorrectly<T>(T[] arr) {
            Assert.IsTrue(RandomAccessSequence.Empty<int>().IsEmpty);
            PexAssume.IsNotNullOrEmpty(arr);
			PexAssert.IsFalse(RandomAccessSequence.FromEnumerable(arr).IsEmpty);
        }

		[PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_Enumerate<T>([PexAssumeNotNull] T[] arr) {
            Assert2.SequenceEqual(arr, RandomAccessSequence.FromEnumerable(arr));
        }

		[PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_Append<T>([PexAssumeNotNull] T[] arr1, T t, [PexAssumeNotNull] T[] arr2) {
            var seq = RandomAccessSequence.FromEnumerable(arr1);
            Assert2.SequenceEqual(arr1.Append(t), seq | t);
            Assert2.SequenceEqual(arr1.Concat(arr2), seq.AppendRange(arr2));
        }

        [PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_Prepend<T>([PexAssumeNotNull] T[] arr1, T t, [PexAssumeNotNull] T[] arr2) {
            var seq = RandomAccessSequence.FromEnumerable(arr1);
            Assert2.SequenceEqual(t.Cons(arr1.AsEnumerable()), t | seq);
            Assert2.SequenceEqual(arr2.Concat(arr1), seq.PrependRange(arr2));
        }

        [PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_SplitAt<T>([PexAssumeNotNull] T[] arr, int i) {
            var seq = RandomAccessSequence.FromEnumerable(arr);
            PexAssume.IsTrue(i >= 0);
            PexAssume.IsTrue(i < arr.Length);
            var split = seq.SplitAt(i);
            Assert2.SequenceEqual(arr.Take(i), split.Item1);
            Assert2.SequenceEqual(arr.Take(i), seq.Take(i));
            Assert2.SequenceEqual(arr.Skip(i), split.Item2); 
            Assert2.SequenceEqual(arr.Skip(i), seq.Skip(i));
        }

		[PexMethod(MaxRunsWithoutNewTests = 1000000000)]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_Indexing<T>([PexAssumeNotNull] T[] arr, int i) {
            var seq = RandomAccessSequence.FromEnumerable(arr);
            PexAssume.IsTrue(i >= 0);
            PexAssume.IsTrue(i < arr.Length);
			PexAssert.AreEqual(arr[i], seq[i]);
        }

        [PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_SetAt<T>([PexAssumeNotNull] T[] arr, int i, T newValue) {
            var seq = RandomAccessSequence.FromEnumerable(arr);
            PexAssume.IsTrue(i >= 0);
            PexAssume.IsTrue(i < seq.Count);
            var newSeq = seq.SetAt(i, newValue);
            var splitOld = seq.SplitAt(i);
            var splitNew = newSeq.SplitAt(i);
            Assert2.SequenceEqual(splitOld.Item1, splitNew.Item1);
            PexAssert.AreEqual(newValue, splitNew.Item2.Head);
            Assert2.SequenceEqual(splitOld.Item2.Tail, splitNew.Item2.Tail);
        }

        [TestMethod]
        public void Test_HeadAndTailOfEmptySequence() {
            var empty = RandomAccessSequence.Empty<int>();
            Assert2.Throws<EmptyEnumerableException>(() => { var a = empty.Head; });
            Assert2.Throws<EmptyEnumerableException>(() => { var a = empty.Tail; });
            Assert2.Throws<EmptyEnumerableException>(() => { var a = empty.Init; });
            Assert2.Throws<EmptyEnumerableException>(() => { var a = empty.Last; });
        }

		[PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_HeadAndTailOfNonEmptySequence<T>([PexAssumeNotNull] T[] arr) {
            PexAssume.IsNotNullOrEmpty(arr);
            var seq0 = RandomAccessSequence.FromEnumerable(arr);
            var seq = seq0;
            foreach (var t in arr) {
                PexAssert.AreEqual(t, seq.Head);
                seq = seq.Tail;
            }
			PexAssert.IsTrue(!seq.Any());

            seq = seq0;
            foreach (var t in arr.Reverse()) {
				PexAssert.AreEqual(t, seq.Last);
                seq = seq.Init;
            }
			PexAssert.IsTrue(!seq.Any());
        }

		[PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_ConcatWithEmpty<T>([PexAssumeNotNull] T[] arr) {
            var seq = RandomAccessSequence.FromEnumerable(arr);
            Assert2.SequenceEqual(seq + RandomAccessSequence<T>.Empty, seq);
            Assert2.SequenceEqual(RandomAccessSequence<T>.Empty + seq, seq);
        }

		[PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_Concat<T>([PexAssumeNotNull] T[] arr1, [PexAssumeNotNull] T[] arr2) {
            var seq1 = RandomAccessSequence.FromEnumerable(arr1);
            var seq2 = RandomAccessSequence.FromEnumerable(arr2);
            Assert2.SequenceEqual(seq1.AsEnumerable().Concat(seq2), seq1 + seq2);
        }

		[PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_InsertAt<T>([PexAssumeNotNull] T[] arr, int i, T newValue) {
            var seq = RandomAccessSequence.FromEnumerable(arr);
            PexAssume.IsTrue(i >= 0);
            PexAssume.IsTrue(i <= seq.Count);
            var newSeq = seq.InsertAt(i, newValue);
            var splitOld = seq.SplitAt(i);
            var splitNew = newSeq.SplitAt(i);
            Assert2.SequenceEqual(splitOld.Item1, splitNew.Item1);
            PexAssert.AreEqual(newValue, splitNew.Item2.Head);
            Assert2.SequenceEqual(splitOld.Item2, splitNew.Item2.Tail);
        }

		[PexMethod]
        [PexGenericArguments(new[] {typeof (int)})]
        public void Test_Reverse<T>([PexAssumeNotNull] T[] arr) {
            var seq = RandomAccessSequence.FromEnumerable(arr);
            Assert2.SequenceEqual(seq.AsEnumerable().Reverse(), seq.Reverse());
        }
    }
}
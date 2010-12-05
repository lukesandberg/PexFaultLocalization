/*
* VectorTests.cs is part of functional-dotnet project
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

using FP.Collections.Persistent;
using XunitExtensions;
using Microsoft.Pex.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FPTests {
	[TestClass]
    [PexClass(typeof (Vector<>))]
    public partial class VectorTests {
        [PexMethod]
        public void Test_InfiniteBounds(int i) {
            PexAssume.IsTrue(i >= 0);
            PexAssert.AreEqual(0, Vector<int>.Empty[i]);
			PexAssert.AreEqual(null, Vector<object>.Empty[i]);
        }

        [PexGenericArguments(typeof(int))]
        [PexGenericArguments(typeof(object))]
        [PexMethod(MaxBranches = 20000)]
        public void Test_SingleElementVector<T>(int i, T value) {
            PexAssume.IsTrue(i >= 0);
            Vector<T> vector = Vector<T>.Empty.SetAt(i, value);
			PexAssert.AreEqual(value, vector[i]);
        } // SingleElementVector()

        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public void Test_ReplaceSingleElement<T>([PexAssumeNotNull] T[] array, T newValue, int i) {
            var vector = Vector.New(array);
            PexAssume.IsTrue(i >= 0);
            var vector2 = vector.SetAt(i, newValue);
			PexAssert.IsTrue(vector2.Count >= vector.Count);
			PexAssert.AreEqual(newValue, vector2[i]);
            PexAssert.TrueForAll(0, vector2.Count, j => j == i || Equals(vector2[j], vector[j]));
        } // ReplaceSingleElement(i, j)

        [PexGenericArguments(typeof(int))]
        // [PexGenericArguments(typeof(object))]
		[PexMethod]
        public void Test_StoreManyElements<T>([PexAssumeNotNull] T[] array) {
            var vector = Vector<T>.Empty;
            for (int i = 0; i < array.Length; i++)
                vector = vector.SetAt(i, array[i]);
            for (int i = 0; i < array.Length; i++)
				PexAssert.AreEqual(array[i], vector[i]);
			PexAssert.AreEqual(array.Length, vector.Count);
            Assert2.SequenceEqual(array, vector);
        } // StoreManyElements()

        [PexGenericArguments(typeof(int))]
        // [PexGenericArguments(typeof(object))]
        [PexMethod(MaxBranches = 40000)]
        public void Test_Append<T>([PexAssumeNotNull] T[] array) {
            var vector = Vector<T>.Empty;
            foreach (T t in array)
                vector = vector.Append(t);
            for (int i = 0; i < array.Length; i++)
                PexAssert.AreEqual(array[i], vector[i]);
			PexAssert.AreEqual(array.Length, vector.Count);
            Assert2.SequenceEqual(array, vector);
        } // StoreManyElements()
    } // class VectorTests
} // namespace FPTests
/*
* Arrays.cs is part of functional-dotnet project
* 
* Copyright (c) 2009 Alexey Romanov
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FP.Validation;

namespace FP.Util {
    /// <summary>
    /// Utility methods for arrays.
    /// </summary>
    public static class Arrays {
        /// <summary>
        /// Determines whether the contents of two arrays are equal.
        /// </summary>
        /// <typeparam name="T">The type of arrays' elements.</typeparam>
        /// <param name="array1">The first array.</param>
        /// <param name="array2">The second array.</param>
        public static bool ContentEquals<T>(T[] array1, T[] array2) {
            if (ReferenceEquals(array1, array2)) return true;
            if (array1.Length != array2.Length) return false;
            for (int i = 0; i < array1.Length; i++) {
                if (!Equals(array1[i], array2[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether the given array contains the given item.
        /// </summary>
        /// <typeparam name="T">The type of array's elements.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item">The item.</param>
        public static bool Contains<T>(this T[] array, T item) {
            var equalityC = EqualityComparer<T>.Default;
            for (int i = 0; i < array.Length; i++)
                if (equalityC.Equals(array[i], item)) return true;
            return false;
        }

        /// <summary>
        /// Copies the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>A copy of the array.</returns>
        public static T[] Copy<T>(this T[] array) {
            return array.CopyNoChecks(0, array.Length);
        }

        /// <summary>
        /// Copies a part of the specified array, starting with.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The index from which copying starts.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <returns>A copy of the array.</returns>
        public static T[] Copy<T>(this T[] array, int startIndex, int length) {
            Requires.That.
                IsIndexAndCountInRange(array, startIndex, length, "startIndex", "length").
                Check();

            return array.CopyNoChecks(startIndex, length);
        }

        internal static T[] CopyNoChecks<T>(this T[] array, int startIndex) {
            return array.CopyNoChecks(startIndex, array.Length - startIndex);
        }

        internal static T[] CopyNoChecks<T>(this T[] array, int startIndex, int length) {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(length >= 0);
            Debug.Assert(startIndex + length <= array.Length);
            if (length == 0) return Empty<T>();
            var result = new T[length];
            Array.Copy(array, startIndex, result, 0, length);
            return result;
        }

        /// <summary>
        /// Specialized version of FoldLeft for arrays.
        /// </summary>
        public static A FoldLeft<T, A>(this T[] array, Func<A, T, A> binOp, A initial) {
            A result = initial;
            for (int i = array.Length - 1; i >= 0; i--)
                result = binOp(result, array[i]);
            return result;
        }

        /// <summary>
        /// Specialized version of FoldRight for arrays.
        /// </summary>
        public static A FoldRight<T, A>(this T[] array, Func<T, A, A> binOp, A initial) {
            A result = initial;
            for (int i = array.Length - 1; i >= 0; i--)
                result = binOp(array[i], result);
            return result;
        }

        /// <summary>
        /// Returns an empty array of the type <typeparamref name="T"/>.
        /// </summary>
        public static T[] Empty<T>() {
            return EmptyArrayCache<T>.Instance;
        }

        /// <summary>
        /// Enumerates the specified array in the reverse order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        public static IEnumerable<T> ReverseIterator<T>(this T[] array) {
            for (int i = array.Length - 1; i >= 0; i--)
                yield return array[i];
        }

        private static class EmptyArrayCache<T> {
            private static T[] _instance;

            public static T[] Instance {
                get {
                    if (_instance == null)
                        _instance = new T[0];
                    return _instance;
                }
            }
        }
    }
}
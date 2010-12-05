/*
* Enumerators.cs is part of functional-dotnet project
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

using System;
using System.Collections;
using System.Collections.Generic;

namespace FP.Core {
    /// <summary>
    /// Contains extension methods on <see cref="IEnumerator{T}"/>.
    /// </summary>
    /// <remarks>
    /// All methods here assume the enumeration has started already (that is, 
    /// <see cref="IEnumerator.MoveNext"/> has been called) and consider <see cref="IEnumerator{T}.Current"/>
    /// if relevant.
    /// 
    /// The methods which return <see cref="IEnumerator{T}"/> use deferred execution, all
    /// other methods execute immediately.
    /// </remarks>
    public static class Enumerators {
        /// <summary>Advances a enumerator by a specified number of elements.</summary>
        /// <param name="enumerator">An <see cref="IEnumerator{T}" /> to advance.</param>
        /// <param name="count">The number of elements to skip.</param>
        public static void Skip<T>(this IEnumerator<T> enumerator, int count) {
            while (count > 0 && enumerator.MoveNext())
                count--;
        }

        /// <summary>Advances a enumerator as long as a specified condition is true.</summary>
        /// <returns>An <see cref="IEnumerator{T}" /> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.</returns>
        /// <param name="enumerator">An <see cref="IEnumerator{T}" /> to advance.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static void SkipWhile<T>(this IEnumerator<T> enumerator, Func<T, bool> predicate) {
            while (predicate(enumerator.Current))
                if (!enumerator.MoveNext()) break;
        }

        /// <summary>Advances a enumerator as long as a specified condition is true.</summary>
        /// <param name="enumerator">An <see cref="IEnumerator{T}" /> to advance.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the function represents the index of the source element..</param>
        public static void SkipWhile<T>(this IEnumerator<T> enumerator, Func<T, int, bool> predicate) {
            int index = 0;
            while (predicate(enumerator.Current, index)) {
                if (!enumerator.MoveNext()) break;
                index++;
            }
        }

        /// <summary>Advances a enumerator by a specified number of elements and returns elements read along the way.</summary>
        /// <param name="enumerator">An <see cref="IEnumerator{T}" /> to advance.</param>
        /// <param name="count">The number of elements to skip.</param>
        public static IEnumerator<T> Take<T>(this IEnumerator<T> enumerator, int count) {
            while (count > 0 && enumerator.MoveNext()) {
                count--;
                yield return enumerator.Current;
            }
        }

        /// <summary>Advances a enumerator as long as a specified condition is true and returns elements read along the way.</summary>
        /// <param name="enumerator">An <see cref="IEnumerator{T}" /> to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static IEnumerator<T> TakeWhile<T>(this IEnumerator<T> enumerator,
                                                  Func<T, bool> predicate) {
            while (predicate(enumerator.Current)) {
                yield return enumerator.Current;
                if (!enumerator.MoveNext()) yield break;
            }
        }

        /// <summary>Advances a enumerator as long as a specified condition is true and returns elements read along the way.</summary>
        /// <param name="enumerator">An <see cref="IEnumerator{T}" /> to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the function represents the index of the source element.</param>
        public static IEnumerator<T> TakeWhile<T>(this IEnumerator<T> enumerator,
                                                  Func<T, int, bool> predicate) {
            int index = 0;
            while (predicate(enumerator.Current, index)) {
                yield return enumerator.Current;
                if (!enumerator.MoveNext()) yield break;
                index++;
            }
        }
    }
}
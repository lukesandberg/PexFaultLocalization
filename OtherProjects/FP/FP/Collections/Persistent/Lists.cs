/*
* Lists.cs is part of functional-dotnet project
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
using System.Linq;
using FP.Core;
using FP.Validation;

namespace FP.Collections.Persistent {
    /// <summary>
    /// More specific versions of some methods from <see cref="Enumerables"/> and
    /// <see cref="Enumerable"/>
    /// </summary>
    public static class Lists {
        /// <summary>
        /// Conses <paramref name="t"/> to <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">Type of <paramref name="t"/></typeparam>
        /// <typeparam name="TList">Type of <paramref name="list"/>. Must be a list of 
        /// <paramref name="t"/>.</typeparam>
        /// <param name="t">The head.</param>
        /// <param name="list">The tail.</param>
        /// <returns>A list starting with <paramref name="t"/> and continuing with
        /// <paramref name="list"/>.</returns>
        public static TList Cons<T, TList>(this T t, TList list) where TList : IList<T, TList> {
            return list.Prepend(t);
        }

        /// <summary>
        /// Gets the "head" (first element) of the list.
        /// </summary>
        /// <returns>The head of the list.</returns>
        /// <exception cref="EmptyEnumerableException">if the current list is empty.</exception>
        /// <param name="list">The list.</param>
        public static T First<T, TList>(this TList list) where TList : IList<T, TList> {
            return list.Head;
        }

        /// <summary>Bypasses a specified number of elements in a list and then returns the remaining elements.</summary>
        /// <returns>A <typeparamref name="TList"/> that contains the elements that occur after the specified index in the input list.</returns>
        /// <param name="list">A <typeparamref name="TList"/> to return elements from.</param>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        public static TList Skip<T, TList>(this TList list, int count) where TList : IList<T, TList> {
            while (count > 0 && !list.IsEmpty) {
                list = list.Tail;
                count--;
            }
            return list;
        }

        /// <summary>Bypasses elements in a list as long as a specified condition is true and then returns the remaining elements.</summary>
        /// <returns>An <typeparamref name="TList"/> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.</returns>
        /// <param name="list">An <typeparamref name="TList"/> to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static TList SkipWhile<T, TList>(this TList list, Func<T, bool> predicate)
            where TList : IList<T, TList> {
            while (!list.IsEmpty && predicate(list.Head))
                list = list.Tail;
            return list;
        }

        /// <summary>Bypasses elements in a list as long as a specified condition is true and then returns the remaining elements.</summary>
        /// <returns>An <typeparamref name="TList"/> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.</returns>
        /// <param name="list">An <typeparamref name="TList"/> to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the function represents the index of the source element..</param>
        public static TList SkipWhile<T, TList>(this TList list, Func<T, int, bool> predicate)
            where TList : IList<T, TList> {
            int index = 0;
            while (!list.IsEmpty && predicate(list.Head, index)) {
                list = list.Tail;
                index++;
            }
            return list;
        }
    }
}
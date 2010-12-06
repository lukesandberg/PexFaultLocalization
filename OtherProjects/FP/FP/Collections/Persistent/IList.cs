/*
* IImmutableList.cs is part of functional-dotnet project
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
using FP.Validation;

namespace FP.Collections.Persistent {
    /// <summary>
    /// Represents an immutable list of values.
    /// </summary>
    /// <typeparam name="T">The type of elements of the list.</typeparam>
    /// <typeparam name="TList">The type of the list.</typeparam>
    public interface IList<T, TList> : ICollection<T> where TList : IList<T, TList> {
        /// <summary>
        /// Gets the "head" (first element) of the list.
        /// </summary>
        /// <value>The head of the list.</value>
        /// <exception cref="EmptyEnumerableException">if the current list <see cref="ICollection{T}.IsEmpty"/>.</exception>
        T Head { get; }

        /// <summary>
        /// Gets the "tail" (all elements but the first) of the list.
        /// </summary>
        /// <value>The tail of the list.</value>
        /// <exception cref="EmptyEnumerableException">if the list <see cref="ICollection{T}.IsEmpty"/>.</exception>
        TList Tail { get; }

        /// <summary>
        /// Prepends a new head.
        /// </summary>
        /// <param name="newHead">The new head.</param>
        /// <returns>The list with <paramref name="newHead"/> as <see cref="Head"/>
        /// and this list as <see cref="Tail"/>.</returns>
        TList Prepend(T newHead);
    }

    /// <summary>
    /// Extension methods for <see cref="IList{T,TList}"/>.
    /// </summary>
    public static class ImmutableLists {
        /// <summary>
        /// Case analysis on lists.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <typeparam name="TList">The type of the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="ifEmpty">The action to do if the list is empty.</param>
        /// <param name="ifNotEmpty">The action to do if the list is not empty.</param>
        public static void Match<T, TList>(
            this TList list, Action ifEmpty, Action<T, TList> ifNotEmpty)
            where TList : IList<T, TList> {
            if (list.IsEmpty)
                ifEmpty();
            else
                ifNotEmpty(list.Head, list.Tail);
        }

        /// <summary>
        /// Case analysis on lists.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <typeparam name="TList">The type of the list.</typeparam>
        /// <typeparam name="R">The return type of the functions.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="ifEmpty">The function to call if the list is empty.</param>
        /// <param name="ifNotEmpty">The function to call if the list is not empty.</param>
        public static R Match<T, TList, R>(
            this TList list, Func<R> ifEmpty, Func<T, TList, R> ifNotEmpty)
            where TList : IList<T, TList> {
            return list.IsEmpty ? ifEmpty() : ifNotEmpty(list.Head, list.Tail);
        }
    }
}
/*
* IDeque.cs is part of functional-dotnet project
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

using FP.Validation;

namespace FP.Collections.Persistent {
    /// <summary>
    /// An immutable double-ended queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDeque">The type of the deque.</typeparam>
    public interface IDeque<T, TDeque> : IList<T, TDeque>, IReversibleEnumerable<T> where TDeque : IDeque<T, TDeque> {
        /// <summary>
        /// Appends the specified element to the end of the list.
        /// </summary>
        /// <param name="newLast">The new last element.</param>
        /// <returns>The resulting list.</returns>
        TDeque Append(T newLast);

        /// <summary>
        /// Gets the initial sublist (all elements but the last) of the list.
        /// </summary>
        /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
        /// <exception cref="EmptyEnumerableException"></exception>
        TDeque Init { get; }

        /// <summary>
        /// Gets the last element of the list.
        /// </summary>
        /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
        /// <exception cref="EmptyEnumerableException"></exception>
        T Last { get; }
    }
}
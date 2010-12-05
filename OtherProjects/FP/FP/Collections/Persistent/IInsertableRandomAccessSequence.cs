/*
* IInsertableRandomAccessSequence.cs is part of functional-dotnet project
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
using System.Collections.Generic;

namespace FP.Collections.Persistent {
    /// <summary>
    /// Represents a <see cref="IAdjustableRandomAccessSequence{T,TSequence}"/> where new elements can be inserted or removed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSequence">The type of the RA deque.</typeparam>
    public interface IInsertableRandomAccessSequence<T, TSequence> :
        IAdjustableRandomAccessSequence<T, TSequence>, IDeque<T, TSequence>
        where TSequence : IInsertableRandomAccessSequence<T, TSequence> {
        /// <summary>
        /// Inserts <paramref name="newValue"/> at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index where the new element shall be inserted.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        TSequence InsertAt(int index, T newValue);

        /// <summary>
        /// Removes the element at index <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        TSequence RemoveAt(int index);

        /// <summary>
        /// Inserts all elements in <paramref name="ts"/> at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index where the new element shall be inserted.</param>
        /// <param name="ts">The collection of values to insert.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        TSequence InsertRangeAt(int index, IEnumerable<T> ts);

        /// <summary>
        /// Removes <paramref name="count"/> elements, starting at index <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="startIndex">The index of the first element to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> or 
        /// <paramref name="count"/> is out of range.</exception>
        TSequence RemoveRangeAt(int startIndex, int count);
        }
}
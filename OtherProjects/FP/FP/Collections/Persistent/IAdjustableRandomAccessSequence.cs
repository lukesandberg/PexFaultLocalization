/*
* IAdjustableRandomAccessSequence.cs is part of functional-dotnet project
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

namespace FP.Collections.Persistent {
    /// <summary>
    /// Represents a random access sequence where elements can be updated.
    /// </summary>
    /// <typeparam name="T">Type of the elements of the sequence.</typeparam>
    /// <typeparam name="TSequence">Type of the sequence.</typeparam>
    public interface IAdjustableRandomAccessSequence<T, TSequence> : IRandomAccess<T>
        where TSequence : IAdjustableRandomAccessSequence<T, TSequence> {
        /// <summary>
        /// Updates the element at <paramref name="index"/> using <paramref name="function"/>.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="function">The function to apply to the element currently at <paramref name="index"/></param>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        /// <remarks>
        /// Equivalent to <code>SetAt(index, function(this[index])), but faster.</code>
        /// </remarks>
        TSequence AdjustAt(int index, Func<T, T> function);
        } // interface IAdjustableRandomAccessSequence`2

    /// <summary>
    /// Utility method for all random access sequences.
    /// </summary>
    public static class AdjustableRandomAccessSequences {
        /// <summary>
        /// Replaces the <see cref="T"/> at the specified index with the specified value.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the sequence.</typeparam>
        /// <typeparam name="TSequence">Type of the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="index">The index.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>
        /// The sequence where the element at <paramref name="index"/> has the value <paramref name="newValue"/>
        /// and all other elements have the same value as in the original sequence.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        public static TSequence SetAt<T, TSequence>(this TSequence sequence, int index, T newValue)
            where TSequence : IAdjustableRandomAccessSequence<T, TSequence> {
            return sequence.AdjustAt(index, _ => newValue);
        }
    }
} // namespace FP.Collections.Immutable
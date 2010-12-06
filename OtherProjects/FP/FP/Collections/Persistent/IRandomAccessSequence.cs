/*
* IRandomAccessSequence.cs is part of functional-dotnet project
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
    /// A read interface for random access sequences.
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    /// <typeparam name="TSequence">The type of the sequence.</typeparam>
    public interface IRandomAccessSequence<T, TSequence> : IRandomAccess<T> where TSequence : IRandomAccessSequence<T, TSequence> {
        /// <summary>
        /// Returns a subsequence starting at <paramref name="startIndex"/> and consisting of <paramref name="count"/> elements.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        TSequence SubSequence(int startIndex, int count);
    }
}
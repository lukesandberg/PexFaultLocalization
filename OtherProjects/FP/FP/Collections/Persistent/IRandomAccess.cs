/*
* IRandomAccess.cs is part of functional-dotnet project
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

namespace FP.Collections.Persistent {
    public interface IRandomAccess<T> : ICollection<T> {
        /// <summary>
        /// Gets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        T this[int index] { get; }

        /// <summary>
        /// Gets the number of elements in the sequence.
        /// </summary>
        /// <value>The number of elements in the sequence.</value>
        int Count { get; }
    }
}
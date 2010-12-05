/*
* IEnumerableFrom.cs is part of functional-dotnet project
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
using System.Collections.Generic;

namespace FP.Collections.Persistent {
    /// <summary>
    /// Exposes a way to enumerate all elements of a collection starting from a given
    /// index and not just from beginning.
    /// </summary>
    /// <typeparam name="T">The type of elements of the collection.</typeparam>
    public interface IEnumerableFrom<T> {
        /// <summary>
        /// Enumerates the collection starting from a given index.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        IEnumerable<T> IteratorFrom(int startIndex);
    }
}
/*
* IReversible.cs is part of functional-dotnet project
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

using System.Collections.Generic;

namespace FP.Collections.Persistent {
    /// <summary>
    /// Represents a sequence which can be effectively enumerated in reverse order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReversibleEnumerable<T> : IEnumerable<T> {
        /// <summary>
        /// Returns an iterator which yields all elements of the sequence in the reverse order.
        /// </summary>
        /// <remarks>This should always be equivalent to, but faster than, 
        /// <code>
        /// AsEnumerable().Reverse();
        /// </code></remarks>
        IEnumerable<T> ReverseIterator();
    } // interface IReversibleEnumerable
}
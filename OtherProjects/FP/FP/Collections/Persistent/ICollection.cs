/*
* ICollection.cs is part of functional-dotnet project
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
    /// Represents a collection.
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    public interface ICollection<T> : IEnumerable<T> {
        /// <summary>
        /// Gets a value indicating whether this collection is empty.
        /// </summary>
        /// <value><c>true</c>.</value>
        bool IsEmpty { get; }
    }
}
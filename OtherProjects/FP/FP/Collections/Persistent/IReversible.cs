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

namespace FP.Collections.Persistent {
    /// <summary>
    /// Represents a collection which can be reversed efficiently.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection.</typeparam>
    public interface IReversible<TCollection> where TCollection : IReversible<TCollection> {
        /// <summary>
        /// Reverses this instance.
        /// </summary>
        TCollection Reverse();
    } // interface IReversible
} // namespace FP.Collections.Immutable
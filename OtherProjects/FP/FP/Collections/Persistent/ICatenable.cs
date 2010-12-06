/*
* ICatenable.cs is part of functional-dotnet project
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
    /// Represents a collection which can be concatenated with <typeparamref name="TCollection"/>
    /// returning a <typeparamref name="TResultCollection"/> as the result.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection to concatenate.</typeparam>
    /// <typeparam name="TResultCollection">The type of the result collection.</typeparam>
    /// <remarks>Normally <c>TResultCollection : ICatenable{TCollection, TResultCollection}</c>
    /// will hold, but not necessarily.</remarks>
    public interface ICatenable<TCollection, TResultCollection> {
        /// <summary>
        /// Concatenates the instance with a <typeparamref name="TCollection"/>.
        /// </summary>
        TResultCollection Concat(TCollection collection);
    }

    /// <summary>
    /// Represents a collection which can be concatenated with <typeparamref name="TCollection"/>
    /// returning a <typeparamref name="TCollection"/> as the result.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection to concatenate and the result collection.</typeparam>
    public interface ICatenable<TCollection> : ICatenable<TCollection, TCollection> {}
}
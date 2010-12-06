/*
* ISortedDictionary.cs is part of functional-dotnet project
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
using System.Collections.Generic;
using FP.Core;

namespace FP.Collections.Persistent {
    /// <summary>
    /// A dictionary which keeps its keys in a sorted order.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TComparer">The type of the comparer. The comparer must
    /// be stateless and have a default constructor.</typeparam>
    /// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
    /// <remarks><see cref="IDictionary{TKey,TValue,TDictionary}.Keys"/> and
    /// <see cref="IDictionary{TKey,TValue,TDictionary}.Values"/> are guaranteed
    /// to return the results in ascending order of keys.</remarks>
    public interface ISortedDictionary<TKey, TValue, TComparer, TDictionary> :
        IDictionary<TKey, TValue, TDictionary>,
		IReversibleEnumerable<FP.Core.Tuple<TKey, TValue>>
        where TDictionary : ISortedDictionary<TKey, TValue, TComparer, TDictionary>
        where TComparer : IComparer<TKey>, new() {
        /// <summary>
        /// Retrieves the minimum key and the associated value.
        /// </summary>
        /// <returns>The tuple of the minimum key, associated value and the
        /// dictionary containing all other elements, if the dictionary is
        /// non-empty; <c>None</c> otherwise.</returns>
		Optional<FP.Core.Tuple<TKey, TValue>> MinKeyAndValue();

        /// <summary>
        /// Retrieves the maximum key and the associated value.
        /// </summary>
        /// <returns>The tuple of the maximum key, associated value and the
        /// dictionary containing all other elements, if the dictionary is
        /// non-empty; <c>None</c> otherwise.</returns>
		Optional<FP.Core.Tuple<TKey, TValue>> MaxKeyAndValue();

        /// <summary>
        /// Splits dictionary on the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A tuple, where the first element contains all keys less
        /// than <paramref name="key"/>, the second element is <c>this[key]</c>,
        /// and the third element contains all keys greater than 
        /// <paramref name="key"/></returns>
		FP.Core.Tuple<TDictionary, Optional<TValue>, TDictionary> Split(TKey key);

        /// <summary>
        /// Retrieves the minimum key, associated value, and the dictionary
        /// containing all other elements.
        /// </summary>
        /// <param name="minKey">Is bound to the minimum key, if the dictionary
        /// isn't empty.</param>
        /// <param name="minKeyValue">Is bound to the value associated to the
        /// minimum key, if the dictionary isn't empty.</param>
        /// <returns>
        /// The dictionary containing all elements except the minimum key, if
        /// the dictionary is non-empty; <c>None</c> otherwise.
        /// </returns>
        Optional<TreeDictionary<TKey, TValue, TComparer>> FindAndRemoveMinKey(out TKey minKey, out TValue minKeyValue);

        /// <summary>
        /// Retrieves the minimum key, associated value, and the dictionary
        /// containing all other elements.
        /// </summary>
        /// <param name="maxKey">Is bound to the maximum key, if the dictionary
        /// isn't empty.</param>
        /// <param name="maxKeyValue">Is bound to the value associated to the
        /// maximum key, if the dictionary isn't empty.</param>
        /// <returns>The dictionary containing all elements except the maximum
        /// key, if the dictionary is non-empty; <c>None</c> otherwise.
        /// </returns>
        Optional<TreeDictionary<TKey, TValue, TComparer>> FindAndRemoveMaxKey(out TKey maxKey, out TValue maxKeyValue);
    }
}
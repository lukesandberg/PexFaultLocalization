/*
* IImmutableDictionary.cs is part of functional-dotnet project
* 
* Copyright (c) 2008-2009 Alexey Romanov
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
using STuple = System.Tuple;
namespace FP.Collections.Persistent {
    /// <summary>
    /// Represents an immutable collection of key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of keys.</typeparam>
    /// <typeparam name="TValue">The type of values.</typeparam>
    /// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
    /// <seealso cref="System.Collections.Generic.IDictionary{TKey,TValue}"/>
    public interface IDictionary<TKey, TValue, TDictionary> : ICollection<FP.Core.Tuple<TKey, TValue>>
        where TDictionary : IDictionary<TKey, TValue, TDictionary> {
        /// <summary>
        /// Gets the number of elements in the dictionary.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }

        /// <summary>
        /// Looks up the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <value><see cref="Optional.Some{T}"/><c>(value)</c> if the
        /// dictionary contains the specified key and associates <c>value</c>
        /// to it and <see cref="Optional.None{T}"/> otherwise.</value>
        Optional<TValue> this[TKey key] { get; }

        /// <summary>
        /// Adds the specified key with the specified value.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="combineFunc">
        /// The function to be called if the given key is already present. The
        /// argument is the value currently associated with the key, the result is the value
        /// to be used in the new dictionary.
        /// </param>
        /// <returns>
        /// The resulting dictionary.
        /// </returns>
        TDictionary Add(TKey key, TValue value, Func<TValue, TValue> combineFunc);

        /// <summary>
        /// Adds the specified key with the specified value. If the key is
        /// already present, replaces the current value.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The resulting dictionary.
        /// </returns>
        TDictionary Add(TKey key, TValue value);

        /// <summary>
        /// Removes the specified key and the associated value. If the
        /// dictionary doesn't contain this key, it is returned unchanged.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">is bound to <c>this[key]</c>.</param>
        /// <returns>The resulting dictionary.</returns>
        TDictionary Remove(TKey key, out Optional<TValue> value);

        /// <summary>
        /// Updates the value corresponding to the specified key. If the
        /// dictionary doesn't contain the key, it is returned unchanged; if
        /// <code>updateFunc(currentValue)</code> returns <code>None</code>, the
        /// key is removed; if it returns <code>Some(newValue)</code>, the
        /// current value is replaced with newValue.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="updateFunc">The updating function.</param>
        /// <param name="value">is bound to <c>this[key]</c>.</param>
        /// <returns>The resulting dictionary.</returns>
        TDictionary Update(TKey key, Func<TValue, Optional<TValue>> updateFunc, out Optional<TValue> value);

        /// <summary>
        /// Updates values of all keys. For each key in the dictionary, if
        /// <code>updateFunc(key, this[key])</code> returns <code>None</code>, the
        /// key is removed; if it returns <code>Some(newValue)</code>, the
        /// current value is replaced with <c>newValue</c>.
        /// </summary>
        /// <param name="updateFunc">The updating function.</param>
        /// <returns>The resulting dictionary.</returns>
        TDictionary MapPartial(Func<TKey, TValue, Optional<TValue>> updateFunc);

        /// <summary>
        /// Updates values of all keys. For each key in the dictionary, the
        /// value is replaced with <code>updateFunc(key, this[key])</code>.
        /// </summary>
        /// <param name="updateFunc">The updating function.</param>
        /// <returns>The resulting dictionary.</returns>
        TDictionary Map(Func<TKey, TValue, TValue> updateFunc);

        /// <summary>
        /// Gets the keys. Doesn't guarantee anything about the order in which they are yielded.
        /// </summary>
        /// <value>The keys.</value>
        IEnumerable<TKey> Keys { get; }

        /// <summary>
        /// Gets the values. Doesn't guarantee anything about the order in which they are yielded.
        /// </summary>
        /// <value>The values.</value>
        IEnumerable<TValue> Values { get; }
    }
}
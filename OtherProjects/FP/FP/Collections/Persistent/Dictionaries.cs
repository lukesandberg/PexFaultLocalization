/*
* Dictionaries.cs is part of functional-dotnet project
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
    /// Utility class for extension methods on <see cref="IDictionary{TKey,TValue,TDictionary}"/>.
    /// </summary>
    public static class Dictionaries {
        /// <summary>
        /// Adds all key-value pairs from 
        /// <paramref name="keyValuePairsSequence"/> to 
        /// <paramref name="dictionary"/> and returns the resulting dictionary.
        /// If the key is already present, the value is replaced by the one from the sequence.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
        /// <param name="keyValuePairsSequence">The key value pairs sequence.</param>
        /// <returns>The resulting dictionary.</returns>
        public static TDictionary AddAll<TKey, TValue, TDictionary>(
			this TDictionary dictionary, IEnumerable<FP.Core.Tuple<TKey, TValue>> keyValuePairsSequence) 
            where TDictionary : IDictionary<TKey, TValue, TDictionary> {
            foreach (var pair in keyValuePairsSequence) {
                TKey key = pair.Item1;
                TValue value = pair.Item2;
                dictionary = dictionary.Add(pair.Item1, pair.Item2);
            }
            return dictionary;
        }

        /// <summary>
        /// Adds all key-value pairs from
        /// <paramref name="keyValuePairsSequence"/> to
        /// <paramref name="dictionary"/> and returns the resulting dictionary.
        /// If the key is already present, the new value is found by calling
        /// <paramref name="combiner"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary.
        /// </typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyValuePairsSequence">The sequence of key-value pairs.
        /// </param>
        /// <param name="combiner">The combining function. Its arguments are the
        /// key, the value from the dictionary, and the value from the sequence,
        /// in this order.</param>
        /// <returns>The resulting dictionary.</returns>
        public static TDictionary AddAll<TKey, TValue, TDictionary>(
			this TDictionary dictionary, IEnumerable<FP.Core.Tuple<TKey, TValue>> keyValuePairsSequence, Func<TKey, TValue, TValue, TValue> combiner) 
            where TDictionary : IDictionary<TKey, TValue, TDictionary> {
            foreach (var pair in keyValuePairsSequence) {
                TKey key = pair.Item1;
                TValue value = pair.Item2;
                dictionary = dictionary.Add(pair.Item1, pair.Item2, val => combiner(key, val, value));
            }
            return dictionary;
        }

        /// <summary>
        /// For each key in <paramref name="keys"/> which is present in 
        /// <paramref name="dictionary"/>: if the key is already present, the
        /// new value is found by calling <paramref name="updateFunc"/>; if the key is absent,
        /// the dictionary isn't changed.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keys">The sequence of keys.</param>
        /// <param name="updateFunc">The combining function. It is called with 
        /// the key and <c>dictionary[key]</c> as arguments.</param>
        /// <returns>The resulting dictionary.</returns>
        public static TDictionary UpdateAll<TKey, TValue, TDictionary>(
            this TDictionary dictionary, IEnumerable<TKey> keys, Func<TKey, TValue, Optional<TValue>> updateFunc)
            where TDictionary : IDictionary<TKey, TValue, TDictionary> {
            foreach (var key in keys) {
                Optional<TValue> ignore;
                TKey key1 = key;
                dictionary = dictionary.Update(key1, val => updateFunc(key1, val), out ignore);
            }
            return dictionary;
        }

        /// <summary>
        /// Updates the value corresponding to the specified key. If the
        /// dictionary doesn't contain the key, it is returned unchanged; if
        /// <code>updateFunc(currentValue)</code> returns <code>None</code>, the
        /// key is removed; if it returns <code>Some(newValue)</code>, the
        /// current value is replaced with newValue.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="updateFunc">The updating function.</param>
        /// <returns>The resulting dictionary.</returns>
        /// <remarks>This is a variant of <see cref="IDictionary{TKey,TValue,TDictionary}.Update"/>
        /// to use when the old value isn't interesting.</remarks>
        public static TDictionary Update<TKey, TValue, TDictionary>(
            this TDictionary dictionary, TKey key, Func<TValue, Optional<TValue>> updateFunc)
            where TDictionary : IDictionary<TKey, TValue, TDictionary> {
                Optional<TValue> ignore;
                dictionary = dictionary.Update(key, updateFunc, out ignore);
            return dictionary;
        }
    }
}
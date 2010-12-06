/*
* FingerTreeHelpers.cs is part of functional-dotnet project
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
using System.Linq;

namespace FP.Collections.Persistent {
    /// <summary>
    /// Utility class.
    /// </summary>
    internal static class FingerTree {
        /// <summary>
        /// Creates the empty tree with the specified measure monoid.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the tree.</typeparam>
        /// <typeparam name="V">The type of the measure values.</typeparam>
        /// <param name="measureMonoid">The measure monoid.</param>
        public static FingerTree<T, V> Empty<T, V>(Monoid<V> measureMonoid)
            where T : IMeasured<V> {
            return FingerTree<T, V>.GetEmptyFromCache<T>(measureMonoid);
        }

        /// <summary>
        /// Creates the tree with the single element <paramref name="item"/> and the specified measure monoid.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the tree.</typeparam>
        /// <typeparam name="V">The type of the measure values.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="measureMonoid">The measure monoid.</param>
        public static FingerTree<T, V> Single<T, V>(T item, Monoid<V> measureMonoid)
            where T : IMeasured<V> {
            return new FingerTree<T, V>.Single(item, measureMonoid);
        }

        /// <summary>
        /// Creates the tree from the specified sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the tree.</typeparam>
        /// <typeparam name="V">The type of the measure values.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="measureMonoid">The measure monoid.</param>
        public static FingerTree<T, V> FromEnumerable<T, V>(IEnumerable<T> sequence, Monoid<V> measureMonoid)
            where T : IMeasured<V> {
            return Empty<T, V>(measureMonoid).AppendRange(sequence);
        }

        internal static V SumMeasures<V, T>(Monoid<V> monoid, V init, IEnumerable<T> sequence)
            where T : IMeasured<V> {
            return monoid.Sum(init, sequence.Select(t => t.Measure));
        }
    }
}
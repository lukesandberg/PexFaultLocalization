/*
* ComparerConversions.cs is part of functional-dotnet project
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

using System;
using System.Collections.Generic;

namespace XunitExtensions {
    /// <summary>
    /// A static class which provides conversions between <see cref="IComparer{T}"/> and <see cref="Comparison{T}"/>.
    /// </summary>
    public static class Comparers {
        /// <summary>
        /// Reverses the specified comparer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comparer">The comparer.</param>
        public static IComparer<T> Reverse<T>(this IComparer<T> comparer) {
            return new ReverseComparer<T>(comparer);
        }

        /// <summary>
        /// Converts a comparer to a comparison.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static Comparison<T> ToComparison<T>(this IComparer<T> comparer) {
            return comparer.Compare;
        }

        /// <summary>
        /// Converts a comparison to a comparer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static IComparer<T> ToComparer<T>(this Comparison<T> comparison) {
            return new FunctorComparer<T>(comparison);
        }

        public static IEqualityComparer<T> ToEqualityComparer<T>(this IComparer<T> comparer) {
            return new FunctorEqualityComparer<T>((x, y) => comparer.Compare(x, y) == 0);
        }
    }

    /// <summary>
    /// The comparer given by a function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FunctorComparer<T> : IComparer<T> {
        private readonly Comparison<T> _comparison;

        public FunctorComparer(Comparison<T> comparison) {
            _comparison = comparison;
        }

        ///<summary>
        ///Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        ///</summary>
        ///
        ///<returns>
        ///Value Condition Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        ///</returns>
        ///
        ///<param name="x">The first object to compare.</param>
        ///<param name="y">The second object to compare.</param>
        public int Compare(T x, T y) {
            return _comparison(x, y);
        }
    }

    /// <summary>
    /// The comparer given by a function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FunctorEqualityComparer<T> : IEqualityComparer<T> {
        private readonly Func<T, T, bool> _comparison;

        public FunctorEqualityComparer(Func<T, T, bool> comparison) {
            _comparison = comparison;
        }

        ///<summary>
        ///Determines whether the specified objects are equal.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified objects are equal; otherwise, false.
        ///</returns>
        ///
        ///<param name="x">The first object of type <typeparamref name="T" /> to compare.</param>
        ///<param name="y">The second object of type <typeparamref name="T" /> to compare.</param>
        public bool Equals(T x, T y) {
            return _comparison(x, y);
        }

        ///<summary>
        ///Returns a hash code for the specified object.
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the specified object.
        ///</returns>
        ///
        ///<param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        ///<exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
        /// <exception cref="NotImplementedException">Always.</exception>
        public int GetHashCode(T obj) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A reverse comparer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReverseComparer<T> : IComparer<T> {
        private readonly IComparer<T> _baseComparer;

        public ReverseComparer(IComparer<T> baseComparer) {
            _baseComparer = baseComparer;
        }

        ///<summary>
        ///Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        ///</summary>
        ///
        ///<returns>
        ///Value Condition Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        ///</returns>
        ///
        ///<param name="x">The first object to compare.</param>
        ///<param name="y">The second object to compare.</param>
        public int Compare(T x, T y) {
            return -_baseComparer.Compare(x, y);
        }
    }
}
/*
* FingerTreeNode.cs is part of functional-dotnet project
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FP.Util;

namespace FP.Collections.Persistent {
    /// <summary>
    /// A node in the middle section of a deep finger tree.
    /// </summary>
    /// <typeparam name="T">Type of the elements in the node.</typeparam>
    /// <typeparam name="V">Type of the weight monoid.</typeparam>
    [Serializable]
    internal struct FTNode<T, V> : IMeasured<V>, IEnumerable<T>, IFoldable<T>,
                                         IEquatable<FTNode<T, V>> where T : IMeasured<V> {
        internal FTNode(V measure, params T[] array) {
            Debug.Assert(array.Length == 2 || array.Length == 3);
            _measure = measure;
            AsArray = array;
        } // FTNode

        public A FoldRight<A>(Func<T, A, A> binOp, A initial) {
            return AsArray.FoldRight(binOp, initial);
        } // FoldRight

        public A FoldLeft<A>(Func<A, T, A> binOp, A initial) {
            return AsArray.FoldLeft(binOp, initial);
        } // FoldLeft

        private readonly V _measure;

        public V Measure {
            get { return _measure; }
        } // Measure

        /// <summary>
        /// Gets the node's representation as an array.
        /// </summary>
        /// <remarks>Do not mutate!</remarks>
        internal readonly T[] AsArray;

        /// <summary>
        /// Returns an enumerator that iterates through the node.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the node.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator() {
            for (int i = 0; i < AsArray.Length; i++)
                yield return AsArray[i];
        } // GetEnumerator

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        } // IEnumerable.GetEnumerator

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(FTNode<T, V> other) {
            return Equals(other.Measure, Measure) && Arrays.ContentEquals(other.AsArray, AsArray);
        } // Equals

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">
        /// Another object to compare to. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj) {
            if (obj.GetType() != typeof (FTNode<T, V>)) return false;
            return Equals((FTNode<T, V>) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() {
            unchecked {
                int result = AsArray[0].GetHashCode();
                result = (result * 397) ^ AsArray[1].GetHashCode();
                if (AsArray.Length == 3)
                    result = (result * 397) ^ AsArray[2].GetHashCode();
                return result;
            }
        } // GetHashCode

        public static bool operator ==(FTNode<T, V> left, FTNode<T, V> right) {
            return Equals(left, right);
        } // op_Equality

        public static bool operator !=(FTNode<T, V> left, FTNode<T, V> right) {
            return !Equals(left, right);
        } // op_Inequality
    } // class FTNode`2
}
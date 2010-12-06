/*
* OrderedSequence.cs is part of functional-dotnet project
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FP.Core;
using FP.Validation;

namespace FP.Collections.Persistent {
    /// <summary>
    /// A finger-tree-based ordered sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    /// <remarks>Do not use the default constructor.</remarks>
    [Serializable]
    public struct OrderedSequence<T> : IReversibleEnumerable<T>, IEquatable<OrderedSequence<T>> where T : IComparable<T> {
        private static readonly OrderedSequence<T> _emptyInstance =
            new OrderedSequence<T>(FingerTreeOrdered<Element, T>.EmptyInstance);

        /// <summary>
        /// Gets the empty ordered sequence.
        /// </summary>
        /// <value>The empty instance.</value>
        public static OrderedSequence<T> EmptyInstance { get { return _emptyInstance; } }

        [DebuggerDisplay("{Value}")]
        [DebuggerStepThrough]
        internal struct Element : IMeasured<T> {
            internal readonly T Value;

            public Element(T value) {
                Value = value;
            }

            public T Measure {
                get { return Value; }
            }
        }

        private readonly FingerTreeOrdered<Element, T> _ft;

        private OrderedSequence(FingerTreeOrdered<Element, T> ft) {
            _ft = ft;
        }

        internal static OrderedSequence<T> Single(T item) {
            return new OrderedSequence<T>(FingerTreeOrdered<Element, T>.MakeSingle(new Element(item)));
        }

        /// <summary>
        /// Gets the maximal element of the sequence.
        /// </summary>
        /// <remarks>If several elements are equal and maximal, ties may be broken arbitrarily.</remarks>
        /// <exception cref="EmptyEnumerableException">There are no elements in the sequence.</exception>
        public T Max {
            get { return _ft.Last.Value; }
        }

        /// <summary>
        /// Gets the minimal element of the sequence.
        /// </summary>
        /// <remarks>O(1). If several elements are equal and minimal, ties may be broken
        /// arbitrarily.</remarks>
        /// <exception cref="EmptyEnumerableException">There are no elements in the
        /// sequence.</exception>
        public T Min {
            get { return _ft.Head.Value; }
        }

        /// <summary>
        /// Gets the sequence except its maximal element.
        /// </summary>
        /// <remarks>O(1). If several elements are equal and maximal, the one returned by 
        /// <see cref="Max"/> will be removed.</remarks>
        /// <exception cref="EmptyEnumerableException">There are no elements in the
        /// sequence.</exception>
        public OrderedSequence<T> ExceptMax {
            get { return new OrderedSequence<T>(_ft.Init); }
        }

        /// <summary>
        /// Gets the sequence except its minimal element.
        /// </summary>
        /// <remarks>O(1). If several elements are equal and minimal, the one returned by 
        /// <see cref="Min"/> will be removed.</remarks>
        /// <exception cref="EmptyEnumerableException">There are no elements in the
        /// sequence.</exception>
        public OrderedSequence<T> ExceptMin {
            get { return new OrderedSequence<T>(_ft.Tail); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to
        /// iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator() {
            foreach (var element in _ft)
                yield return element.Value;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same
        /// type.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(OrderedSequence<T> other) {
            return Equals(other._ft, _ft);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj) {
            if (obj.GetType() != typeof(OrderedSequence<T>)) return false;
            return Equals((OrderedSequence<T>) obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() {
            return _ft.GetHashCode();
        }

        /// <summary>
        /// Returns the union of <paramref name="seq1"/> and <paramref name="seq2"/>.
        /// </summary>
        /// <param name="seq1">The first sequence.</param>
        /// <param name="seq2">The second sequence.</param>
        /// <remarks>O(m * log (n / m)), where m is the size of the shorter sequence.
        /// O(log m) in case max(seq1) &lt;= min(seq2) or vice versa.</remarks>
        public static OrderedSequence<T> operator +(OrderedSequence<T> seq1, OrderedSequence<T> seq2) {
            return seq1.Union(seq2);
        }

        /// <summary>
        /// Determines whether two ordered sequences are equal.
        /// </summary>
        /// <param name="left">The left sequence.</param>
        /// <param name="right">The right sequence.</param>
        /// <returns>Whether the sequences are equal.</returns>
        public static bool operator ==(OrderedSequence<T> left, OrderedSequence<T> right) {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left sequence.</param>
        /// <param name="right">The right sequence.</param>
        /// <returns>Whether the sequences are not equal.</returns>
        public static bool operator !=(OrderedSequence<T> left, OrderedSequence<T> right) {
            return !left.Equals(right);
        }

        /// <summary>
        /// Inserts <paramref name="item"/> into <paramref name="seq"/>.
        /// </summary>
        /// <param name="seq">The sequence.</param>
        /// <param name="item">The item.</param>
        /// <remarks>O(log(n))</remarks>
        public static OrderedSequence<T> operator |(OrderedSequence<T> seq, T item) {
            return seq.Insert(item);
        }

        /// <summary>
        /// Gets a value indicating whether this sequence is empty.
        /// </summary>
        /// <value><c>true</c> if this sequence is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty {
            get { return _ft.IsEmpty; }
        }

        /// <summary>
        ///  Returns an iterator which yields all elements of the sequence in the reverse order.
        /// </summary>
        /// <remarks>
        /// This should always be equivalent to, but faster than, 
        /// <code>
        ///  AsEnumerable().Reverse();
        /// </code>
        /// </remarks>
        public IEnumerable<T> ReverseIterator() {
            return _ft.ReverseIterator().Select(el => el.Value);
        }

        /// <summary>
        /// Determines whether the sequence contains the given item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// <c>true</c> if the sequence contains the given item; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>O(log(m)), where m is the number of elements from the closer side of
        /// the sequence to the position of the item.</remarks>
        public bool Contains(T item) {
            return _ft[item].HasValue;
        }

        /// <summary>
        /// Gets an element equal to <paramref name="item"/> if there is one;
        /// otherwise, <see cref="Optional{T}.None"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <remarks>O(log(m)), where m is the number of elements from the closer side of
        /// the sequence to the position of the item.</remarks>
        public Optional<T> this[T item] {
            get { return _ft[item].Map(el => el.Value); }
        }

        /// <summary>
        /// Finds all elements equal to the given item.
        /// </summary>
        /// <param name="item">The key.</param>
        /// <remarks>O(log(m)), where m is the number of elements from the closer side of
        /// the sequence to the position of the item.</remarks>
        public OrderedSequence<T> FindAll(T item) {
            return new OrderedSequence<T>(_ft.FindAll(item));
        }

        /// <summary>
        /// Splits the sequence into three subsequences. The first one contains all
        /// elements less than <paramref cref="item"/>, the second one all elements equal
        /// to it, and the third one greater elements.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <remarks>O(log(m)), where m is the number of elements from the closer side of
        /// the sequence to the position of the item.</remarks>
		public FP.Core.Tuple<OrderedSequence<T>, OrderedSequence<T>, OrderedSequence<T>> ExtractAll(T item)
		{
            var triple = _ft.ExtractAll(item);
			return FP.Core.Tuple.New(
                new OrderedSequence<T>(triple.Item1),
                new OrderedSequence<T>(triple.Item2),
                new OrderedSequence<T>(triple.Item3));
        }

        /// <summary>
        /// Inserts the specified item into the sequence.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <remarks>O(log n).</remarks>
        public OrderedSequence<T> Insert(T item) {
            return new OrderedSequence<T>(_ft.Insert(new Element(item)));
        }

        /// <summary>
        /// Inserts all specified items into the sequence.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <remarks>O(log max(n, m)), where m is the number of elements in 
        /// <paramref name="items"/>.</remarks>
        public OrderedSequence<T> Insert(params T[] items) {
            return InsertRange(items.AsEnumerable());
        }

        /// <summary>
        /// Inserts all specified items into the sequence.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <remarks>O(log max(n, m)), where m is the number of elements in 
        /// <paramref name="items"/>.</remarks>
        public OrderedSequence<T> InsertRange(IEnumerable<T> items) {
            return new OrderedSequence<T>(_ft.InsertRange(items.Select(t => new Element(t))));
        }

        /// <summary>
        /// Returns the union of the sequence with specified other sequence.
        /// </summary>
        /// <param name="otherSequence">The other sequence.</param>
        public OrderedSequence<T> Union(OrderedSequence<T> otherSequence) {
            return new OrderedSequence<T>(_ft.Merge(otherSequence._ft));
        }

        /// <summary>
        /// Intersects the sequence with <paramref name="otherSequence"/>.
        /// </summary>
        /// <param name="otherSequence">The other sequence.</param>
        public OrderedSequence<T> Intersect(OrderedSequence<T> otherSequence) {
            return new OrderedSequence<T>(_ft.Intersect(otherSequence._ft));
        }

        /// <summary>
        /// Splits the sequence into two parts. The first one contains all elements less
        /// than <paramref cref="item"/>, the second one all greater elements. Elements
        /// equal to <paramref cref="item"/> go into one of them according to the 
        /// <paramref name="equalGoLeft"/> parameter.
        /// </summary>
        /// <param name="item">The element on which the sequence is split.</param>
        /// <param name="equalGoLeft">if set to <c>true</c>, elements with the measure
        /// equal to <see cref="item"/> will be at the left side of the split; otherwise,
        /// they will be on the right side.</param>
		public FP.Core.Tuple<OrderedSequence<T>, OrderedSequence<T>> Split(T item, bool equalGoLeft)
		{
            var pair = _ft.Split(item, equalGoLeft);
            return Pair.New(
                new OrderedSequence<T>(pair.Item1),
                new OrderedSequence<T>(pair.Item2));
        }

        /// <summary>
        /// Returns all elements in this sequence less than <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        public OrderedSequence<T> LessThan(T item) {
            return new OrderedSequence<T>(_ft.LessThan(item));
        }

        /// <summary>
        /// Returns all elements in this sequence less than, or equal to, 
        /// <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        public OrderedSequence<T> AtMost(T item) {
            return new OrderedSequence<T>(_ft.AtMost(item));
        }

        /// <summary>
        /// Returns all elements in this sequence greater than, or equal to, 
        /// <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        public OrderedSequence<T> AtLeast(T item) {
            return new OrderedSequence<T>(_ft.AtLeast(item));
        }

        /// <summary>
        /// Returns all elements in this sequence greater than <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        public OrderedSequence<T> GreaterThan(T item) {
            return new OrderedSequence<T>(_ft.GreaterThan(item));
        }

        /// <summary>
        /// Reduces the sequence in descending order.
        /// </summary>
        /// <typeparam name="A">The type of the accumulator.</typeparam>
        /// <param name="binOp">The binary operation.</param>
        /// <param name="initial">The initial accumulator value.</param>
        /// <returns>
        /// The final accumulator value.
        /// </returns>
        public A FoldRight<A>(Func<T, A, A> binOp, A initial) {
            return _ft.FoldRight((el, a) => binOp(el.Value, a), initial);
        }

        /// <summary>
        /// Reduces the sequence in ascending order.
        /// </summary>
        /// <typeparam name="A">The type of the accumulator.</typeparam>
        /// <param name="binOp">The binary operation.</param>
        /// <param name="initial">The initial accumulator value.</param>
        /// <returns>
        /// The final accumulator value.
        /// </returns>
        public A FoldLeft<A>(Func<A, T, A> binOp, A initial) {
            return _ft.FoldLeft((a, el) => binOp(a, el.Value), initial);
        }
    }

    /// <summary>
    /// Utility methods for creating <see cref="OrderedSequence{T}"/>.
    /// </summary>
    /// <seealso cref="OrderedSequence{T}"/>
    public static class OrderedSequence {
        /// <summary>
        /// Returns the empty <see cref="OrderedSequence{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
        public static OrderedSequence<T> Empty<T>() where T : IComparable<T> {
            return OrderedSequence<T>.EmptyInstance;
        }

        /// <summary>
        /// Returns the <see cref="OrderedSequence{T}"/> containing only the specified 
        /// <paramref name="item"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
        /// <param name="item">The item.</param>
        public static OrderedSequence<T> Single<T>(T item) where T : IComparable<T> {
            return OrderedSequence<T>.Single(item);
        }

        /// <summary>
        /// Creates a <see cref="OrderedSequence{T}"/> with the elements from 
        /// <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
        /// <param name="sequence">The sequence of pairs (key, element) placed into the
        /// sequence initially.
        /// </param>
        /// <remarks>If <paramref name="sequence"/> is ordered (either descending or
        /// ascending), the running time is O(n); otherwise it is O(n * log n)</remarks>
        public static OrderedSequence<T> FromEnumerable<T>(IEnumerable<T> sequence) where T : IComparable<T> {
            return Empty<T>().InsertRange(sequence);
        }
    }
}
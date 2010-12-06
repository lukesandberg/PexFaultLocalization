/*
* RandomAccessSequence.cs is part of functional-dotnet project
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
using FP.Core;
using FP.HaskellNames;
using FP.Validation;

namespace FP.Collections.Persistent {
    /// <summary>
    /// A finger-tree-based random access sequence.
    /// An amortized running time is given for each operation, with <i>n</i> referring to the length 
    /// of the sequence and <i>i</i> being the integral index used by some operations. 
    /// </summary>
    /// <typeparam name="T">Type of the elements of the sequence.</typeparam>
    /// <remarks>Do not use the default constructor.</remarks>
    public struct RandomAccessSequence<T> :
        IEquatable<RandomAccessSequence<T>>,
        IInsertableRandomAccessSequence<T, RandomAccessSequence<T>>,
        IRandomAccessSequence<T, RandomAccessSequence<T>>,
        IReversible<RandomAccessSequence<T>>, ICatenable<RandomAccessSequence<T>>,
        ISplittable<RandomAccessSequence<T>> {
        private static readonly RandomAccessSequence<T> _emptyInstance =
            new RandomAccessSequence<T>(FingerTreeSized<Element>.EmptyInstance);

        /// <summary>
        /// Gets the empty <see cref="RandomAccessSequence{T}"/>.
        /// </summary>
        public static RandomAccessSequence<T> Empty {
            get { return _emptyInstance; }
        }

        private readonly FingerTreeSized<Element> _ft;

        /// <summary>
        /// An element of the sequence.
        /// </summary>
        [DebuggerDisplay("Value = {Value}")]
        internal struct Element : IMeasured<int> {
            /// <summary>
            /// Gets the measure of the object.
            /// </summary>
            /// <value>The measure.</value>
            public int Measure {
                get { return 1; }
            }

            internal readonly T Value;

            /// <summary>
            /// Initializes a new instance of the <see cref="RandomAccessSequence{T}.Element"/> struct.
            /// </summary>
            /// <param name="value">The value.</param>
            public Element(T value) {
                Value = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomAccessSequence{T}"/> struct
        /// containing the same elements as <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">The original sequence.</param>
        internal RandomAccessSequence(IEnumerable<T> sequence) {
            _ft = _emptyInstance._ft.AppendRange(sequence.Map(x => new Element(x)));
        }

        private RandomAccessSequence(FingerTreeSized<Element> ft) {
            _ft = ft;
        }

        internal static RandomAccessSequence<T> Single(T item) {
            return new RandomAccessSequence<T>(new FingerTreeSized<Element>.Single(new Element(item)));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator() {
            foreach (var element in _ft)
                yield return element.Value;
        }

        /// <summary>
        /// Appends the specified element to the end of the list.
        /// </summary>
        /// <param name="newLast">The new last element.</param>
        /// <returns>The resulting list.</returns>
        public RandomAccessSequence<T> Append(T newLast) {
            return new RandomAccessSequence<T>(_ft | new Element(newLast));
        }

        /// <summary>
        /// Returns a pair of sequences, the first contains the first <paramref name="index"/> of
        /// the sequence and the second one contains the rest of them.
        /// </summary>
        /// <param name="index">The index at which the sequence will be split.</param>
        /// <remarks>if <code>index &lt;= 0 || index &gt;= Count</code>, the corresponding part 
        /// of the result will be empty.</remarks>
		public FP.Core.Tuple<RandomAccessSequence<T>, RandomAccessSequence<T>> SplitAt(int index)
		{
            if (index <= 0)
                return Pair.New(Empty, this);
            if (index >= Count)
                return Pair.New(this, Empty);
            var ftSplit = _ft.SplitAt(index);
            return Pair.New(
                new RandomAccessSequence<T>(ftSplit.Item1),
                new RandomAccessSequence<T>(ftSplit.Item2));
        }

        /// <summary>Returns a specified number of contiguous elements from the start of the sequence.</summary>
        /// <returns>A <see cref="RandomAccessSequence{T}" /> that contains the specified number of elements from the start of the input sequence.</returns>
        /// <param name="count">The number of elements to return.</param>
        public RandomAccessSequence<T> Take(int count) {
            return new RandomAccessSequence<T>(_ft.Take(count));
        }

        /// <summary>Bypasses a specified number of elements in a sequence and then returns the remaining elements.</summary>
        /// <returns>A <see cref="RandomAccessSequence{T}" /> that contains the elements that occur after the specified index in the input sequence.</returns>
        /// <param name="count">The number of elements to return.</param>
        public RandomAccessSequence<T> Skip(int count) {
            return new RandomAccessSequence<T>(_ft.Skip(count));
        }

        /// <summary>
        /// Gets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        public T this[int index] {
            get {
                Requires.That.IsIndexInRange(this, index, "index").Check();
                return _ft[index].Item1.Value;
            }
        }

        /// <summary>
        /// Updates the element at <paramref name="index"/> using <paramref name="function"/>.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="function">The function to apply to the element currently at <paramref name="index"/></param>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        /// <remarks>
        /// Equivalent to <code>SetAt(index, function(this[index])), but faster.</code>
        /// </remarks>
        public RandomAccessSequence<T> AdjustAt(int index, Func<T, T> function) {
            Requires.That.IsIndexInRange(this, index, "index").Check();
            var split = _ft.SplitTreeAt(index, true, true);
            T currentValue = split.Pivot.Value;
            return new RandomAccessSequence<T>(
                (split.Left | new Element(function(currentValue))) + split.Right);
        }

        /// <summary>
        /// Concatenates the sequence with another.
        /// </summary>
        /// <param name="otherSequence">Another sequence.</param>
        /// <returns>The result of concatenation.</returns>
        public RandomAccessSequence<T> Concat(RandomAccessSequence<T> otherSequence) {
            return new RandomAccessSequence<T>(_ft + otherSequence._ft);
        }

        /// <summary>
        /// Gets the number of elements in the sequence.
        /// </summary>
        /// <value>The number of elements in the sequence.</value>
        public int Count {
            get { return _ft.Measure; }
        }

        /// <summary>
        /// Gets a value indicating whether this list is empty.
        /// </summary>
        /// <value><c>true</c>.</value>
        public bool IsEmpty {
            get { return _ft.IsEmpty; }
        }

        /// <summary>
        /// Gets the head of the list.
        /// </summary>
        /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
        /// <exception cref="EmptyEnumerableException"></exception>
        public T Head {
            get { return _ft.Head.Value; }
        }

        /// <summary>
        /// Gets the tail of the list.
        /// </summary>
        /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
        /// <exception cref="EmptyEnumerableException"></exception>
        public RandomAccessSequence<T> Tail {
            get { return new RandomAccessSequence<T>(_ft.Tail); }
        }

        /// <summary>
        /// Gets the initial sublist (all elements but the last) of the list.
        /// </summary>
        /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
        /// <exception cref="EmptyEnumerableException"></exception>
        public RandomAccessSequence<T> Init {
            get { return new RandomAccessSequence<T>(_ft.Init); }
        }

        /// <summary>
        /// Gets the last element of the list.
        /// </summary>
        /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
        /// <exception cref="EmptyEnumerableException"></exception>
        public T Last {
            get { return _ft.Last.Value; }
        }

        /// <summary>
        /// Inserts <paramref name="newValue"/> at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index where the new element shall be inserted.</param>
        /// <param name="newValue">The new value.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        public RandomAccessSequence<T> InsertAt(int index, T newValue) {
            if (index == Count)
                return new RandomAccessSequence<T>(_ft | new Element(newValue));
            if (index == 0)
                return new RandomAccessSequence<T>(new Element(newValue) | _ft);
            Requires.That.IsIndexInRange(this, index, "index").Check();
            var ftSplit = _ft.SplitAt(index);
            return
                new RandomAccessSequence<T>((ftSplit.Item1 | new Element(newValue)) + ftSplit.Item2);
        }

        /// <summary>
        /// Removes the element at index <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        public RandomAccessSequence<T> RemoveAt(int index) {
            Requires.That.IsIndexInRange(this, index, "index").Check();
            var split = _ft.SplitTreeAt(index, true, true);
            return new RandomAccessSequence<T>(split.Left + split.Right);
        }

        /// <summary>
        /// Prepends a sequence of elements.
        /// </summary>
        /// <param name="ts">The sequence of elements to prepend.</param>
        /// <returns>The random access sequence consisting of elements of <paramref name="ts"/>
        /// and this random access sequence.</returns>
        public RandomAccessSequence<T> PrependRange(IEnumerable<T> ts) {
            return new RandomAccessSequence<T>(_ft.PrependRange(ts.Map(t => new Element(t))));
        }

        /// <summary>
        /// Appends a sequence.
        /// </summary>
        /// <param name="ts">The sequence of elements to prepend.</param>
        /// <returns>The random access sequence consisting of
        /// this random access sequence and elements of <paramref name="ts"/>.</returns>
        public RandomAccessSequence<T> AppendRange(IEnumerable<T> ts) {
            return new RandomAccessSequence<T>(_ft.AppendRange(ts.Map(t => new Element(t))));
        }

        /// <summary>
        /// Inserts all elements in <paramref name="ts"/> at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index where the new element shall be inserted.</param>
        /// <param name="ts">The collection of values to insert.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        public RandomAccessSequence<T> InsertRangeAt(int index, IEnumerable<T> ts) {
            Requires.That.IsIndexInRange(this, index, "index").Check();
            var ftSplit = _ft.SplitAt(index);
            return new RandomAccessSequence<T>(
                ftSplit.Item1.AppendRange(ts.Map(x => new Element(x))) + ftSplit.Item2);
        }

        /// <summary>
        /// Removes <paramref name="count"/> elements, starting at index <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="startIndex">The index of the first element to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> or 
        /// <paramref name="count"/> is out of range.</exception>
        public RandomAccessSequence<T> RemoveRangeAt(int startIndex, int count) {
            Requires.That
                .IsIndexAndCountInRange(this, startIndex, count, "startIndex", "count")
                .Check();
            if (count == 0)
                return this;
            if (startIndex == 0)
                return Skip(count);
            var splitAtStartOfRange = _ft.SplitAt(startIndex);
            var afterRemovedRange = splitAtStartOfRange.Item2.Skip(count);
            return new RandomAccessSequence<T>(splitAtStartOfRange.Item1 + afterRemovedRange);
        }

        /// <summary>
        /// Returns the subsequence of length <paramref name="count"/> starting at index <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="startIndex">The index of the first element in the subsequence.</param>
        /// <param name="count">The number of elements in the subsequence.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        public RandomAccessSequence<T> SubSequence(int startIndex, int count) {
            Requires.That
                .IsIndexAndCountInRange(this, startIndex, count, "startIndex", "count")
                .Check();
            if (count == 0)
                return Empty;
            return new RandomAccessSequence<T>(_ft.Skip(startIndex).Take(count));
        }

        /// <summary>
        /// Reverses the sequence.
        /// </summary>
        /// <returns>The sequence containing the same elements in reverse order.</returns>
        public RandomAccessSequence<T> Reverse() {
            return new RandomAccessSequence<T>(_ft.ReverseTree(Functions.Id<Element>()));
        }

        /// <summary>
        /// Prepends the specified element to the beginning of the list.
        /// </summary>
        /// <param name="newHead">The new head.</param>
        /// <returns>The resulting list.</returns>
        public RandomAccessSequence<T> Prepend(T newHead) {
            return new RandomAccessSequence<T>(new Element(newHead) | _ft);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerable<T> ReverseIterator() {
            foreach (var element in _ft.ReverseIterator()) {
                yield return element.Value;
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// <i>O(log(n))</i>
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        /// <remarks>It is possible to have two unequal sequences with the same elements.</remarks>
        public bool Equals(RandomAccessSequence<T> other) {
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
            if (obj.GetType() != typeof(RandomAccessSequence<T>)) return false;
            return Equals((RandomAccessSequence<T>)obj);
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
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(RandomAccessSequence<T> left, RandomAccessSequence<T> right) {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(RandomAccessSequence<T> left, RandomAccessSequence<T> right) {
            return !left.Equals(right);
        }

        /// <summary>
        /// Prepends <paramref name="item"/> to <paramref name="seq"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="seq">The sequence.</param>
        public static RandomAccessSequence<T> operator |(T item, RandomAccessSequence<T> seq) {
            return seq.Prepend(item);
        }

        /// <summary>
        /// Appends <paramref name="item"/> to <paramref name="seq"/>.
        /// </summary>
        /// <param name="seq">The sequence.</param>
        /// <param name="item">The item.</param>
        public static RandomAccessSequence<T> operator |(RandomAccessSequence<T> seq, T item) {
            return seq.Append(item);
        }

        /// <summary>
        /// Concatenates <paramref name="seq1"/> and <paramref name="seq2"/>.
        /// </summary>
        /// <param name="seq1">The first sequence.</param>
        /// <param name="seq2">The second sequence.</param>
        public static RandomAccessSequence<T> operator +(
            RandomAccessSequence<T> seq1, RandomAccessSequence<T> seq2) {
            return seq1.Concat(seq2);
        }
    }

    /// <summary>
    /// Utility methods for creating <see cref="RandomAccessSequence{T}"/>.
    /// </summary>
    /// <seealso cref="RandomAccessSequence{T}"/>
    public static class RandomAccessSequence {
        /// <summary>
        /// Returns an empty <see cref="RandomAccessSequence{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        public static RandomAccessSequence<T> Empty<T>() {
            return RandomAccessSequence<T>.Empty;
        }

        /// <summary>
        /// Returns a <see cref="RandomAccessSequence{T}"/> with a single element.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="item">The only item in the sequence.</param>
        public static RandomAccessSequence<T> Single<T>(T item) {
            return RandomAccessSequence<T>.Single(item);
        }

        /// <summary>
        /// Returns a <see cref="RandomAccessSequence{T}"/> containing the elements in <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="sequence">The sequence.</param>
        public static RandomAccessSequence<T> FromEnumerable<T>(IEnumerable<T> sequence) {
            return new RandomAccessSequence<T>(sequence);
        }
    }
}
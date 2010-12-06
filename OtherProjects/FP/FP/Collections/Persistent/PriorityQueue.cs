/*
* PriorityQueue.cs is part of functional-dotnet project
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FP.Core;
using FP.Validation;

namespace FP.Collections.Persistent {
    /// <summary>
    /// A finger-tree-based priority queue.
    /// </summary>
    /// <typeparam name="T">The type of the values in the queue.</typeparam>
    /// <typeparam name="TPriority">The type of the priority.</typeparam>
    /// <remarks>Do not use the default constructor.</remarks>
    [Serializable]
    public struct PriorityQueue<T, TPriority> : 
        IReversibleEnumerable<PriorityValuePair<T, TPriority>>, IEquatable<PriorityQueue<T, TPriority>>,
        IFoldable<PriorityValuePair<T, TPriority>>
        where T : IComparable<T>
        where TPriority : IComparable<TPriority> {
        private static readonly PriorityQueue<T, TPriority> _emptyInstance =
            new PriorityQueue<T, TPriority>(FingerTreeOrdered<PriorityValuePair<T, TPriority>, TPriority>.EmptyInstance);

        /// <summary>
        /// Gets the empty priority queue.
        /// </summary>
        /// <value>The empty instance.</value>
        public static PriorityQueue<T, TPriority> EmptyInstance { get { return _emptyInstance; } }

        private readonly FingerTreeOrdered<PriorityValuePair<T, TPriority>, TPriority> _ft;

        private PriorityQueue(FingerTreeOrdered<PriorityValuePair<T, TPriority>, TPriority> ft) {
            _ft = ft;
        }

        internal static PriorityQueue<T, TPriority> Single(PriorityValuePair<T, TPriority> item) {
            return new PriorityQueue<T, TPriority>(
                FingerTreeOrdered<PriorityValuePair<T, TPriority>, TPriority>.MakeSingle(item));
        }

        /// <summary>
        /// Gets the <see cref="PriorityValuePair{T,TPriority}"/> with the maximal
        /// priority.
        /// </summary>
        /// <remarks>O(1). If several priorities are equal and maximal, ties may be broken
        /// arbitrarily.</remarks>
        /// <exception cref="EmptyEnumerableException">There are no values in the queue.
        /// </exception>
        public PriorityValuePair<T, TPriority> MaxPriority {
            get { return _ft.Last; }
        }

        /// <summary>
        /// Gets the <see cref="PriorityValuePair{T,TPriority}"/> with the minimal
        /// priority.
        /// </summary>
        /// <remarks>O(1). If several priorities are equal and maximal, ties may be broken
        /// arbitrarily.</remarks>
        /// <exception cref="EmptyEnumerableException">There are no values in the queue.
        /// </exception>
        public PriorityValuePair<T, TPriority> MinPriority {
            get { return _ft.Head; }
        }

        /// <summary>
        /// Gets the queue with the maximal priority 
        /// <see cref="PriorityValuePair{T,TPriority}"/> removed.
        /// </summary>
        /// <remarks>O(1). If several priorities are equal and maximal, the pair returned by 
        /// <see cref="MaxPriority"/> will be removed.</remarks>
        /// <exception cref="EmptyEnumerableException">There are no values in the queue.
        /// </exception>
        public PriorityQueue<T, TPriority> ExceptMaxPriority {
            get { return new PriorityQueue<T, TPriority>(_ft.Init); }
        }

        /// <summary>
        /// Gets the queue with the minimal priority 
        /// <see cref="PriorityValuePair{T,TPriority}"/> removed.
        /// </summary>
        /// <remarks>O(1). If several values are equal and minimal, the pair returned by 
        /// <see cref="MinPriority"/> will be removed.</remarks>
        /// <exception cref="EmptyEnumerableException">There are no values in the
        /// queue.</exception>
        public PriorityQueue<T, TPriority> ExceptMinPriority {
            get { return new PriorityQueue<T, TPriority>(_ft.Tail); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to
        /// iterate through the collection.
        /// </returns>
        public IEnumerator<PriorityValuePair<T, TPriority>> GetEnumerator() {
            foreach (var pvPair in _ft)
                yield return pvPair;
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
        public bool Equals(PriorityQueue<T, TPriority> other) {
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
            if (obj.GetType() != typeof(PriorityQueue<T, TPriority>)) return false;
            return Equals((PriorityQueue<T, TPriority>)obj);
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
        /// Returns the union of <paramref name="queue1"/> and <paramref name="queue2"/>.
        /// </summary>
        /// <param name="queue1">The first queue.</param>
        /// <param name="queue2">The second queue.</param>
        /// <remarks>O(m * log (n / m)), where m is the size of the shorter queue.
        /// O(log m) in case max(queue1) &lt;= min(queue2) or vice versa.</remarks>
        public static PriorityQueue<T, TPriority> operator +(PriorityQueue<T, TPriority> queue1, PriorityQueue<T, TPriority> queue2) {
            return queue1.Union(queue2);
        }

        /// <summary>
        /// Determines whether two queues are equal.
        /// </summary>
        /// <param name="left">The left queue.</param>
        /// <param name="right">The right queue.</param>
        /// <returns>Whether the queues are equal.</returns>
        public static bool operator ==(PriorityQueue<T, TPriority> left, PriorityQueue<T, TPriority> right) {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left queue.</param>
        /// <param name="right">The right queue.</param>
        /// <returns>Whether the queues are not equal.</returns>
        public static bool operator !=(PriorityQueue<T, TPriority> left, PriorityQueue<T, TPriority> right) {
            return !left.Equals(right);
        }

        /// <summary>
        /// Inserts <paramref name="priorityValuePair"/> into <paramref name="seq"/>.
        /// </summary>
        /// <param name="seq">The queue.</param>
        /// <param name="priorityValuePair">The priority-value pair.</param>
        /// <remarks>O(log(n))</remarks>
        public static PriorityQueue<T, TPriority> operator |(PriorityQueue<T, TPriority> seq, PriorityValuePair<T, TPriority> priorityValuePair) {
            return seq.Insert(priorityValuePair);
        }

        /// <summary>
        /// Gets a value indicating whether this queue is empty.
        /// </summary>
        /// <value><c>true</c> if this queue is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty {
            get { return _ft.IsEmpty; }
        }

        /// <summary>
        ///  Returns an iterator which yields all values of the queue in the reverse order.
        /// </summary>
        /// <remarks>
        /// This should always be equivalent to, but faster than, 
        /// <code>
        ///  AsEnumerable().Reverse();
        /// </code>
        /// </remarks>
        public IEnumerable<PriorityValuePair<T, TPriority>> ReverseIterator() {
            return _ft.ReverseIterator();
        }

        /// <summary>
        /// Determines whether the queue contains a value with the specified priority.
        /// </summary>
        /// <param name="priority">The item.</param>
        /// <returns>
        /// <c>true</c> if the queue contains a value with the given priority;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>O(log(m)), where m is the number of values from the closer side of the
        /// queue to the position of the priority.</remarks>
        public bool Contains(TPriority priority) {
            return _ft[priority].HasValue;
        }

        /// <summary>
        /// Gets a value with priority <paramref name="priority"/> if there is one;
        /// otherwise, <see cref="Optional{T}.None"/>.
        /// </summary>
        /// <param name="priority">The item.</param>
        /// <remarks>O(log(m)), where m is the number of values from the closer side of
        /// the queue to the position of the priority.</remarks>
        public Optional<T> this[TPriority priority] {
            get { return _ft[priority].Map(el => el.Value); }
        }

        /// <summary>
        /// Finds all elements with the given priority.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <remarks>O(log(m)), where m is the number of values from the closer side of
        /// the queue to the position of the priority.</remarks>
        public PriorityQueue<T, TPriority> FindAll(TPriority priority) {
            return new PriorityQueue<T, TPriority>(_ft.FindAll(priority));
        }

        /// <summary>
        /// Splits the queue into three subqueues. The first one contains all values
        /// with priority less than <paramref cref="priority"/>, the second one all values
        /// with equal priority, and the third one values with greater priorities.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <remarks>O(log(m)), where m is the number of elements from the closer side of
        /// the queue to the position of the priority.</remarks>
		public FP.Core.Tuple<PriorityQueue<T, TPriority>, PriorityQueue<T, TPriority>, PriorityQueue<T, TPriority>> ExtractAll(TPriority priority)
		{
            var triple = _ft.ExtractAll(priority);
			return FP.Core.Tuple.New(
                new PriorityQueue<T, TPriority>(triple.Item1),
                new PriorityQueue<T, TPriority>(triple.Item2),
                new PriorityQueue<T, TPriority>(triple.Item3));
        }

        /// <summary>
        /// Inserts the specified priority-value pair into the queue.
        /// </summary>
        /// <param name="priorityValuePair">The priority-value pair.</param>
        /// <remarks>O(log n).</remarks>
        public PriorityQueue<T, TPriority> Insert(PriorityValuePair<T, TPriority> priorityValuePair) {
            return new PriorityQueue<T, TPriority>(_ft.Insert(priorityValuePair));
        }

        /// <summary>
        /// Inserts the specified value with the specified priority into the queue.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="priority">The priority.</param>
        public PriorityQueue<T, TPriority> Insert(T value, TPriority priority) {
            return Insert(new PriorityValuePair<T, TPriority>(value, priority));
        }

        /// <summary>
        /// Inserts all specified priority-value pairs into the queue.
        /// </summary>
        /// <param name="priorityValuePairs">The priority-value pairs.</param>
        /// <remarks>O(log max(n, m)), where m is the number of values in 
        /// <paramref name="priorityValuePairs"/>.</remarks>
        public PriorityQueue<T, TPriority> Insert(params PriorityValuePair<T, TPriority>[] priorityValuePairs) {
            return InsertRange(priorityValuePairs.AsEnumerable());
        }

        /// <summary>
        /// Inserts all specified priority-value pairs into the queue.
        /// </summary>
        /// <param name="priorityValuePairs">The priority-value pairs.</param>
        /// <remarks>O(log max(n, m)), where m is the number of values in 
        /// <paramref name="priorityValuePairs"/>.</remarks>
        public PriorityQueue<T, TPriority> InsertRange(IEnumerable<PriorityValuePair<T, TPriority>> priorityValuePairs) {
            return new PriorityQueue<T, TPriority>(_ft.InsertRange(priorityValuePairs));
        }

        /// <summary>
        /// Returns the union of the queue with specified other queue.
        /// </summary>
        /// <param name="otherqueue">The other queue.</param>
        public PriorityQueue<T, TPriority> Union(PriorityQueue<T, TPriority> otherqueue) {
            return new PriorityQueue<T, TPriority>(_ft.Merge(otherqueue._ft));
        }

        /// <summary>
        /// Intersects the queue with <paramref name="otherQueue"/>.
        /// </summary>
        /// <param name="otherQueue">The other queue.</param>
        public PriorityQueue<T, TPriority> Intersect(PriorityQueue<T, TPriority> otherQueue) {
            return new PriorityQueue<T, TPriority>(_ft.Intersect(otherQueue._ft));
        }

        /// <summary>
        /// Splits the queue into two subqueues. The first one contains all values
        /// with priority less than <paramref cref="priority"/>, the second one all values
        /// with equal priority, and the third one values with greater priorities.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="equalGoLeft">if set to <c>true</c>, values with the priority
        /// equal to <see cref="priority"/> will be at the left side of the split; otherwise,
        /// they will be on the right side.</param>
		public FP.Core.Tuple<PriorityQueue<T, TPriority>, PriorityQueue<T, TPriority>> Split(TPriority priority, bool equalGoLeft)
		{
            var pair = _ft.Split(priority, equalGoLeft);
            return Pair.New(
                new PriorityQueue<T, TPriority>(pair.Item1),
                new PriorityQueue<T, TPriority>(pair.Item2));
        }

        /// <summary>
        /// Returns all values with priority at most <paramref name="priority"/>.
        /// </summary>
        /// <param name="priority">The priority.</param>
        public PriorityQueue<T, TPriority> AtMost(TPriority priority) {
            return new PriorityQueue<T, TPriority>(_ft.AtMost(priority));
        }

        /// <summary>
        /// Returns all values with priority at least <paramref name="priority"/>.
        /// </summary>
        /// <param name="priority">The priority.</param>
        public PriorityQueue<T, TPriority> AtLeast(TPriority priority) {
            return new PriorityQueue<T, TPriority>(_ft.AtLeast(priority));
        }

        /// <summary>
        /// Returns all values with priority less than <paramref name="priority"/>.
        /// </summary>
        /// <param name="priority">The priority.</param>
        public PriorityQueue<T, TPriority> LessThan(TPriority priority) {
            return new PriorityQueue<T, TPriority>(_ft.LessThan(priority));
        }

        /// <summary>
        /// Returns all values with priority greater than <paramref name="priority"/>.
        /// </summary>
        /// <param name="priority">The priority.</param>
        public PriorityQueue<T, TPriority> GreaterThan(TPriority priority) {
            return new PriorityQueue<T, TPriority>(_ft.GreaterThan(priority));
        }

        /// <summary>
        /// Reduces the queue in descending order.
        /// </summary>
        /// <typeparam name="A">The type of the accumulator.</typeparam>
        /// <param name="binOp">The binary operation.</param>
        /// <param name="initial">The initial accumulator value.</param>
        /// <returns>
        /// The final accumulator value.
        /// </returns>
        public A FoldRight<A>(Func<PriorityValuePair<T, TPriority>, A, A> binOp, A initial) {
            return _ft.FoldRight(binOp, initial);
        }

        /// <summary>
        /// Reduces the queue in ascending order.
        /// </summary>
        /// <typeparam name="A">The type of the accumulator.</typeparam>
        /// <param name="binOp">The binary operation.</param>
        /// <param name="initial">The initial accumulator value.</param>
        /// <returns>
        /// The final accumulator value.
        /// </returns>
        public A FoldLeft<A>(Func<A, PriorityValuePair<T, TPriority>, A> binOp, A initial) {
            return _ft.FoldLeft(binOp, initial);
        }
        }

    /// <summary>
    /// Utility methods for creating <see cref="OrderedSequence{T}"/>.
    /// </summary>
    /// <seealso cref="OrderedSequence{T}"/>
    public static class PriorityQueue {
        /// <summary>
        /// Returns the empty <see cref="OrderedSequence{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="TPriority">The type of the priority.</typeparam>
        public static PriorityQueue<T, TPriority> Empty<T, TPriority>()
            where T : IComparable<T> 
            where TPriority : IComparable<TPriority> {
            return PriorityQueue<T, TPriority>.EmptyInstance;
        }

        /// <summary>
        /// Returns the <see cref="OrderedSequence{T}"/> containing only the specified 
        /// <paramref name="priorityValuePair"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
        /// <param name="priorityValuePair">The item.</param>
        /// <typeparam name="TPriority">The type of the priority.</typeparam>
        public static PriorityQueue<T, TPriority> Single<T, TPriority>(PriorityValuePair<T, TPriority> priorityValuePair) 
            where T : IComparable<T> where TPriority : IComparable<TPriority> {
            return PriorityQueue<T, TPriority>.Single(priorityValuePair);
        }

        /// <summary>
        /// Returns the <see cref="OrderedSequence{T}"/> containing only the specified
        /// <paramref name="value"/> with the specified <paramref name="priority"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="TPriority">The type of the priority.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="priority">The priority.</param>
        public static PriorityQueue<T, TPriority> Single<T, TPriority>(T value, TPriority priority)
            where T : IComparable<T>
            where TPriority : IComparable<TPriority> {
            return PriorityQueue<T, TPriority>.Single(new PriorityValuePair<T, TPriority>(value, priority));
        }

        /// <summary>
        /// Creates a <see cref="PriorityQueue{T,TPriority}"/> with the elements from 
        /// <paramref name="priorityValuePairs"/>.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <typeparam name="TPriority">The type of the priorities.</typeparam>
        /// <param name="priorityValuePairs">The sequence of priority-value pairs placed
        /// into the queue initially.
        /// </param>
        /// <remarks>If <paramref name="priorityValuePairs"/> is ordered by priorities
        /// (either descending or ascending), the running time is O(n); otherwise it is O(n
        /// * log n)</remarks>
        public static PriorityQueue<T, TPriority> FromEnumerable<T, TPriority>(IEnumerable<PriorityValuePair<T, TPriority>> priorityValuePairs) 
            where T : IComparable<T> where TPriority : IComparable<TPriority> {
            return Empty<T, TPriority>().InsertRange(priorityValuePairs);
        }
    }
}
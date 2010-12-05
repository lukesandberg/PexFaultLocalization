/*
* FingerTree.cs is part of functional-dotnet project
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
using FP.HaskellNames;
using FP.Util;
using FP.Validation;

// TODO: performance test

// ReSharper disable RedundantThisQualifier
namespace FP.Collections.Persistent {
    /// <summary>
    /// <para>
    /// A finger tree (see Ralf Hinze and Ross Paterson, "Finger trees: a simple
    /// general-purpose data structure", Journal of Functional Programming 16:2 (2006)
    /// pp 197-217). Implements adding and removing elements at both ends (the deque
    /// operations), concatenation, and splitting efficiently. </para>
    /// </summary>
    /// <typeparam name="T">The type of the leaves.</typeparam>
    /// <typeparam name="V">The type of measure annotations.</typeparam>
    [Serializable]
    internal abstract class FingerTree<T, V> : IEquatable<FingerTree<T, V>>,
                                               IDeque<T, FingerTree<T, V>>,
                                               IMeasured<V>, IFoldable<T>,
                                               ICatenable<FingerTree<T, V>> where T : IMeasured<V> {
        /// <summary>
        /// The monoid to be used to combine the measures of values.
        /// </summary>
        public readonly Monoid<V> MeasureMonoid;

        public Func<V, V, V> Plus { get { return MeasureMonoid.Plus; } }

        public V Zero { get { return MeasureMonoid.Zero; } }

        /// <summary>
        /// Gets the measure of the tree.
        /// </summary>
        /// <value>The measure.</value>
        public abstract V Measure { get; } // Measure

        private static readonly Dictionary<Monoid<V>, FingerTree<T, V>> _emptyInstancesCache = 
            new Dictionary<Monoid<V>, FingerTree<T, V>>();

        internal FingerTree(Monoid<V> measureMonoid) {
            MeasureMonoid = measureMonoid;
        }

        internal FingerTree<T, V> EmptyInstance {
            get {
                return GetEmptyFromCache<T>();
            }
        } // EmptyInstance

        internal FingerTree<FTNode<T, V>, V> EmptyInstanceNested {
            get {
                return GetEmptyFromCache<FTNode<T, V>>();
            }
        } // EmptyInstanceNested

        private FingerTree<T1, V> GetEmptyFromCache<T1>() where T1 : IMeasured<V> {
            return GetEmptyFromCache<T1>(MeasureMonoid);
        } // GetEmptyFromCache

        internal static FingerTree<T1, V> GetEmptyFromCache<T1>(Monoid<V> measureMonoid) where T1 : IMeasured<V> {
            FingerTree<T1, V> instance;
            var emptyInstancesCache = FingerTree<T1, V>._emptyInstancesCache;
            lock (emptyInstancesCache) {
                if (!emptyInstancesCache.TryGetValue(measureMonoid, out instance))
                    instance = emptyInstancesCache[measureMonoid] = new FingerTree<T1, V>.Empty(measureMonoid);
            }
            return instance;
        } // GetEmptyFromCache

        private V SumMeasures(IEnumerable<T> sequence) {
            return SumMeasures(Zero, sequence);
        }

        private V SumMeasures(V init, IEnumerable<T> sequence) {
            return FingerTree.SumMeasures(MeasureMonoid, init, sequence);
        }

        private Single MakeSingle(T value) {
            return new Single(value, MeasureMonoid);
        } // MakeSingle

        private Deep MakeDeep(T[] left, Func<FingerTree<FTNode<T, V>, V>> func, T[] right, V measure) {
            return MakeDeep(left, Lazy.New(func), right, measure);
        } // MakeDeep

		private Deep MakeDeep(T[] left, FP.Core.Lazy<FingerTree<FTNode<T, V>, V>> middle, T[] right, V measure)
		{
            return new Deep(left, middle, right, measure, MeasureMonoid);
        } // MakeDeep

		private Deep MakeDeepForceMiddle(T[] left, FP.Core.Lazy<FingerTree<FTNode<T, V>, V>> middle, T[] right)
		{
            return new Deep(left, middle, right, MeasureMonoid);
        } // MakeDeepForceMiddle

        private FingerTree<T, V> DeepL(T[] left, FingerTree<FTNode<T, V>, V> middle, T[] right) {
            Debug.Assert(left.Length <= 4);
            Debug.Assert(right.Length != 0 && right.Length <= 4);

            if (left.Length != 0)
                return MakeDeepForceMiddle(left, middle, right);

            return RotateL(middle, right);
        } // DeepL

        private FingerTree<T, V> RotateL(FingerTree<FTNode<T, V>, V> middle, T[] right) {
            if (middle.IsEmpty)
                return FromArray(right);
            V measure = SumMeasures(middle.Measure, right);
            return MakeDeep(middle.Head.AsArray, () => middle.Tail, right, measure);
        } // RotateL

        private FingerTree<T, V> DeepR(T[] left, FingerTree<FTNode<T, V>, V> middle, T[] right) {
            Debug.Assert(left.Length != 0 && left.Length <= 4);
            Debug.Assert(right.Length <= 4);

            if (right.Length != 0)
                return MakeDeepForceMiddle(left, middle, right);

            return RotateR(left, middle);
        } // DeepR

        private FingerTree<T, V> RotateR(T[] left, FingerTree<FTNode<T, V>, V> middle) {
            if (middle.IsEmpty)
                return FromArray(left);
            V measure = middle.PrependMeasure(SumMeasures(left));
            return MakeDeep(left, () => middle.Init, middle.Last.AsArray, measure);
        }

        /// <summary>
        /// Creates the tree from the specified sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the tree.</typeparam>
        /// <typeparam name="V">The type of the measure values.</typeparam>
        /// <param name="array">The small array.</param>
        /// <remarks>Calls <see cref="FingerTree.FromEnumerable{T,V}"/> when <paramref name="array"/>'s 
        /// length is more than 6.</remarks>
        private FingerTree<T, V> FromArray(T[] array) {
            switch (array.Length) {
                case 0:
                    return EmptyInstance;
                case 1:
                    return MakeSingle(array[0]);
                default:
                    var emptyNested =
                        (FingerTree<FTNode<T, V>, V>)FingerTree.Empty<FTNode<T, V>, V>(MeasureMonoid);
                    switch (array.Length) {
                        case 2:
                            return MakeDeepForceMiddle(
                                new[] { array[0] }, emptyNested, new[] { array[1] });
                        case 3:
                            return MakeDeepForceMiddle(
                                new[] { array[0], array[1] }, emptyNested, new[] { array[2] });
                        case 4:
                            return MakeDeepForceMiddle(
                                new[] { array[0], array[1] }, emptyNested, new[] { array[2], array[3] });
                        case 5:
                            return MakeDeepForceMiddle(
                                new[] { array[0], array[1], array[2] }, emptyNested, new[] { array[3], array[4] });
                        case 6:
                            return MakeDeepForceMiddle(
                                new[] { array[0], array[1], array[2] }, emptyNested, new[] { array[3], array[4], array[5] });
                        default:
                            return FingerTree.FromEnumerable(array, MeasureMonoid);
                    }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{T}"/> object that can be used to iterate through the collection.
        /// </returns>
        public abstract IEnumerator<T> GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        } // IEnumerable.GetEnumerator

        public abstract IEnumerable<T> ReverseIterator();

        /// <summary>
        /// Gets the head of the list, provided it is not empty.
        /// </summary>
        /// <value>The head.</value>
        /// <exception cref="EmptyEnumerableException">if the list is empty.</exception>
        public abstract T Head { get; } // Head

        /// <summary>
        /// Gets the tail of the list, provided it is not empty.
        /// </summary>
        /// <value>The tail.</value>
        /// <exception cref="EmptyEnumerableException">if the list is empty.</exception>
        public abstract FingerTree<T, V> Tail { get; } // Tail

        /// <summary>
        /// Gets the initial sublist (all elements but the last) of the list, provided it is not empty.
        /// </summary>
        /// <value>The last element.</value>
        /// <exception cref="EmptyEnumerableException">if the list is empty.</exception>
        public abstract FingerTree<T, V> Init { get; } // Init

        /// <summary>
        /// Gets the last element of the list, provided it is not empty.
        /// </summary>
        /// <value>The last element of the list.</value>
        /// <exception cref="EmptyEnumerableException">if the list is empty.</exception>
        public abstract T Last { get; } // Last

        /// <summary>
        /// Gets a value indicating whether this list is empty.
        /// </summary>
        /// <value><c>true</c> if this list is empty; otherwise, <c>false</c>.</value>
        public abstract bool IsEmpty { get; }

        protected abstract bool IsSingle { get; }

        /// <summary>
        /// Appends the sequence of elements to the end of the tree.
        /// </summary>
        /// <param name="ts">The sequence.</param>
        public FingerTree<T, V> AppendRange(IEnumerable<T> ts) {
            return ts.FoldLeft((tree, a) => tree | a, this);
        } // AppendRange

        /// <summary>
        /// Prepends the sequence of elements to the beginning of the tree.
        /// </summary>
        /// <param name="ts">The sequence.</param>
        public FingerTree<T, V> PrependRange(IEnumerable<T> ts) {
            return ts.FoldRight((a, tree) => a | tree, this);
        } // PrependRange

        /// <summary>
        /// Prepends the specified element to the beginning of the list.
        /// </summary>
        /// <param name="newHead">The new head.</param>
        /// <returns>The resulting list.</returns>
        public abstract FingerTree<T, V> Prepend(T newHead);

        /// <summary>
        /// Appends the specified element to the end of the list.
        /// </summary>
        /// <param name="newLast">The new last element.</param>
        /// <returns>The resulting list.</returns>
        public abstract FingerTree<T, V> Append(T newLast);

        /// <summary>
        /// Concatenates the tree with another tree.
        /// </summary>
        /// <param name="otherTree">Another tree.</param>
        /// <returns>The result of concatenation.</returns>
        public abstract FingerTree<T, V> Concat(FingerTree<T, V> otherTree);

        protected abstract FingerTree<T, V> App3(IEnumerable<T> middleList, FingerTree<T, V> rightTree);

        /// <summary>
        /// Prepends the measure.
        /// </summary>
        /// <param name="prependedMeasure">The prepended measure.</param>
        /// <remarks>Used in order not to rely on <see cref="MeasureMonoid"/>'s Zero being
        /// the right identity.</remarks>
        protected virtual V PrependMeasure(V prependedMeasure) {
            return Plus(prependedMeasure, Measure);
        }

        protected abstract Split<T, FingerTree<T, V>> SplitTree(Func<V, bool> predicate, V initial, bool needLeft, bool needRight);

        /// <summary>
        /// Splits the list according to the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <remarks>The result has the following properties.
        /// <code>
        /// var left = tree.Split(predicate).Item1;
        /// var right = tree.Split(predicate).Item2;
        /// ---------
        /// tree.SequenceEquals(left + right);
        /// left.IsEmpty() || !predicate(left.Measure);
        /// right.IsEmpty() || predicate(left.Measure + right.Head.Measure);
        /// </code>
        /// If there are several possible splits for which these properties hold,
        /// any of them may be returned.
        /// </remarks>
		public virtual FP.Core.Tuple<FingerTree<T, V>, FingerTree<T, V>> Split(Func<V, bool> predicate, bool needLeft, bool needRight)
		{
            if (!predicate(Measure)) return
                Pair.New(needLeft ? this : EmptyInstance, EmptyInstance);
            var split = SplitTree(predicate, Zero, needLeft, needRight);
            return Pair.New(split.Left, needRight ? (split.Pivot | split.Right) : EmptyInstance);
        } // Split

        /// <summary>
        /// Prepends <paramref name="item"/> to <paramref name="tree"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tree">The tree.</param>
        public static FingerTree<T, V> operator |(T item, FingerTree<T, V> tree) {
            return tree.Prepend(item);
        } // op_BitwiseOr

        /// <summary>
        /// Appends <paramref name="item"/> to <paramref name="tree"/>.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="item">The item.</param>
        public static FingerTree<T, V> operator |(FingerTree<T, V> tree, T item) {
            return tree.Append(item);
        } // op_BitwiseOr

        /// <summary>
        /// Concatenates <paramref name="tree1"/> and <paramref name="tree2"/>.
        /// </summary>
        /// <param name="tree1">The tree1.</param>
        /// <param name="tree2">The tree2.</param>
        public static FingerTree<T, V> operator +(FingerTree<T, V> tree1, FingerTree<T, V> tree2) {
            return tree1.Concat(tree2);
        } // op_Addition

        /// <summary>
        /// Reduces the finger tree from the right.
        /// </summary>
        /// <typeparam name="A">The type of the accumulator.</typeparam>
        /// <param name="binOp">The binary operation.</param>
        /// <param name="initial">The initial accumulator value.</param>
        /// <returns>
        /// The final accumulator value.
        /// </returns>
        public abstract A FoldRight<A>(Func<T, A, A> binOp, A initial);

        /// <summary>
        /// Reduces the finger tree from the left.
        /// </summary>
        /// <typeparam name="A">The type of the accumulator.</typeparam>
        /// <param name="binOp">The binary operation.</param>
        /// <param name="initial">The initial accumulator value.</param>
        /// <returns>
        /// The final accumulator value.
        /// </returns>
        public abstract A FoldLeft<A>(Func<A, T, A> binOp, A initial);

        public abstract bool Equals(FingerTree<T, V> other);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this
        /// instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this
        /// instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this
        /// instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var ft = obj as FingerTree<T, V>;
            if (ft == null) return false;
            return Equals(ft);
        } // Equals

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract override int GetHashCode();

        /// <summary>
        /// Determines whether two instances of <see cref="FingerTree{T, V}"/> are equal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// <c>true</c> if the arguments are equal; <c>false</c> otherwise.
        /// </returns>
        public static bool operator ==(FingerTree<T, V> left, FingerTree<T, V> right) {
            return Equals(left, right);
        } // op_Equality

        /// <summary>
        /// Determines whether two instances of <see cref="FingerTree{T, V}"/> are unequal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// <c>true</c> if the arguments are not equal; <c>false</c> otherwise.
        /// </returns>
        public static bool operator !=(FingerTree<T, V> left, FingerTree<T, V> right) {
            return !Equals(left, right);
        } // op_Inequality

        /// <summary>
        /// An empty <see cref="FingerTree{T,V}"/>.
        /// </summary>
        [DebuggerDisplay("Empty")]
        [Serializable]
        public sealed class Empty : FingerTree<T, V> {
            internal Empty(Monoid<V> measureMonoid) : base(measureMonoid) { } // Empty

            /// <summary>
            /// Gets the measure of the tree.
            /// </summary>
            /// <value>The measure.</value>
            public override V Measure {
                get { return Zero; }
            } // Measure

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="IEnumerator{T}"/> object that can be used to iterate through the collection.
            /// </returns>
            public override IEnumerator<T> GetEnumerator() {
                yield break;
            }

            public override IEnumerable<T> ReverseIterator() {
                yield break;
            }

            /// <summary>
            /// Gets the head of the list.
            /// </summary>
            /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
            /// <exception cref="EmptyEnumerableException"></exception>
            public override T Head {
                get { throw new EmptyEnumerableException(); }
            } // Head

            /// <summary>
            /// Gets the tail of the list.
            /// </summary>
            /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
            /// <exception cref="EmptyEnumerableException"></exception>
            public override FingerTree<T, V> Tail {
                get { throw new EmptyEnumerableException(); }
            } // Tail

            /// <summary>
            /// Gets the initial sublist (all elements but the last) of the list.
            /// </summary>
            /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
            /// <exception cref="EmptyEnumerableException"></exception>
            public override FingerTree<T, V> Init {
                get { throw new EmptyEnumerableException(); }
            } // Init

            /// <summary>
            /// Gets the last element of the list.
            /// </summary>
            /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
            /// <exception cref="EmptyEnumerableException"></exception>
            public override T Last {
                get { throw new EmptyEnumerableException(); }
            } // Last

            /// <summary>
            /// Gets a value indicating whether this list is empty.
            /// </summary>
            /// <value><c>true</c>.</value>
            public override bool IsEmpty {
                get { return true; }
            }

            protected override bool IsSingle {
                get { return false; }
            }

            /// <summary>
            /// Prepends the specified element to the beginning of the list.
            /// </summary>
            /// <param name="newHead">The new head.</param>
            /// <returns>The resulting list.</returns>
            public override FingerTree<T, V> Prepend(T newHead) {
                return MakeSingle(newHead);
            } // Prepend

            /// <summary>
            /// Appends the specified element to the end of the list.
            /// </summary>
            /// <param name="newLast">The new last element..</param>
            /// <returns>The resulting list.</returns>
            public override FingerTree<T, V> Append(T newLast) {
                return MakeSingle(newLast);
            }

            /// <summary>
            /// Concatenates the tree with another.
            /// </summary>
            /// <param name="otherTree">Another tree.</param>
            /// <returns>The result of concatenation.</returns>
            public override FingerTree<T, V> Concat(FingerTree<T, V> otherTree) {
                return otherTree;
            }

            protected override FingerTree<T, V> App3(IEnumerable<T> middleList, FingerTree<T, V> rightTree) {
                if (rightTree.IsEmpty) return this.AppendRange(middleList);
                return rightTree.PrependRange(middleList);
            }

            protected override V PrependMeasure(V prependedMeasure) {
                return prependedMeasure;
            }

            /// <summary>
            /// Splits the tree.
            /// </summary>
            /// <param name="predicate">The predicate.</param>
            /// <param name="initial">The initial.</param>
            /// <exception cref="EmptyEnumerableException">Empty tree can't be split.</exception>
            protected override Split<T, FingerTree<T, V>> SplitTree(Func<V, bool> predicate, V initial, bool needLeft, bool needRight) {
                throw new EmptyEnumerableException("Empty tree can't be split");
            } // SplitTree

            /// <summary>
            /// Splits the list according to the specified predicate. The result has the following properties.
            /// <code>
            /// var left = tree.Split(predicate).Item1;
            /// var right = tree.Split(predicate).Item2;
            /// ---------
            /// tree.SequenceEquals(left + right);
            /// left.IsEmpty() || !predicate(left.TotalMeasure);
            /// right.IsEmpty() || !predicate(left.TotalMeasure + right.Head.Measure);
            /// </code>
            /// If there are several splits, the split returned is not guaranteed to be the first one!
            /// </summary>
            /// <param name="predicate">The predicate.</param>
			public override FP.Core.Tuple<FingerTree<T, V>, FingerTree<T, V>> Split(Func<V, bool> predicate, bool needLeft, bool needRight)
			{
                var empty = (FingerTree<T, V>)this;
                return Pair.New(empty, empty);
            } // Split

            // ReverseTree

            /// <summary>
            /// Reduces the finger tree from the right.
            /// </summary>
            /// <typeparam name="A">The type of the accumulator.</typeparam>
            /// <param name="binOp">The binary operation.</param>
            /// <param name="initial">The initial accumulator value.</param>
            /// <returns>
            /// The final accumulator value.
            /// </returns>
            public override A FoldRight<A>(Func<T, A, A> binOp, A initial) {
                return initial;
            } // FoldRight

            /// <summary>
            /// Reduces the finger tree from the left.
            /// </summary>
            /// <typeparam name="A">The type of the accumulator.</typeparam>
            /// <param name="binOp">The binary operation.</param>
            /// <param name="initial">The initial accumulator value.</param>
            /// <returns>
            /// The final accumulator value.
            /// </returns>
            public override A FoldLeft<A>(Func<A, T, A> binOp, A initial) {
                return initial;
            }

            // Invariant

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public override bool Equals(FingerTree<T, V> other) {
                return !ReferenceEquals(null, other) && other.IsEmpty && other.MeasureMonoid == MeasureMonoid;
            } // Equals

            /// <summary>
            /// Serves as a hash function for a particular type. 
            /// </summary>
            /// <returns>
            /// A hash code for the current <see cref="T:System.Object" />.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override int GetHashCode() {
                return Zero.GetHashCode();
            } // GetHashCode
        } // class Empty

        /// <summary>
        /// A <see cref="FingerTree{T,V}"/> with the single element <see cref="Value"/>.
        /// </summary>
        [DebuggerDisplay("Single(Value = {Value}")]
        [Serializable]
        public sealed class Single : FingerTree<T, V> {
            /// <summary>
            /// The value of the element.
            /// </summary>
            public readonly T Value;

            internal Single(T value, Monoid<V> measureMonoid)
                : base(measureMonoid) {
                Value = value;
            } // Single

            // FoldLeft

            /// <summary>
            /// Gets the measure of the tree.
            /// </summary>
            /// <value>The measure.</value>
            public override V Measure {
                get { return Value.Measure; }
            } // Measure

            public override IEnumerator<T> GetEnumerator() {
                yield return Value;
            } // GetEnumerator

            public override IEnumerable<T> ReverseIterator() {
                yield return Value;
            }

            /// <summary>
            /// Gets the head of the list.
            /// </summary>
            /// <value><see cref="Value"/>.</value>
            public override T Head {
                get { return Value; }
            } // Head

            /// <summary>
            /// Gets the tail of the list.
            /// </summary>
            /// <value><see cref="FingerTree{T,V}.EmptyInstance"/>.</value>
            public override FingerTree<T, V> Tail {
                get { return EmptyInstance; }
            } // Tail

            /// <summary>
            /// Gets the initial sublist (all elements but the last) of the list.
            /// </summary>
            /// <value><see cref="FingerTree{T,V}.EmptyInstance"/>.</value>
            public override FingerTree<T, V> Init {
                get { return EmptyInstance; }
            } // Init

            /// <summary>
            /// Gets the last element of the list.
            /// </summary>
            /// <value><see cref="Value"/>.</value>
            public override T Last {
                get { return Value; }
            } // Last

            /// <summary>
            /// Gets a value indicating whether this list is empty.
            /// </summary>
            /// <value><c>false</c>.</value>
            public override bool IsEmpty {
                get { return false; }
            }

            protected override bool IsSingle {
                get { return true; }
            }

            /// <summary>
            /// Prepends the specified element to the beginning of the list.
            /// </summary>
            /// <param name="newHead">The new head.</param>
            /// <returns>The resulting list.</returns>
            public override FingerTree<T, V> Prepend(T newHead) {
                return MakeDeep(
                    new[] { newHead }, EmptyInstanceNested, new[] { Value },
                    Plus(newHead.Measure, Value.Measure));
            } // Prepend

            /// <summary>
            /// Appends the specified element to the end of the list.
            /// </summary>
            /// <param name="newLast">The new last element.</param>
            /// <returns>The resulting list.</returns>
            public override FingerTree<T, V> Append(T newLast) {
                return MakeDeep(
                    new[] { Value }, EmptyInstanceNested, new[] { newLast },
                    Plus(Value.Measure, newLast.Measure));
            }

            /// <summary>
            /// Concatenates the tree with another.
            /// </summary>
            /// <param name="otherTree">Another tree.</param>
            /// <returns>The result of concatenation.</returns>
            public override FingerTree<T, V> Concat(FingerTree<T, V> otherTree) {
                return Value | otherTree;
            }

            protected override FingerTree<T, V> App3(IEnumerable<T> middleList, FingerTree<T, V> rightTree) {
                if (rightTree.IsEmpty) return this.AppendRange(middleList);
                if (rightTree.IsSingle)
                    return this.AppendRange(middleList) | rightTree.Head;
                return Value | rightTree.PrependRange(middleList);
            }

            protected override Split<T, FingerTree<T, V>> SplitTree(Func<V, bool> predicate, V initial, bool needLeft, bool needRight) {
                return new Split<T, FingerTree<T, V>>(EmptyInstance, Value, EmptyInstance);
            } // SplitTree

            /// <summary>
            /// Reduces the finger tree from the right.
            /// </summary>
            /// <typeparam name="A">The type of the accumulator.</typeparam>
            /// <param name="binOp">The binary operation.</param>
            /// <param name="initial">The initial accumulator value.</param>
            /// <returns>
            /// The final accumulator value.
            /// </returns>
            public override A FoldRight<A>(Func<T, A, A> binOp, A initial) {
                return binOp(Value, initial);
            } // FoldRight

            /// <summary>
            /// Reduces the finger tree from the left.
            /// </summary>
            /// <typeparam name="A">The type of the accumulator.</typeparam>
            /// <param name="binOp">The binary operation.</param>
            /// <param name="initial">The initial accumulator value.</param>
            /// <returns>
            /// The final accumulator value.
            /// </returns>
            public override A FoldLeft<A>(Func<A, T, A> binOp, A initial) {
                return binOp(initial, Value);
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public override bool Equals(FingerTree<T, V> other) {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                if (!other.IsSingle)
                    return false;
                return Equals(other.Head, Value) && other.MeasureMonoid == MeasureMonoid;
            } // Equals

            /// <summary>
            /// Serves as a hash function for a particular type. 
            /// </summary>
            /// <returns>
            /// A hash code for the current <see cref="T:System.Object" />.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override int GetHashCode() {
                return Value.GetHashCode();
            } // GetHashCode
        } // class Single

        /// <summary>
        /// A <see cref="FingerTree{T,V}"/> with more than one element.
        /// </summary>
        [Serializable]
        public sealed class Deep : FingerTree<T, V> {
            private readonly V _measure;

            /// <summary>
            /// Gets the measure of the tree.
            /// </summary>
            /// <value>The measure.</value>
            public override V Measure {
                get {
                    return _measure;
                }
            }

            private readonly T[] _left;
            private readonly T[] _right;
			private readonly FP.Core.Lazy<FingerTree<FTNode<T, V>, V>> _middleLazy;

            private FingerTree<FTNode<T, V>, V> Middle {
                get {
                    return _middleLazy.Value;
                }
            }

			internal Deep(T[] left, FP.Core.Lazy<FingerTree<FTNode<T, V>, V>> middleLazy, T[] right,
                          Monoid<V> measureMonoid)
                : base(measureMonoid) {
                _left = left;
                _right = right;
                _middleLazy = middleLazy;
                V measure = Middle.PrependMeasure(SumMeasures(_left));
                measure = SumMeasures(measure, _right);
                _measure = measure;
                Debug.Assert(_left != null && _left.Length > 0 && _left.Length <= 4);
                Debug.Assert(_right != null && _right.Length > 0 && _right.Length <= 4);
            } // Deep

			internal Deep(T[] left, FP.Core.Lazy<FingerTree<FTNode<T, V>, V>> middleLazy, T[] right,
                          V measure, Monoid<V> measureMonoid)
                : base(measureMonoid) {
                _left = left;
                _right = right;
                _middleLazy = middleLazy;
                _measure = measure;
                Debug.Assert(_left != null && _left.Length > 0 && _left.Length <= 4);
                Debug.Assert(_right != null && _right.Length > 0 && _right.Length <= 4);
            } // Deep

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="IEnumerator{T}"/> object that can be used to iterate through the collection.
            /// </returns>
            public override IEnumerator<T> GetEnumerator() {
                foreach (var t in _left)
                    yield return t;
                // CompilerBug: doesn't compile without AsEnumerable!
                foreach (var node in Middle.AsEnumerable()) {
                    foreach (var t in node)
                        yield return t;
                }
                foreach (var t in _right)
                    yield return t;
            } // GetEnumerator

            public override IEnumerable<T> ReverseIterator() {
                foreach (var t in _right.ReverseIterator())
                    yield return t;
                foreach (var node in Middle.ReverseIterator()) {
                    foreach (var t in node.AsArray.ReverseIterator())
                        yield return t;
                }
                foreach (var t in _left.ReverseIterator())
                    yield return t;
            }

            /// <summary>
            /// Gets the head of the list, provided it is not empty.
            /// </summary>
            /// <value>The head.</value>
            /// <exception cref="EmptyEnumerableException">if the list is empty.</exception>
            public override T Head {
                get { return _left[0]; }
            } // Head

            /// <summary>
            /// Gets the tail of the list, provided it is not empty.
            /// </summary>
            /// <value>The tail.</value>
            /// <exception cref="EmptyEnumerableException">if the list is empty.</exception>
            public override FingerTree<T, V> Tail {
                get {
                    if (_left.Length == 1)
                        return RotateL(Middle, _right);
                    return MakeDeepForceMiddle(_left.CopyNoChecks(1), _middleLazy, _right);
                }
            } // Tail

            /// <summary>
            /// Gets the initial sublist (all elements but the last) of the list, provided it is not empty.
            /// </summary>
            /// <value>The last element.</value>
            /// <exception cref="EmptyEnumerableException">if the list is empty.</exception>
            public override FingerTree<T, V> Init {
                get {
                    if (_right.Length == 1)
                        return RotateR(_left, Middle);
                    return MakeDeepForceMiddle(_left, _middleLazy, _right.CopyNoChecks(0, _right.Length - 1));
                }
            } // Init

            /// <summary>
            /// Gets the last element of the list, provided it is not empty.
            /// </summary>
            /// <value>The last element of the list.</value>
            /// <exception cref="EmptyEnumerableException">if the list is empty.</exception>
            public override T Last {
                get { return _right[_right.Length - 1]; }
            } // Last

            /// <summary>
            /// Gets a value indicating whether this list is empty.
            /// </summary>
            /// <value><c>true</c>.</value>
            public override bool IsEmpty {
                get { return false; }
            }

            protected override bool IsSingle {
                get { return false; }
            }

            /// <summary>
            /// Prepends the specified element to the beginning of the list.
            /// </summary>
            /// <param name="newHead">The new head.</param>
            /// <returns>The resulting list.</returns>
            public override FingerTree<T, V> Prepend(T newHead) {
                V newMeasure = Plus(newHead.Measure, Measure);

                var leftLength = _left.Length;
                if (leftLength != 4) {
                    var newLeft = new T[leftLength + 1];
                    newLeft[0] = newHead;
                    Array.Copy(_left, 0, newLeft, 1, leftLength);
                    return MakeDeep(newLeft, _middleLazy, _right, newMeasure);
                }

                // refinement from Scala version: http://scala.sygneca.com/code/finger-trees
                // Should make repeated prepends a bit more efficient
                if (_right.Length == 1) {
                    if (Middle.IsEmpty) {
                        return MakeDeep(
                            new[] { newHead, _left[0], _left[1] },
                            EmptyInstanceNested,
                            new[] { _left[2], _left[3], _right[0] },
                            newMeasure);
                    }
                    if (Middle.IsSingle) {
                        var middle = Middle.Head.AsArray;
                        if (middle.Length == 2) {
                            return MakeDeep(
                                new[] { newHead, _left[0] },
                                MakeSingleNested(MakeNode(_left[1], _left[2], _left[3])),
                                new[] { middle[0], middle[1], _right[0] },
                                newMeasure);
                        }
                        Debug.Assert(middle.Length == 3);
                        return MakeDeep(
                            new[] { newHead, _left[0], _left[1] },
                            MakeSingleNested(MakeNode(_left[2], _left[3], middle[0])),
                            new[] { middle[1], middle[2], _right[0] },
                            newMeasure);
                    }
                }

                return MakeDeep(
                    new[] { newHead, _left[0] },
                    MakeNode(_left[1], _left[2], _left[3]) | Middle,
                    _right,
                    newMeasure);
            } // Prepend

            /// <summary>
            /// Appends the specified element to the end of the list.
            /// </summary>
            /// <param name="newLast">The new last element..</param>
            /// <returns>The resulting list.</returns>
            public override FingerTree<T, V> Append(T newLast) {
                V newMeasure = Plus(Measure, newLast.Measure);

                var rightLength = _right.Length;
                if (rightLength != 4) {
                    var newRight = new T[rightLength + 1];
                    Array.Copy(_right, 0, newRight, 0, rightLength);
                    newRight[rightLength] = newLast;
                    return MakeDeep(_left, _middleLazy, newRight, newMeasure);
                }
                
                // refinement from Scala version: http://scala.sygneca.com/code/finger-trees
                // Should make repeated appends a bit more efficient
                if (_left.Length == 1) {
                    if (Middle.IsEmpty) {
                        return MakeDeep(
                            new[] { _left[0], _right[0], _right[1] },
                            EmptyInstanceNested,
                            new[] { _right[2], _right[3], newLast },
                            newMeasure);
                    }
                    if (Middle.IsSingle) {
                        var middle = Middle.Head.AsArray;
                        if (middle.Length == 2) {
                            return MakeDeep(
                                new[] { _left[0], middle[0], middle[1] },
                                MakeSingleNested(MakeNode(_right[0], _right[1], _right[2])),
                                new[] { _right[3], newLast },
                                newMeasure);
                        }
                        Debug.Assert(middle.Length == 3);
                        return MakeDeep(
                            new[] { _left[0], middle[0], middle[1] },
                            MakeSingleNested(MakeNode(middle[2], _right[0], _right[1])),
                            new[] { _right[2], _right[3], newLast },
                            newMeasure);
                    }
                }

                return MakeDeep(
                    _left,
                    Middle | MakeNode(_right[0], _right[1], _right[2]),
                    new[] { _right[3], newLast },
                    newMeasure);
            }

            private FingerTree<FTNode<T, V>, V> MakeSingleNested(FTNode<T, V> node) {
                return new FingerTree<FTNode<T, V>, V>.Single(node, MeasureMonoid);
            }

            /// <summary>
            /// Concatenates the tree with another.
            /// </summary>
            /// <param name="otherTree">Another tree.</param>
            /// <returns>The result of concatenation.</returns>
            public override FingerTree<T, V> Concat(FingerTree<T, V> otherTree) {
                if (otherTree.IsEmpty)
                    return this;
                if (otherTree.IsSingle)
                    return this | otherTree.Head;
                return App3(Enumerable.Empty<T>(), otherTree);
            }

            protected override FingerTree<T, V> App3(IEnumerable<T> middleList, FingerTree<T, V> rightTree) {
                if (rightTree.IsEmpty) return this.AppendRange(middleList);
                if (rightTree.IsSingle) return this.AppendRange(middleList) | rightTree.Head;
                // ReSharper disable PossibleNullReferenceException
                var rightDeep = rightTree as Deep;
                V newMeasure = SumMeasures(Measure, middleList);
                newMeasure = Plus(newMeasure, rightDeep.Measure);
                return MakeDeep(
                    _left,
                    () => Middle.App3(
                              Nodes(_right.Concat(middleList).Concat(rightDeep._left)),
                              rightDeep.Middle),
                    rightDeep._right,
                    newMeasure);
                // ReSharper restore PossibleNullReferenceException
            }

            private IEnumerable<FTNode<T, V>> Nodes(IEnumerable<T> elements) {
                Debug.Assert(elements.Count() >= 2);
                var buffer = new Queue<T>(5);
                foreach (var t in elements) {
                    buffer.Enqueue(t);
                    if (buffer.Count == 5)
                        yield return MakeNode(buffer.Dequeue(), buffer.Dequeue(), buffer.Dequeue());
                } // foreach
                switch (buffer.Count) {
                    case 2:
                        yield return MakeNode(buffer.Dequeue(), buffer.Dequeue());
                        break;
                    case 3:
                        yield return MakeNode(buffer.Dequeue(), buffer.Dequeue(), buffer.Dequeue());
                        break;
                    case 4:
                        yield return MakeNode(buffer.Dequeue(), buffer.Dequeue());
                        yield return MakeNode(buffer.Dequeue(), buffer.Dequeue());
                        break;
                } // switch
            }

            private FTNode<T, V> MakeNode(T t1, T t2) {
                return new FTNode<T, V>(Plus(t1.Measure, t2.Measure), t1, t2);
            }

            private FTNode<T, V> MakeNode(T t1, T t2, T t3) {
                return new FTNode<T, V>(
                    Plus(Plus(t1.Measure, t2.Measure), t3.Measure), t1, t2, t3);
            }

            protected override Split<T, FingerTree<T, V>> SplitTree(Func<V, bool> predicate, V initial, bool needLeft, bool needRight) {
                V totalLeft = SumMeasures(initial, _left);
                // is split on the left?
                if (predicate(totalLeft)) {
                    var splitLeft = SplitArray(_left, predicate, initial, needLeft, needRight);
                    return new Split<T, FingerTree<T, V>>(
                        FromArray(splitLeft.Left),
                        splitLeft.Pivot,
                        needRight ? DeepL(splitLeft.Right, Middle, _right) : EmptyInstance);
                } // if

                V totalMiddle = Middle.PrependMeasure(totalLeft);
                // is split in the middle?
                if (predicate(totalMiddle)) {
                    var splitMiddle = Middle.SplitTree(predicate, totalLeft, true, needRight);
                    V totalLeftAndMiddleLeft = splitMiddle.Left.PrependMeasure(totalLeft);
                    var splitMiddlePivot =
                        SplitArray(splitMiddle.Pivot.AsArray, predicate, totalLeftAndMiddleLeft, needLeft, needRight);
                    return new Split<T, FingerTree<T, V>>(
                        needLeft ? DeepR(_left, splitMiddle.Left, splitMiddlePivot.Left) : EmptyInstance,
                        splitMiddlePivot.Pivot,
                        needRight ? DeepL(splitMiddlePivot.Right, splitMiddle.Right, _right) : EmptyInstance);
                } // if

                // it must be on the right
                var splitRight = SplitArray(_right, predicate, totalMiddle, needLeft, needRight);
                return new Split<T, FingerTree<T, V>>(
                    needLeft ? DeepR(_left, Middle, splitRight.Left) : EmptyInstance,
                    splitRight.Pivot,
                    FromArray(splitRight.Right));
            } // SplitTree

            private Split<T, T[]> SplitArray(T[] array, Func<V, bool> pred, V init, bool needLeft, bool needRight) {
                if (array.Length == 1) {
                    return new Split<T, T[]>(
                        Arrays.Empty<T>(), array[0], Arrays.Empty<T>());
                }

                V total = init;
                int offset;
                for (offset = 0; offset < array.Length - 1; offset++) {
                    total = Plus(total, array[offset].Measure);
                    if (pred(total)) break;
                }
                var left = needLeft
                               ? array.CopyNoChecks(0, offset)
                               : Arrays.Empty<T>();
                var right = needRight
                                ? array.CopyNoChecks(offset + 1)
                                : Arrays.Empty<T>();
                return new Split<T, T[]>(left, array[offset], right);
            }

            /// <summary>
            /// Reduces the finger tree from the right.
            /// </summary>
            /// <typeparam name="A">The type of the accumulator.</typeparam>
            /// <param name="binOp">The binary operation.</param>
            /// <param name="initial">The initial accumulator value.</param>
            /// <returns>
            /// The final accumulator value.
            /// </returns>
            public override A FoldRight<A>(Func<T, A, A> binOp, A initial) {
                Func<FTNode<T, V>, A, A> binOp1 = (n, a) => n.FoldRight(binOp, a);
                return _left.FoldRight(
                    binOp, Middle.FoldRight(binOp1, _right.FoldRight(binOp, initial)));
            } // FoldRight

            /// <summary>
            /// Reduces the finger tree from the left.
            /// </summary>
            /// <typeparam name="A">The type of the accumulator.</typeparam>
            /// <param name="binOp">The binary operation.</param>
            /// <param name="initial">The initial accumulator value.</param>
            /// <returns>
            /// The final accumulator value.
            /// </returns>
            public override A FoldLeft<A>(Func<A, T, A> binOp, A initial) {
                Func<A, FTNode<T, V>, A> binOp1 = (a, n) => n.FoldLeft(binOp, a);
                return _right.FoldLeft(
                    binOp, Middle.FoldLeft(binOp1, _left.FoldLeft(binOp, initial)));
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public override bool Equals(FingerTree<T, V> other) {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                if (other.IsEmpty || other.IsSingle) return false;
                // ReSharper disable PossibleNullReferenceException
                // other is Deep
                var otherDeep = other as Deep;
                return Arrays.ContentEquals(otherDeep._left, _left) &&
                    Arrays.ContentEquals(otherDeep._right, _right) &&
                    (Equals(otherDeep._middleLazy, _middleLazy) || otherDeep.Middle.Equals(Middle));
                // ReSharper restore PossibleNullReferenceException
            } // Equals

            /// <summary>
            /// Serves as a hash function for a particular type. 
            /// </summary>
            /// <returns>
            /// A hash code for the current <see cref="T:System.Object" />.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override int GetHashCode() {
                unchecked {
                    int result = _measure.GetHashCode();
                    result = (result * 397) ^ _left.GetHashCode();
                    result = (result * 397) ^ _right.GetHashCode();
                    result = (result * 397) ^ _middleLazy.GetHashCode();
                    return result;
                }
            } // GetHashCode
        } // class Deep
    } // class FingerTree`2
}
// ReSharper restore RedundantThisQualifier
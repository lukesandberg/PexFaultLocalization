/*
* FingerTreeOrdered.cs is part of functional-dotnet project
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
using System.Diagnostics;
using System.Linq;
using FP.Core;
using FP.HaskellNames;
using FP.Util;
using FP.Validation;

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
    /// <typeparam name="K">The type of measure annotations.</typeparam>
    [Serializable]
    internal abstract class FingerTreeOrdered<T, K> : IEquatable<FingerTreeOrdered<T, K>>,
                                                      IMeasured<K>, IFoldable<T>,
                                                      IReversibleEnumerable<T>
        where T : IMeasured<K>
        where K : IComparable<K> {
        /// <summary>
        /// Gets the measure of the tree.
        /// </summary>
        /// <value>The measure.</value>
        public abstract K Measure { get; } // Measure

        private static readonly FingerTreeOrdered<T, K> _emptyInstance = new Empty();

        internal static FingerTreeOrdered<T, K> EmptyInstance {
            get {
                return _emptyInstance;
            }
        } // EmptyInstance

        internal static FingerTreeOrdered<FTNode<T, K>, K> EmptyInstanceNested {
            get {
                return FingerTreeOrdered<FTNode<T, K>, K>._emptyInstance;
            }
        } // EmptyInstanceNested

        private static K MeasureArray(T[] array) {
            return array[array.Length - 1].Measure;
        }

        private static FingerTreeOrdered<T, K> RotateL(FingerTreeOrdered<FTNode<T, K>, K> middle, T[] right) {
            if (middle.IsEmpty)
                return FromSortedArray(right);
            K measure = MeasureArray(right);
            return MakeDeep(middle.Head.AsArray, () => middle.Tail, right, measure);
        }

		private static Deep MakeDeepForceMiddle(T[] left, FP.Core.Lazy<FingerTreeOrdered<FTNode<T, K>, K>> middle, T[] right)
		{
            return new Deep(left, middle, right);
        }

        internal static Single MakeSingle(T value) {
            return new Single(value);
        } // MakeSingle

        private static Deep MakeDeep(T[] left, Func<FingerTreeOrdered<FTNode<T, K>, K>> func, T[] right, K measure) {
            return MakeDeep(left, Lazy.New(func), right, measure);
        } // MakeDeep

		private static Deep MakeDeep(T[] left, FP.Core.Lazy<FingerTreeOrdered<FTNode<T, K>, K>> middle, T[] right, K measure)
		{
            return new Deep(left, middle, right, measure);
        } // MakeDeep

        private static FingerTreeOrdered<T, K> DeepL(T[] left, FingerTreeOrdered<FTNode<T, K>, K> middle, T[] right) {
            Debug.Assert(left.Length <= 4);
            Debug.Assert(right.Length != 0 && right.Length <= 4);

            if (left.Length != 0)
                return MakeDeepForceMiddle(left, middle, right);

            return RotateL(middle, right);
        } // DeepL

        private static FingerTreeOrdered<T, K> DeepR(T[] left, FingerTreeOrdered<FTNode<T, K>, K> middle, T[] right) {
            Debug.Assert(left.Length != 0 && left.Length <= 4);
            Debug.Assert(right.Length <= 4);

            if (right.Length != 0)
                return MakeDeepForceMiddle(left, middle, right);

            return RotateR(left, middle);
        } // DeepR

        private static FingerTreeOrdered<T, K> RotateR(T[] left, FingerTreeOrdered<FTNode<T, K>, K> middle) {
            if (middle.IsEmpty)
                return FromSortedArray(left);
            K measure = middle.PrependMeasure(MeasureArray(left));
            return MakeDeep(left, () => middle.Init, middle.Last.AsArray, measure);
        }

        /// <summary>
        /// Creates the tree from the specified sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the tree.</typeparam>
        /// <param name="array">The small array.</param>
        /// <remarks>Calls <see cref="FingerTree.FromEnumerable{T,V}"/> when <paramref name="array"/>'s 
        /// length is more than 6.</remarks>
        private static FingerTreeOrdered<T, K> FromSortedArray(T[] array) {
            switch (array.Length) {
                case 0:
                    return EmptyInstance;
                case 1:
                    return MakeSingle(array[0]);
                default:
                    switch (array.Length) {
                        case 2:
                            return new Deep(
                                new[] { array[0] }, EmptyInstanceNested, new[] { array[1] });
                        case 3:
                            return new Deep(
                                new[] { array[0], array[1] }, EmptyInstanceNested, new[] { array[2] });
                        case 4:
                            return new Deep(
                                new[] { array[0], array[1] }, EmptyInstanceNested, new[] { array[2], array[3] });
                        case 5:
                            return new Deep(
                                new[] { array[0], array[1], array[2] }, EmptyInstanceNested, new[] { array[3], array[4] });
                        case 6:
                            return new Deep(
                                new[] { array[0], array[1], array[2] }, EmptyInstanceNested, new[] { array[3], array[4], array[5] });
                        default:
                            return EmptyInstance.AppendRange(array);
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
        public abstract FingerTreeOrdered<T, K> Tail { get; } // Tail

        /// <summary>
        /// Gets the initial sublist (all elements but the last) of the list, provided it is not empty.
        /// </summary>
        /// <value>The last element.</value>
        /// <exception cref="EmptyEnumerableException">if the list is empty.</exception>
        public abstract FingerTreeOrdered<T, K> Init { get; } // Init

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
        protected FingerTreeOrdered<T, K> AppendRange(IEnumerable<T> ts) {
            return ts.FoldLeft((tree, a) => tree | a, this);
        } // AppendRange

        /// <summary>
        /// Prepends the sequence of elements to the beginning of the tree.
        /// </summary>
        /// <param name="ts">The sequence.</param>
        protected FingerTreeOrdered<T, K> PrependRange(IEnumerable<T> ts) {
            return ts.FoldRight((a, tree) => a | tree, this);
        } // PrependRange

        /// <summary>
        /// Prepends the specified element to the beginning of the list.
        /// </summary>
        /// <param name="newHead">The new head.</param>
        /// <returns>The resulting list.</returns>
        protected abstract FingerTreeOrdered<T, K> Prepend(T newHead);

        /// <summary>
        /// Appends the specified element to the end of the list.
        /// </summary>
        /// <param name="newLast">The new last element.</param>
        /// <returns>The resulting list.</returns>
        protected abstract FingerTreeOrdered<T, K> Append(T newLast);

        /// <summary>
        /// Concatenates the tree with another tree.
        /// </summary>
        /// <param name="otherTree">Another tree.</param>
        /// <returns>The result of concatenation.</returns>
        protected abstract FingerTreeOrdered<T, K> Concat(FingerTreeOrdered<T, K> otherTree);

        protected abstract FingerTreeOrdered<T, K> App3(IEnumerable<T> middleList, FingerTreeOrdered<T, K> rightTree);

        /// <summary>
        /// Extracts all elements with the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Three trees. The first one contains all elements less than
        /// <paramref cref="key"/>, the second one equal elements, and the third one greater
        /// elements.</returns>
		public FP.Core.Tuple<FingerTreeOrdered<T, K>, FingerTreeOrdered<T, K>, FingerTreeOrdered<T, K>> ExtractAll(K key)
		{
            var split1 = Split(key, false);
            var split2 = split1.Item2.Split(key, true);
			return FP.Core.Tuple.New(split1.Item1, split2.Item1, split2.Item2);
        }

        /// <summary>
        /// Finds all elements with the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        public FingerTreeOrdered<T, K> FindAll(K key) {
            return AtLeast(key).AtMost(key);
        }

        /// <summary>
        /// Extracts one element with the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><see cref="Optional{T}.None"/> if there are no elements equal to <paramref cref="key"/>;
        /// The first field of the result contains all elements less than or equal to
        /// <see cref="key"/> except one, the second is equal to it, and the third one greater elements.</returns>
		public abstract Optional<FP.Core.Tuple<FingerTreeOrdered<T, K>, T, FingerTreeOrdered<T, K>>> ExtractOne(K key);

        public Optional<T> this[K key] {
            get {
                return LeastGtOrEqual(key).Filter(t => key.CompareTo(t.Measure) == 0);
            }
        }

        protected abstract Optional<T> LeastGtOrEqual(K key);

        public FingerTreeOrdered<T, K> Insert(T item) {
            var split = Split(item.Measure, false);
            return (split.Item1 | item) + split.Item2;
        }

        public FingerTreeOrdered<T, K> InsertRange(IEnumerable<T> ts) {
            return ts.Aggregate(this, (tree, t) => tree.Insert(t));
        }

        /// <summary>
        /// Prepends the measure.
        /// </summary>
        /// <param name="prependedMeasure">The prepended measure.</param>
        /// <remarks>Overridden in <see cref="Empty"/>, where <see cref="Measure"/> throws
        /// an exception.</remarks>
        protected virtual K PrependMeasure(K prependedMeasure) {
            return Measure;
        }

        public FingerTreeOrdered<T, K> Merge(FingerTreeOrdered<T, K> otherTree) {
            if (IsEmpty)
                return otherTree;

            var minElem = Head;
            var split = otherTree.Split(minElem.Measure, true);
            return (split.Item1 | minElem) + split.Item2.Merge(Tail);
        }

        public FingerTreeOrdered<T, K> Intersect(FingerTreeOrdered<T, K> otherTree) {
            if (IsEmpty)
                return EmptyInstance;

            var minElem = Head;
            var minMeasure = minElem.Measure;
            var split = otherTree.Split(minMeasure, true);
            var other_LTE_minElem = split.Item1;
            var recursive = split.Item2.Intersect(Tail);
            return other_LTE_minElem.IsEmpty || other_LTE_minElem.Measure.CompareTo(minMeasure) < 0
                       ? recursive
                       : minElem | recursive;
        }

        internal abstract Split<T, FingerTreeOrdered<T, K>> SplitTreeAt(K key, bool equalGoLeft, bool needLeft, bool needRight);

        /// <summary>
        /// Splits the list according to the specified predicate.
        /// </summary>
        /// <param name="key">The predicate.</param>
        /// <param name="equalGoLeft">if set to <c>true</c>, elements with the measure
        /// equal to <see cref="key"/> will be at the left side of the split;
        /// otherwise, they will be on the right side.</param>
        /// <remarks>The result has the following properties.
        /// <code>
        /// var left = tree.Split(predicate).Item1;
        /// var right = tree.Split(predicate).Item2;
        /// ---------
        /// tree.SequenceEquals(left + right);
        /// left.IsEmpty() || !predicate(left.Measure);
        /// right.IsEmpty() || predicate(left.Measure + right.Head.Measure);
        /// </code>If there are several possible splits for which these properties hold,
        /// any of them may be returned.
        /// </remarks>
        /// <remarks>Overridden in <see cref="Empty"/>, where <see cref="Measure"/> throws
        /// an exception.</remarks>
		public virtual FP.Core.Tuple<FingerTreeOrdered<T, K>, FingerTreeOrdered<T, K>> Split(K key, bool equalGoLeft)
		{
            return Split(key, equalGoLeft, true, true);
        }

        /// <summary>
        /// Splits the list according to the specified predicate.
        /// </summary>
        /// <param name="key">The predicate.</param>
        /// <param name="equalGoLeft">if set to <c>true</c>, elements with the measure
        /// equal to <see cref="key"/> will be at the left side of the split;
        /// otherwise, they will be on the right side.</param>
        /// <remarks>The result has the following properties.
        /// <code>
        /// var left = tree.Split(predicate).Item1;
        /// var right = tree.Split(predicate).Item2;
        /// ---------
        /// tree.SequenceEquals(left + right);
        /// left.IsEmpty() || !predicate(left.Measure);
        /// right.IsEmpty() || predicate(left.Measure + right.Head.Measure);
        /// </code>If there are several possible splits for which these properties hold,
        /// any of them may be returned.
        /// </remarks>
        /// <remarks>Overridden in <see cref="Empty"/>, where <see cref="Measure"/> throws
        /// an exception.</remarks>
		public virtual FP.Core.Tuple<FingerTreeOrdered<T, K>, FingerTreeOrdered<T, K>> Split(K key, bool equalGoLeft, bool needLeft, bool needRight)
		{
            if (key.CompareTo(Measure) > 0 || (equalGoLeft && key.CompareTo(Measure) == 0))
                return Pair.New(this, EmptyInstance);
            var split = SplitTreeAt(key, equalGoLeft, needLeft, needRight);
            if (equalGoLeft && key.CompareTo(split.Pivot.Measure) == 0)
                return Pair.New(needLeft ? (split.Left | split.Pivot) : EmptyInstance, split.Right);
            else return Pair.New(split.Left, needRight ? (split.Pivot | split.Right) : EmptyInstance);
        } // Split

        public FingerTreeOrdered<T, K> LessThan(K key) {
            return Split(key, false, true, false).Item1;
        }

        public FingerTreeOrdered<T, K> AtMost(K key) {
            return Split(key, true, true, false).Item1;
        }

        public FingerTreeOrdered<T, K> AtLeast(K key) {
            return Split(key, false, false, true).Item2;
        }

        public FingerTreeOrdered<T, K> GreaterThan(K key) {
            return Split(key, true, false, true).Item2;
        }

        /// <summary>
        /// Prepends <paramref name="item"/> to <paramref name="tree"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tree">The tree.</param>
        public static FingerTreeOrdered<T, K> operator |(T item, FingerTreeOrdered<T, K> tree) {
            return tree.Prepend(item);
        } // op_BitwiseOr

        /// <summary>
        /// Appends <paramref name="item"/> to <paramref name="tree"/>.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="item">The item.</param>
        public static FingerTreeOrdered<T, K> operator |(FingerTreeOrdered<T, K> tree, T item) {
            return tree.Append(item);
        } // op_BitwiseOr

        /// <summary>
        /// Concatenates <paramref name="tree1"/> and <paramref name="tree2"/>.
        /// </summary>
        /// <param name="tree1">The tree1.</param>
        /// <param name="tree2">The tree2.</param>
        public static FingerTreeOrdered<T, K> operator +(FingerTreeOrdered<T, K> tree1, FingerTreeOrdered<T, K> tree2) {
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

        public abstract bool Equals(FingerTreeOrdered<T, K> other);

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
            var ft = obj as FingerTreeOrdered<T, K>;
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
        /// Determines whether two instances of <see cref="FingerTree{T, K}"/> are equal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// <c>true</c> if the arguments are equal; <c>false</c> otherwise.
        /// </returns>
        public static bool operator ==(FingerTreeOrdered<T, K> left, FingerTreeOrdered<T, K> right) {
            return Equals(left, right);
        } // op_Equality

        /// <summary>
        /// Determines whether two instances of <see cref="FingerTree{T, K}"/> are unequal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// <c>true</c> if the arguments are not equal; <c>false</c> otherwise.
        /// </returns>
        public static bool operator !=(FingerTreeOrdered<T, K> left, FingerTreeOrdered<T, K> right) {
            return !Equals(left, right);
        } // op_Inequality

        /// <summary>
        /// An empty <see cref="FingerTree{T,K}"/>.
        /// </summary>
        [DebuggerDisplay("Empty")]
        [Serializable]
        public sealed class Empty : FingerTreeOrdered<T, K> {
            internal Empty() { } // Empty

            /// <summary>
            /// Gets the measure of the tree.
            /// </summary>
            /// <value>The measure.</value>
            public override K Measure {
                get { throw new EmptyEnumerableException(); }
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
            public override FingerTreeOrdered<T, K> Tail {
                get { throw new EmptyEnumerableException(); }
            } // Tail

            /// <summary>
            /// Gets the initial sublist (all elements but the last) of the list.
            /// </summary>
            /// <value>Throws <see cref="EmptyEnumerableException"/>.</value>
            /// <exception cref="EmptyEnumerableException"></exception>
            public override FingerTreeOrdered<T, K> Init {
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
            protected override FingerTreeOrdered<T, K> Prepend(T newHead) {
                return MakeSingle(newHead);
            } // Prepend

            /// <summary>
            /// Appends the specified element to the end of the list.
            /// </summary>
            /// <param name="newLast">The new last element..</param>
            /// <returns>The resulting list.</returns>
            protected override FingerTreeOrdered<T, K> Append(T newLast) {
                return MakeSingle(newLast);
            }

            /// <summary>
            /// Concatenates the tree with another.
            /// </summary>
            /// <param name="otherTree">Another tree.</param>
            /// <returns>The result of concatenation.</returns>
            protected override FingerTreeOrdered<T, K> Concat(FingerTreeOrdered<T, K> otherTree) {
                return otherTree;
            }

            protected override FingerTreeOrdered<T, K> App3(IEnumerable<T> middleList, FingerTreeOrdered<T, K> rightTree) {
                if (rightTree.IsEmpty) return this.AppendRange(middleList);
                return rightTree.PrependRange(middleList);
            }

			public override Optional<FP.Core.Tuple<FingerTreeOrdered<T, K>, T, FingerTreeOrdered<T, K>>> ExtractOne(K key)
			{
				return Optional<FP.Core.Tuple<FingerTreeOrdered<T, K>, T, FingerTreeOrdered<T, K>>>.None;
            }

            protected override Optional<T> LeastGtOrEqual(K key) {
                return Optional<T>.None;
            }

            protected override K PrependMeasure(K prependedMeasure) {
                return prependedMeasure;
            }

            internal override Split<T, FingerTreeOrdered<T, K>> SplitTreeAt(K key, bool equalGoLeft, bool needLeft, bool needRight) {
                throw new EmptyEnumerableException("Empty tree can't be split");
            } // SplitTree

			public override FP.Core.Tuple<FingerTreeOrdered<T, K>, FingerTreeOrdered<T, K>> Split(K key, bool equalGoLeft, bool needLeft, bool needRight)
			{
                var empty = (FingerTreeOrdered<T, K>)this;
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

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public override bool Equals(FingerTreeOrdered<T, K> other) {
                return !ReferenceEquals(null, other) && other.IsEmpty;
            } // Equals

            /// <summary>
            /// Serves as a hash function for a particular type. 
            /// </summary>
            /// <returns>
            /// A hash code for the current <see cref="T:System.Object" />.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override int GetHashCode() {
                return 0;
            } // GetHashCode
        } // class Empty

        /// <summary>
        /// A <see cref="FingerTree{T,K}"/> with the single element <see cref="Value"/>.
        /// </summary>
        [DebuggerDisplay("Single(Value = {Value}")]
        [Serializable]
        public sealed class Single : FingerTreeOrdered<T, K> {
            /// <summary>
            /// The value of the element.
            /// </summary>
            public readonly T Value;

            internal Single(T value) {
                Value = value;
            } // Single

            /// <summary>
            /// Gets the measure of the tree.
            /// </summary>
            /// <value>The measure.</value>
            public override K Measure {
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
            /// <value><see cref="FingerTreeOrdered{T,K}.EmptyInstance"/>.</value>
            public override FingerTreeOrdered<T, K> Tail {
                get { return EmptyInstance; }
            } // Tail

            /// <summary>
            /// Gets the initial sublist (all elements but the last) of the list.
            /// </summary>
            /// <value><see cref="FingerTreeOrdered{T,K}.EmptyInstance"/>.</value>
            public override FingerTreeOrdered<T, K> Init {
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
            protected override FingerTreeOrdered<T, K> Prepend(T newHead) {
                return MakeDeep(
                    new[] { newHead }, EmptyInstanceNested, new[] { Value }, Value.Measure);
            } // Prepend

            /// <summary>
            /// Appends the specified element to the end of the list.
            /// </summary>
            /// <param name="newLast">The new last element.</param>
            /// <returns>The resulting list.</returns>
            protected override FingerTreeOrdered<T, K> Append(T newLast) {
                return MakeDeep(
                    new[] { Value }, EmptyInstanceNested, new[] { newLast }, newLast.Measure);
            }

            /// <summary>
            /// Concatenates the tree with another.
            /// </summary>
            /// <param name="otherTree">Another tree.</param>
            /// <returns>The result of concatenation.</returns>
            protected override FingerTreeOrdered<T, K> Concat(FingerTreeOrdered<T, K> otherTree) {
                return Value | otherTree;
            }

            protected override FingerTreeOrdered<T, K> App3(IEnumerable<T> middleList, FingerTreeOrdered<T, K> rightTree) {
                if (rightTree.IsEmpty) return this.AppendRange(middleList);
                if (rightTree.IsSingle)
                    return this.AppendRange(middleList) | rightTree.Head;
                return Value | rightTree.PrependRange(middleList);
            }

			public override Optional<FP.Core.Tuple<FingerTreeOrdered<T, K>, T, FingerTreeOrdered<T, K>>> ExtractOne(K key)
			{
                return
                    key.CompareTo(Measure) == 0
                        ? FP.Core.Tuple.New(EmptyInstance, Value, EmptyInstance)
						: Optional<FP.Core.Tuple<FingerTreeOrdered<T, K>, T, FingerTreeOrdered<T, K>>>.None;
            }

            protected override Optional<T> LeastGtOrEqual(K key) {
                return key.CompareTo(Measure) <= 0
                           ? Value
                           : Optional<T>.None;
            }

            internal override Split<T, FingerTreeOrdered<T, K>> SplitTreeAt(K key, bool equalGoLeft, bool needLeft, bool needRight) {
                return new Split<T, FingerTreeOrdered<T, K>>(EmptyInstance, Value, EmptyInstance);
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
            public override bool Equals(FingerTreeOrdered<T, K> other) {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                if (!other.IsSingle)
                    return false;
                return Equals(other.Head, Value);
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
        /// A <see cref="FingerTree{T,K}"/> with more than one element.
        /// </summary>
        [Serializable]
        public sealed class Deep : FingerTreeOrdered<T, K> {
            private readonly K _measure;

            /// <summary>
            /// Gets the measure of the tree.
            /// </summary>
            /// <value>The measure.</value>
            public override K Measure {
                get {
                    return _measure;
                }
            }

            private readonly T[] _left;
            private readonly T[] _right;
			private readonly FP.Core.Lazy<FingerTreeOrdered<FTNode<T, K>, K>> _middleLazy;

            private FingerTreeOrdered<FTNode<T, K>, K> Middle {
                get {
                    return _middleLazy.Value;
                }
            }

			internal Deep(T[] left, FP.Core.Lazy<FingerTreeOrdered<FTNode<T, K>, K>> middleLazy, T[] right)
			{
                _left = left;
                _right = right;
                _middleLazy = middleLazy;
                _measure = MeasureArray(right);
                Debug.Assert(_left != null && _left.Length > 0 && _left.Length <= 4);
                Debug.Assert(_right != null && _right.Length > 0 && _right.Length <= 4);
            } // Deep

            internal Deep(
				T[] left, FP.Core.Lazy<FingerTreeOrdered<FTNode<T, K>, K>> middleLazy, T[] right,
                K measure) {
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
            public override FingerTreeOrdered<T, K> Tail {
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
            public override FingerTreeOrdered<T, K> Init {
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
            protected override FingerTreeOrdered<T, K> Prepend(T newHead) {
                var leftLength = _left.Length;
                if (leftLength != 4) {
                    var newLeft = new T[leftLength + 1];
                    newLeft[0] = newHead;
                    Array.Copy(_left, 0, newLeft, 1, leftLength);
                    return MakeDeep(newLeft, _middleLazy, _right, _measure);
                }

                // refinement from Scala version: http://scala.sygneca.com/code/finger-trees
                // Should make repeated prepends a bit more efficient
                if (_right.Length == 1) {
                    if (Middle.IsEmpty) {
                        return MakeDeep(
                            new[] { newHead, _left[0], _left[1] },
                            EmptyInstanceNested,
                            new[] { _left[2], _left[3], _right[0] },
                            _measure);
                    }
                    if (Middle.IsSingle) {
                        var middle = Middle.Head.AsArray;
                        if (middle.Length == 2) {
                            return MakeDeep(
                                new[] { newHead, _left[0] },
                                MakeSingleNested(MakeNode(_left[1], _left[2], _left[3])),
                                new[] { middle[0], middle[1], _right[0] },
                                _measure);
                        }
                        Debug.Assert(middle.Length == 3);
                        return MakeDeep(
                            new[] { newHead, _left[0], _left[1] },
                            MakeSingleNested(MakeNode(_left[2], _left[3], middle[0])),
                            new[] { middle[1], middle[2], _right[0] },
                            _measure);
                    }
                }

                return MakeDeep(
                    new[] { newHead, _left[0] },
                    MakeNode(_left[1], _left[2], _left[3]) | Middle,
                    _right,
                    _measure);
            } // Prepend

            /// <summary>
            /// Appends the specified element to the end of the list.
            /// </summary>
            /// <param name="newLast">The new last element..</param>
            /// <returns>The resulting list.</returns>
            protected override FingerTreeOrdered<T, K> Append(T newLast) {
                var rightLength = _right.Length;
                var newMeasure = newLast.Measure;
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

            private static FingerTreeOrdered<FTNode<T, K>, K> MakeSingleNested(FTNode<T, K> node) {
                return FingerTreeOrdered<FTNode<T, K>, K>.MakeSingle(node);
            }

            /// <summary>
            /// Concatenates the tree with another.
            /// </summary>
            /// <param name="otherTree">Another tree.</param>
            /// <returns>The result of concatenation.</returns>
            protected override FingerTreeOrdered<T, K> Concat(FingerTreeOrdered<T, K> otherTree) {
                if (otherTree.IsEmpty)
                    return this;
                if (otherTree.IsSingle)
                    return this | otherTree.Head;
                return App3(Enumerable.Empty<T>(), otherTree);
            }

            protected override FingerTreeOrdered<T, K> App3(IEnumerable<T> middleList, FingerTreeOrdered<T, K> rightTree) {
                if (rightTree.IsEmpty) return this.AppendRange(middleList);
                if (rightTree.IsSingle) return this.AppendRange(middleList) | rightTree.Head;
                // ReSharper disable PossibleNullReferenceException
                var rightDeep = rightTree as Deep;
                return MakeDeep(
                    _left,
                    () => Middle.App3(
                              Nodes(_right.Concat(middleList).Concat(rightDeep._left)),
                              rightDeep.Middle),
                    rightDeep._right,
                    rightDeep.Measure);
                // ReSharper restore PossibleNullReferenceException
            }

			public override Optional<FP.Core.Tuple<FingerTreeOrdered<T, K>, T, FingerTreeOrdered<T, K>>> ExtractOne(K key)
			{
                var split = Split(key, true);
                var lessOrEqual = split.Item1;
                if (lessOrEqual.IsEmpty || key.CompareTo(lessOrEqual.Measure) > 0)
					return Optional<FP.Core.Tuple<FingerTreeOrdered<T, K>, T, FingerTreeOrdered<T, K>>>.None;
				return FP.Core.Tuple.New(lessOrEqual.Init, lessOrEqual.Last, split.Item2);
            }

            protected override Optional<T> LeastGtOrEqual(K key) {
                // is split on the left?
                K maxLeft = MeasureArray(_left);
                int keyComparedToMaxLeft = key.CompareTo(maxLeft);
                if (keyComparedToMaxLeft <= 0)
                    return LeastGtOrEqual(_left, key);

                int keyComparedToMaxMiddle = key.CompareTo(Middle.PrependMeasure(maxLeft));
                // is split in the middle?
                if (keyComparedToMaxMiddle <= 0)
                    return Middle.LeastGtOrEqual(key).MapPartial(node => LeastGtOrEqual(node.AsArray, key));

                // it must be on the right
                return LeastGtOrEqual(_right, key);
            }

            private Optional<T> LeastGtOrEqual(T[] array, K key) {
                foreach (T t in array)
                    if (key.CompareTo(t.Measure) <= 0) return Optional.Some(t);
                return Optional<T>.None;
            }

            private static IEnumerable<FTNode<T, K>> Nodes(IEnumerable<T> elements) {
                Debug.Assert(elements.Count() >= 2);
                var buffer = new Queue<T>(5);
                foreach (var t in elements) {
                    buffer.Enqueue(t);
                    if (buffer.Count == 5) {
                        yield return MakeNode(buffer.Dequeue(), buffer.Dequeue(), buffer.Dequeue());
                    }
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

            private static FTNode<T, K> MakeNode(T t1, T t2) {
                return new FTNode<T, K>(t2.Measure, t1, t2);
            }

            private static FTNode<T, K> MakeNode(T t1, T t2, T t3) {
                return new FTNode<T, K>(t3.Measure, t1, t2, t3);
            }

            internal override Split<T, FingerTreeOrdered<T, K>> SplitTreeAt(K key, bool equalGoLeft, bool needLeft, bool needRight) {
                // is split on the left?
                K maxLeft = MeasureArray(_left);
                int keyComparedToMaxLeft = key.CompareTo(maxLeft);
                if (keyComparedToMaxLeft < 0 || (!equalGoLeft && keyComparedToMaxLeft == 0)) {
                    var splitLeft = SplitArrayAt(_left, key, equalGoLeft, needLeft, needRight);
                    return new Split<T, FingerTreeOrdered<T, K>>(
                        FromSortedArray(splitLeft.Left),
                        splitLeft.Pivot,
                        needRight ? DeepL(splitLeft.Right, Middle, _right) : EmptyInstance);
                } // if

                int keyComparedToMaxMiddle = key.CompareTo(Middle.PrependMeasure(maxLeft));
                // is split in the middle?
                if (keyComparedToMaxMiddle < 0 || (!equalGoLeft && keyComparedToMaxMiddle == 0)) {
                    var splitMiddle = Middle.SplitTreeAt(key, equalGoLeft, needLeft, needRight);
                    var splitMiddlePivot = SplitArrayAt(splitMiddle.Pivot.AsArray, key, equalGoLeft, needLeft, needRight);
                    return new Split<T, FingerTreeOrdered<T, K>>(
                        needLeft ? DeepR(_left, splitMiddle.Left, splitMiddlePivot.Left) : EmptyInstance,
                        splitMiddlePivot.Pivot,
                        needRight ? DeepL(splitMiddlePivot.Right, splitMiddle.Right, _right) : EmptyInstance);
                } // if

                // it must be on the right
                var splitRight = SplitArrayAt(_right, key, equalGoLeft, needLeft, needRight);
                return new Split<T, FingerTreeOrdered<T, K>>(
                    needLeft ? DeepR(_left, Middle, splitRight.Left) : EmptyInstance,
                    splitRight.Pivot,
                    FromSortedArray(splitRight.Right));
            } // SplitTree

            private static Split<T, T[]> SplitArrayAt(T[] array, K key, bool equalGoLeft, bool needLeft, bool needRight) {
                if (array.Length == 1) {
                    return new Split<T, T[]>(
                        Arrays.Empty<T>(), array[0], Arrays.Empty<T>());
                }

                int offset;
                for (offset = 0; offset < array.Length - 1; offset++) {
                    var keyComparedToElement = key.CompareTo(array[offset].Measure);
                    if (keyComparedToElement < 0 || (!equalGoLeft && keyComparedToElement == 0)) break;
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
                Func<FTNode<T, K>, A, A> binOp1 = (n, a) => n.FoldRight(binOp, a);
                return Enumerables.FoldRight(_left, binOp, Middle.FoldRight(binOp1, Enumerables.FoldRight(_right, binOp, initial)));
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
                Func<A, FTNode<T, K>, A> binOp1 = (a, n) => n.FoldLeft(binOp, a);
                return Enumerables2.FoldLeft(_right, binOp, Middle.FoldLeft(binOp1, Enumerables2.FoldLeft(_left, binOp, initial)));
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public override bool Equals(FingerTreeOrdered<T, K> other) {
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
    }
}
// ReSharper restore RedundantThisQualifier
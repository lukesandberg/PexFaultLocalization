/*
* TreeDictionary.cs is part of functional-dotnet project
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

namespace FP.Collections.Persistent {
    /// <summary>
    /// A dictionary based on weight-balanced trees.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TComparer">The type of the comparer.</typeparam>
    [Serializable]
    public class TreeDictionary<TKey, TValue, TComparer> :
        ISortedDictionary<TKey, TValue, TComparer, TreeDictionary<TKey, TValue, TComparer>>,
        ICombinableDictionary<TKey, TValue, TreeDictionary<TKey, TValue, TComparer>>,
        IEquatable<TreeDictionary<TKey, TValue, TComparer>>
        where TComparer : IComparer<TKey>, new() {
        private static readonly TComparer _Comparer = new TComparer();
        
        /// <summary>
        /// An empty dictionary with no keys or values.
        /// </summary>
        public static readonly TreeDictionary<TKey, TValue, TComparer> Empty = 
            new TreeDictionary<TKey, TValue, TComparer>(0, default(TKey), default(TValue), null, null);
        
        private readonly int _count;
        private readonly TKey _key;
        private readonly TValue _value;
        private readonly TreeDictionary<TKey, TValue, TComparer> _left;
        private readonly TreeDictionary<TKey, TValue, TComparer> _right;

        internal TreeDictionary(
            int count, TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            _count = count;
            _right = right;
            _left = left;
            _value = value;
            _key = key;
            AssertInvariant();
        }

        private TreeDictionary(
            TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) :
            this(1 + left.Count() + right.Count(), key, value, left, right) { }

        [Conditional("DEBUG")]
        private void AssertInvariant() {
            int leftCount = _left.Count();
            int rightCount = _right.Count();
            bool searchTree = true;
            // ReSharper disable PossibleNullReferenceException
            if (leftCount > 0)
                searchTree = _Comparer.Compare(_left._key, _key) < 0;
            if (rightCount > 0)
                searchTree = searchTree && _Comparer.Compare(_key, _right._key) < 0;
            // ReSharper restore PossibleNullReferenceException
            Debug.Assert(_count == 0 || _count == 1 + leftCount + rightCount);
            Debug.Assert(searchTree);
            Debug.Assert(_count == 0 || (_left != null && _right != null));
            Debug.Assert(_count <= 2 || (leftCount <= DELTA * rightCount && rightCount <= DELTA * leftCount));
        }

        /// <summary>
        /// Returns an iterator which yields all elements of the sequence in the reverse order.
        /// </summary>
        /// <remarks>This should always be equivalent to, but faster than, 
        /// <code>
        /// AsEnumerable().Reverse();
        /// </code></remarks>
		public IEnumerable<FP.Core.Tuple<TKey, TValue>> ReverseIterator()
		{
            if (_count == 0) yield break;
            if (_right.Count() != 0) {
                foreach (var pair in _right.ReverseIterator())
                    yield return pair;
            }
			yield return FP.Core.Tuple.New(_key, _value);
            if (_left.Count() != 0) {
                foreach (var pair in _left.ReverseIterator())
                    yield return pair;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
		public IEnumerator<FP.Core.Tuple<TKey, TValue>> GetEnumerator()
		{
            if (_count == 0) yield break;
            if (_left.Count() != 0) {
                foreach (var pair in _left)
                    yield return pair;
            }
			yield return FP.Core.Tuple.New(_key, _value);
            if (_right.Count() != 0) {
                foreach (var pair in _right)
                    yield return pair;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets a value indicating whether this collection is empty.
        /// </summary>
        /// <value><c>true</c>.</value>
        public bool IsEmpty {
            get { return _count != 0; }
        }

        /// <summary>
        /// Gets the number of elements in the dictionary.
        /// </summary>
        /// <value>The count.</value>
        public int Count {
            get { return _count; }
        }

        /// <summary>
        /// Looks up the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <value><see cref="Optional.Some{T}"/><c>(value)</c> if the
        /// dictionary contains the specified key and associates <c>value</c>
        /// to it and <see cref="Optional.None{T}"/> otherwise.</value>
        public Optional<TValue> this[TKey key] {
            get {
                var dict = this;
                while (dict.Count() != 0) {
                    int comparison = _Comparer.Compare(key, dict._key);
                    if (comparison == 0) return dict._value;
                    dict = comparison < 0 ? dict._left : dict._right;
                }
                return Optional.None<TValue>();
            }
        }

        public TreeDictionary<TKey, TValue, TComparer> Add(
            TKey key, TValue value, Func<TValue, TValue> combineFunc) {
            // Max height is under 3*log(Count), so recursive methods shouldn't cause a stack overflow
            bool needRebalance = false;
            return AddRecursive(key, value, combineFunc, ref needRebalance);
            // return AddStack(key, value, combineFunc);
            // return AddRecursiveShortcut(key, value, combineFunc);
        }

        private TreeDictionary<TKey, TValue, TComparer> AddRecursive(TKey key, TValue value, Func<TValue, TValue> combiner, ref bool needRebalance) {
            if (_count == 0) {
                needRebalance = true;
                return TreeDictionary.Single<TKey, TValue, TComparer>(key, value);
            }
            int comparison = _Comparer.Compare(key, _key);
            if (comparison == 0) {
                needRebalance = false;
                return ReplaceValue(combiner(_value));
            }
            if (comparison < 0) {
                var dict = _left.AddRecursive(key, value, combiner, ref needRebalance);
                return needRebalance
                           ? NearlyBalanced(dict, _right)
                           : Balanced(dict, _right);
            }
            else {
                var dict = _right.AddRecursive(key, value, combiner, ref needRebalance);
                return needRebalance
                           ? NearlyBalanced(_left, dict)
                           : Balanced(_left, dict);
            }
        }

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
        public TreeDictionary<TKey, TValue, TComparer> Add(TKey key, TValue value) {
            bool needRebalance = false;
            return AddRecursive(key, value, ref needRebalance);
        }

        private TreeDictionary<TKey, TValue, TComparer> AddRecursive(TKey key, TValue value, ref bool needRebalance) {
            if (_count == 0) {
                needRebalance = true;
                return TreeDictionary.Single<TKey, TValue, TComparer>(key, value);
            }
            int comparison = _Comparer.Compare(key, _key);
            if (comparison == 0) {
                needRebalance = false;
                return ReplaceValue(value);
            }
            if (comparison < 0) {
                var newLeft = _left.AddRecursive(key, value, ref needRebalance);
                return needRebalance
                           ? NearlyBalanced(newLeft, _right)
                           : Balanced(newLeft, _right);
            }
            else {
                var newRight = _right.AddRecursive(key, value, ref needRebalance);
                return needRebalance
                           ? NearlyBalanced(_left, newRight)
                           : Balanced(_left, newRight);
            }
        }

        public TreeDictionary<TKey, TValue, TComparer> Remove(TKey key, out Optional<TValue> value) {
            if (Count == 0) {
                value = Optional<TValue>.None;
                return this;
            }
            int comparison = _Comparer.Compare(key, _key);
            if (comparison == 0) {
                value = _value;
                return GlueBalanced(_left, _right);
            }
            if (comparison < 0) {
                var newLeft = _left.Remove(key, out value);
                return value.HasValue
                           ? NearlyBalanced(newLeft, _right)
                           : Balanced(newLeft, _right);
            }
            else {
                var newRight = _right.Remove(key, out value);
                return value.HasValue
                           ? NearlyBalanced(_left, newRight)
                           : Balanced(_left, newRight);                
            }
        }

        public TreeDictionary<TKey, TValue, TComparer> Update(
            TKey key, Func<TValue, Optional<TValue>> updateFunc, out Optional<TValue> value) {
            bool needRebalance;
            return UpdateHelper(key, updateFunc, out value, out needRebalance);
        }

        private TreeDictionary<TKey, TValue, TComparer> UpdateHelper(
            TKey key, Func<TValue, Optional<TValue>> updateFunc, out Optional<TValue> value, out bool needRebalance) {
            if (Count == 0) {
                value = Optional<TValue>.None;
                needRebalance = false;
                return this;
            }
            int comparison = _Comparer.Compare(key, _key);
            if (comparison == 0) {
                value = _value;
                var newValue = updateFunc(_value);
                needRebalance = !newValue.HasValue;
                return needRebalance
                    ? GlueBalanced(_left, _right)
                    : ReplaceValue(newValue.Value);
            }
            if (comparison < 0) {
                var newLeft = _left.UpdateHelper(key, updateFunc, out value, out needRebalance);
                return needRebalance
                           ? NearlyBalanced(newLeft, _right)
                           : Balanced(newLeft, _right);
            }
            else {
                var newRight = _right.UpdateHelper(key, updateFunc, out value, out needRebalance);
                return needRebalance
                           ? NearlyBalanced(_left, newRight)
                           : Balanced(_left, newRight);
            }
        }

        /// <summary>
        /// Updates values of all keys. For each key in the dictionary, if
        /// <code>updateFunc(key, this[key])</code> returns <code>None</code>, the
        /// key is removed; if it returns <code>Some(newValue)</code>, the
        /// current value is replaced with <c>newValue</c>.
        /// </summary>
        /// <param name="updateFunc">The updating function.</param>
        /// <returns>The resulting dictionary.</returns>
        public TreeDictionary<TKey, TValue, TComparer> MapPartial(Func<TKey, TValue, Optional<TValue>> updateFunc) {
            if (Count == 0)
                return this;
            var newValue = updateFunc(_key, _value);
            return newValue.HasValue
                       ? Unbalanced(newValue.Value, _left.MapPartial(updateFunc), _right.MapPartial(updateFunc))
                       : GlueUnbalanced(_left.MapPartial(updateFunc), _right.MapPartial(updateFunc));
        }

        /// <summary>
        /// Returns a dictionary with the same keys. For each key in the dictionary, the
        /// value is obtained by calling <code>func(key, this[key])</code>.
        /// </summary>
        /// <param name="func">The function mapped.</param>
        /// <returns>The resulting dictionary.</returns>
        public TreeDictionary<TKey, TValue, TComparer> Map(Func<TKey, TValue, TValue> func) {
            if (Count == 0)
                return this;
            return Balanced(
                func(_key, _value), _left.Map(func), _right.Map(func));
        }

        /// <summary>
        /// Returns a dictionary with the same keys. The value for each key is
        /// obtained by calling <c>func(key, this[key])</c>.
        /// </summary>
        /// <typeparam name="TValue1">The type of the values in the returned dictionary.</typeparam>
        /// <param name="func">The function mapped.</param>
        /// <returns>The resulting dictionary.</returns>
        public TreeDictionary<TKey, TValue1, TComparer> Map<TValue1>(Func<TKey, TValue, TValue1> func) {
            if (Count == 0)
                return TreeDictionary<TKey, TValue1, TComparer>.Empty;
            return TreeDictionary<TKey, TValue1, TComparer>.Balanced(
                _key, func(_key, _value), _left.Map(func), _right.Map(func));
        }

        /// <summary>
        /// Updates values of all keys. For each key in the dictionary, the
        /// value is replaced with <code>func(key, this[key])</code>.
        /// </summary>
        /// <typeparam name="TValue1">The type of the values in the returned dictionary.</typeparam>
        /// <param name="func">The updating function.</param>
        /// <returns>The resulting dictionary.</returns>
        public TreeDictionary<TKey, TValue1, TComparer> MapPartial<TValue1>(Func<TKey, TValue, Optional<TValue1>> func) {
            if (Count == 0)
                return TreeDictionary<TKey, TValue1, TComparer>.Empty;
            var newValue = func(_key, _value);
            return newValue.HasValue
                       ? TreeDictionary<TKey, TValue1, TComparer>.Unbalanced(_key, newValue.Value, _left.MapPartial(func), _right.MapPartial(func))
                       : TreeDictionary<TKey, TValue1, TComparer>.GlueUnbalanced(_left.MapPartial(func), _right.MapPartial(func));
        }

        /// <summary>
        /// Gets the values. Doesn't guarantee anything about the order in which they are yielded.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<TValue> Values {
            get {
                if (Count == 0)
                    yield break;
                if (_left.Count() != 0) {
                    foreach (var value in _left.Values)
                        yield return value;
                }
                yield return _value;
                if (_right.Count() != 0) {
                    foreach (var value in _right.Values)
                        yield return value;
                }
            }
        }

        /// <summary>
        /// Retrieves the minimum key, associated value, and the dictionary
        /// containing all other elements.
        /// </summary>
        /// <param name="minKey">Is bound to the minimum key, if the dictionary
        /// isn't empty.</param>
        /// <param name="minKeyValue">Is bound to the value associated to the
        /// minimum key, if the dictionary isn't empty.</param>
        /// <returns>
        /// The dictionary containing all elements except the minimum key, if
        /// the dictionary is non-empty; <c>None</c> otherwise.
        /// </returns>
        public Optional<TreeDictionary<TKey, TValue, TComparer>> FindAndRemoveMinKey(out TKey minKey, out TValue minKeyValue) {
            if (Count == 0) {
                minKey = default(TKey);
                minKeyValue = default(TValue);
                return Optional.None<TreeDictionary<TKey, TValue, TComparer>>();
            }
            return FindAndRemoveMinKeyFromNonEmpty(out minKey, out minKeyValue);
        }

        private TreeDictionary<TKey, TValue, TComparer> FindAndRemoveMinKeyFromNonEmpty(out TKey minKey, out TValue minKeyValue) {
            if (_left.Count() == 0) {
                minKey = _key;
                minKeyValue = _value;
                return _right;
            }
            return NearlyBalanced(_left.FindAndRemoveMinKeyFromNonEmpty(out minKey, out minKeyValue), _right);
        }

        /// <summary>
        /// Retrieves the minimum key, associated value, and the dictionary
        /// containing all other elements.
        /// </summary>
        /// <param name="maxKey">Is bound to the maximum key, if the dictionary
        /// isn't empty.</param>
        /// <param name="maxKeyValue">Is bound to the value associated to the
        /// maximum key, if the dictionary isn't empty.</param>
        /// <returns>The dictionary containing all elements except the maximum
        /// key, if the dictionary is non-empty; <c>None</c> otherwise.
        /// </returns>
        public Optional<TreeDictionary<TKey, TValue, TComparer>> FindAndRemoveMaxKey(out TKey maxKey, out TValue maxKeyValue) {
            if (Count == 0) {
                maxKey = default(TKey);
                maxKeyValue = default(TValue);
                return Optional.None<TreeDictionary<TKey, TValue, TComparer>>();
            }
            return FindAndRemoveMaxKeyFromNonEmpty(out maxKey, out maxKeyValue);
        }

        private TreeDictionary<TKey, TValue, TComparer> FindAndRemoveMaxKeyFromNonEmpty(out TKey maxKey, out TValue maxKeyValue) {
            if (_right.Count() == 0) {
                maxKey = _key;
                maxKeyValue = _value;
                return _left;
            }
            return NearlyBalanced(_left, _right.FindAndRemoveMaxKeyFromNonEmpty(out maxKey, out maxKeyValue));
        }

        /// <summary>
        /// Retrieves the minimum key and the associated value.
        /// </summary>
        /// <returns>The tuple of the minimum key, associated value and the
        /// dictionary containing all other elements, if the dictionary is
        /// non-empty; <c>None</c> otherwise.</returns>
		public Optional<FP.Core.Tuple<TKey, TValue>> MinKeyAndValue()
		{
            if (Count == 0)
				return Optional<FP.Core.Tuple<TKey, TValue>>.None;
            if (Count == 1)
				return Optional.Some(FP.Core.Tuple.New(_key, _value));
            return _left.MinKeyAndValue();
        }

        /// <summary>
        /// Retrieves the maximum key and the associated value.
        /// </summary>
        /// <returns>The tuple of the maximum key, associated value and the
        /// dictionary containing all other elements, if the dictionary is
        /// non-empty; <c>None</c> otherwise.</returns>
		public Optional<FP.Core.Tuple<TKey, TValue>> MaxKeyAndValue()
		{
            if (Count == 0)
				return Optional<FP.Core.Tuple<TKey, TValue>>.None;
            if (Count == 1)
				return Optional.Some(FP.Core.Tuple.New(_key, _value));
            return _right.MaxKeyAndValue();
        }

        /// <summary>
        /// Splits dictionary on the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A tuple, where the first element contains all keys less
        /// than <paramref name="key"/>, the second element is <c>this[key]</c>,
        /// and the third element contains all keys greater than 
        /// <paramref name="key"/></returns>
		public FP.Core.Tuple<TreeDictionary<TKey, TValue, TComparer>, Optional<TValue>, TreeDictionary<TKey, TValue, TComparer>> Split(TKey key)
		{
            TreeDictionary<TKey, TValue, TComparer> less;
            Optional<TValue> value;
            TreeDictionary<TKey, TValue, TComparer> greater;
            SplitRecursive(key, out less, out value, out greater);
			return FP.Core.Tuple.New(less, value, greater);
        }

        private void SplitRecursive(
            TKey key,
            out TreeDictionary<TKey, TValue, TComparer> less, 
            out Optional<TValue> value, 
            out TreeDictionary<TKey, TValue, TComparer> greater) {
            if (Count == 0) {
                less = Empty;
                value = Optional<TValue>.None;
                greater = Empty;
                return;
            }
            int comparison = _Comparer.Compare(key, _key);
            if (comparison == 0) {
                less = _left;
                value = Optional.Some(_value);
                greater = _right;
                return;
            }
            if (comparison < 0) {
                _left.SplitRecursive(key, out less, out value, out greater);
                greater = Unbalanced(_key, _value, greater, _right);
                return;
            }
            else {
                _right.SplitRecursive(key, out less, out value, out greater);
                less = Unbalanced(_key, _value, _left, less);
                return;
            }
        }

        /// <summary>
        /// Returns the dictionary containing all keys present in one of the
        /// dictionaries with the same values. If a key is present in both dictionaries, 
        /// the value from <c>this</c> dictionary is used.
        /// </summary>
        /// <param name="otherDict">
        /// The other dictionary.
        /// </param>
        /// <returns>
        /// The union of two dictionaries.
        /// </returns>
        public TreeDictionary<TKey, TValue, TComparer> UnionLeftBiased(
            TreeDictionary<TKey, TValue, TComparer> otherDict) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the dictionary containing all keys present in one of the
        /// dictionaries with the same values. If a key is present in both dictionaries, 
        /// the value is obtained by using the <paramref name="combiner"/>.
        /// </summary>
        /// <param name="otherDict">
        /// The other dictionary.
        /// </param>
        /// <param name="combiner">
        /// The function used for combining values of duplicate keys.
        /// </param>
        /// <returns>
        /// The union of two dictionaries.
        /// </returns>
        public TreeDictionary<TKey, TValue, TComparer> Union(
            TreeDictionary<TKey, TValue, TComparer> otherDict, Func<TKey, TValue, TValue, TValue> combiner) {
            throw new NotImplementedException();
        }

        public TreeDictionary<TKey, TValue, TComparer> Difference(
            TreeDictionary<TKey, TValue, TComparer> otherDict) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the dictionary containing all keys present in both
        /// dictionaries. The value from <c>this</c> dictionary is used.
        /// </summary>
        /// <param name="otherDict">
        /// The other dictionary.
        /// </param>
        /// <param name="combiner">
        /// The function used for combining values of duplicate keys.
        /// </param>
        /// <returns>
        /// The difference of two dictionaries.
        /// </returns>
        public TreeDictionary<TKey, TValue, TComparer> IntersectionLeftBiased(TreeDictionary<TKey, TValue, TComparer> otherDict) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the dictionary containing all keys present in both
        /// dictionaries. The value is obtained by using the 
        /// <paramref name="combiner"/>.
        /// </summary>
        /// <param name="otherDict">
        /// The other dictionary.
        /// </param>
        /// <param name="combiner">
        /// The function used for combining values of duplicate keys.
        /// </param>
        /// <returns>
        /// The difference of two dictionaries.
        /// </returns>
        public TreeDictionary<TKey, TValue, TComparer> Intersection(
            TreeDictionary<TKey, TValue, TComparer> otherDict, Func<TKey, TValue, TValue, TValue> combiner) {
            throw new NotImplementedException();
        }

        private TreeDictionary<TKey, TValue, TComparer> Trim(TKey low, TKey high) {
            if (Count == 0)
                return this;
            if (_Comparer.Compare(low, _key) < 0) {
                if (_Comparer.Compare(high, _key) > 0) return this;
                else return _left.Trim(low, high);
            }
            else return _right.Trim(low, high);
        }

        private TreeDictionary<TKey, TValue, TComparer> TrimLow(TKey low) {
            if (Count == 0)
                return this;
            if (_Comparer.Compare(low, _key) < 0)
                return this;
            else 
                return _right.TrimLow(low);
        }

        private TreeDictionary<TKey, TValue, TComparer> TrimHigh(TKey high) {
            if (Count == 0)
                return this;
            if (_Comparer.Compare(high, _key) > 0) 
                return this;
            else 
                return _left.TrimHigh(high);
        }

        private TreeDictionary<TKey, TValue, TComparer> UnionTrim(
            TreeDictionary<TKey, TValue, TComparer> otherDict, 
            TKey low, TKey high, 
            Func<TKey, TValue, TValue, TValue> combiner) {
            if (otherDict.Count == 0)
                return this;
            if (Count == 0) {
                var otherLeft = otherDict._left.FilterLess(high);
                var otherRight = otherDict._right.FilterGreater(low);
                return ReferenceEquals(otherDict._left, otherLeft) && ReferenceEquals(otherDict._right, otherRight)
                           ? otherDict
                           : otherDict.Unbalanced(otherDict._value, otherLeft, otherRight);
            }
            Debug.Assert(_Comparer.Compare(low, _key) < 0 && _Comparer.Compare(high, _key) > 0);
            return Unbalanced(
                _value,
                _left.UnionTrim(otherDict.Trim(low, _key), low, _key, combiner),
                _right.UnionTrim(otherDict.Trim(_key, high), _key, high, combiner));
        }

        private TreeDictionary<TKey, TValue, TComparer> FilterGreater(TKey key) {
            throw new NotImplementedException();
        }

        private TreeDictionary<TKey, TValue, TComparer> FilterLess(TKey key) {
            throw new NotImplementedException();
        }

        private const int DELTA = 5;
        private const int RATIO = 2;

        private TreeDictionary<TKey, TValue, TComparer> ReplaceValue(TValue value) {
            return
                EqualityComparer<TValue>.Default.Equals(_value, value)
                    ? this
                    : ReplaceValueDontCheckEquality(value);
        }

        private TreeDictionary<TKey, TValue, TComparer> ReplaceValueDontCheckEquality(TValue value) {
            return new TreeDictionary<TKey, TValue, TComparer>(_count, _key, value, _left, _right);
        }

        private TreeDictionary<TKey, TValue, TComparer> Unbalanced(
            TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            if (left.Count() == 0)
                return right.InsertMin(_key, value);
            if (right.Count() == 0)
                return left.InsertMax(_key, value);
            if (DELTA * left.Count <= right.Count) {
                return right.NearlyBalanced(Unbalanced(_key, value, left, right._left), right._right);
            }
            if (DELTA * right.Count <= left.Count) {
                return left.NearlyBalanced(left._left, Unbalanced(_key, value, left._right, right));
            }
            return Balanced(_key, value, left, right);
        }

        private static TreeDictionary<TKey, TValue, TComparer> Unbalanced(
            TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            if (left.Count() == 0)
                return right.InsertMin(key, value);
            if (right.Count() == 0)
                return left.InsertMax(key, value);
            if (DELTA * left.Count <= right.Count) {
                return right.NearlyBalanced(Unbalanced(key, value, left, right._left), right._right);
            }
            if (DELTA * right.Count <= left.Count) {
                return left.NearlyBalanced(left._left, Unbalanced(key, value, left._right, right));
            }
            return Balanced(key, value, left, right);
        }

        private TreeDictionary<TKey, TValue, TComparer> InsertMin(TKey key, TValue value) {
            if (Count == 0)
                return TreeDictionary.Single<TKey, TValue, TComparer>(key, value);
            return NearlyBalanced(_left.InsertMin(key, value), _right);
        }

        private TreeDictionary<TKey, TValue, TComparer> InsertMax(TKey key, TValue value) {
            if (Count == 0)
                return TreeDictionary.Single<TKey, TValue, TComparer>(key, value);
            return NearlyBalanced(_left, _right.InsertMax(key, value));
        }

        private static TreeDictionary<TKey, TValue, TComparer> NearlyBalanced(
            TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            int countLeft = left.Count();
            int countRight = right.Count();
            int count = 1 + countLeft + countRight;
            if (countLeft + countRight <= 1)
                return new TreeDictionary<TKey, TValue, TComparer>(count, key, value, left, right);
            if (countLeft >= DELTA * countRight) // >= in Data.Map
                return RotateRight(key, value, left, right);
            if (countRight >= DELTA * countLeft) // >= in Data.Map
                return RotateLeft(key, value, left, right);
            // Balanced already
            return new TreeDictionary<TKey, TValue, TComparer>(count, key, value, left, right);
        }

        private TreeDictionary<TKey, TValue, TComparer> NearlyBalanced(TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            return NearlyBalanced(_key, _value, left, right);
        }

        private static TreeDictionary<TKey, TValue, TComparer> Balanced(
            TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            return new TreeDictionary<TKey, TValue, TComparer>(key, value, left, right);
        }

        private TreeDictionary<TKey, TValue, TComparer> Balanced(TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            return EqualityComparer<TValue>.Default.Equals(_value, value)
                && ReferenceEquals(_left, left) && ReferenceEquals(_right, right)
                       ? this
                       : Balanced(_key, value, left, right);
        }

        private TreeDictionary<TKey, TValue, TComparer> Balanced(
            TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            return ReferenceEquals(_left, left) && ReferenceEquals(_right, right)
                       ? this
                       : Balanced(_key, _value, left, right);
        }

        /// <summary>
        /// Gets the keys. Doesn't guarantee anything about the order in which they are yielded.
        /// </summary>
        /// <value>The keys.</value>
        public IEnumerable<TKey> Keys {
            get {
                if (Count == 0)
                    yield break;
                if (_left.Count() != 0) {
                    foreach (var key in _left.Keys)
                        yield return key;
                }
                yield return _key;
                if (_right.Count() != 0) {
                    foreach (var key in _right.Keys)
                        yield return key;
                }
            }
        }

        private static TreeDictionary<TKey, TValue, TComparer> RotateLeft(
            TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            return right._left.Count() < RATIO * right._right.Count()
                       ? RotateSingleLeft(key, value, left, right)
                       : RotateDoubleLeft(key, value, left, right);
        }

        private static TreeDictionary<TKey, TValue, TComparer> RotateRight(
            TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            return left._right.Count() < RATIO * left._left.Count()
                       ? RotateSingleRight(key, value, left, right)
                       : RotateDoubleRight(key, value, left, right);
        }

        private static TreeDictionary<TKey, TValue, TComparer> RotateSingleLeft(
            TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            return Balanced(
                right._key, right._value,
                Balanced(key, value, left, right._left),
                right._right);
        }

        private static TreeDictionary<TKey, TValue, TComparer> RotateSingleRight(
            TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            return Balanced(
                left._key, left._value,
                left._left,
                Balanced(key, value, left._right, right));
        }

        private static TreeDictionary<TKey, TValue, TComparer> RotateDoubleLeft(
            TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            TreeDictionary<TKey, TValue, TComparer> rightLeft = right._left;
            return Balanced(
                rightLeft._key, rightLeft._value,
                Balanced(key, value, left, rightLeft._left),
                Balanced(right._key, right._value, rightLeft._right, right._right));
        }

        private static TreeDictionary<TKey, TValue, TComparer> RotateDoubleRight(
            TKey key, TValue value, TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            var leftRight = left._right;
            return Balanced(
                leftRight._key, leftRight._value,
                Balanced(left._key, left._value, left._left, leftRight._left),
                Balanced(key, value, leftRight._right, right));
        }

        private static TreeDictionary<TKey, TValue, TComparer> GlueBalanced(
            TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            if (left.Count() == 0)
                return right;
            if (right.Count() == 0)
                return left;
            TKey minKeyRight;
            TValue minKeyValueRight;
            var newRight = right.FindAndRemoveMinKeyFromNonEmpty(out minKeyRight, out minKeyValueRight);
            return NearlyBalanced(minKeyRight, minKeyValueRight, left, newRight);
        }

        private static TreeDictionary<TKey, TValue, TComparer> GlueUnbalanced(
            TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            if (left.Count() == 0)
                return right;
            if (right.Count() == 0)
                return left;
            if (DELTA * left.Count <= right.Count)
                return right.NearlyBalanced(GlueUnbalanced(left, right._left), right._right);
            if (DELTA * right.Count <= left.Count)
                return left.NearlyBalanced(left._left, GlueUnbalanced(left._right, right));
            return GlueBalanced(left, right);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of
        /// the same type. Dictionaries are considered equal, if they have the
        /// same keys and values.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the current object is equal to the 
        /// <paramref name="other"/> parameter; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        /// </param>
        public bool Equals(TreeDictionary<TKey, TValue, TComparer> other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _count == other._count && this.SequenceEqual(other);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(TreeDictionary<TKey, TValue, TComparer>)) return false;
            return Equals((TreeDictionary<TKey, TValue, TComparer>) obj);
        }

        public override int GetHashCode() {
            unchecked {
                int result = _count;
                result = (result * 397) ^ _key.GetHashCode();
                result = (result * 397) ^ (_value != null ? _value.GetHashCode() : 0);
                if (_left != null) {
                    result = (result * 397) ^ _left._key.GetHashCode();
                    result = (result * 397) ^ _left._value.GetHashCode();
                }
                if (_right != null) {
                    result = (result * 397) ^ _right._key.GetHashCode();
                    result = (result * 397) ^ _right._value.GetHashCode();
                }
                return result;
            }
        }

        /// <summary>
        /// Determines whether two instances of 
        /// <see cref="TreeDictionary&lt;TKey, TValue, TComparer&gt;"/> are
        /// equal (that is, have same keys and values).
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// <c>true</c> if the arguments are equal; <c>false</c> otherwise.
        /// </returns>
        public static bool operator ==(TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether two instances of 
        /// <see cref="TreeDictionary&lt;TKey, TValue, TComparer&gt;"/> are
        /// unequal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// <c>true</c> if the arguments are not equal; <c>false</c> otherwise.
        /// </returns>
        public static bool operator !=(TreeDictionary<TKey, TValue, TComparer> left, TreeDictionary<TKey, TValue, TComparer> right) {
            return !Equals(left, right);
        }
    }

    internal class ReturnThisException : Exception { }

    public static class TreeDictionary {
        internal static int Count<TKey, TValue, TComparer>(this TreeDictionary<TKey, TValue, TComparer> dict) where TComparer : IComparer<TKey>, new() {
            return dict != null ? dict.Count : 0;
        }

        public static TreeDictionary<TKey, TValue, TComparer> Empty<TKey, TValue, TComparer>() 
            where TComparer : IComparer<TKey>, new() {
            return TreeDictionary<TKey, TValue, TComparer>.Empty;
        }

        public static TreeDictionary<TKey, TValue, DefaultComparer<TKey>> Empty<TKey, TValue>() {
            return TreeDictionary<TKey, TValue, DefaultComparer<TKey>>.Empty;
        }

        public static TreeDictionary<TKey, TValue, TComparer> Single<TKey, TValue, TComparer>(
            TKey key, TValue value) where TComparer : IComparer<TKey>, new() {
            return new TreeDictionary<TKey, TValue, TComparer>(
                1, key, value, Empty<TKey, TValue, TComparer>(), Empty<TKey, TValue, TComparer>());
        }

        public static TreeDictionary<TKey, TValue, DefaultComparer<TKey>> Single<TKey, TValue>(
        TKey key, TValue value) {
            return new TreeDictionary<TKey, TValue, DefaultComparer<TKey>>(
                1, key, value, Empty<TKey, TValue>(), Empty<TKey, TValue>());
        }
    }
}
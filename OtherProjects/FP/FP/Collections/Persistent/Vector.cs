/*
* Vector.cs is part of functional-dotnet project
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
using System.Linq;
using FP.Core;
using FP.Util;
using FP.Validation;

// Inspired by the Scala implementation by Daniel Spiewak, 
// http://www.codecommit.com/blog/scala/implementing-persistent-vectors-in-scala
namespace FP.Collections.Persistent {
    /// <summary>
    /// A vector (nearly array-like sequence). Implemented as a 32-nary trie.
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    [Serializable]
    public sealed class Vector<T> : IAdjustableRandomAccessSequence<T, Vector<T>> {
        private const int BRANCHING = 32;

        /// <summary>
        /// Empty vector instance.
        /// </summary>
        public static readonly Vector<T> Empty = new Vector<T>(default(T), 0, Arrays.Empty<Vector<T>>());
        private static readonly Vector<T> _default = new Vector<T>(default(T), 1, Arrays.Empty<Vector<T>>());

        private readonly T _data;
        private readonly int _count;
        private readonly Vector<T>[] _branches;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="count">The count.</param>
        /// <param name="branches">The branches.</param>
        private Vector(T data, int count, Vector<T>[] branches) {
            _data = data;
            _count = count;
            _branches = branches;
        }

        /// <summary>
        /// Updates the element at <paramref name="index"/> using <paramref name="function"/>.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="function">The function to apply to the element currently at <paramref name="index"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        /// <remarks>
        /// Equivalent to <code>SetAt(index, function(this[index])), but faster.</code>
        /// </remarks>
        public Vector<T> AdjustAt(int index, Func<T, T> function) {
            return UpdateAtPath(Digits(index), function);
        }

        private Vector<T> UpdateAtPath(IEnumerable<int> path, Func<T, T> function) {
            if (path.IsEmpty())
                return new Vector<T>(function(_data), Math.Max(_count, 1), _branches);
            int head = path.First();
            IEnumerable<int> tail = path.Tail();
            int newBranchesLength = Math.Max(_branches.Length, head + 1);
            var newBranches = new Vector<T>[newBranchesLength];
            Vector<T> vector = head >= _branches.Length ? _default : (_branches[head] ?? _default);
            Array.Copy(_branches, newBranches, _branches.Length);
            newBranches[head] = vector.UpdateAtPath(tail, function);
            return new Vector<T>(_data, Math.Max(_count, Number(path) + 1), newBranches);
        }

        /// <summary>
        /// Appends the specified new value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>The new vector which contains all elements of this instance and 
        /// <paramref name="newValue"/>.</returns>
        public Vector<T> Append(T newValue) {
            return AdjustAt(_count, _ => newValue);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to
        /// iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator() {
            Vector<T> currentVector = this;
            var vectorStack = new Stack<Vector<T>>();
            var digitStack = new Stack<int>();
            for (int currentIndex = 0, lastDigit = 0;
                 currentIndex < _count;
                 currentIndex++, lastDigit++) {
                if (lastDigit == BRANCHING) {
                    int numZeroDigits = 0;
                    while (digitStack.Count > 0 && lastDigit == BRANCHING) {
                        numZeroDigits++;
                        vectorStack.Pop();
                        lastDigit = digitStack.Pop();
                        lastDigit++;
                    }
                    if (digitStack.Count == 0) {
                        vectorStack.Push(this);
                        if (lastDigit == BRANCHING) {
                            numZeroDigits++;
                            lastDigit = 1;
                        }
                    }
                    currentVector = vectorStack.Peek();
                    digitStack.Push(lastDigit);
                    currentVector = currentVector.Branches(lastDigit);
                    vectorStack.Push(currentVector);
                    lastDigit = 0;
                    for (int i = 0; i < numZeroDigits - 1; i++) {
                        digitStack.Push(lastDigit);
                        currentVector = currentVector.Branches(lastDigit);
                        vectorStack.Push(currentVector);
                    }
                }
                yield return currentVector.BranchesData(lastDigit);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the number of elements in the sequence.
        /// </summary>
        /// <value>The number of elements in the sequence.</value>
        public int Count {
            get { return _count; }
        }

        /// <summary>
        /// Gets a value indicating whether this collection is empty.
        /// </summary>
        /// <value><c>true</c>.</value>
        public bool IsEmpty {
            get { return _count == 0; }
        }

        /// <summary>
        /// Gets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        public T this[int index] {
            get {
                // Requires.That.IsIndexInRange(this, index, "index").Check();
                var vector = this;
                foreach (int i in Digits(index)) {
                    if (vector._branches.Length <= i || vector._branches[i] == null)
                        return default(T);
                    vector = vector._branches[i];
                }
                return vector._data;
            }
        }

        private Vector<T> Branches(int index) {
            return _branches.Length <= index || _branches[index] == null
                       ? _default
                       : _branches[index];
        }

        private T BranchesData(int index) {
            return _branches.Length <= index || _branches[index] == null
                       ? default(T)
                       : _branches[index]._data;
        }

        private static IEnumerable<int> Digits(int number) {
            return Digits(number, BRANCHING);
        }

        private static IEnumerable<int> Digits(int number, int @base) {
            var list = new List<int>(5);
            if (number == 0) return new[] { 0 };
            while (number > 0) {
                list.Add(number % @base);
                number /= @base;
            }
            list.Reverse();
            return list;
        }

        private static int Number(IEnumerable<int> digits) {
            return Number(digits, BRANCHING);
        }

        private static int Number(IEnumerable<int> digits, int @base) {
            int number = 0;
            foreach (int digit in digits) {
                number *= @base;
                number += digit;
            }
            return number;
        }
    }

    /// <summary>
    /// Utility class for instantiating vectors.
    /// </summary>
    public static class Vector {
        /// <summary>
        /// Creates a new vector containing the specified elements.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="ts">The elements.</param>
        /// <returns>The vector containing the specified elements.</returns>
        public static Vector<T> New<T>(IEnumerable<T> ts) {
            var vector = Vector<T>.Empty;
            int i = 0;
            foreach (T t in ts)
                vector = vector.SetAt(i++, t);
            return vector;
        }

        /// <summary>
        /// Creates a new vector containing the specified elements.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="ts">The elements.</param>
        /// <returns>The vector containing the specified elements.</returns>
        public static Vector<T> New<T>(params T[] ts) {
            return New(ts.AsEnumerable());
        }
    }
}
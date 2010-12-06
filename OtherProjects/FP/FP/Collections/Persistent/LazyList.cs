/*
* LazyList.cs is part of functional-dotnet project
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

// Code adapted from http://blogs.msdn.com/wesdyer/archive/2007/02/12/why-all-of-the-love-for-lists.aspx

using System.Collections;
using System.Collections.Generic;
using FP.Validation;

namespace FP.Collections.Persistent {

    /// <summary>
    /// A lazy singly linked list which allows saving the state of enumerators.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyList<T> : IList<T, LazyList<T>> {
        private readonly T _head;
        private LazyList<T> _tail;
        private IEnumerator<T> _enumerator;

        /// <summary>
        /// The empty list.
        /// </summary>
        public static readonly LazyList<T> Empty = new EmptyList();

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyList{T}"/> class.
        /// </summary>
        /// <param name="head">The head.</param>
        /// <param name="enumerator">The enumerator used to create following elements.
        /// Do _not_ hold on to any other references to it!</param>
        private LazyList(T head, IEnumerator<T> enumerator) {
            _head = head;
            _enumerator = enumerator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyList{T}"/> class.
        /// </summary>
        /// <param name="head">The head.</param>
        /// <param name="tail">The tail.</param>
        private LazyList(T head, LazyList<T> tail) {
            _head = head;
            _tail = tail;
        }

        /// <summary>
        /// Creates the list based on the specified enumerator.
        /// </summary>
        /// <param name="enumerator">The enumerator.
        /// Do _not_ hold on to any other references to it!</param>
        /// <returns></returns>
        internal static LazyList<T> Create(IEnumerator<T> enumerator) {
			enumerator.MoveNext();
			return new LazyList<T>(enumerator.Current, enumerator);
			//return enumerator.MoveNext()
			//           ? new LazyList<T>(enumerator.Current, enumerator)
			//           : Empty;
        }

        /// <summary>
        /// Gets the "head" (first element) of the list.
        /// </summary>
        /// <value>The head of the list.</value>
        /// <exception cref="EmptyEnumerableException">is the current list <see cref="IsEmpty"/>.</exception>
        public virtual T Head {
            get { return _head; }
        }

        /// <summary>
        /// Gets the "tail" (all elements but the first) of the list.
        /// </summary>
        /// <value>The tail of the list.</value>
        /// <exception cref="EmptyEnumerableException">is the current list <see cref="IsEmpty"/>.</exception>
        public virtual LazyList<T> Tail {
            get {
                if (_enumerator != null) {
                    _tail = Create(_enumerator);
                    _enumerator = null;
                }
                return _tail;
            } // get
        } // Tail

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public virtual bool IsEmpty {
            get { return false; }
        }

        /// <summary>
        /// Prepends a new head.
        /// </summary>
        /// <param name="newHead">The new head.</param>
        /// <returns>
        /// The list with <paramref name="newHead"/> as <see cref="Head"/>
        /// and the current list as <see cref="Tail"/>.
        /// </returns>
        public LazyList<T> Prepend(T newHead) {
            return new LazyList<T>(newHead, this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public virtual IEnumerator<T> GetEnumerator() {
            LazyList<T> list = this;
            do {
                yield return list.Head;
                list = list.Tail;
            }
            while (!list.IsEmpty);
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

        /// <summary>
        /// The empty list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class EmptyList : LazyList<T> {
            internal EmptyList() : base(default(T), (LazyList<T>) null) { }

            /// <summary>
            /// Gets the "head" (first element) of the list.
            /// </summary>
            /// <exception cref="EmptyEnumerableException"></exception>
            public override T Head {
                get { throw new EmptyEnumerableException(); }
            }

            /// <summary>
            /// Gets a value indicating whether this instance is empty.
            /// </summary>
            /// <value><c>true</c>.</value>
            public override bool IsEmpty {
                get { return true; }
            }

            /// <summary>
            /// Gets the "tail" (all elements but the first) of the list.
            /// </summary>
            /// <exception cref="EmptyEnumerableException">is the current list <see cref="IsEmpty"/>.</exception>
            public override LazyList<T> Tail {
                get { throw new EmptyEnumerableException(); }
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            public override IEnumerator<T> GetEnumerator() {
                yield break;
            }
        } // class EmptyList
    } // class LazyList
} // namespace FP.Collections.Immutable
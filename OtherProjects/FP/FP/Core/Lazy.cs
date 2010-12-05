/*
* Lazy.cs is part of functional-dotnet project
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

namespace FP.Core {
    /// <summary>
    /// A lazy value.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <remarks>The difference with <see cref="Lazy{T}"/> is that any
    /// exceptions thrown during calculation will be thrown immediately.
    /// </remarks>
    public class Lazy<T> {
        private readonly object _syncRoot;
        private T _value;
        private Func<T> _calculation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Lazy{T}"/> class.
        /// </summary>
        /// <param name="calculation">The calculation the new future will do on-demand.</param>
        /// <remarks><paramref name="calculation"/> should be side-effect-free.</remarks>
        public Lazy(Func<T> calculation) {
            _calculation = calculation;
            _syncRoot = new object();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lazy{T}"/> class which already
        /// holds a value.
        /// </summary>
        /// <param name="value">The result.</param>
        public Lazy(T value) {
            _calculation = null;
            _value = value;
        }

        /// <summary>
        /// Gets the value of the lazy calculation.
        /// </summary>
        /// <value>The value.</value>
        /// <remarks>If the calculation has not been done before, it is called and its
        /// result stored.</remarks>
        public T Value {
            get {
                Force();
                return _value;
            }
        }

        /// <summary>
        /// Forces calculation of this lazy calculation's value.
        /// </summary>
        internal void Force() {
            if (IsCompleted) return;
            lock (_syncRoot) {
                if (IsCompleted) return;
                _value = _calculation();
                _calculation = null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a result (is successful or failed).
        /// </summary>
        /// <value>
        /// <c>true</c> if this lazy value has been forced; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompleted {
            get { return _calculation == null; }
        }

        public override string ToString() {
            return IsCompleted ? _value.ToString() : "Lazy";
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Lazy{T}"/> to <see cref="T"/>.
        /// </summary>
        /// <param name="lazy">The lazy value.</param>
        /// <returns>The result of forcing this lazy value.</returns>
        public static implicit operator T(Lazy<T> lazy) {
            return lazy.Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="T"/> to <see cref="Lazy{T}"/>.
        /// </summary>
        /// <param name="t">The lazy value.</param>
        /// <returns>The ready future with result <paramref name="t"/>.</returns>
        public static implicit operator Lazy<T>(T t) {
            return new Lazy<T>(t);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Lazy{T}"/> to <see cref="System.Func{T}"/>.
        /// </summary>
        /// <param name="lazy">The lazy value.</param>
        /// <returns>The calculation needed to return the value.</returns>
        public static explicit operator Func<T>(Lazy<T> lazy) {
            return lazy.IsCompleted ? () => lazy._value : lazy._calculation;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Func{T}"/> to 
        /// <see cref="Lazy{T}"/>.
        /// </summary>
        /// <param name="calculation">The calculation the returned future will do
        /// on-demand.</param>
        /// <returns>The lazy future with the specified calculation.</returns>
        public static implicit operator Lazy<T>(Func<T> calculation) {
            return new Lazy<T>(calculation);
        }
    }

    /// <summary>
    /// A convenience class for creating lazy values.
    /// </summary>
    public static class Lazy {
        /// <summary>
        /// Returns a lazy value based on the specified calculation.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="calculation">The calculation.</param>
        /// <returns>The lazy value.</returns>
        public static Lazy<T> New<T>(Func<T> calculation) {
            return new Lazy<T>(calculation);
        }

        /// <summary>
        /// Returns a lazy value wrapping the specified value. This should
        /// rarely be necessary due to implicit conversion from 
        /// <typeparamref name="T"/> to <see cref="Lazy{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The lazy value.</returns>
        public static Lazy<T> New<T>(T value) {
            return new Lazy<T>(value);
        }
    }
}

/*
* IRef.cs is part of functional-dotnet project
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

namespace FP.Core {
    /// <summary>
    /// An interface for mutable references to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of reference's contents.</typeparam>
    /// <remarks>
    /// <para>Type <typeparamref name="T"/> should be immutable or at least the value
    /// should not be mutated.
    /// </para>
    /// <para>Behaves similarly to <c>'a ref</c> type in ML and atoms in Clojure.</para>
    /// <para>When implementing the interface, you can almost always derive from either
    /// <see cref="RefBase{T}"/> of <see cref="RefBaseEx{T}"/>.</para>
    /// <para>All members of any implementing class are required to be thread-safe.</para>
    /// </remarks>
    public interface IRef<T> {
        /// <summary>
        /// Gets the current value of the reference.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets the validator action.
        /// </summary>
        /// <value>The validator function.</value>
        /// <remarks><para>This can be set when creating the reference. Must be side-effect free.
        /// It will be called before any change of value with the intended new value as
        /// the argument and should throw an exception if the change is invalid.</para></remarks>
        Action<T> Validator { get; }

        /// <summary>
        /// Stores the specified new value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>The old value.</returns>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="Validator"/>.</exception>
        T Store(T newValue);

        /// <summary>
        /// Atomically sets <see cref="Value"/> to <paramref name="newValue"/> if and only
        /// if the current value of the atom is identical to <see cref="oldValue"/>
        /// according to <c>Value.Equals(oldValue)</c> and <c>Validator(newValue)</c>
        /// succeeds.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns><c>true</c> if the change happened, else <c>false</c>.</returns>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/>
        /// doesn't pass <see cref="Validator"/>.</exception>
        bool CompareAndSet(T oldValue, T newValue);

        /// <summary>
        /// Atomically replaces the current value of this ref with
        /// <c>f(Value)</c>.
        /// </summary>
        /// <param name="f">The function to apply to the value. Must be side-effect free.
        /// </param>
        /// <returns>The pair (value swapped out, value swapped in).
        /// </returns>
        /// <remarks>Reads the current <see cref="Value"/>, applies <paramref name="f"/> to
        /// it, and attempts to <see cref="CompareAndSet"/> the result in. If this doesn't
        /// succeed, retry in a spin loop. The net effect is that the value will always be
        /// the result of the application of the supplied function to a current value,
        /// atomically. However, <paramref name="f"/> may be called multiple times and
        /// therefore it must have no side effects.
        /// </remarks>
        /// <exception cref="RefValidationException"> when the new value doesn't pass 
        /// <see cref="Validator"/>.</exception>
        Tuple<T, T> Adjust(Func<T, T> f);

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The intended new value for this reference.</param>
        /// <exception cref="RefValidationException">when <paramref name="value"/> is
        /// not valid.</exception>
        void Validate(T value);
    }
}
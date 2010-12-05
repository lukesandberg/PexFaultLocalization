/*
* Optional.cs is part of functional-dotnet project
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
using FP.Collections.Persistent;
using FP.HaskellNames;

// ReSharper disable RedundantIfElseBlock

namespace FP.Core {
    /// <summary>
    /// This struct represents an optional value, like <see cref="Nullable{T}"/>, but
    /// works with reference types as well. Also known as <c>Option</c> in ML/F#/Scala.
    /// </summary>
    /// <typeparam name="T">The type of wrapped object.</typeparam>
    /// <remarks>
    /// While the class is declared to be <see cref="IEquatable{T}"/> and <see cref="IComparable{T}"/>,
    /// this is only true if <typeparamref name="T"/> is <see cref="IEquatable{T}"/> and <see cref="IComparable{T}"/>
    /// respectively!
    /// </remarks>
    /// <seealso cref="Nullable{T}"/>
    [Serializable]
    public struct Optional<T> : IEnumerable<T>, IComparable<Optional<T>>, IEquatable<Optional<T>>,
        IFoldable<T> {
        /// <summary>
        /// _value
        /// </summary>
        private readonly T _value;

        /// <summary>
        /// Gets the value, if it exists.
        /// </summary>
        /// <value>The value.</value>
        /// <exception cref="InvalidOperationException"><see cref="HasValue"/> is <c>false</c>.</exception>
        public T Value {
            get {
                if (HasValue)
                    return _value;
                else
                    throw new InvalidOperationException("Optional<T> doesn't have a value.");
            }
        } // Value

        /// <summary>
        /// Gets a value indicating whether this instance has value.
        /// </summary>
        /// <value><c>true</c> if this instance has value; otherwise, <c>false</c>.</value>
        public bool HasValue { get; private set; } // HasValue

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator() {
            if (HasValue)
                yield return _value;
        } // GetEnumerator()

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<T>)this).GetEnumerator();
        } // IEnumerable.GetEnumerator()

        /// <summary>
        /// Represents absence of value.
        /// </summary>
        /// <summary>
        /// ReSharper disable RedundantDefaultFieldInitializer
        /// </summary>
        public static readonly Optional<T> None = new Optional<T>();
        // ReSharper restore RedundantDefaultFieldInitializer

        /// <summary>
        /// Initializes a new instance of the <see cref="Optional{T}"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Optional(T value)
            : this() {
            HasValue = true;
            _value = value;
        } // Optional(value)

        /// <summary>
        /// If the current instance has a value, do <paramref name="action"/> with it.
        /// </summary>
        /// <param name="action">The action to try.</param>
        public void Do(Action<T> action) {
            DoOrElse(action, Functions.DoNothing);
        } // Do(action)

        /// <summary>
        /// If the current instance has a value, do <paramref name="action"/> with it.
        /// Otherwise do <paramref name="defaultAction"/>.
        /// </summary>
        /// <param name="action">The action to try.</param>
        /// <param name="defaultAction">The default action.</param>
        public void DoOrElse(Action<T> action, Action defaultAction) {
            if (HasValue)
                action(_value);
            else
                defaultAction();
        } // DoOrElse(action, defaultAction)

        /// <summary>
        /// If the current instance has a value, return it.
        /// Otherwise return <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public T ValueOrElse(T defaultValue) {
            return HasValue ? _value : defaultValue;
        } // ValueOrElse(defaultValue)

        /// <summary>
        /// If the current instance has a value, calls <paramref name="function"/> on it and returns the result.
        /// Otherwise returns <paramref name="defaultResult"/>.
        /// </summary>
        /// <param name="function">The function to call.</param>
        /// <param name="defaultResult">The default result.</param>
        public R MapOrElse<R>(Func<T, R> function, R defaultResult) {
            return HasValue ? function(_value) : defaultResult;
        } // MapOrElse(, function, defaultResult)

        /// <summary>
        /// If the current instance has a value, calls <paramref name="function"/> on it
        /// and returns the result. Otherwise calculates <paramref name="defaultResult"/> and
        /// returns it. This is the deferred version of
        /// <see cref="MapOrElse(Func{T,R},R)"/>.
        /// </summary>
        /// <param name="function">The function to call.</param>
        /// <param name="defaultResult">The function which calculates default result.</param>
        public R MapOrElse<R>(Func<T, R> function, Func<R> defaultResult) {
            return HasValue ? function(_value) : defaultResult();
        } // MapOrElse(, function, defaultResult)

        /// <summary>
        /// Maps the partial function.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="function">The function to map.</param>
        /// <returns><c>None</c> if </returns>
        public Optional<R> MapPartial<R>(Func<T, Optional<R>> function) {
            return MapOrElse(function, Optional<R>.None);
        } // MapPartial(, function)

        /// <summary>
        /// Maps the partial function.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="function">The function to map.</param>
        /// <returns><c>None</c> if </returns>
        public OptionalNotNull<R> MapPartial<R>(Func<T, OptionalNotNull<R>> function) {
            return MapOrElse(function, OptionalNotNull<R>.None);
        } // MapPartial(, function)

        /// <summary>
        /// If the current instance has a value, calls <paramref name="function"/> on it and returns <c>Some</c> the result.
        /// Otherwise returns <c>None</c>.
        /// </summary>
        /// <typeparam name="R">The return type of <paramref name="function"/>.</typeparam>
        /// <param name="function">The function.</param>
        public Optional<R> Map<R>(Func<T, R> function) {
            return HasValue ? Optional.Some(function(_value)) : Optional<R>.None;
        } // Map(, function)

        /// <summary>
        /// Returns the optional if it has a value and the predicate is true on this value.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public Optional<T> Filter(Func<T, bool> predicate) {
            return MapOrElse(predicate, false) ? this : None;
        }

        /// <summary>
        /// If this optional has a value, returns itself; else evaluates 
        /// <paramref name="@default"/> and returns the result.
        /// </summary>
        /// <param name="default">The @default.</param>
        public Optional<T> OrElse(Func<Optional<T>> @default) {
            return HasValue ? this : @default();
        }

        /// <summary>
        /// If this optional has a value, returns itself; else returns 
        /// <paramref name="@default"/>.
        /// </summary>
        /// <param name="default">The @default.</param>
        public Optional<T> OrElse(Optional<T> @default) {
            return HasValue ? this : @default;
        }

        /// <summary>
        /// Asserts the value is not null.
        /// </summary>
        /// <returns><c>Some(Value)</c> if this optional has a value and this value is not
        /// <c>null</c>; <c>None</c> if this optional doesn't have a value.</returns>
        /// <exception cref="InvalidOperationException">when the value exists and is 
        /// <c>null</c>.</exception>
        public OptionalNotNull<T> AssertNotNull() {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (HasValue)
                if (Value == null) {
                    throw new InvalidOperationException(
                        "Value can't be null", new ArgumentNullException("Value"));
                }
                else
                    return OptionalNotNull.Some(Value);
            else
                return OptionalNotNull.None<T>();
            // ReSharper restore CompareNonConstrainedGenericWithNull
        }

        /// <summary>
        /// Similar to the <c>??<\c> operator.
        /// </summary>
        /// <seealso cref="ValueOrElse"/>
        /// <example><c>None || Some(3) || Some(5) == Some(3).</c></example>
        public static Optional<T> operator |(Optional<T> optional, Optional<T> @default) {
            return optional.HasValue ? optional : @default;
        } // op_BitwiseOr(optional, @default)

        /// <summary>
        /// Implements the operator true.
        /// </summary>
        /// <param name="optional">The maybe.</param>
        /// <returns><c>true</c> if <paramref name="optional"/> has a value.</returns>
        public static bool operator true(Optional<T> optional) {
            return optional.HasValue;
        } // op_True(optional)

        /// <summary>
        /// Implements the operator false.
        /// </summary>
        /// <param name="optional">The maybe.</param>
        /// <returns><c>true</c> if <paramref name="optional"/> doesn't have a value.</returns>
        public static bool operator false(Optional<T> optional) {
            return !optional.HasValue;
        } // op_False(optional)

        /// <summary>
        /// Performs an implicit conversion from <see cref="T"/> to <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>Some(value)</c> if value is not null; <c>None</c> otherwise.</returns>
        /// <remarks>It is implicit by parallel with <see cref="Nullable{T}"/>.</remarks>
        public static implicit operator Optional<T>(T value) {
            return new Optional<T>(value);
        } // op_Implicit(value)

        /// <summary>
        /// Performs an explicit conversion from <see cref="Optional{T}"/> to <see cref="T"/>.
        /// </summary>
        /// <param name="optional">The maybe.</param>
        /// <returns><see cref="Value"/> if it exists; <c>default(T)</c> otherwise.</returns>
        public static explicit operator T(Optional<T> optional) {
            return optional.ValueOrElse(default(T));
        } // op_Explicit(optional)

        ///<summary>
        ///Compares the current object with another object of the same type. <c>None</c> is
        ///considered to be less than all <c>Some(value)</c>; if both objects have values,
        ///they are compared.
        ///</summary>
        ///
        ///<returns>
        ///A 32-bit signed integer that indicates the relative order of the objects being
        ///compared.  The return value has the following meanings: Value Meaning Less than
        ///zero This object is less than the <paramref name="other" /> parameter.Zero This
        ///object is equal to <paramref name="other" />. Greater than zero This object is
        ///greater than <paramref name="other" />. 
        ///</returns>
        ///
        ///<param name="other">An object to compare with this object.</param>
        /// <remarks>Requires that <typeparamref name="T"/> is <see cref="IComparable{T}"/>.
        /// Null is considered to be less than <c>None</c>.</remarks>
        public int CompareTo(Optional<T> other) {
            return HasValue
                       ? (other.HasValue
                              ? Comparer<T>.Default.Compare(_value, other._value)
                              : 1)
                       : (other.HasValue ? -1 : 0);
        } // CompareTo(other)

        /// <summary>
        /// Implements the equality operator. Calls <see cref="Equals(Optional{T})"/>.
        /// </summary>
        /// <param name="one">The one.</param>
        /// <param name="other">The other.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Optional<T> one, Optional<T> other) {
            return one.Equals(other);
        } // op_Equality(one, other)

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="one">The one.</param>
        /// <param name="other">The other.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Optional<T> one, Optional<T> other) {
            return !(one == other);
        } // op_Inequality(one, other)

        ///<summary>
        ///Indicates whether the current object is equal to another object of the same
        ///type.
        ///</summary>
        ///<returns>
        ///<c>true</c> if the current object is equal to the <paramref name="other" />
        ///parameter; otherwise, <c>false</c>.
        ///</returns>
        ///<param name="other">An object to compare with this object.</param>
        /// <remarks>Requires that <typeparamref name="T"/> is <see cref="IEquatable{T}"/>.
        /// </remarks>
        public bool Equals(Optional<T> other) {
            return HasValue
                       ? other.HasValue && _value.Equals(other._value)
                       : !other.HasValue;
        } // Equals(other)

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and
        /// represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj) {
            if (!(obj is Optional<T>)) return false;
            return Equals((Optional<T>)obj);
        } // Equals(obj)

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode() {
            return 29 * typeof(T).GetHashCode() + MapOrElse(v => v.GetHashCode(), 0);
        } // GetHashCode()

        /// <summary>
        /// Folds this optional value from the left.
        /// </summary>
        /// <typeparam name="TAcc">The type of the result.</typeparam>
        /// <param name="binOp">The binary operation.</param>
        /// <param name="initial">The initial value.</param>
        /// <returns><paramref name="binOp"/>(<paramref name="initial"/>, 
        /// <see cref="Value"/>) if this optional has a value; 
        /// <paramref name="initial"/> otherwise.</returns>
        public TAcc FoldLeft<TAcc>(Func<TAcc, T, TAcc> binOp, TAcc initial) {
            return HasValue ? binOp(initial, _value) : initial;
        } // FoldLeft(, binOp, initial)

        /// <summary>
        /// Folds this optional value from the right.
        /// </summary>
        /// <typeparam name="TAcc">The type of the result.</typeparam>
        /// <param name="binOp">The binary operation.</param>
        /// <param name="initial">The initial value.</param>
        /// <returns><paramref name="binOp"/>(<see cref="Value"/>, 
        /// <paramref name="initial"/>) if this optional has a value; 
        /// <paramref name="initial"/> otherwise.</returns>
        public TAcc FoldRight<TAcc>(Func<T, TAcc, TAcc> binOp, TAcc initial) {
            return HasValue ? binOp(_value, initial) : initial;
        } // FoldRight(, binOp, initial)

        public override string ToString() {
            return HasValue ? "Some(" + _value + ")" : "None";
        }
    } // struct Optional

    /// <summary>
    /// A convenience static class to provide static methods for 
    /// <see cref="Optional{T}"/> and <see cref="Nullable{T}"/>.
    /// </summary>
    public static class Optional {
        /// <summary>
        /// Return a wrapper around the specified object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="t">The object.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">if <paramref name="t"/> is <c>null</c>.
        /// </exception>
        public static Optional<T> Some<T>(T t) {
            return new Optional<T>(t);
        }

        /// <summary>
        /// Returns <see cref="Optional{T}.None"/>.
        /// </summary>
        public static Optional<T> None<T>() {
            return Optional<T>.None;
        }

        /// <summary>
        /// Converts a sequence to maybe.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns><c>None</c> if <paramref name="sequence"/> is empty;
        /// <c>Some(sequence.Item1())</c> otherwise.</returns>
        public static Optional<T> ToOptional<T>(this IEnumerable<T> sequence) {
            using (var enumerator = sequence.GetEnumerator())
                return enumerator.MoveNext() ? Some(enumerator.Current) : None<T>();
        }

        /// <summary>
        /// Converts <see cref="Optional{T}"/> to <see cref="Nullable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optional">The maybe.</param>
        /// <returns><c>null</c> if <paramref name="optional"/> is <c>None</c>;
        /// <c>optional.Value</c> otherwise.</returns>
        public static T? ToNullable<T>(this Optional<T> optional) where T : struct {
            return optional.MapOrElse(x => x, null);
        }

        /// <summary>
        /// Flattens the specified optional value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optional">The maybe.</param>
        /// <returns></returns>
        public static Optional<T> Flatten<T>(this Optional<Optional<T>> optional) {
            return optional.Map(m => m.Value);
        }

        /// <summary>
        /// Selects the values of elements which have them.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns></returns>
        public static IEnumerable<T> SelectValues<T>(this IEnumerable<Optional<T>> sequence) {
            foreach (var maybe in sequence) {
                if (maybe.HasValue)
                    yield return maybe.Value;
            }
        }

        /// <summary>
        /// A version of <see cref="Enumerables2.Map{T,TR}"/> which can throw away
        /// elements. In particular, if <c>function(x)</c> doesn't have a value for an
        /// element <c>x</c> of  the <paramref name="sequence"/>, no element is included in
        /// the result; if it has value <c>y</c>,
        /// <c>y</c> is included in the list.
        /// </summary>
        /// <typeparam name="T">Type of elements of the sequence.</typeparam>
        /// <typeparam name="R">Type of elements of the resulting sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public static IEnumerable<R> MapSome<T, R>(this IEnumerable<T> sequence,
                                                   Func<T, Optional<R>> function) {
            return SelectValues(sequence.Map(function));
        }

        /// <summary>
        /// Tries the specified function.
        /// </summary>
        /// <typeparam name="T">The return type of the function.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns><see cref="Optional{T}.None"/> if there were exceptions or the
        /// function returns <c>null</c>;
        /// <c>Some(function())</c> otherwise.</returns>
        public static Optional<T> Try<T>(this Func<T> function) {
            return Try<T, Exception>(function);
        }

        /// <summary>
        /// Tries the specified function, catching only exceptions of type 
        /// <typeparam name="E"/>.
        /// </summary>
        /// <typeparam name="T">The return type of the function.</typeparam>
        /// <typeparam name="E">The type of exceptions to catch.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns><see cref="Optional{T}.None"/> if there were exceptions or the
        /// function returns <c>null</c>;
        /// <c>Some(function())</c> otherwise.</returns>
        public static Optional<T> Try<T, E>(this Func<T> function) where E : Exception {
            try {
                return Some(function());
            }
            catch (E) {
                return None<T>();
            }
        }
    }
}

// ReSharper restore RedundantIfElseBlock
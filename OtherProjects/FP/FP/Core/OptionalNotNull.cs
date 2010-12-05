/*
* OptionalNotNull.cs is part of functional-dotnet project
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
using System.Data.Common;
using System.Globalization;
using System.Net;
using FP.HaskellNames;

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
    public struct OptionalNotNull<T> : IEnumerable<T>, IComparable<OptionalNotNull<T>>,
                                       IEquatable<OptionalNotNull<T>> {
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
                throw new InvalidOperationException("OptionalNotNull<T> doesn't have a value.");
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
        public static readonly OptionalNotNull<T> None = new OptionalNotNull<T>();
        // ReSharper restore RedundantDefaultFieldInitializer

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalNotNull{T}"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public OptionalNotNull(T value)
            : this() {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (value != null) {
                // ReSharper restore CompareNonConstrainedGenericWithNull
                HasValue = true;
                _value = value;
            }
            else {
                HasValue = false;
                _value = default(T);
            }
        } // OptionalNotNull(value)

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
        /// Otherwise return <paramref name="default"/>.
        /// </summary>
        /// <param name="default">The default value.</param>
        /// <returns></returns>
        public T ValueOrElse(T @default) {
            return HasValue ? _value : @default;
        } // ValueOrElse(@default)

        /// <summary>
        /// If this optional has a value, returns itself; else evaluates 
        /// <paramref name="@default"/> and returns the result.
        /// </summary>
        /// <param name="default">The @default.</param>
        /// <returns></returns>
        public OptionalNotNull<T> OrElse(Func<OptionalNotNull<T>> @default) {
            return HasValue ? this : @default();
        } // OrElse(@default)

        /// <summary>
        /// If this optional has a value, returns itself; else returns 
        /// <paramref name="@default"/>.
        /// </summary>
        /// <param name="default">The @default.</param>
        /// <returns></returns>
        public OptionalNotNull<T> OrElse(OptionalNotNull<T> @default) {
            return HasValue ? this : @default;
        } // OrElse(@default)

        /// <summary>
        /// Similar to the <c>??<\c> operator.
        /// </summary>
        /// <seealso cref="ValueOrElse"/>
        /// <example><c>None | 5 == 5.</c></example>
        public static T operator |(OptionalNotNull<T> optional, T @default) {
            return optional.HasValue ? optional.Value : @default;
        } // op_BitwiseOr(optional, @default)

        /// <summary>
        /// Similar to the <c>??<\c> operator.
        /// </summary>
        /// <seealso cref="ValueOrElse"/>
        /// <example><c>None || Some(3) || Some(5) == Some(3).</c></example>
        public static OptionalNotNull<T> operator |(
            OptionalNotNull<T> optional, OptionalNotNull<T> @default) {
            return optional.HasValue ? optional : @default;
        } // op_BitwiseOr(optional, @default)

        /// <summary>
        /// Implements the operator true.
        /// </summary>
        /// <param name="optional">The maybe.</param>
        /// <returns><c>true</c> if <paramref name="optional"/> has a value.</returns>
        public static bool operator true(OptionalNotNull<T> optional) {
            return optional.HasValue;
        } // op_True(optional)

        /// <summary>
        /// Implements the operator false.
        /// </summary>
        /// <param name="optional">The maybe.</param>
        /// <returns><c>true</c> if <paramref name="optional"/> doesn't have a value.</returns>
        public static bool operator false(OptionalNotNull<T> optional) {
            return !optional.HasValue;
        } // op_False(optional)

        /// <summary>
        /// If the current instance has a value, calls <paramref name="function"/> on it and returns the result.
        /// Otherwise returns <paramref name="default"/>.
        /// </summary>
        /// <param name="function">The function to call.</param>
        /// <param name="default">The default result.</param>
        public R MapOrElse<R>(Func<T, R> function, R @default) {
            return HasValue ? function(_value) : @default;
        } // MapOrElse(, function, @default)

        /// <summary>
        /// If the current instance has a value, calls <paramref name="function"/> on it
        /// and returns the result. Otherwise calculates <paramref name="default"/> and
        /// returns it. This is the lazy version of
        /// <see cref="MapOrElse(Func{T,R},R)"/>.
        /// </summary>
        /// <param name="function">The function to call.</param>
        /// <param name="default">The default result.</param>
        public R MapOrElse<R>(Func<T, R> function, Func<R> @default) {
            return HasValue ? function(_value) : @default();
        } // MapOrElse(, function, @default)

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
        /// Performs an explicit conversion from <see cref="T"/> to <see cref="OptionalNotNull{T}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>Some(value)</c> if value is not null; <c>None</c> otherwise.</returns>
        /// <remarks>It is implicit by parallel with <see cref="Nullable{T}"/>.</remarks>
        public static explicit operator OptionalNotNull<T>(T value) {
            return new OptionalNotNull<T>(value);
        } // op_Explicit(value)

        /// <summary>
        /// Performs an explicit conversion from <see cref="OptionalNotNull{T}"/> to <see cref="T"/>.
        /// </summary>
        /// <param name="optional">The maybe.</param>
        /// <returns><see cref="Value"/> if it exists; <c>default(T)</c> otherwise.</returns>
        public static explicit operator T(OptionalNotNull<T> optional) {
            return optional.ValueOrElse(default(T));
        } // op_Explicit(optional)

        /// <summary>
        /// Performs an explicit conversion from <see cref="T"/> to 
        /// <see cref="OptionalNotNull{T}"/>.
        /// </summary>
        /// <param name="optional">The value.</param>
        /// <returns><c>Some(optional)</c> if <paramref name="optional"/> is not null; 
        /// <c>None</c> otherwise.</returns>
        /// <remarks>It is implicit by parallel with <see cref="Nullable{T}"/>.</remarks>
        public static explicit operator OptionalNotNull<T>(Optional<T> optional) {
            return optional.MapOrElse(x => new OptionalNotNull<T>(x), None);
        } // op_Explicit(optional)

        /// <summary>
        /// Performs an explicit conversion from <see cref="OptionalNotNull{T}"/> to <see cref="T"/>.
        /// </summary>
        /// <param name="optional">The maybe.</param>
        /// <returns><see cref="Value"/> if it exists; <c>default(T)</c> otherwise.</returns>
        public static implicit operator Optional<T>(OptionalNotNull<T> optional) {
            return optional.MapOrElse(t => t, Optional<T>.None);
        } // op_Implicit(optional)

        /// <summary>
        /// Compares the current object with another object of the same type. 
        /// <c>None</c> is considered to be less than all <c>Some(value)</c>; if
        /// both objects have values, they are compared.
        /// </summary>
        /// 
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the
        /// objects being compared.  The return value has the following
        /// meanings: Value Meaning Less than zero This object is less than the 
        /// <paramref name="other" /> parameter.Zero This object is equal to 
        /// <paramref name="other" />. Greater than zero This object is greater
        /// than <paramref name="other" />. 
        /// </returns>
        ///
        /// <param name="other">An object to compare with this object.</param>
        /// <remarks>Requires that <typeparamref name="T"/> is 
        /// <see cref="IComparable{T}"/>. Null is considered to be less than 
        /// <c>None</c>.</remarks>
        public int CompareTo(OptionalNotNull<T> other) {
            return HasValue
                       ? (other.HasValue
                              ? Comparer<T>.Default.Compare(_value, other._value)
                              : 1)
                       : (other.HasValue ? -1 : 0);
        } // CompareTo(other)

        /// <summary>
        /// Implements the equality operator. Calls <see cref="Equals(OptionalNotNull{T})"/>.
        /// </summary>
        /// <param name="one">The one.</param>
        /// <param name="other">The other.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OptionalNotNull<T> one, OptionalNotNull<T> other) {
            return one.Equals(other);
        } // op_Equality(one, other)

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="one">The one.</param>
        /// <param name="other">The other.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OptionalNotNull<T> one, OptionalNotNull<T> other) {
            return !(one == other);
        } // op_Inequality(one, other)

        ///<summary>
        ///Indicates whether the current object is equal to another object of the same type.
        ///</summary>
        ///
        ///<returns>
        ///true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        ///</returns>
        ///
        ///<param name="other">An object to compare with this object.</param>
        /// <remarks>Requires that <typeparamref name="T"/> is <see cref="IEquatable{T}"/>.</remarks>
        public bool Equals(OptionalNotNull<T> other) {
            return HasValue
                       ? other.HasValue && _value.Equals(other._value)
                       : !other.HasValue;
        } // Equals(other)

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj) {
            if (!(obj is OptionalNotNull<T>)) return false;
            return Equals((OptionalNotNull<T>)obj);
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
    } // struct OptionalNotNull

    /// <summary>
    /// A convenience static class to provide static methods for <see cref="Optional{T}"/> and
    /// <see cref="Nullable{T}"/>.
    /// </summary>
    public static class OptionalNotNull {
        /// <summary>
        /// Return a wrapper around the specified non-null object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="t">The object.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">if <paramref name="t"/> is <c>null</c>.</exception>
        public static OptionalNotNull<T> Some<T>(T t) {
// ReSharper disable CompareNonConstrainedGenericWithNull
            if (t == null)
// ReSharper restore CompareNonConstrainedGenericWithNull
                throw new ArgumentNullException();
            return new OptionalNotNull<T>(t);
        }

        /// <summary>
        /// Returns <see cref="Optional{T}.None"/>.
        /// </summary>
        public static OptionalNotNull<T> None<T>() {
            return OptionalNotNull<T>.None;
        }

        /// <summary>
        /// Converts <see cref="Nullable{T}"/> to <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="nullable">The nullable.</param>
        /// <returns><c>None</c> if <paramref name="nullable"/> doesn't have a value;
        /// <c>Some(nullable.Value)</c> otherwise.</returns>
        public static OptionalNotNull<T> ToOptionalNotNull<T>(this T? nullable) where T : struct {
            return nullable.HasValue ? Some(nullable.Value) : None<T>();
        }

        /// <summary>
        /// An explicit conversion from <typeparamref name="T"/> to <see cref="OptionalNotNull{T}"/>.
        /// Works the same as the implicit cast.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static OptionalNotNull<T> Wrap<T>(T value) {
            return new OptionalNotNull<T>(value);
        }

        /// <summary>
        /// Flattens the specified optional.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optional">The maybe.</param>
        /// <returns></returns>
        public static OptionalNotNull<T> Flatten<T>(
            this OptionalNotNull<OptionalNotNull<T>> optional) {
            return optional.MapOrElse(m => m, OptionalNotNull<T>.None);
        }

        /// <summary>
        /// Selects the values of elements which have them.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns></returns>
        public static IEnumerable<T> SelectValues<T>(this IEnumerable<OptionalNotNull<T>> sequence) {
            foreach (var maybe in sequence) {
                if (maybe.HasValue)
                    yield return maybe.Value;
            }
        }

        /// <summary>
        /// Selects the values of elements which have them.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns></returns>
        public static IEnumerable<T> SelectValues<T>(this IEnumerable<T?> sequence) where T : struct {
            return SelectValues(sequence.Map(t => t.ToOptionalNotNull()));
        }

        /// <summary>
        /// A version of <see cref="Enumerables2.Map{T,TR}"/> which can throw away elements.
        /// In particular, if <c>function(x)</c> doesn't have a value for an element <c>x</c> of 
        /// the <paramref name="sequence"/>, no element is included in the result; if it has value <c>y</c>,
        /// <c>y</c> is included in the list.
        /// </summary>
        /// <typeparam name="T">Type of elements of the sequence.</typeparam>
        /// <typeparam name="R">Type of elements of the resulting sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public static IEnumerable<R> MapSome<T, R>(this IEnumerable<T> sequence,
                                                   Func<T, OptionalNotNull<R>> function) {
            return SelectValues(sequence.Map(function));
        }

        /// <summary>
        /// A version of <see cref="Enumerables2.Map{T,TR}"/> which can throw away elements.
        /// In particular, if <c>function(x)</c> doesn't have a value for an element <c>x</c> of 
        /// the <paramref name="sequence"/>, no element is included in the result; if it has value <c>y</c>,
        /// <c>y</c> is included in the list.
        /// </summary>
        /// <typeparam name="T">Type of elements of the sequence.</typeparam>
        /// <typeparam name="R">Type of elements of the resulting sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public static IEnumerable<R> MapSome<T, R>(this IEnumerable<T> sequence,
                                                   Func<T, R?> function) where R : struct {
            return SelectValues(sequence.Map(function));
        }

        #region Tries -- The versions of "Try..." methods from the framework

        /// <summary>
        /// Creates a new Uri using the specified String instance and a 
        /// <see cref="UriKind"/>.
        /// </summary>
        public static OptionalNotNull<Uri> CreateUri(string uriString, UriKind uriKind) {
            Uri uri;
            return Uri.TryCreate(uriString, uriKind, out uri) ? Wrap(uri) : None<Uri>();
        }

        /// <summary>
        /// Creates a new Uri using the specified base and relative String instances.
        /// </summary>
        public static OptionalNotNull<Uri> CreateUri(Uri baseUri, string relativeUri) {
            Uri uri;
            return Uri.TryCreate(baseUri, relativeUri, out uri) ? Wrap(uri) : None<Uri>();
        }

        /// <summary>
        /// Creates a new Uri using the specified base and relative Uri instances.
        /// </summary>
        public static OptionalNotNull<Uri> CreateUri(Uri baseUri, Uri relativeUri) {
            Uri uri;
            return Uri.TryCreate(baseUri, relativeUri, out uri) ? Wrap(uri) : None<Uri>();
        }

        /// <summary>
        /// Retrieves a value corresponding to the supplied key from this <see cref="DbConnectionStringBuilder"/>.
        /// </summary>
        public static OptionalNotNull<object> GetValue(
            this DbConnectionStringBuilder connectionStringBuilder, string keyword) {
            object value;
            return connectionStringBuilder.TryGetValue(keyword, out value)
                       ? Wrap(value)
                       : None<object>();
        }

        ///<summary>
        ///Contains static methods for parsing strings which return <see cref="Optional{T}"/>.
        ///</summary>
        public static class Parse {
            /// <summary>
            /// Parses a string which represents an <see cref="int"/>.
            /// </summary>
            public static OptionalNotNull<int> Int(string s) {
                int result;
                return int.TryParse(s, out result) ? Wrap(result) : None<int>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="int"/>.
            /// </summary>
            public static OptionalNotNull<int> Int(string s, NumberStyles styles,
                                                   IFormatProvider formatProvider) {
                int result;
                return int.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<int>();
            }

            /// <summary>
            /// Parses a string which represents a <see cref="T:System.DateTime"/>.
            /// </summary>
            public static OptionalNotNull<DateTime> DateTime(string s) {
                DateTime result;
                return System.DateTime.TryParse(s, out result)
                           ? Wrap(result)
                           : None<DateTime>();
            }

            /// <summary>
            /// Parses a string which represents a <see cref="T:System.DateTime"/>.
            /// </summary>
            public static OptionalNotNull<DateTime> DateTime(string s,
                                                             IFormatProvider formatProvider,
                                                             DateTimeStyles styles) {
                DateTime result;
                return System.DateTime.TryParse(s, formatProvider, styles, out result)
                           ? Wrap(result)
                           : None<DateTime>();
            }

            /// <summary>
            /// Parses a string which represents a <see cref="T:System.DateTimeOffset"/>.
            /// </summary>
            public static OptionalNotNull<DateTimeOffset> DateTimeOffset(string s) {
                DateTimeOffset result;
                return System.DateTimeOffset.TryParse(s, out result)
                           ? Wrap(result)
                           : None<DateTimeOffset>();
            }

            /// <summary>
            /// Parses a string which represents a <see cref="T:System.DateTimeOffset"/>.
            /// </summary>
            public static OptionalNotNull<DateTimeOffset> DateTimeOffset(string s,
                                                                         IFormatProvider
                                                                             formatProvider,
                                                                         DateTimeStyles styles) {
                DateTimeOffset result;
                return System.DateTimeOffset.TryParse(s, formatProvider, styles, out result)
                           ? Wrap(result)
                           : None<DateTimeOffset>();
            }

            /// <summary>
            /// Parses a string which represents a <see cref="T:System.DateTime"/>.
            /// </summary>
            public static OptionalNotNull<DateTime> ExactDateTime(string s, string format,
                                                                  IFormatProvider formatProvider,
                                                                  DateTimeStyles styles) {
                DateTime result;
                return System.DateTime.TryParseExact(s, format, formatProvider, styles, out result)
                           ? Wrap(result)
                           : None<DateTime>();
            }

            /// <summary>
            /// Parses a string which represents a <see cref="T:System.DateTime"/>.
            /// </summary>
            public static OptionalNotNull<DateTime> ExactDateTime(string s, string[] formats,
                                                                  IFormatProvider formatProvider,
                                                                  DateTimeStyles styles) {
                DateTime result;
                return System.DateTime.TryParseExact(s, formats, formatProvider, styles,
                                                     out result)
                           ? Wrap(result)
                           : None<DateTime>();
            }

            /// <summary>
            /// Parses a string which represents a <see cref="T:System.DateTimeOffset"/>.
            /// </summary>
            public static OptionalNotNull<DateTimeOffset> ExactDateTimeOffset(string s,
                                                                              string format,
                                                                              IFormatProvider
                                                                                  formatProvider,
                                                                              DateTimeStyles styles) {
                DateTimeOffset result;
                return System.DateTimeOffset.TryParseExact(s, format, formatProvider, styles,
                                                           out result)
                           ? Wrap(result)
                           : None<DateTimeOffset>();
            }

            /// <summary>
            /// Parses a string which represents a <see cref="T:System.DateTimeOffset"/>.
            /// </summary>
            public static OptionalNotNull<DateTimeOffset> ExactDateTimeOffset(string s,
                                                                              string[] formats,
                                                                              IFormatProvider
                                                                                  formatProvider,
                                                                              DateTimeStyles styles) {
                DateTimeOffset result;
                return System.DateTimeOffset.TryParseExact(s, formats, formatProvider, styles,
                                                           out result)
                           ? Wrap(result)
                           : None<DateTimeOffset>();
            }

            /// <summary>
            /// Parses a string which represents a <see cref="T:System.TimeSpan"/>.
            /// </summary>
            public static OptionalNotNull<TimeSpan> TimeSpan(string s) {
                TimeSpan result;
                return System.TimeSpan.TryParse(s, out result)
                           ? Wrap(result)
                           : None<TimeSpan>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="int"/>.
            /// </summary>
            public static OptionalNotNull<bool> Bool(string s) {
                bool result;
                return bool.TryParse(s, out result) ? Wrap(result) : None<bool>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="int"/>.
            /// </summary>
            public static OptionalNotNull<char> Char(string s) {
                char result;
                return char.TryParse(s, out result) ? Wrap(result) : None<char>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="byte"/>.
            /// </summary>
            public static OptionalNotNull<byte> Byte(string s) {
                byte result;
                return byte.TryParse(s, out result) ? Wrap(result) : None<byte>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="byte"/>.
            /// </summary>
            public static OptionalNotNull<byte> Byte(string s, NumberStyles styles,
                                                     IFormatProvider formatProvider) {
                byte result;
                return byte.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<byte>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="decimal"/>.
            /// </summary>
            public static OptionalNotNull<decimal> Decimal(string s) {
                decimal result;
                return decimal.TryParse(s, out result) ? Wrap(result) : None<decimal>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="decimal"/>.
            /// </summary>
            public static OptionalNotNull<decimal> Decimal(string s, NumberStyles styles,
                                                           IFormatProvider formatProvider) {
                decimal result;
                return decimal.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<decimal>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="double"/>.
            /// </summary>
            public static OptionalNotNull<double> Double(string s) {
                double result;
                return double.TryParse(s, out result) ? Wrap(result) : None<double>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="double"/>.
            /// </summary>
            public static OptionalNotNull<double> Double(string s, NumberStyles styles,
                                                         IFormatProvider formatProvider) {
                double result;
                return double.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<double>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="short"/>.
            /// </summary>
            public static OptionalNotNull<short> Short(string s) {
                short result;
                return short.TryParse(s, out result) ? Wrap(result) : None<short>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="short"/>.
            /// </summary>
            public static OptionalNotNull<short> Short(string s, NumberStyles styles,
                                                       IFormatProvider formatProvider) {
                short result;
                return short.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<short>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="long"/>.
            /// </summary>
            public static OptionalNotNull<long> Long(string s) {
                long result;
                return long.TryParse(s, out result) ? Wrap(result) : None<long>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="long"/>.
            /// </summary>
            public static OptionalNotNull<long> Long(string s, NumberStyles styles,
                                                     IFormatProvider formatProvider) {
                long result;
                return long.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<long>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="sbyte"/>.
            /// </summary>
            [CLSCompliant(false)]
            public static OptionalNotNull<sbyte> SByte(string s) {
                sbyte result;
                return sbyte.TryParse(s, out result) ? Wrap(result) : None<sbyte>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="sbyte"/>.
            /// </summary>
            [CLSCompliant(false)]
            public static OptionalNotNull<sbyte> SByte(string s, NumberStyles styles,
                                                       IFormatProvider formatProvider) {
                sbyte result;
                return sbyte.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<sbyte>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="float"/>.
            /// </summary>
            public static OptionalNotNull<float> Float(string s) {
                float result;
                return float.TryParse(s, out result) ? Wrap(result) : None<float>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="float"/>.
            /// </summary>
            public static OptionalNotNull<float> Float(string s, NumberStyles styles,
                                                       IFormatProvider formatProvider) {
                float result;
                return float.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<float>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="ushort"/>.
            /// </summary>
            [CLSCompliant(false)]
            public static OptionalNotNull<ushort> UShort(string s) {
                ushort result;
                return ushort.TryParse(s, out result) ? Wrap(result) : None<ushort>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="ushort"/>.
            /// </summary>
            [CLSCompliant(false)]
            public static OptionalNotNull<ushort> UShort(string s, NumberStyles styles,
                                                         IFormatProvider formatProvider) {
                ushort result;
                return ushort.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<ushort>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="uint"/>.
            /// </summary>
            [CLSCompliant(false)]
            public static OptionalNotNull<uint> UInt(string s) {
                uint result;
                return uint.TryParse(s, out result) ? Wrap(result) : None<uint>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="uint"/>.
            /// </summary>
            [CLSCompliant(false)]
            public static OptionalNotNull<uint> UInt(string s, NumberStyles styles,
                                                     IFormatProvider formatProvider) {
                uint result;
                return uint.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<uint>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="ulong"/>.
            /// </summary>
            [CLSCompliant(false)]
            public static OptionalNotNull<ulong> ULong(string s) {
                ulong result;
                return ulong.TryParse(s, out result) ? Wrap(result) : None<ulong>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="ulong"/>.
            /// </summary>
            [CLSCompliant(false)]
            public static OptionalNotNull<ulong> ULong(string s, NumberStyles styles,
                                                       IFormatProvider formatProvider) {
                ulong result;
                return ulong.TryParse(s, styles, formatProvider, out result)
                           ? Wrap(result)
                           : None<ulong>();
            }

            /// <summary>
            /// Parses a string which represents an <see cref="ulong"/>.
            /// </summary>
            public static OptionalNotNull<IPAddress> IPAddress(string s) {
                IPAddress result;
                return System.Net.IPAddress.TryParse(s, out result)
                           ? Wrap(result)
                           : None<IPAddress>();
            }
        }

        #endregion
    }
}
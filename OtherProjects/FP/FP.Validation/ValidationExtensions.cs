/*
* ValidationExtensions.cs is part of functional-dotnet project
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
using System.Collections.Generic;
using System.Linq;

namespace FP.Validation {
    /// <summary>
    /// Validation extensions
    /// </summary>
    public static class ValidationExtensions {
        /// <summary>
        /// Validate that the specified parameter is not null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsNotNull<T>(this Validation validation, T value, string paramName) {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (value != null) return validation;
            // ReSharper restore CompareNonConstrainedGenericWithNull
            return
                validation.AddException(new ArgumentNullException(paramName));
        } // IsNotNull(, validation, t)

        /// <summary>
        /// Validate that the specified parameter is not null. A more specific overload for
        /// <see cref="Nullable{T}"/> only.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsNotNull<T>(
            this Validation validation, T? value, string paramName) where T : struct {
            if (value.HasValue) return validation;
            return
                validation.AddException(new ArgumentNullException(paramName));
        } // IsNotNull(, validation, t)

        /// <summary>
        /// Validate that the specified parameter is in range between two bounds (both
        /// bounds are valid).
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="lowerBound">The lower bound (inclusive).</param>
        /// <param name="upperBound">The upper bound (inclusive).</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsInRange<T>(this Validation validation, T lowerBound, T upperBound, T value, string paramName)
        where T : IComparable<T> {
            if (value.CompareTo(lowerBound) >= 0 && value.CompareTo(upperBound) <= 0)
                return validation;
            return
                validation.AddException(
                    new ArgumentOutOfRangeException(
                        paramName,
                        string.Format("must be between {0} and {1}, but was {2}",
                                      lowerBound, upperBound, value)));
        } // IsInRange(validation, lowerBound, upperBound, value, paramName)

        /// <summary>
        /// Validate that the specified parameter is in range between two bounds (lower
        /// bound is valid, but upper bound is not).
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="lowerBound">The lower bound (inclusive).</param>
        /// <param name="upperBound">The upper bound (exclusive).</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsInUpperExclusiveRange<T>(this Validation validation, T lowerBound, T upperBound, T value, string paramName)
        where T : IComparable<T> {
            if (value.CompareTo(lowerBound) >= 0 && value.CompareTo(upperBound) < 0)
                return validation;
            return
                validation.AddException(
                    new ArgumentOutOfRangeException(
                        paramName,
                        string.Format("must be between {0} and {1} (exclusive), but was {2}", lowerBound, upperBound, value)));
        } // IsInRange(validation, lowerBound, upperBound, value, paramName)

        /// <summary>
        /// Validate that the specified parameter is in range between two bounds (neither
        /// bound is valid).
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="lowerBound">The lower bound (exclusive).</param>
        /// <param name="upperBound">The upper bound (exclusive).</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsInExclusiveRange<T>(this Validation validation, T lowerBound, T upperBound, T value, string paramName)
        where T : IComparable<T> {
            if (value.CompareTo(lowerBound) > 0 && value.CompareTo(upperBound) < 0)
                return validation;
            return
                validation.AddException(
                    new ArgumentOutOfRangeException(
                        paramName,
                        string.Format("must be strictly between {0} and {1}, but was {2}", lowerBound, upperBound, value)));
        } // IsInRange(validation, lowerBound, upperBound, value, paramName)

        /// <summary>
        /// Validate that the specified parameter is not less than the specified lower bound.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="lowerBound">The lower bound (inclusive).</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsAtLeast<T>(this Validation validation, T lowerBound, T value, string paramName)
        where T : IComparable<T> {
            if (value.CompareTo(lowerBound) >= 0)
                return validation;
            return
                validation.AddException(
                    new ArgumentOutOfRangeException(
                        paramName,
                        string.Format("must be at least {0}, but was {1}", lowerBound, value)));
        } // IsInRange(validation, lowerBound, upperBound, value, paramName)

        /// <summary>
        /// Validate that the specified parameter is greater than the specified lower bound.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="lowerBound">The lower bound (exclusive).</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsGreaterThan<T>(this Validation validation, T lowerBound, T value, string paramName)
        where T : IComparable<T> {
            if (value.CompareTo(lowerBound) > 0)
                return validation;
            return
                validation.AddException(
                    new ArgumentOutOfRangeException(
                        paramName,
                        string.Format("must be greater than {0}, but was {1}", lowerBound, value)));
        } // IsInRange(validation, lowerBound, upperBound, value, paramName)

        /// <summary>
        /// Validate that the specified parameter is not greater than the specified lower bound.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="upperBound">The upper bound (inclusive).</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsAtMost<T>(this Validation validation, T upperBound, T value, string paramName)
        where T : IComparable<T> {
            if (value.CompareTo(upperBound) <= 0)
                return validation;
            return
                validation.AddException(
                    new ArgumentOutOfRangeException(
                        paramName,
                        string.Format("must be at most {0}, but was {1}", upperBound, value)));
        } // IsInRange(validation, lowerBound, upperBound, value, paramName)

        /// <summary>
        /// Validate that the specified parameter is greater than the specified lower bound.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="upperBound">The upper bound (exclusive).</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsLessThan<T>(this Validation validation, T upperBound, T value, string paramName)
        where T : IComparable<T> {
            if (value.CompareTo(upperBound) < 0)
                return validation;
            return
                validation.AddException(
                    new ArgumentOutOfRangeException(
                        paramName,
                        string.Format("must be less than {0}, but was {1}", upperBound, value)));
        } // IsInRange(validation, lowerBound, upperBound, value, paramName)

        /// <summary>
        /// Check whether there were any exceptions. Do not forget to call this after
        /// setting conditions!
        /// </summary>
        /// <param name="validation">The validation.</param>
        public static Validation Check(this Validation validation) {
            if (validation != null)
                validation.Throw();
            return null;
        } // Throw(validation)

        /// <summary>
        /// Validate that the specified sequence is not empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="sequence"/>.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="sequence">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsNotEmpty<T>(
            this Validation validation, IEnumerable<T> sequence, string paramName) {
            if (sequence.Any())
                return validation;
            return
                validation.AddException(
                    new EmptyEnumerableException(paramName));
        } // IsNotEmpty(validation, sequence, paramName)

        /// <summary>
        /// Validate that the specified collection is not empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="collection"/>.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="collection">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsNotEmpty<T>(
            this Validation validation, ICollection<T> collection, string paramName) {
            if (collection.Count != 0)
                return validation;
            return
                validation.AddException(new EmptyEnumerableException(paramName));
        } // IsNotEmpty(validation, sequence, paramName)

        /// <summary>
        /// Validate that the specified collection contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="collection"/>.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="collection">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        public static Validation IsEmpty<T>(
            this Validation validation, ICollection<T> collection, string paramName) {
            if (collection.Count == 0)
                return validation;
            return
                validation.AddException(
                    new ArgumentException("must be empty, but it isn't", paramName));
        } // IsEmpty(validation, sequence, paramName)

        /// <summary>
        /// Validate that the specified parameter can be used as an index for 
        /// <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of sequence.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The value of the parameter.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static Validation IsIndexInRange<T>(
            this Validation validation, ICollection<T> collection, int index, string paramName) {
            if (index >= 0 && index < collection.Count)
                return validation;
            return validation.AddException(
                new ArgumentOutOfRangeException(
                    paramName,
                    string.Format(
                        "tried to access element with index {1} in a collection with {0} elements",
                        collection.Count, index)));
        }

        /// <summary>
        /// Validate that the specified parameter can be used as an index for 
        /// <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of sequence.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="array">The array.</param>
        /// <param name="index">The value of the parameter.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static Validation IsIndexInRange<T>(
            this Validation validation, T[] array, int index, string paramName) {
            if (index >= 0 && index < array.Length)
                return validation;
            return validation.AddException(
                new ArgumentOutOfRangeException(
                    paramName,
                    string.Format(
                        "tried to access element with index {1} in an array with {0} elements",
                        array.Length, index)));
        }

        /// <summary>
        /// Validate that the two specified parameters can be used as starting index and length for 
        /// a subsequence of <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of sequence.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The value of the index parameter.</param>
        /// <param name="count">The number of elements in the subsequence.</param>
        /// <param name="indexParamName">Name of the "index" parameter.</param>
        /// <param name="countParamName">Name of the "count" parameter.</param>
        public static Validation IsIndexAndCountInRange<T>(
            this Validation validation, ICollection<T> collection, int index, int count, string indexParamName, string countParamName) {
            if (index >= 0) {
                if (count >= 0 && index + count <= collection.Count)
                    return validation;
                return validation.AddException(
                    new ArgumentOutOfRangeException(
                        countParamName,
                        string.Format(
                            "tried to access the subsequence of {1} elements starting at {2} and ending at {3} in a collection with {0} elements",
                            collection.Count, count, index, index + count)));
            }
            return validation.AddException(
                new ArgumentOutOfRangeException(
                    indexParamName,
                    string.Format(
                        "tried to access the subsequence of {1} elements starting at {2} and ending at {3} in a collection with {0} elements",
                        collection.Count, count, index, index + count)));
        }

        /// <summary>
        /// Validate that the two specified parameters can be used as starting index and length for 
        /// a subsequence of <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of sequence.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="array">The collection.</param>
        /// <param name="index">The value of the index parameter.</param>
        /// <param name="count"></param>
        /// <param name="indexParamName">Name of the parameter.</param>
        /// <param name="countParamName"></param>
        public static Validation IsIndexAndCountInRange<T>(
            this Validation validation, T[] array, int index, int count, string indexParamName, string countParamName) {
            if (index >= 0) {
                if (count >= 0 && index + count <= array.Length)
                    return validation;
                return validation.AddException(
                    new ArgumentOutOfRangeException(
                        countParamName,
                        string.Format(
                            "tried to access the subsequence of {1} elements starting at {2} and ending at {3} in an array with {0} elements",
                            array.Length, count, index, index + count)));
            }
            return validation.AddException(
                new ArgumentOutOfRangeException(
                    indexParamName,
                    string.Format(
                        "tried to access the subsequence of {1} elements starting at {2} and ending at {3} in an array with {0} elements",
                        array.Length, count, index, index + count)));
        }

        /// <summary>
        /// Allocates a new <see cref="Validation"/> if <paramref name="validation"/> is 
        /// <c>null</c> and adds <paramref name="exception"/> to the list of exceptions.
        /// </summary>
        /// <param name="validation">The validation.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks>Use this method for adding new validations. Here is an example implementation
        /// of IsPositive:
        /// <example>
        /// <![CDATA[
        /// public static Validation IsPositive(this Validation validation, decimal value, 
        ///     string paramName) {
        ///     if (value > 0) return validation;
        ///     return
        ///         validation.AddException(
        ///             new ArgumentOutOfRangeException(paramName,
        ///                                             "must be positive, but was " + value));
        /// ]]></example>
        /// </remarks>
        public static Validation AddException(this Validation validation, ArgumentException exception) {
            return (validation ?? new Validation()).AddExceptionInternal(exception);
        }
    } // class ValidationExtensions
} // namespace FP.Validation
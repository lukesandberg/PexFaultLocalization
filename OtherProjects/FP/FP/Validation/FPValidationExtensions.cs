/*
* FPValidationExtensions.cs is part of functional-dotnet project
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
using FP.Collections.Persistent;

namespace FP.Validation {
    /// <summary>
    /// Extension methods on Validation for the types in FP.Core and FP.Collections.
    /// </summary>
    public static class FPValidationExtensions {
        /// <summary>
        /// Validate that the specified parameter can be used as an index for 
        /// <paramref name="seq"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of sequence.</typeparam>
        /// <typeparam name="TSeq">The type of the sequence.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="seq">The sequence.</param>
        /// <param name="index">The value of the parameter.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static Validation IsIndexInRange<T, TSeq>(
            this Validation validation, TSeq seq, int index, string paramName) where TSeq : IRandomAccess<T> {
            if (index >= 0 && index < seq.Count)
                return validation;
            return validation.AddException(
                new ArgumentOutOfRangeException(
                    paramName,
                    string.Format("the collection has {0} elements, but tried to access element with index {1}", seq.Count, index)));
        }

        /// <summary>
        /// Validate that the two specified parameters can be used as starting index and length for 
        /// a subsequence of <paramref name="seq"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of sequence.</typeparam>
        /// <typeparam name="TSeq">The type of the sequence.</typeparam>
        /// <param name="validation">The validation.</param>
        /// <param name="seq">The sequence.</param>
        /// <param name="index">The value of the index parameter.</param>
        /// <param name="count"></param>
        /// <param name="indexParamName">Name of the parameter.</param>
        /// <param name="countParamName"></param>
        public static Validation IsIndexAndCountInRange<T, TSeq>(
            this Validation validation, TSeq seq, int index, int count, string indexParamName, string countParamName) where TSeq : IRandomAccess<T> {
            if (index >= 0) {
                if (count >= 0 && index + count <= seq.Count)
                    return validation;
                return validation.AddException(
                    new ArgumentOutOfRangeException(
                        countParamName,
                        string.Format(
                            "the collection has {0} elements, but tried to access the subsequence of {1} elements starting at {2} and ending at {3}",
                            seq.Count, count, index, index + count)));
            }
            return validation.AddException(
                new ArgumentOutOfRangeException(
                    indexParamName,
                    string.Format(
                        "the collection has {0} elements, but tried to access the subsequence of {1} elements starting at {2} and ending at {3}",
                        seq.Count, count, index, index + count)));
        }

        /// <summary>
        /// Overload of <see cref="IsIndexInRange{T,TSeq}"/> for type inference.
        /// </summary>
        public static Validation IsIndexInRange<T>(this Validation validation, RandomAccessSequence<T> seq, int index, string paramName) {
            return validation.IsIndexInRange<T, RandomAccessSequence<T>>(seq, index, paramName);
        }

        /// <summary>
        /// Overload of <see cref="IsIndexAndCountInRange{T,TSeq}"/> for type inference.
        /// </summary>
        public static Validation IsIndexAndCountInRange<T>(this Validation validation, RandomAccessSequence<T> seq, int index, int count, string indexParamName, string countParamName) {
            return validation.IsIndexAndCountInRange<T, RandomAccessSequence<T>>(seq, index, count, indexParamName, countParamName);
        }

        /// <summary>
        /// Overload of <see cref="IsIndexInRange{T,TSeq}"/> for type inference.
        /// </summary>
        public static Validation IsIndexInRange<T>(this Validation validation, IRandomAccess<T> seq, int index, string paramName) {
            return validation.IsIndexInRange<T, IRandomAccess<T>>(seq, index, paramName);
        }

        /// <summary>
        /// Overload of <see cref="IsIndexAndCountInRange{T,TSeq}"/> for type inference.
        /// </summary>
        public static Validation IsIndexAndCountInRange<T>(this Validation validation, IRandomAccess<T> seq, int index, int count, string indexParamName, string countParamName) {
            return validation.IsIndexAndCountInRange<T, IRandomAccess<T>>(seq, index, count, indexParamName, countParamName);
        }

    } // class FPValidationExtensions
} // namespace FP.Validation

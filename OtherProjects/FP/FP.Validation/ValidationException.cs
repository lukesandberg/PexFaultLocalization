/*
* ValidationException.cs is part of functional-dotnet project
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
using System.Runtime.Serialization;

namespace FP.Validation {
    /// <summary>
    /// Exception thrown when validation fails. Simply wraps the original exception if
    /// there was only one, or a <see cref="MultiException"/> otherwise. Needed to
    /// prevent losing the stack trace.
    /// </summary>
    [Serializable]
    public class ValidationException : ArgumentException {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public ValidationException(Exception innerException) : base(null, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerExceptions">The inner exception.</param>
        public ValidationException(string message, ArgumentException[] innerExceptions) : base(message, AggregateException(innerExceptions)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="innerExceptions">The inner exception.</param>
        public ValidationException(ArgumentException[] innerExceptions) : base(null, AggregateException(innerExceptions)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) {}

        private static ArgumentException AggregateException(ArgumentException[] exceptions) {
            return exceptions.Length == 1
                       ? exceptions[0]
                       : new MultiException(exceptions);
        }
    }
}
/*
* EmptyEnumerableException.cs is part of functional-dotnet project
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
    /// Thrown by the methods which require their argument to be non-empty
    /// when called on an empty sequence.
    /// </summary>
    public class EmptyEnumerableException : ArgumentException {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyEnumerableException"/> class.
        /// </summary>
        public EmptyEnumerableException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyEnumerableException"/> class.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        public EmptyEnumerableException(string paramName) : base("must not be empty, but it was", paramName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyEnumerableException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EmptyEnumerableException(string message, Exception innerException)
            : base(message, innerException) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyEnumerableException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="innerException">The inner exception.</param>
        public EmptyEnumerableException(string message, string paramName, Exception innerException) : base(message, paramName, innerException) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyEnumerableException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="paramName">Name of the param.</param>
        public EmptyEnumerableException(string message, string paramName) : base(message, paramName) {}

        protected EmptyEnumerableException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}
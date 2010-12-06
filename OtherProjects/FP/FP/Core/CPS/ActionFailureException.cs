/*
* ActionFailureException.cs is part of functional-dotnet project
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

namespace FP.Core.CPS {
    /// <summary>
    /// Exception thrown when a <see cref="ActionCPS{T}"/> fails.
    /// </summary>
    [Serializable]
    public sealed class ActionFailureException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionFailureException"/> class.
        /// </summary>
        public ActionFailureException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionFailureException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ActionFailureException(string message, Exception innerException)
            : base(message, innerException) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionFailureException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ActionFailureException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionFailureException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public ActionFailureException(Exception innerException)
            : base(innerException.Message, innerException) {}
    } // class ActionFailureException
} // namespace FP.Core.CPS
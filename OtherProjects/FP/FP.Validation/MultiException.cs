/*
* MultiException.cs is part of functional-dotnet project
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
using System.Runtime.Serialization;
using System.Text;

/// <summary>
/// An exception type which wraps several exceptions at once.
/// </summary>
[Serializable]
public sealed class MultiException : ArgumentException {
    private readonly ArgumentException[] _innerExceptions;

    /// <summary>
    /// Inner exceptions.
    /// </summary>
    public IEnumerable<ArgumentException> InnerExceptions {
        get { return _innerExceptions ?? new ArgumentException[0]; }
    } // InnerExceptions

    /// <summary>
    /// Builds a message from the <paramref name="exceptions"/>' messages.
    /// </summary>
    /// <param name="exceptions">Exceptions.</param>
    /// <exception cref="ArgumentNullException"><see cref="exceptions"/> contains a
    /// <c>null</c>.</exception>
    private static string BuildMessage(IEnumerable<ArgumentException> exceptions) {
        var sb = new StringBuilder();
        foreach (var exception in exceptions) {
            if (exception == null)
                throw new ArgumentNullException();
            sb.AppendLine(exception.Message);
        }
        return sb.ToString();
    } // BuildMessage(exceptions)

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public MultiException(string message, ArgumentException innerException)
        : base(message, innerException) {
        _innerExceptions = new[] { innerException };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiException"/> class.
    /// </summary>
    /// <param name="innerExceptions">The inner exceptions.</param>
    public MultiException(params ArgumentException[] innerExceptions) : 
        this(BuildMessage(innerExceptions), innerExceptions) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerExceptions">The inner exceptions.</param>
    public MultiException(string message, params ArgumentException[] innerExceptions)
        : base(message, innerExceptions.FirstOrDefault()) {
        if (innerExceptions.Contains(null))
            throw new ArgumentNullException();

        _innerExceptions = innerExceptions;
    } // MultiException(message, innerExceptions)

    /// <summary>
    /// Create multi exception
    /// </summary>
    /// <param name="innerExceptions">The inner exceptions.</param>
    public MultiException(IEnumerable<ArgumentException> innerExceptions) : 
        this(BuildMessage(innerExceptions), innerExceptions) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerExceptions">The inner exceptions.</param>
    public MultiException(string message, IEnumerable<ArgumentException> innerExceptions)
        : this(message, innerExceptions.ToArray()) {
    } // MultiException(message, innerExceptions)

    /// <summary>
    /// Create multi exception
    /// </summary>
    private MultiException(SerializationInfo info, StreamingContext context)
        : base(info, context) { } // MultiException(info, context)
} // class MultiException

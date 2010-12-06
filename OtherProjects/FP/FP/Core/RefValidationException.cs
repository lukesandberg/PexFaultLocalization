using System;

namespace FP.Core {
    /// <summary>
    /// Exception thrown when an attempt to change the <see cref="IRef{T}.Value"/> of
    /// an <see cref="IRef{T}"/> to an invalid one is made.
    /// </summary>
    [Serializable]
    public sealed class RefValidationException : ArgumentException {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefValidationException"/> class.
        /// </summary>
        public RefValidationException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="RefValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public RefValidationException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="RefValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RefValidationException(string message, Exception innerException) : base(message, innerException) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="RefValidationException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public RefValidationException(Exception innerException) : base("Invalid value for an IRef", innerException) {}
    }
}
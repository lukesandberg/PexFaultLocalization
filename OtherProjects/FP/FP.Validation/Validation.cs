using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FP.Validation {
    /// <summary>
    /// Represents validation of preconditions.
    /// </summary>
    /// <remarks>Cannot be constructed directly; call <see cref="Requires.That"/> to
    /// begin validation and extension methods from <see cref="ValidationExtensions"/> to
    /// check conditions.</remarks>
    /// <seealso cref="ValidationExtensions"/>
    public class Validation {
        private readonly List<ArgumentException> _exceptions;

        internal ArgumentException[] Exceptions {
            get { return _exceptions.ToArray(); }
        }

        internal Validation AddExceptionInternal(ArgumentException ex) {
            // lock (_exceptions) { // We should never validate a method's arguments in two threads concurrently
            _exceptions.Add(ex);
            // }
            return this;
        }

        internal Validation() {
            _exceptions = new List<ArgumentException>(1); // optimize for only having 1 exception
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Validation"/> is reclaimed by garbage collection.
        /// </summary>
        /// <exception cref="ValidationException">validation.Check() was not called!
        /// </exception>
        /// <remarks>Generally destructor should never throw exceptions, but I believe it's
        /// appropriate in this case.</remarks>
        ~Validation() {
            if (_exceptions.Count != 0)
                throw new ValidationException("validation.Check() was not called!", Exceptions);
        }

        [DebuggerNonUserCode]
        internal virtual void Throw() {
            Exception exception = _exceptions.Count == 1
                          ? _exceptions[0]
                          : new MultiException(Exceptions);
            _exceptions.Clear();
            throw exception;
        }
    }
}
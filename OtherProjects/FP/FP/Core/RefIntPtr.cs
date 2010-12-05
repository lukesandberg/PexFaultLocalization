using System;
using System.Threading;

namespace FP.Core {
    /// <summary>
    /// Type of mutable references to (immutable) objects.
    /// </summary>
    internal sealed class RefIntPtr : RefBaseEx<IntPtr> {
        /// <summary>
        /// Creates a reference with the specified initial value.
        /// </summary>
        /// <param name="value">The value.</param>
        public RefIntPtr(IntPtr value) : this(value, null) { }

        /// <summary>
        /// Creates a reference with the specified initial value and validator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="validator">The validator function.</param>
        /// <exception cref="RefValidationException">when <paramref name="value"/> doesn't
        /// pass <paramref name="validator"/>.</exception>
        public RefIntPtr(IntPtr value, Action<IntPtr> validator) : base(value, validator) { }

        /// <summary>
        /// Stores the specified new value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="IRef{T}.Validator"/>.</exception>
        public override IntPtr Store(IntPtr newValue) {
            Validate(newValue);
            return Interlocked.Exchange(ref _value, newValue);
        }

        /// <summary>
        /// Atomically sets <see cref="IRef{T}.Value"/> to <paramref name="newValue"/> if and only
        /// if the current value of the atom is identical to <see cref="oldValue"/>
        /// according to <c>Value.Equals(oldValue)</c> and <c>Validator(newValue)</c>
        /// succeeds.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns><c>true</c> if the change happened, else <c>false</c>.</returns>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="IRef{T}.Validator"/>.</exception>
        public override bool CompareAndSet(IntPtr oldValue, IntPtr newValue) {
            Validate(newValue);
            IntPtr currentValue = Interlocked.CompareExchange(ref _value, newValue, oldValue);
            return oldValue == currentValue;
        }
    }
}
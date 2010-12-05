using System;
using System.Threading;

namespace FP.Core {
    /// <summary>
    /// Type of mutable references to <see cref="double"/>.
    /// </summary>
    internal sealed class RefDouble : RefBaseEx<double> {
        /// <summary>
        /// Creates a reference with the specified initial value.
        /// </summary>
        /// <param name="value">The value.</param>
        public RefDouble(double value) : this(value, null) { }

        /// <summary>
        /// Creates a reference with the specified initial value and validator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="validator">The validator function.</param>
        /// <exception cref="RefValidationException">when <paramref name="value"/> doesn't
        /// pass <paramref name="validator"/>.</exception>
        public RefDouble(double value, Action<double> validator) : base(value, validator) { }

        /// <summary>
        /// Stores the specified new value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="RefBase{T}.Validator"/>.</exception>
        public override double Store(double newValue) {
            Validate(newValue);
            return Interlocked.Exchange(ref _value, newValue);
        }

        /// <summary>
        /// Atomically sets <see cref="RefBase{T}.Value"/> to <paramref name="newValue"/> if and only
        /// if the current value of the atom is identical to <see cref="oldValue"/>
        /// according to <c>Value.Equals(oldValue)</c> and <c>Validator(newValue)</c>
        /// succeeds.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns><c>true</c> if the change happened, else <c>false</c>.</returns>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="RefBase{T}.Validator"/>.</exception>
        public override bool CompareAndSet(double oldValue, double newValue) {
            Validate(newValue);
            double currentValue = Interlocked.CompareExchange(ref _value, newValue, oldValue);
            return oldValue == currentValue;
        }
    }
}
using System;
using System.Threading;

namespace FP.Core {
    /// <summary>
    /// Type of mutable references to <see cref="bool"/>.
    /// </summary>
    public class RefBool : RefBase<bool> {
        private int _value;

        /// <summary>
        /// Creates a reference with the specified initial value.
        /// </summary>
        /// <param name="value">The value.</param>
        public RefBool(bool value) : this(value, null) { }

        /// <summary>
        /// Creates a reference with the specified initial value and validator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="validator">The validator function.</param>
        /// <exception cref="RefValidationException">when <paramref name="value"/> doesn't
        /// pass <paramref name="validator"/>.</exception>
        public RefBool(bool value, Action<bool> validator) : base(value, validator) {
            Store(value);
        }

        public override bool Value {
            get { return _value == 1; }
        }

        /// <summary>
        /// Stores the specified new value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="RefBase{T}.Validator"/>.</exception>
        public sealed override bool Store(bool newValue) {
            Validate(newValue);
            return Interlocked.Exchange(ref _value, newValue ? 1 : 0) == 1;
        }

        /// <summary>
        /// Atomically sets <see cref="Value"/> to <paramref name="newValue"/> if and only
        /// if the current value of the atom is identical to <see cref="oldValue"/>
        /// according to <c>Value.Equals(oldValue)</c> and <c>Validator(newValue)</c>
        /// succeeds.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns><c>true</c> if the change happened, else <c>false</c>.</returns>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="RefBase{T}.Validator"/>.</exception>
        public override bool CompareAndSet(bool oldValue, bool newValue) {
            Validate(newValue);
            bool currentValue =
                Interlocked.CompareExchange(ref _value, newValue ? 1 : 0, oldValue ? 1 : 0) == 1;
            return oldValue == currentValue;
        }
    }
}
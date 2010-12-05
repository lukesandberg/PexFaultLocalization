using System;

namespace FP.Core {
    /// <summary>
    /// An extended base class for implementing <see cref="IRef{T}"/>.
    /// </summary>
    /// <remarks>
    /// Adds a protected <see cref="_value"/> field and <see cref="Value"/> to 
    /// <see cref="RefBase{T}"/>.
    /// </remarks>
    public abstract class RefBaseEx<T> : RefBase<T> {
        [CLSCompliant(false)]
        protected T _value;

        /// <summary>
        /// Creates a reference with the specified initial value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected RefBaseEx(T value) : this(value, null) {}

        /// <summary>
        /// Creates a reference with the specified initial value and validator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="validator">The validator function.</param>
        /// <exception cref="RefValidationException">when <paramref name="value"/> doesn't
        /// pass <paramref name="validator"/>.</exception>
        protected RefBaseEx(T value, Action<T> validator) : base(value, validator) {
            _value = value;
        }

        /// <summary>
        /// Value of the reference.
        /// </summary>
        public override T Value {
            get { return _value; }
        }

        /// <summary>
        /// Stores the specified new value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>The old value.</returns>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="RefBase{T}.Validator"/>.</exception>
        public abstract override T Store(T newValue);

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
        public abstract override bool CompareAndSet(T oldValue, T newValue);
    }
}
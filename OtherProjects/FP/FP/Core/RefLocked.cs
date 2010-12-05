using System;

namespace FP.Core {
    /// <summary>
    /// The type of mutable references to <typeparamref name="T"/> which use locks to
    /// make access atomic.
    /// </summary>
    /// <typeparam name="T">The type of values.</typeparam>
    /// <remarks>Type <typeparamref name="T"/> should be immutable or at least the
    /// value should not be mutated. In this case the use of this type is thread-safe.
    /// All value changes except directly mutating the value are atomic.
    /// 
    /// Similar to Alice ML's/OCaml's ref type and Clojure's atom type.</remarks>
    internal sealed class RefLocked<T> : RefBaseEx<T> {
        readonly object _syncRoot = new object();

        /// <summary>
        /// Creates a reference with the specified initial value.
        /// </summary>
        /// <param name="value">The value.</param>
        public RefLocked(T value) : base(value) {}

        /// <summary>
        /// Creates a reference with the specified initial value and validator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="validator">The validator function.</param>
        /// <exception cref="RefValidationException">when <paramref name="value"/> doesn't
        /// pass <paramref name="validator"/>.</exception>
        public RefLocked(T value, Action<T> validator) : base(value, validator) {}

        /// <summary>
        /// Stores the specified new value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>The old value.</returns>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="RefBase{T}.Validator"/>.</exception>
        public override T Store(T newValue) {
            Validate(newValue);
            lock (_syncRoot) {
                T tmp = _value;
                _value = newValue;
                return tmp;
            }
        } // Store(newValue)

        /// <summary>
        /// Atomically sets <see cref="RefBaseEx{T}.Value"/> to <paramref name="newValue"/> if and only
        /// if the current value of the atom is identical to <see cref="oldValue"/>
        /// according to <c>Value.Equals(oldValue)</c> and <c>Validator(newValue)</c>
        /// succeeds.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns><c>true</c> if the change happened, else <c>false</c>.</returns>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="RefBase{T}.Validator"/>.</exception>
        public override bool CompareAndSet(T oldValue, T newValue) {
            Validate(newValue);
            lock (_syncRoot) {
                if (!Value.Equals(oldValue))
                    return false;
                Store(newValue);
                return true;
            }
        } // CompareAndSet(oldValue, newValue)

        // Adjust()
    }
}
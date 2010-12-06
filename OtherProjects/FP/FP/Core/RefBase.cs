using System;

namespace FP.Core {
    /// <summary>
    /// A base class for implementing <see cref="IRef{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of reference's contents.</typeparam>
    /// <remarks>Contains an implementation of <see cref="IRef{T}.Adjust"/> and an
    /// automatic property for <see cref="Validator"/>. Therefore, implementors just
    /// need to provide overrides for <see cref="Value"/>, <see cref="Store"/>, and 
    /// <see cref="CompareAndSet"/>.</remarks>
    public abstract class RefBase<T> : IRef<T> {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefBase{T}"/> class
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="validator">The validator function.</param>
        /// <exception cref="RefValidationException">when <paramref name="value"/> doesn't
        /// pass <paramref name="validator"/>.</exception>
        /// <remarks>This constructor validates the value, but doesn't actually store it,
        /// as that would require calling a virtual method. Do not forget to store the
        /// value in any derived constructors.</remarks>
        protected RefBase(T value, Action<T> validator) {
            Validator = validator;
            Validate(value);
        }

        /// <summary>
        /// Gets the current value of the reference.
        /// </summary>
        /// <value></value>
        public abstract T Value { get; }

        /// <summary>
        /// Gets the validator action.
        /// </summary>
        /// <value>The validator function.</value>
        /// <remarks>
        /// This can be set when creating the reference. Must be side-effect free.
        /// It will be called before any change of value with the intended new value as
        /// the argument and should throw an exception if the change is invalid.
        /// </remarks>
        public Action<T> Validator { get; protected set; }

        /// <summary>
        /// Stores the specified new value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>The old value.</returns>
        /// <exception cref="RefValidationException">when <paramref name="newValue"/> doesn't
        /// pass <see cref="Validator"/>.</exception>
        public abstract T Store(T newValue);

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
        /// pass <see cref="Validator"/>.</exception>
        public abstract bool CompareAndSet(T oldValue, T newValue);

        /// <summary>
        /// Atomically replaces the current value of this ref with
        /// <c>f(Value)</c>.
        /// </summary>
        /// <param name="f">The function to apply to the value. Must be side-effect free.
        /// </param>
        /// <returns>The pair (value swapped out, value swapped in).
        /// </returns>
        /// <remarks>Reads the current <see cref="Value"/>, applies <paramref name="f"/> to
        /// it, and attempts to <see cref="CompareAndSet(T,T)"/> the result in. If this doesn't
        /// succeed, retry in a spin loop. The net effect is that the value will always be
        /// the result of the application of the supplied function to a current value,
        /// atomically. However, because <paramref name="f"/> might be called multiple
        /// times, it must be free of side effects. <c>false</c> is only returned if
        /// validation of the replacement value fails.
        /// </remarks>
        /// <exception cref="RefValidationException"> when the new value doesn't
        /// pass <see cref="Validator"/>.</exception>
        public Tuple<T, T> Adjust(Func<T, T> f) {
            T oldValue;
            T newValue;
            do {
                oldValue = Value;
                newValue = f(oldValue);
                Validate(newValue);
            }
            while (!CompareAndSet(oldValue, newValue));
            return Tuple.New(oldValue, newValue);
        }

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The intended new value for this reference.</param>
        /// <exception cref="RefValidationException">when <paramref name="value"/> is
        /// not valid.</exception>
        public void Validate(T value) {
            if (Validator == null) 
                return;

            try {
                Validator(value);
            }
            catch (Exception e) {
                throw new RefValidationException(e);
            }
        }
    }
}
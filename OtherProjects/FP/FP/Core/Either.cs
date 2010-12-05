/*
* Either.cs is part of functional-dotnet project
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

namespace FP.Core {
    /// <summary>
    /// A static class to help with type inference.
    /// </summary>
    public static class Either {
        ///<summary>
        ///Constructs a <see cref="Either{L,R}.Left"/>.
        ///</summary>
        public static Either<L, R>.Left Left<L, R>(L value) {
            return new Either<L, R>.Left(value);
        }

        ///<summary>
        ///Constructs a <see cref="Either{L,R}.Right"/>.
        ///</summary>
        public static Either<L, R>.Right Right<L, R>(R value) {
            return new Either<L, R>.Right(value);
        }
    }

    /// <summary>
    /// A type representing a value of one of two types: <typeparamref name="L"/> or <typeparamref name="R"/>. 
    /// When differentiating a correct value from an error, the convention is to use Right for the correct value.
    /// </summary>
    /// <seealso cref="Result{T}"/>
    /// <seealso cref="Optional{T}"/>
    /// <typeparam name="L">The left type.</typeparam>
    /// <typeparam name="R">The right type.</typeparam>
    [Serializable]
    public abstract class Either<L, R> : IEquatable<Either<L, R>> {
        /// <summary>
        /// Gets a value indicating whether this instance is a <see cref="Right"/>.
        /// </summary>
        /// <value><c>true</c> if this instance is a <see cref="Right"/>; otherwise, <c>false</c>.</value>
        public abstract bool IsRight { get; }

        /// <summary>
        /// Case analysis.
        /// </summary>
        /// <param name="onLeft">Action to do if this is a <see cref="Left"/>.</param>
        /// <param name="onRight">Action to do if this is a <see cref="Right"/>.</param>
        public abstract void Match(Action<L> onLeft, Action<R> onRight);

        /// <summary>
        /// Case analysis.
        /// </summary>
        /// <param name="onLeft">Function to apply if this is a <see cref="Left"/>.</param>
        /// <param name="onRight">Function to apply if this is a <see cref="Right"/>.</param>
        public abstract Res Match<Res>(Func<L, Res> onLeft, Func<R, Res> onRight);

        /// <summary>
        /// Gets the left value.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Either{L,R}.IsRight"/> is true.</exception>
        public abstract L LeftValue { get; }

        /// <summary>
        /// Gets the right value.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Either{L,R}.IsRight"/> is false.</exception>
        public abstract R RightValue { get; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public abstract bool Equals(Either<L, R> other);

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Either<L, R>)) return false;
            return Equals((Either<L, R>) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() {
            return 0;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        public static bool operator ==(Either<L, R> left, Either<L, R> right) {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        public static bool operator !=(Either<L, R> left, Either<L, R> right) {
            return !Equals(left, right);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="L"/> to <see cref="Either{L,R}"/>.
        /// </summary>
        /// <param name="leftValue">The left value.</param>
        /// <returns><see cref="Left"/> with value <paramref name="leftValue"/>.</returns>
        [CLSCompliant(false)]
        public static implicit operator Either<L, R>(L leftValue) {
            return new Left(leftValue);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="R"/> to <see cref="Either{L,R}"/>.
        /// </summary>
        /// <param name="rightValue">The right value.</param>
        /// <returns><see cref="Right"/> with value <paramref name="rightValue"/>.</returns>
        [CLSCompliant(false)]
        public static implicit operator Either<L, R>(R rightValue) {
            return new Right(rightValue);
        }

        /// <summary>
        /// Represents a value of the type <typeparamref name="L"/>.
        /// </summary>
        [Serializable]
        public sealed class Left : Either<L, R>, IEquatable<Left> {
            private readonly L _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="Either{L, R}.Left"/> class.
            /// </summary>
            /// <param name="value">The value.</param>
            public Left(L value) {
                _value = value;
            }

            /// <summary>
            /// Gets a value indicating whether this instance is a <see cref="Either{L,R}.Right"/>.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is a <see cref="Either{L,R}.Right"/>; otherwise, <c>false</c>.
            /// </value>
            public override bool IsRight {
                get { return false; }
            }

            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <value>The value.</value>
            public L Value {
                get { return _value; }
            }

            /// <summary>
            /// Case analysis. Does <paramref name="onLeft"/>.
            /// </summary>
            /// <param name="onLeft">Action to do if this is a <see cref="Either{L,R}.Left"/>.</param>
            /// <param name="onRight">Action to do if this is a <see cref="Either{L,R}.Right"/>.</param>
            public override void Match(Action<L> onLeft, Action<R> onRight) {
                onLeft(_value);
            }

            /// <summary>
            /// Case analysis. Applies <paramref name="onLeft"/>.
            /// </summary>
            /// <param name="onLeft">Function to apply if this is a <see cref="Either{L,R}.Left"/>.</param>
            /// <param name="onRight">Function to apply if this is a <see cref="Either{L,R}.Right"/>.</param>
            public override Res Match<Res>(Func<L, Res> onLeft, Func<R, Res> onRight) {
                return onLeft(_value);
            }

            /// <summary>
            /// Gets the left value.
            /// </summary>
            public override L LeftValue {
                get { return Value; }
            }

            /// <summary>
            /// Throws <see cref="InvalidOperationException"/>.
            /// </summary>
            /// <exception cref="InvalidOperationException"></exception>
            public override R RightValue {
                get { throw new InvalidOperationException("Either.Left doesn't have a RightValue"); }
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public override bool Equals(Either<L, R> other) {
                return other.Match(o => o.Equals(Value), _ => false);
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(Left other) {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(other._value, _value);
            }

            /// <summary>
            /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
            /// </summary>
            /// <returns>
            /// true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
            /// </returns>
            /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
            /// <exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception><filterpriority>2</filterpriority>
            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != typeof (Left)) return false;
                return Equals((Left) obj);
            }

            /// <summary>
            /// Serves as a hash function for a particular type. 
            /// </summary>
            /// <returns>
            /// A hash code for the current <see cref="T:System.Object" />.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override int GetHashCode() {
                return _value.GetHashCode();
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            public static bool operator ==(Left left, Left right) {
                return Equals(left, right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            public static bool operator !=(Left left, Left right) {
                return !Equals(left, right);
            }
        }

        /// <summary>
        /// Represents a value of the type <typeparamref name="L"/>.
        /// </summary>
        [Serializable]
        public sealed class Right : Either<L, R>, IEquatable<Right> {
            private readonly R _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="Either{L, R}.Right"/> class.
            /// </summary>
            /// <param name="value">The value.</param>
            public Right(R value) {
                _value = value;
            }

            /// <summary>
            /// Gets a value indicating whether this instance is a <see cref="Either{L,R}.Right"/>.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is a <see cref="Either{L,R}.Right"/>; otherwise, <c>false</c>.
            /// </value>
            public override bool IsRight {
                get { return true; }
            }

            /// <summary>
            /// Case analysis. Does <paramref name="onRight"/>.
            /// </summary>
            /// <param name="onLeft">Action to do if this is a <see cref="Either{L,R}.Left"/>.</param>
            /// <param name="onRight">Action to do if this is a <see cref="Either{L,R}.Right"/>.</param>
            public override void Match(Action<L> onLeft, Action<R> onRight) {
                onRight(_value);
            }

            /// <summary>
            /// Case analysis. Applies <paramref name="onRight"/>.
            /// </summary>
            /// <param name="onLeft">Function to apply if this is a <see cref="Either{L,R}.Left"/>.</param>
            /// <param name="onRight">Function to apply if this is a <see cref="Either{L,R}.Right"/>.</param>
            public override Res Match<Res>(Func<L, Res> onLeft, Func<R, Res> onRight) {
                return onRight(_value);
            }

            /// <summary>
            /// Throws <see cref="InvalidOperationException"/>.
            /// </summary>
            /// <exception cref="InvalidOperationException"></exception>
            public override L LeftValue {
                get { throw new InvalidOperationException("Either.Right doesn't have a LeftValue"); }
            }

            /// <summary>
            /// Gets the right value.
            /// </summary>
            public override R RightValue {
                get { return Value; }
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public override bool Equals(Either<L, R> other) {
                return other.Match(_ => false, o => o.Equals(Value));
            }

            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <value>The value.</value>
            public R Value {
                get { return _value; }
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(Right other) {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(other._value, _value);
            }

            /// <summary>
            /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
            /// </summary>
            /// <returns>
            /// true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
            /// </returns>
            /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
            /// <exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception><filterpriority>2</filterpriority>
            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != typeof (Right)) return false;
                return Equals((Right) obj);
            }

            /// <summary>
            /// Serves as a hash function for a particular type. 
            /// </summary>
            /// <returns>
            /// A hash code for the current <see cref="T:System.Object" />.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override int GetHashCode() {
                return _value.GetHashCode();
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            public static bool operator ==(Right left, Right right) {
                return Equals(left, right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            public static bool operator !=(Right left, Right right) {
                return !Equals(left, right);
            }
        }
    }
}
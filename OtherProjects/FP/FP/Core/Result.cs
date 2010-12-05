/*
* Result.cs is part of functional-dotnet project
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
    /// A static class to help with type inference on <see cref="Result{T}"/>.
    /// </summary>
    public static class Result {
        /// <summary>
        /// Returns a <see cref="Result{T}.Success"/> with the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        public static Result<T>.Success Success<T>(T value) {
            return new Result<T>.Success(value);
        }

        /// <summary>
        /// Returns a <see cref="Result{T}.Failure"/> with the specified reason.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reason">The reason.</param>
        public static Result<T>.Failure Failure<T>(string reason) {
            return new Result<T>.Failure(reason);
        }

        /// <summary>
        /// Returns a <see cref="Result{T}.Failure"/> with the specified exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception">The exception.</param>
        public static Result<T>.Failure Failure<T>(Exception exception) {
            return new Result<T>.Failure(exception);
        }

        /// <summary>
        /// Tries the specified function.
        /// </summary>
        /// <typeparam name="T">The return type of the function.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns><see cref="Result{T}.Failure"/> if <paramref name="function"/> throws exception;
        /// <see cref="Result{T}.Success"/> otherwise.</returns>
        public static Result<T> Try<T>(this Func<T> function) {
            try {
                return Success(function());
            }
            catch (Exception e) {
                return Failure<T>(e);
            }
        }
    }

    /// <summary>
    /// A class which describes a result of a calculation. This is either a <see cref="Success"/> 
    /// or a <see cref="Failure"/>.
    /// </summary>
    /// <seealso cref="Either{L,R}"/>
    /// <seealso cref="Optional{T}"/>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <seealso cref="Result"/>
    public abstract class Result<T> {
        private Result() {}

        /// <summary>
        /// Case analysis on results.
        /// </summary>
        /// <param name="onSuccess">The action to do if the result is a success.</param>
        /// <param name="onFailure">The action to do if the result is a failure.</param>
        public abstract void Match(Action<T> onSuccess, Action<Exception> onFailure);

        /// <summary>
        /// Case analysis on results.
        /// </summary>
        /// <param name="onSuccess">The function to evaluate if the result is a success.</param>
        /// <param name="onFailure">The function to evaluate if the result is a failure.</param>
        public abstract R Match<R>(Func<T, R> onSuccess, Func<Exception, R> onFailure);

        /// <summary>
        /// Converts the result to <see cref="Optional{T}"/>.
        /// </summary>
        /// <returns></returns>
        public abstract Optional<T> ToOptional();

        /// <summary>
        /// Gets the value, if the result is a success; throws an exception otherwise.
        /// </summary>
        /// <value>The value, if the result is a success.</value>
        public abstract T Value { get; }

        /// <summary>
        /// Performs an explicit conversion from <see cref="Result{T}"/> to <see cref="T"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The value.</returns>
        public static explicit operator T(Result<T> result) {
            return result.Value;
        }

        /// <summary>
        /// Implements the operator false.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if the result is <see cref="Failure"/>.</returns>
        public static bool operator false(Result<T> result) {
            return result is Failure;
        }

        /// <summary>
        /// Implements the operator true.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if the result is <see cref="Success"/>.</returns>
        public static bool operator true(Result<T> result) {
            return result is Success;
        }

        /// <summary>
        /// Represents a failure.
        /// </summary>
        public sealed class Failure : Result<T> {
            /// <summary>
            /// Gets the reason.
            /// </summary>
            /// <value>The reason.</value>
            public string Reason {
                get { return Exception.Message; }
            }

            /// <summary>
            /// Gets the exception.
            /// </summary>
            /// <value>The exception.</value>
            public Exception Exception { get; private set; }

            /// <summary>
            /// Throws the <see cref="Exception"/>.
            /// </summary>
            /// <value>Doesn't return a value.</value>
            public override T Value {
                get { throw Exception; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Result{T}.Failure"/> class.
            /// </summary>
            /// <param name="reason">The reason.</param>
            public Failure(string reason) {
                Exception = new Exception(reason);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Result{T}.Failure"/> class.
            /// </summary>
            /// <param name="exception">The exception.</param>
            public Failure(Exception exception) {
                Exception = exception;
            }

            /// <summary>
            /// Performs an implicit conversion from <see cref="Result{T}.Failure"/> to <see cref="System.Exception"/>.
            /// </summary>
            /// <param name="failure">The failure.</param>
            /// <returns>The exception of the failure.</returns>
            public static implicit operator Exception(Failure failure) {
                return failure.Exception;
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override string ToString() {
                return Reason;
            }

            /// <summary>
            /// Case analysis on results.
            /// </summary>
            /// <param name="onSuccess">The function to evaluate if the result is a success.</param>
            /// <param name="onFailure">The function to evaluate if the result is a failure.</param>
            public override R Match<R>(Func<T, R> onSuccess,
                                       Func<Exception, R> onFailure) {
                return onFailure(Exception);
            }

            /// <summary>
            /// Case analysis on results.
            /// </summary>
            /// <param name="onSuccess">The action to do if the result is a success.</param>
            /// <param name="onFailure">The action to do if the result is a failure.</param>
            public override void Match(Action<T> onSuccess, Action<Exception> onFailure) {
                onFailure(Exception);
            }

            /// <summary>
            /// Converts the result to <see cref="Optional{T}"/>.
            /// </summary>
            /// <returns></returns>
            public override Optional<T> ToOptional() {
                return Optional<T>.None;
            }
        }

        /// <summary>
        /// Represents a success.
        /// </summary>
        public sealed class Success : Result<T> {
            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <value>The value.</value>
            public override T Value {
                get { return _value; }
            }

            private readonly T _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="Result{T}.Success"/> class.
            /// </summary>
            /// <param name="value">The value.</param>
            public Success(T value) {
                _value = value;
            }

            /// <summary>
            /// Performs an implicit conversion from <see cref="Result{T}.Success"/> to 
            /// <see cref="T"/>.
            /// </summary>
            /// <param name="success">The success.</param>
            /// <returns>The value.</returns>
            public static implicit operator T(Success success) {
                return success.Value;
            }

            /// <summary>
            /// Performs an implicit conversion from <see cref="T"/> to 
            /// <see cref="Result{T}.Success"/>.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>The success with value <paramref name="value"/>.</returns>
            public static implicit operator Success(T value) {
                return new Success(value);
            }

            /// <summary>
            /// Case analysis on results.
            /// </summary>
            /// <param name="onSuccess">The function to evaluate if the result is a success.</param>
            /// <param name="onFailure">The function to evaluate if the result is a failure.</param>
            public override R Match<R>(Func<T, R> onSuccess,
                                       Func<Exception, R> onFailure) {
                return onSuccess(Value);
            }

            /// <summary>
            /// Case analysis on results.
            /// </summary>
            /// <param name="onSuccess">The action to do if the result is a success.</param>
            /// <param name="onFailure">The action to do if the result is a failure.</param>
            public override void Match(Action<T> onSuccess, Action<Exception> onFailure) {
                onSuccess(Value);
            }

            /// <summary>
            /// Converts the result to <see cref="Optional{T}"/>.
            /// </summary>
            /// <returns></returns>
            public override Optional<T> ToOptional() {
                return Optional.Some(Value);
            }
        }
    }
}
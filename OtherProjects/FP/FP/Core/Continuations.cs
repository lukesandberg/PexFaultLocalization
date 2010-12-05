/*
* Continuations.cs is part of functional-dotnet project
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
    /// A delegate describing a continuation which does <paramref name="computation"> given a value of
    /// the type <typeparamref name="T"/> and returns a result of the type <typeparamref name="T"/>.
    /// </summary>
    public delegate R Continuation<T, R>(Func<T, R> computation);

    /// <summary>
    /// Static methods for working with <see cref="Continuation{T,R}"/>.
    /// </summary>
    public static class Continuations {
        /// <summary>
        /// Given any function from <typeparamref name="T"/> to <typeparamref name="R"/>,
        /// calls it on the <see cref="arg"/>.
        /// </summary>
        public static Continuation<T, R> FromArgument<T, R>(T arg) {
            return Wrap<T, R>(arg);
        }

        /// <summary>
        /// Given any function from <typeparamref name="T"/> to <typeparamref name="R"/>,
        /// calls it on the <see cref="arg"/>.
        /// </summary>
        public static Continuation<T, R> Wrap<T, R>(T arg) {
            return comp => comp(arg);
        }

        /// <summary>
        /// Runs the specified continuation.
        /// </summary>
        /// <param name="continuation">The continuation.</param>
        /// <param name="final">The final computation.</param>
        /// <returns></returns>
        public static R Run<T, R>(this Continuation<T, R> continuation, Func<T, R> final) {
            return continuation(final);
        }

        /// <summary>
        /// Maps the specified function over the specified continuation.
        /// </summary>
        /// <param name="continuation">The continuation.</param>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public static Continuation<T, R> Map<T, R>(this Continuation<T, R> continuation,
                                                   Func<R, R> func) {
            return comp => func(continuation(comp));
        }

        ///<summary>
        /// Call with current continuation.
        ///</summary>
        ///<param name="func"></param>
        ///<typeparam name="T1"></typeparam>
        ///<typeparam name="T2"></typeparam>
        ///<typeparam name="R"></typeparam>
        ///<returns></returns>
        public static Continuation<T1, R> CallCC<T1, T2, R>(
            this Func<Func<T1, Continuation<T2, R>>, Continuation<T1, R>> func) {
            return c => func(x => y => c(x)).Run(c);
        }
    }
}
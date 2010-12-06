/*
* Functions.cs is part of functional-dotnet project
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
using System.Collections.Generic;

namespace FP.Core {
    /// <summary>
    /// Encapsulates a method that has four parameters and returns a value of the type
    /// specified by the <typeparamref name="TResult" /> parameter.
    /// </summary>
    /// <returns>
    /// The return value of the method that this delegate encapsulates.
    /// </returns>
    /// <param name="arg1">
    /// The first parameter of the method that this delegate encapsulates.
    /// </param>
    /// <param name="arg2">
    /// The second parameter of the method that this delegate encapsulates.
    /// </param>
    /// <param name="arg3">
    /// The third parameter of the method that this delegate encapsulates.
    /// </param>
    /// <param name="arg4">
    /// The fourth parameter of the method that this delegate encapsulates.
    /// </param>
    /// <param name="arg5">
    /// The fifth parameter of the method that this delegate encapsulates.
    /// </param>
    /// <typeparam name="T1">
    /// The type of the first parameter of the method that this delegate encapsulates.
    /// </typeparam>
    /// <typeparam name="T2">
    /// The type of the second parameter of the method that this delegate encapsulates.
    /// </typeparam>
    /// <typeparam name="T3">
    /// The type of the third parameter of the method that this delegate encapsulates.
    /// </typeparam>
    /// <typeparam name="T4">
    /// The type of the fourth parameter of the method that this delegate encapsulates.
    /// </typeparam>
    /// <typeparam name="T5">
    /// The type of the fifth parameter of the method that this delegate encapsulates.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the return value of the method that this delegate encapsulates.
    /// </typeparam>
    public delegate TResult Func<T1, T2, T3, T4, T5, TResult>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    /// <summary>
    /// Encapsulates a method that has four parameters and does not return a value.
    /// </summary>
    /// <returns>
    /// The return value of the method that this delegate encapsulates.
    /// </returns>
    /// <param name="arg1">
    /// The first parameter of the method that this delegate encapsulates.
    /// </param>
    /// <param name="arg2">
    /// The second parameter of the method that this delegate encapsulates.
    /// </param>
    /// <param name="arg3">
    /// The third parameter of the method that this delegate encapsulates.
    /// </param>
    /// <param name="arg4">
    /// The fourth parameter of the method that this delegate encapsulates.
    /// </param>
    /// <param name="arg5">
    /// The fifth parameter of the method that this delegate encapsulates.
    /// </param>
    /// <typeparam name="T1">
    /// The type of the first parameter of the method that this delegate encapsulates.
    /// </typeparam>
    /// <typeparam name="T2">
    /// The type of the second parameter of the method that this delegate encapsulates.
    /// </typeparam>
    /// <typeparam name="T3">
    /// The type of the third parameter of the method that this delegate encapsulates.
    /// </typeparam>
    /// <typeparam name="T4">
    /// The type of the fourth parameter of the method that this delegate encapsulates.
    /// </typeparam>
    /// <typeparam name="T5">
    /// The type of the fifth parameter of the method that this delegate encapsulates.
    /// </typeparam>
    public delegate void Action<T1, T2, T3, T4, T5>(
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for manipulating functions.
    /// </summary>
    public static class Functions {
        /// <summary>
        /// Composes two functions left to right. This is the same as <c>flip (.)</c> in Haskell.
        /// </summary>
        /// <param name="func1">First function.</param>
        /// <param name="func2">Second function.</param>
        /// <returns>The composition of functions.</returns>
        public static Func<T1, T3> Compose<T1, T2, T3>(
            this Func<T1, T2> func1, Func<T2, T3> func2) {
            return x => func2(func1(x));
        }

        /// <summary>
        /// Flips the order of arguments of a function.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>The function with flipped arguments.</returns>
        public static Func<T2, T1, TR> Flip<T1, T2, TR>(
            this Func<T1, T2, TR> func) {
            return (y, x) => func(x, y);
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public static readonly Action DoNothing = () => { };

        /// <summary>
        /// The identity function.
        /// </summary>
        /// <typeparam name="T">The type of argument and result.</typeparam>
        public static Func<T, T> Id<T>() {
            return x => x;
        }

        /// <summary>
        /// Curries the specified function.
        /// </summary>
        /// <param name="func">The function which takes two parameters.</param>
        /// <returns>The same function which takes parameters separately.</returns>
        public static Func<T1, Func<T2, TR>> Curry<T1, T2, TR>(this Func<T1, T2, TR> func) {
            return x => y => func(x, y);
        }

        /// <summary>
        /// Curries the specified function.
        /// </summary>
        /// <param name="func">The function which takes three parameters.</param>
        /// <returns>The same function which takes parameters separately.</returns>
        public static Func<T1, T2, Func<T3, TR>> Curry<T1, T2, T3, TR>(
            this Func<T1, T2, T3, TR> func) {
            return (x, y) => z => func(x, y, z);
        }

        /// <summary>
        /// Uncurries the specified function.
        /// </summary>
        /// <param name="func">The function which takes two parameters separately.</param>
        /// <returns>The same function which takes parameters together.</returns>
        public static Func<T1, T2, TR> Uncurry<T1, T2, TR>(this Func<T1, Func<T2, TR>> func) {
            return (x, y) => func(x)(y);
        }

        /// <summary>
        /// Uncurries the specified function.
        /// </summary>
        /// <param name="func">The function which takes three parameters separately.</param>
        /// <returns>The same function which takes parameters together.</returns>
        public static Func<T1, T2, T3, TR> Uncurry<T1, T2, T3, TR>(
            this Func<T1, T2, Func<T3, TR>> func) {
            return (x, y, z) => func(x, y)(z);
        }

        /// <summary>
        /// Negates the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the argument of <paramref name="predicate"/>.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static Func<T, bool> Not<T>(this Func<T, bool> predicate) {
            return x => !predicate(x);
        }

        public static Func<R> Memoize<R>(this Func<R> f) {
            R value = default(R);
            bool hasValue = false;
            return () => {
                  if (!hasValue) {
                      hasValue = true;
                      value = f();
                  }
                  return value;
              };
        }

        public static Func<T, R> Memoize<T, R>(this Func<T, R> f) {
            var map = new Dictionary<T, R>();
            return arg => {
                  R value;
                  if (map.TryGetValue(arg, out value))
                      return value;
                  value = f(arg);
                  map.Add(arg, value);
                  return value;
              };
        }

        public static Func<T1, T2, R> Memoize<T1, T2, R>(this Func<T1, T2, R> f) {
            var map = new Dictionary<Tuple<T1, T2>, R>();
            return (arg1, arg2) => {
                R value;
                if (map.TryGetValue(Tuple.New(arg1, arg2), out value))
                    return value;
                value = f(arg1, arg2);
                map.Add(Tuple.New(arg1, arg2), value);
                return value;
            };
        }
    }
}
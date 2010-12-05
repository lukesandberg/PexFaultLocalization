/*
* Combinators.cs is part of functional-dotnet project
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

namespace FP.Core.Combinators {
    ///<summary>
    /// A class with several combinators borrowed from Raganwald's weblog.
    /// In a separate namespace, since it provides generic extension methods for all types.
    ///</summary>
    public static class Combinators {
        ///<summary>
        /// Does something with an object and returns the object itself. Useful for chaining.
        ///</summary>
        ///<param name="t">The target object.</param>
        ///<param name="action">The action.</param>
        ///<returns><paramref name="t"/></returns>
        public static T Tap<T>(this T t, Action<T> action) {
            action(t);
            return t;
        }

        ///<summary>
        /// Does something with an object, ignores the result and returns the object itself. 
        /// Useful for chaining.
        ///</summary>
        ///<param name="t">The target object.</param>
        ///<param name="func">The function to call.</param>
        ///<returns><paramref name="t"/></returns>
        public static T Tap<T, R>(this T t, Func<T, R> func) {
            func(t);
            return t;
        }

        ///<summary>
        ///Passes an object as the argument to the function.
        ///</summary>
        ///<param name="t">The object to be used as the argument.</param>
        ///<param name="func">The function to call.</param>
        public static R Into<T, R>(this T t, Func<T, R> func) {
            return func(t);
        }
    } // class Combinators
} // namespace FP.Core.Combinators
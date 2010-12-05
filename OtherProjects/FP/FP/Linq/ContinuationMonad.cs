/*
* ContinuationMonad.cs is part of functional-dotnet project
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
using FP.Core;

namespace FP.Linq {
    /// <summary>
    /// Implements query pattern on <see cref="Continuation{T,R}"/>. Makes <see cref="Continuation{T,R}"/> a monad.
    /// </summary>
    public static class ContinuationMonad {
        public static Continuation<T2, R> Select<T1, T2, R>(this Continuation<T1, R> cont,
                                                            Func<T1, T2> function) {
            return cont.SelectMany(function.Compose(t2 => Continuations.FromArgument<T2, R>(t2)));
        }

        public static Continuation<T3, R> SelectMany<T1, T2, T3, R>(this Continuation<T1, R> cont,
                                                                    Func<T1, Continuation<T2, R>>
                                                                        function,
                                                                    Func<T1, T2, T3> combiner) {
            return cont.SelectMany(x => function(x).Select(y => combiner(x, y)));
        }

        public static Continuation<T2, R> SelectMany<T1, T2, R>(this Continuation<T1, R> cont,
                                                                Func<T1, Continuation<T2, R>>
                                                                    function) {
            return comp => cont(t1 => function(t1)(comp));
        }
    }
}
/*
* ResultMonad.cs is part of functional-dotnet project
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
    /// Implements query pattern on <see cref="Result{T}"/>. Makes <see cref="Result{T}"/> a monad.
    /// </summary>
    public static class ResultMonad {
        public static Result<T> Where<T>(this Result<T> result, Func<T, bool> function) {
            return result.Match(function, x => false) ? result : Result.Failure<T>("Filtered out");
        }

        public static Result<T2> Select<T1, T2>(this Result<T1> result, Func<T1, T2> function) {
            return result.SelectMany(function.Compose<T1, T2, Result<T2>>(Result.Success));
        }

        public static Result<T3> SelectMany<T1, T2, T3>(this Result<T1> result,
                                                        Func<T1, Result<T2>> function,
                                                        Func<T1, T2, T3> combiner) {
            return result.SelectMany(x => function(x).Select(y => combiner(x, y)));
        }

        public static Result<T2> SelectMany<T1, T2>(this Result<T1> result,
                                                    Func<T1, Result<T2>> function) {
            return result.Match(function, Result.Failure<T2>);
        }
    }
}
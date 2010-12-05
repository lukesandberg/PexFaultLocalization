/*
* Actions.cs is part of functional-dotnet project
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
using System.Threading;

namespace FP.Core.CPS {
    ///<summary>
    ///Simple actions and their combinators.
    ///</summary>
    public static class Actions {
        ///<summary>
        ///Runs the action with the trivial continuation. Throws an 
        ///<see cref="ActionFailureException"/> in the case action fails.
        ///</summary>
        ///<param name="action"></param>
        ///<typeparam name="T"></typeparam>
        public static T Run<T>(this ActionCPS<T> action) {
            return action.RunR().Match(
                x => x,
                e => { throw new ActionFailureException(e); });
        }

        ///<summary>
        ///Runs the action with the trivial continuation. Returns 
        ///<see cref="Result{T}.Failure"/> in the case action fails.
        ///</summary>
        ///<param name="action"></param>
        ///<typeparam name="T"></typeparam>
        public static Result<T> RunR<T>(this ActionCPS<T> action) {
            Result<T> result = null;
            try {
                action(new Cont<T>(
                           t => { result = Result.Success(t); },
                           e => { result = Result.Failure<T>(e); }));
                return result;
            }
            catch (Exception e) {
                return Result.Failure<T>(e);
            }
        }

        ///<summary>
        ///Returns the action which immediately passes <paramref name="t"/> to its
        ///continuation.
        ///</summary>
        public static ActionCPS<T> Return<T>(T t) {
            return cont => cont.Ok(t);
        }

        ///<summary>
        ///Returns the action which always fails with exception <paramref name="e"/>.
        ///</summary>
        public static ActionCPS<T> Fail<T>(Exception e) {
            return cont => cont.Fail(e);
        }

        ///<summary>
        ///Returns an action which sleeps for <paramref name="milliseconds"/> and then returns <c>true</c>.
        ///</summary>
        ///<param name="milliseconds">How long to sleep, in milliseconds.</param>
        public static ActionCPS<bool> TrueAfter(int milliseconds) {
            return cont => {
                       Thread.Sleep(milliseconds);
                       cont.Ok(true);
                   };
        }

        ///<summary>
        ///Returns an action which sleeps for <paramref name="timeSpan"/> and then returns <c>true</c>.
        ///</summary>
        ///<param name="timeSpan">How long to sleep.</param>
        public static ActionCPS<bool> TrueAfter(TimeSpan timeSpan) {
            return cont => {
                       Thread.Sleep(timeSpan);
                       cont.Ok(true);
                   };
        }

        /// <summary>
        /// Do <paramref name="first"/>; if it succeeds then do <paramref name="second"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the <paramref name="first"/> action.</typeparam>
        /// <typeparam name="T2">The type of the <paramref name="second"/> action.</typeparam>
        /// <param name="first">The first action.</param>
        /// <param name="second">The second action.</param>
        /// <returns>The combined action</returns>
        public static ActionCPS<T2> Then<T1, T2>(this ActionCPS<T1> first,
                                                 Func<T1, ActionCPS<T2>> second) {
            return cont => first(new Cont<T1>(
                                     t1 => second(t1)(cont),
                                     e => cont.Fail(e)));
        }

        ///<summary>
        ///Returns an action which attempts <paramref name="action"/> first, calls 
        ///<paramref name="rescue"/> in case of failure.
        ///</summary>
        ///<param name="action">The action to try.</param>
        ///<param name="rescue">The action to call if <paramref name="action"/> fails.</param>
        public static ActionCPS<T> WithRescue<T>(this ActionCPS<T> action,
                                                 Func<Exception, ActionCPS<T>> rescue) {
            return cont => action.RunR().Match(
                               t => cont.Ok(t),
                               e => rescue(e)(cont));
        }

        ///<summary>
        ///Returns an action which attempts <paramref name="action"/> first, returns <paramref name="defaultResult"/> if
        /// it fails.
        ///</summary>
        ///<param name="action">The action to try.</param>
        ///<param name="defaultResult">The result if <paramref name="action"/> fails.</param>
        public static ActionCPS<T> WithDefault<T>(this ActionCPS<T> action, T defaultResult) {
            return action.WithRescue(e => Return(defaultResult));
        }

        ///<summary>
        ///Returns an action which attempts <paramref name="action"/> first and does 
        ///<paramref name="ifFirstActionFails"/> if it fails.
        ///</summary>
        ///<param name="action">The action to try.</param>
        ///<param name="ifFirstActionFails">The action to do if <paramref name="action"/>
        ///fails.</param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public static ActionCPS<T> OrElse<T>(this ActionCPS<T> action,
                                             ActionCPS<T> ifFirstActionFails) {
            return action.WithRescue(e => ifFirstActionFails);
        }

        ///<summary>
        ///Returns an action which 
        ///</summary>
        ///<param name="action"></param>
        ///<param name="finalizer"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public static ActionCPS<T> WithFinally<T>(this ActionCPS<T> action, Action finalizer) {
            return cont => {
                       action(cont);
                       finalizer();
                   };
        }
    }
}
/*
* Switch.cs is part of functional-dotnet project
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

// Adapted from http://community.bartdesmet.net/blogs/bart/archive/2008/03/30/a-functional-c-type-switch.aspx

using System;

namespace FP.Core {
    /// <summary>
    /// A helper class used to instantiate generic switches.
    /// See <seealso cref="Switch{T,S}"/> and <seealso cref="Switch{T,S,R}"/>.
    /// </summary>
    public static class Switch {
        /// <summary>
        /// Switches on the specified object.
        /// </summary>
        /// <param name="value">The object we are switching on.</param>
        /// <returns>The switch.</returns>
        public static Switch<T, T> On<T>(T value) {
            return new Switch<T, T>(value, value);
        }

        /// <summary>
        /// Switches on the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Switch<T, T, R> ExprOn<T, R>(T value) {
            return new Switch<T, T, R>(value, value);
        }
    }

    /// <summary>
    /// A generic functional switch.
    /// </summary>
    /// <typeparam name="T">The current type of the object we are switching on.</typeparam>
    /// <typeparam name="S">The real type of the object we are switching on.</typeparam>
    /// <example><![CDATA[
    /// void Do(Control c)
    /// {
    ///      Switch.On(c)
    ///           .Case<Label>(l =>
    ///           {
    ///                // ...
    ///           })
    ///           .Case<Button>(b => b.Text == "", b =>
    ///           {
    ///                // ...
    ///           })
    ///           .Case(b => //still a Button
    ///           {
    ///                // ...
    ///           })
    ///           .Default(cc =>
    ///           {
    ///                // ...
    ///           });
    /// }
    /// ]]></example>
    /// <remarks>
    /// Exposes no public constructors. Use <see cref="Switch.On{T}"/> to create an instance, as in the example.
    /// </remarks>
    public class Switch<T, S> where T : S {
        private readonly S _object; //the real object we are switching on
        private readonly T _value; // (T) _object, if _object is T; default(T) otherwise
        private bool _badType; //_object is not T
        private bool _finished; //a case without fallthrough has been applied

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch{T, S}"/> class.
        /// </summary>
        /// <param name="value">The object we are switching on.</param>
        /// <param name="realValue">The object we we are switching on, as <see cref="object"/>.</param>
        internal Switch(T value, S realValue) {
            _value = value;
            _object = realValue;
        }

        /// <summary>
        /// Breaks the chain.
        /// </summary>
        /// <typeparam name="TNew">The type of the new switch.</typeparam>
        /// <returns>A switch which does nothing in any case.</returns>
        public static Switch<TNew, S> Break<TNew>() where TNew : S {
            return new Switch<TNew, S>(default(TNew), default(S))
                   {_finished = true};
        }

        /// <summary>
        /// In case the value we are switching on equals <paramref name="t"/>, do <paramref name="action"/>.
        /// </summary>
        /// <param name="t">The value to compare the value we are switching on to.</param>
        /// <param name="action">The action to do in this case.</param>
        public Switch<T, S> Case(T t, Action<T> action) {
            return Case(x => x.Equals(t), action);
        }

        /// <summary>
        /// In case the value we are switching on equals <paramref name="t"/>, do <paramref name="action"/>
        /// and fall through to the next case.
        /// </summary>
        /// <param name="t">The value to compare the value we are switching on to.</param>
        /// <param name="action">The action to do in this case.</param>
        public Switch<T, S> CaseWithFallThrough(T t, Action<T> action) {
            return CaseWithFallThrough(x => x.Equals(t), action);
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparamref name="T"/>, do <paramref name="action"/>.
        /// </summary>
        /// <param name="action">The action to do in this case.</param>
        public Switch<T, S> Case(Action<T> action) {
            return Case(x => true, action);
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparamref name="T"/>, do <paramref name="action"/>
        /// and fall through to the next case.
        /// </summary>
        /// <param name="action">The action to do in this case.</param>
        public Switch<T, S> CaseWithFallThrough(Action<T> action) {
            return CaseWithFallThrough(x => true, action);
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparamref name="T"/> and
        /// satisfies <paramref name="predicate"/>, do <paramref name="action"/>.
        /// </summary>
        /// <param name="predicate">The predicate to test the value we are switching on.</param>
        /// <param name="action">The action to do in this case.</param>
        public Switch<T, S> Case(Func<T, bool> predicate,
                                 Action<T> action) {
            if (_finished || _badType)
                return this;
            if (predicate(_value)) {
                action(_value);
                _finished = true;
            }
            return this;
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparamref name="T"/> and 
        /// satisfies <paramref name="predicate"/>, do <paramref name="action"/> and fall through to the next case.
        /// </summary>
        /// <param name="predicate">The predicate to test the value we are switching on.</param>
        /// <param name="action">The action to do in this case.</param>
        public Switch<T, S> CaseWithFallThrough(Func<T, bool> predicate, Action<T> action) {
            if (_finished || _badType)
                return this;
            if (predicate(_value))
                action(_value);
            return this;
        }

        /// <summary>
        /// In case we have reached this, do <paramref name="action"/>.
        /// </summary>
        /// <param name="action">The action to do.</param>
        public void Default(Action<S> action) {
            if (!_finished)
                action(_object);
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparam name="TNew"/>, do <paramref name="action"/>.
        /// </summary>
        /// <param name="action">The action to do in this case.</param>
        public Switch<TNew, S> Case<TNew>(Action<TNew> action)
            where TNew : S {
            return Case(x => true, action);
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparam name="TNew"/>, do <paramref name="action"/>
        /// and fall through to the next case.
        /// </summary>
        /// <param name="action">The action to do in this case.</param>
        public Switch<TNew, S> CaseWithFallThrough<TNew>(
            Action<TNew> action) where TNew : S {
            return CaseWithFallThrough(x => true, action);
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparam name="TNew"/> and equals <paramref name="t"/>, do <paramref name="action"/>.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="action">The action to do in this case.</param>
        public Switch<TNew, S> Case<TNew>(TNew t, Action<TNew> action)
            where TNew : S {
            return Case(x => true, action);
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparam name="TNew"/> and equals <paramref name="t"/>, do <paramref name="action"/>
        /// and fall through to the next case.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="action">The action to do in this case.</param>
        public Switch<TNew, S> CaseWithFallThrough<TNew>(TNew t, Action<TNew> action) where TNew : S {
            return CaseWithFallThrough(x => true, action);
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparam name="TNew"/> and satisfies <paramref name="predicate"/>, do <paramref name="action"/>.
        /// </summary>
        /// <param name="predicate">The predicate to test the value we are switching on.</param>
        /// <param name="action">The action to do in this case.</param>
        public Switch<TNew, S> Case<TNew>(Func<TNew, bool> predicate,
                                          Action<TNew> action)
            where TNew : S {
            bool goodType = _object is TNew;
            return new Switch<TNew, S>(
                goodType ? (TNew) _object : default(TNew), _object)
                   {_badType = !goodType}.
                Case(predicate, action);
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparam name="TNew"/> and satisfies <paramref name="predicate"/>, do <paramref name="action"/>
        /// and fall through to the next case.
        /// </summary>
        /// <param name="predicate">The predicate to test the value we are switching on.</param>
        /// <param name="action">The action to do in this case.</param>
        public Switch<TNew, S> CaseWithFallThrough<TNew>(
            Func<TNew, bool> predicate, Action<TNew> action)
            where TNew : S {
            bool goodType = _object is TNew;
            return new Switch<TNew, S>(
                goodType ? (TNew) _object : default(TNew), _object)
                   {_badType = !goodType}
                .CaseWithFallThrough(predicate, action);
        }

        /// <summary>
        /// In case we have reached this, if the value we are switching on has type <typeparam name="TNew"/>, do <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TNew">The type of the new value.</typeparam>
        /// <param name="action">The action to do.</param>
        public void Default<TNew>(Action<TNew> action) where TNew : S {
            if (_object is TNew) {
                new Switch<TNew, S>((TNew) _object, _object).Default(
                    action);
            }
        }
    }

    /// <summary>
    /// A generic functional switch expression (as opposed to a statement).
    /// </summary>
    /// <typeparam name="T">The current type of the object we are switching on.</typeparam>
    /// <typeparam name="S">The real type of the object we are switching on.</typeparam>
    /// <typeparam name="R">The type of the result.</typeparam>
    /// <example><![CDATA[
    /// var res =
    ///      from x in typeof(string).GetMembers()
    ///      select Switch.ExprOn<MemberInfo, string>(x)
    ///             .Case<MethodInfo>(m => m.Name + " is a method")
    ///             .Case<PropertyInfo>(m => m.Name + " is a property")
    ///             .Default(m => m.Name + " is something else");
    ///
    /// foreach (var s in res)
    ///     Console.WriteLine(s);
    /// ]]></example>
    /// <remarks>
    /// Exposes no public constructors. Use <see cref="Switch.ExprOn{T, R}"/> to create an instance, as in the example.
    /// </remarks>
    public class Switch<T, S, R> where T : S {
        private readonly S _object;
        private readonly T _value;
        private bool _badType;
        private bool _hasResult;
        private R _result;

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch{T, S}"/> class.
        /// </summary>
        /// <param name="value">The object we are switching on, as <typeparamref name="T"/>.</param>
        /// <param name="realValue">The object we we are switching on, as <typeparamref name="S"/>/>.</param>
        internal Switch(T value, S realValue) {
            _value = value;
            _object = realValue;
        }

        /// <summary>
        /// Gets a value indicating whether this switch has a result.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this switch "expression" has a result; otherwise, <c>false</c>.
        /// </value>
        public bool HasResult {
            get { return _hasResult; }
        }

        /// <summary>
        /// Gets the result of this switch.
        /// </summary>
        /// <value>The result, if <see cref="HasResult"/> is <c>true</c>; <c>default(R)</c> otherwise.</value>
        public R Result {
            get { return _result; }
            private set {
                _hasResult = true;
                _result = value;
            }
        }

        /// <summary>
        /// In case the value we are switching on equals <paramref name="t"/>, apply 
        /// <paramref name="function"/> to it and return the result.
        /// </summary>
        /// <param name="t">The value to compare the value we are switching on to.</param>
        /// <param name="function">The action to do in this case.</param>
        public Switch<T, S, R> Case(T t, Func<T, R> function) {
            return Case(x => x.Equals(t), function);
        }

        /// <summary>
        /// In case the value we are switching on satisfies <paramref name="predicate"/>, apply 
        /// <paramref name="function"/> to it and return the result.
        /// </summary>
        /// <param name="predicate">The predicate to test the value we are switching on.</param>
        /// <param name="function">The action to do in this case.</param>
        public Switch<T, S, R> Case(Func<T, bool> predicate,
                                    Func<T, R> function) {
            if (!HasResult && !_badType && predicate(_value))
                Result = function(_value);
            return this;
        }

        /// <summary>
        /// In case we have reached this, apply 
        /// <paramref name="function"/> to the value we are switching on and return the result.
        /// </summary>
        /// <param name="function">The action to do in this case.</param>
        public R Default(Func<S, R> function) {
            if (!HasResult)
                Result = function(_object);
            return Result;
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparam name="TNew"/>, apply
        /// <paramref name="function"/> to it and return the result.
        /// </summary>
        /// <typeparam name="TNew">The type of the new value.</typeparam>
        /// <param name="function">The action to do in this case.</param>
        public Switch<TNew, S, R> Case<TNew>(Func<TNew, R> function)
            where TNew : S {
            return Case(x => true, function);
        }

        /// <summary>
        /// In case the value we are switching on has type <typeparam name="TNew"/>
        /// and satisfies <paramref name="predicate"/>, apply 
        /// <paramref name="function"/> to it and return the result.
        /// </summary>
        /// <typeparam name="TNew">The type of the new value.</typeparam>
        /// <param name="predicate">The predicate to test the value we are switching on.</param>
        /// <param name="function">The action to do in this case.</param>
        public Switch<TNew, S, R> Case<TNew>(
            Func<TNew, bool> predicate, Func<TNew, R> function)
            where TNew : S {
            bool goodType = _object is TNew;
            bool hasResult = _hasResult;
            R result = _result;
            return new Switch<TNew, S, R>(
                goodType ? (TNew) _object : default(TNew), _object) {
                                                                        _badType = !goodType,
                                                                        _hasResult = hasResult,
                                                                        _result = result
                                                                    }.
                Case(predicate, function);
        }
    }
}
/*
* Monoid.cs is part of functional-dotnet project
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
using System.Linq;
using System.Collections.Generic;
using FP.Core;

namespace FP.Collections.Persistent {
    /// <summary>
    /// A monoid structure on the type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the elements of the monoid.</typeparam>
    public class Monoid<T> {
        /// <summary>
        /// The zero of the monoid. It should always be the case that <code>forall x. Zero.Plus(x) == x</code>
        /// and <code>x.Plus(Zero) == x</code>.
        /// </summary>
        public readonly T Zero;

        /// <summary>
        /// The addition operation. It should always be associative, that is, <code>forall x y z. x.Plus(y).Plus(z) == x.Plus(y.Plus(z))</code>.
        /// </summary>
        public readonly Func<T, T, T> Plus;

        /// <summary>
        /// Initializes a new instance of the <see cref="Monoid{T}"/> class.
        /// </summary>
        /// <param name="zero">The zero.</param>
        /// <param name="plus">The add.</param>
        public Monoid(T zero, Func<T, T, T> plus) {
            Zero = zero;
            Plus = plus;
        }

        /// <summary>
        /// Sums three <typeparamref name="T"/>s.
        /// </summary>
        public T Sum(T t1, T t2, T t3) {
            return Plus(Plus(t1, t2), t3);
        }

        /// <summary>
        /// Sums three <typeparamref name="T"/>s.
        /// </summary>
        public T Sum(params T[] ts) {
            return Sum(Zero, ts.AsEnumerable());
        }

        /// <summary>
        /// Sums three <typeparamref name="T"/>s.
        /// </summary>
        public T Sum(T init, params T[] ts) {
            return Sum(init, ts.AsEnumerable());
        }

        /// <summary>
        /// Sums a sequence of <typeparamref name="T"/>s.
        /// </summary>
        public T Sum(IEnumerable<T> ts) {
            return Sum(Zero, ts);
        }

        /// <summary>
        /// Sums a sequence of <typeparamref name="T"/>s.
        /// </summary>
        public T Sum(T init, IEnumerable<T> ts) {
            T total = init;
            foreach (T t in ts)
                total = Plus(total, t);
            return total;
        }
    }

    /// <summary>
    /// Predefined monoids.
    /// </summary>
    public static class Monoids {
        /// <summary>
        /// Natural numbers with addition. The monoid for 
        /// <see cref="RandomAccessSequence{T}"/>.
        /// </summary>
        public static readonly Monoid<int> Size = new Monoid<int>(0, (x, y) => x + y);

        /// <summary>
        /// Double numbers with max operation. The monoid for 
        /// <see cref="PriorityQueue{P,T}"/>.
        /// </summary>
        public static readonly Monoid<double> Priority =
            new Monoid<double>(double.NegativeInfinity, Math.Max);

        /// <summary>
        /// The product of two monoids.
        /// </summary>
		public static Monoid<FP.Core.Tuple<T1, T2>> Product<T1, T2>(this Monoid<T1> monoid1,
                                                            Monoid<T2> monoid2) {
																return new Monoid<FP.Core.Tuple<T1, T2>>(Pair.New(monoid1.Zero, monoid2.Zero),
                                             (p1, p2) =>
                                             Pair.New(monoid1.Plus(p1.Item1, p2.Item1),
                                                      monoid2.Plus(p1.Item2, p2.Item2)));
        }

        /// <summary>
        /// The monoid used for <see cref="PriorityQueue{P,T}"/>.
        /// </summary>
        public static Monoid<P> PriorityM<P>(P min, IComparer<P> comparer) {
            return new Monoid<P>(min, (x, y) => comparer.Max(x, y));
        }
    }
}
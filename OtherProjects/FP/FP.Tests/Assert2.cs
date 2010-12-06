/*
* Assert2.cs is part of functional-dotnet project
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XunitExtensions {
    /// <summary>
    /// Contains some assertions missing in xunit.net
    /// </summary>
    public static class Assert2 {
        /// <summary>
        /// Verifies that two sequences contain same elements (by the default equality comparer) in the same order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected">The first sequence.</param>
        /// <param name="actual">The second sequence.</param>
        /// <exception cref="SequenceEqualException">If the sequences turn out not to be equal.</exception>
        public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual) {
            SequenceEqual(expected, actual, EqualityComparer<T>.Default);
        }

		public static void IsType<T>(object o)
		{
			Assert.AreEqual(typeof(T), o.GetType());
		}
		public static void Throws<T>(Action a)
		{
			Throws(typeof(T), a);
		}
		public static void Throws(Type ex, Action a)
		{
			try
			{
				a();
			}
			catch(Exception e)
			{
				Assert.AreEqual(e.GetType(), ex);
				return;
			}
			Assert.Fail();
		}

        /// <summary>
        /// Verifies that two sequences contain same elements (by the given comparer) in the same order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected">The first sequence.</param>
        /// <param name="actual">The second sequence.</param>
        /// <param name="comparer">The comparer.</param>
        /// <exception cref="SequenceEqualException">If the sequences turn out not to be equal.</exception>
        public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual,
                                            IComparer<T> comparer) {
            SequenceEqual(expected, actual, comparer.ToEqualityComparer());
        }

        /// <summary>
        /// Verifies that two sequences contain same elements (by the given equality comparer) in the same order.
        /// </summary>
        /// <param name="expected">The first sequence.</param>
        /// <param name="actual">The second sequence.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <exception cref="SequenceEqualException">If the sequences turn out not to be equal.</exception>
        public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual,
                                            IEqualityComparer<T> equalityComparer) {
            int i = 0;
            using (var enum1 = expected.GetEnumerator())
            using (var enum2 = actual.GetEnumerator()) {
                while (true) {
                    bool finished1 = !enum1.MoveNext();
                    bool finished2 = !enum2.MoveNext();
                    if (finished1 && finished2)
                        break;
                    if (finished1)
                        throw new SequenceEqualException(i, "nothing", enum2.Current);
                    if (finished2)
                        throw new SequenceEqualException(i, enum1.Current, "nothing");
                    if (!equalityComparer.Equals(enum1.Current, enum2.Current))
                        throw new SequenceEqualException(i, enum1.Current, enum2.Current);
                    i++;
                }
            }
        }

        /// <summary>
        /// Verifies that two sequences contain same elements, independent of their order.
        /// </summary>
        /// <param name="seq1">The first sequence.</param>
        /// <param name="actual">The second sequence.</param>
        /// <exception cref="Xunit.TrueException">Thrown if the sequences do not have same elements.</exception>
        public static void SequenceEquivalent<T>(IEnumerable<T> seq1, IEnumerable<T> actual)
            where T : IComparable<T> {
            Assert.IsTrue(seq1.OrderBy(x => x).SequenceEqual(actual.OrderBy(x => x)),
                        "Sequences do not have the same elements");
        }

        /// <summary>
        /// Verifies that the specified assertion fails.
        /// </summary>
        /// <param name="assertion">The assertion.</param>
        /// <param name="userMessage">The user message.</param>
        /// <exception cref="NotException">Thrown if assertion succeeds.</exception>
        public static void Not(Action assertion, string userMessage) {
            bool failed;
            try {
                assertion();
                failed = true;
            }
            catch (AssertFailedException) {
                failed = false;
            }
            if (failed)
                throw new NotException(userMessage);
        }

        /// <summary>
        /// Verifies that the <c>Count</c> property is equal to the number of iterated elements.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="Xunit.EqualException">Thrown when the collection doesn't have the correct count.</exception>
        public static void IsCountCorrect(ICollection collection) {
            int i = 0;
            foreach (var elt in collection)
                i++;
            Assert.AreEqual(collection.Count, i);
        }

        /// <summary>
        /// Verifies that <paramref name="subset"/> is a subset of <paramref name="superset"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subset">The subset.</param>
        /// <param name="superset">The superset.</param>
        /// <exception cref="SubsetException">Thrown if <paramref name="subset"/> is not a subset of <paramref name="superset"/>.</exception>
        public static void IsSubsetOf<T>(IEnumerable<T> subset, IEnumerable<T> superset)
            where T : IComparable<T> {
            var sortedSubset = subset.OrderBy(x => x);
            var sortedSuperset = superset.OrderBy(x => x);
            using (var enumerator = sortedSuperset.GetEnumerator()) {
                enumerator.MoveNext();
                foreach (var t in sortedSubset) {
                    while (enumerator.Current.CompareTo(t) < 0) {
                        if (!enumerator.MoveNext())
                            throw new SubsetException();
                    }
                    if (enumerator.Current.CompareTo(t) > 0)
                        throw new SubsetException();
                }
            }
        }
    }
}
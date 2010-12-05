/*
* Dicts.cs is part of functional-dotnet project
* 
* Copyright (c) 2009 Alexey Romanov
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
using FP.Collections.Persistent;
using FP.Core;
using MiniBench;

namespace Benchmarks {
    public class Dicts {
        private const int N = 5000;

        private static readonly TreeDictionary<int, int, DefaultComparer<int>> dict =
            TreeDictionary.Empty<int, int>().AddAll(Ints.Range(0, 2 * N, 2).Zip(Ints.Range(0, 2 * N, 2)));

        public static void Main() {
//            var results1 = new TestSuite<IEnumerable<int>, int>(
//                "Compare adding to dictionary by different methods without repeats")
//                .Plus(AddDirect, "Call Add(key, value)")
//                .Plus(AddWithFunc, "Call Add(key, value, combiner)")
//                .RunTests(Ints.Range(1, 2 * N + 1, 2), 2 * N + 2);
//
//            results1.Display(ResultColumns.All, results1.FindBest());
//
//            var results2 = new TestSuite<IEnumerable<int>, int>(
//                "Compare adding to dictionary by different methods with repeats")
//                .Plus(AddDirect, "Call Add(key, value)")
//                .Plus(AddWithFunc, "Call Add(key, value, combiner)")
//                .RunTests(Ints.Range(0, 2 * N, 2), N + 1);
//
//            results2.Display(ResultColumns.All, results1.FindBest());
        }

//        private static int AddStack(IEnumerable<int> arg) {
//            var dict1 = dict;
//
//            foreach (var i in arg) {
//                dict1 = dict1.AddStack(i, i);
//            }
//
//            return dict1.Count;
//        }

        private static int AddDirect(IEnumerable<int> arg) {
            return dict.AddAll(arg.Zip(arg)).Count;
        }

        private static int AddWithFunc(IEnumerable<int> arg) {
            return dict.AddAll(arg.Zip(arg), (key, v1, v2) => v2).Count;
        }
    }
}
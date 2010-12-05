/*
* TreeDictionaryTests.cs is part of functional-dotnet project
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
using System.Linq;
using FP.Collections.Persistent;
using FP.Core;
using Microsoft.Pex.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FPTests {
    [PexClass(typeof(TreeDictionary<,,>))]
	[TestClass]
    public partial class TreeDictionaryTests {
        private TreeDictionary<TKey, TValue, DefaultComparer<TKey>> CreateDictionary<TKey, TValue>(
			TKey[] keys, TValue[] values) where TKey : struct, IEquatable<TKey>
		{
            PexAssume.IsNotNull(keys);
            PexAssume.IsNotNull(values);
            PexAssume.AreEqual(keys.Length, values.Length);
			
            PexAssume.AreDistinctValues<TKey>(keys);
            return TreeDictionary.Empty<TKey, TValue>().AddAll(keys.Zip(values));
        }

        [PexMethod]
        [PexGenericArguments(new[] { typeof(int), typeof(object) })]
        public void Test_Lookup<TValue>(int[] keys, TValue[] values) {
            var dict = CreateDictionary(keys, values);
            for (int i = 0; i < keys.Length; i++) {
                PexAssert.AreEqual(values[i], dict[keys[i]]);
            }
        }

        //[TestMethod]
        public void Test_NoStackOverflowForLargeDictionaries() {
            const int N = 1000000;
            var array = new int[N];
            for (int i = 0; i < N; i++) {
                array[i] = i;
            }
            CreateDictionary(array, array);
        }

        [PexMethod]
        [PexGenericArguments(new[] { typeof(int), typeof(object) })]
        public void Test_Enumeration<TValue>(int[] keys, TValue[] values) {
            var dict = CreateDictionary(keys, values);
            Array.Sort(keys, values);
            int i = 0;
            foreach (var pair in dict) {
                PexAssert.AreEqual(pair.Item1, keys[i]);
                PexAssert.AreEqual(pair.Item2, values[i]);
                i++;
            }
        }

        [PexMethod]
        [PexGenericArguments(new[] { typeof(int), typeof(object) })]
        public void Test_Remove<TValue>(int[] keys, TValue[] values, int key, int key1) {
            var dict = CreateDictionary(keys, values);
            PexAssume.AreNotEqual(key, key1);
            Optional<TValue> value;
            var dict1 = dict.Remove(key, out value);
            Array.Sort(keys, values);
            int i = Array.BinarySearch(keys, key);
            PexAssert
                .Case(i >= 0).Implies(() => dict[key] == value && dict1[key] == Optional<TValue>.None && dict[key1] == dict1[key1])
                .Case(i < 0).Implies(() => dict[key] == Optional<TValue>.None && ReferenceEquals(dict1, dict))
                .ExpectExactlyOne();
        }

        [PexMethod]
        public void Test_Update(int[] keys, int[] values, int key, int key1, [PexAssumeNotNull] Func<int, Optional<int>> updateFunc) {
            var dict = CreateDictionary(keys, values);
            PexAssume.AreNotEqual(key, key1);
            Optional<int> value;
            var dict1 = dict.Update(key, updateFunc, out value);
            Array.Sort(keys, values);
            int i = Array.BinarySearch(keys, key);
            PexAssert
                .Case(i >= 0).Implies(() => dict[key] == value && dict1[key] == updateFunc(value.Value) && dict[key1] == dict1[key1])
                .Case(i < 0).Implies(() => dict[key] == Optional<int>.None && ReferenceEquals(dict1, dict))
                .ExpectExactlyOne();
        }

        [PexMethod]
        public void Test_Map(int[] keys, int[] values, [PexAssumeNotNull] Func<int, int, int> func) {
            var dict = CreateDictionary(keys, values);
            PexAssume.AreNotEqual(0, keys.Length);
            func = func.Memoize();
            var dict1 = dict.Map(func);
            PexAssert.AreEqual(func(keys[0], values[0]), dict1[keys[0]]);
        }

        [PexMethod]
        public void Test_MapPartial(int[] keys, int[] values, int key1, [PexAssumeNotNull] Func<int, int, Optional<int>> func) {
            var dict = CreateDictionary(keys, values);
            PexAssume.AreNotEqual(0, keys.Length);
            int i = PexChoose.IndexValue("From keys", keys);
            foreach (int key in keys)
                PexAssume.AreNotEqual(key, key1);
            var dict1 = dict.MapPartial(func);
            PexAssert.AreEqual(Optional<int>.None, dict1[key1]);
            PexAssert.AreEqual(func(keys[i], values[i]), dict1[keys[i]]);
        }
    }
}
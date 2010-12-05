/*
* Factories.cs is part of functional-dotnet project
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

using FP.Core;
using Microsoft.Pex.Framework;

namespace FPTests {
    public static class Factories {
        public static Optional<T> OptionalFactory<T>(bool hasValue, T value) {
            return hasValue ? Optional.Some(value) : Optional.None<T>();
        }

        [PexFactoryMethod]
        public static Optional<int> OptionalIntFactory(bool hasValue, int value) {
            return OptionalFactory(hasValue, value);
        }
    }
}
/*
* Pair.cs is part of functional-dotnet project
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

namespace FP.Core {
    /// <summary>
    /// A static class which contains a method to create <see cref="Tuple{T1,T2}"/>.
    /// </summary>
    public static class Pair {
        /// <summary>
        /// Creates a <see cref="Tuple{T1,T2}"/> with the specified elements.
        /// Provided to make use of generic parameter inference.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="first">The first element of the tuple.</param>
        /// <param name="second">The second element of the tuple.</param>
        /// <returns></returns>
        public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second) {
            return new Tuple<T1, T2>(first, second);
        }
    } // class Pair
} // namespace FP.Core
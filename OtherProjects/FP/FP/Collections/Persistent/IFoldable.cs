/*
* IFoldable.cs is part of functional-dotnet project
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

namespace FP.Collections.Persistent {
    /// <summary>
    /// Any structure which can be folded into a summary value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFoldable<T> {
        /// <summary>
        /// Reduces the finger tree from the right.
        /// </summary>
        /// <typeparam name="TAcc">The type of the accumulator.</typeparam>
        /// <param name="binOp">The binary operation.</param>
        /// <param name="initial">The initial accumulator value.</param>
        /// <returns>
        /// The final accumulator value.
        /// </returns>
        TAcc FoldRight<TAcc>(Func<T, TAcc, TAcc> binOp, TAcc initial);

        /// <summary>
        /// Reduces the finger tree from the left.
        /// </summary>
        /// <typeparam name="TAcc">The type of the accumulator.</typeparam>
        /// <param name="binOp">The binary operation.</param>
        /// <param name="initial">The initial accumulator value.</param>
        /// <returns>
        /// The final accumulator value.
        /// </returns>
        TAcc FoldLeft<TAcc>(Func<TAcc, T, TAcc> binOp, TAcc initial);
    }
}
/*
* IMeasured.cs is part of functional-dotnet project
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

namespace FP.Collections.Persistent {
    /// <summary>
    /// The type implementing the interface can be measured by elements of the type <typeparamref name="V"/>.
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public interface IMeasured<V> {
        /// <summary>
        /// Gets the measure of the object.
        /// </summary>
        /// <value>The measure.</value>
        V Measure { get; }
    } // interface IMeasured
} // namespace FP.Collections.Immutable
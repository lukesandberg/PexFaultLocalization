/*
* PriorityValuePair.cs is part of functional-dotnet project
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

namespace FP.Collections.Persistent {
    /// <summary>
    /// Simply a pair of priority and value for use in <see cref="PriorityQueue{T,TPriority}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TPriority">The type of the priority.</typeparam>
    [Serializable]
    public struct PriorityValuePair<T, TPriority> : IMeasured<TPriority> where TPriority : IComparable<TPriority> {
        /// <summary>
        /// The priority.
        /// </summary>
        public readonly TPriority Priority;

        /// <summary>
        /// The value.
        /// </summary>
        public readonly T Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityValuePair{T, TPriority}"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="priority">The priority.</param>
        public PriorityValuePair(T value, TPriority priority) {
            Priority = priority;
            Value = value;
        }

        TPriority IMeasured<TPriority>.Measure {
            get { return Priority; }
        }
    }
}

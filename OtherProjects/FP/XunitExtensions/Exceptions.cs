/*
* Exceptions.cs is part of functional-dotnet project
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XunitExtensions {
    /// <summary>
    /// An exception thrown if sequences should be equal, but aren't.
    /// </summary>
	public class SequenceEqualException : AssertFailedException
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceEqualException"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        public SequenceEqualException(int index, object expected, object actual) :
            base(string.Format("Sequences are different at index {0}: expected {1} but was {2}",
                               index, expected, actual)) {}
    }

    /// <summary>
    /// An exception thrown if sequences should be equivalent, but aren't.
    /// </summary>
	public class SequenceEquivalentException : AssertFailedException
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceEquivalentException"/> class.
        /// </summary>
        public SequenceEquivalentException() :
            base("Sequences do not have same elements") {}
    }

    /// <summary>
    /// An exception thrown if one sequence should be a subset of another, but isn't.
    /// </summary>
    public class SubsetException : AssertFailedException {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubsetException"/> class.
        /// </summary>
        public SubsetException() :
            base("One sequence is not a subset of another") {}
    }

    /// <summary>
    /// An exception thrown if an assertion should fail, but succeeds.
    /// </summary>
	public class NotException : AssertFailedException
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="NotException"/> class.
        /// </summary>
        /// <param name="userMessage">The user message to be displayed</param>
        public NotException(string userMessage) : base(userMessage) {}
    }
}
/*
* Grouping.cs is part of functional-dotnet project
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FP.Collections.Persistent {
    /// <summary>
    /// Represents a grouping of elements which correspond to a single key.
    /// </summary>
    /// <typeparam name="K">The type of the key.</typeparam>
    /// <typeparam name="E">The type of elements.</typeparam>
    public class Grouping<K, E> : IGrouping<K, E> {
        internal Grouping(List<E> elements, K key) {
            _elements = elements;
            _key = key;
        }

        private readonly List<E> _elements;
        private readonly K _key;

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<E>) this).GetEnumerator();
        }

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public IEnumerator<E> GetEnumerator() {
            return _elements.GetEnumerator();
        }

        ///<summary>
        ///Gets the key of the <see cref="T:System.Linq.IGrouping`2" />.
        ///</summary>
        ///
        ///<returns>
        ///The key of the <see cref="T:System.Linq.IGrouping`2" />.
        ///</returns>
        ///
        public K Key {
            get { return _key; }
        }
    }
}
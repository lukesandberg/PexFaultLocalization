using System;

namespace FP.Collections.Persistent {
    public interface ICombinableDictionary<TKey, TValue, TDictionary> :
        IDictionary<TKey, TValue, TDictionary> 
        where TDictionary : ICombinableDictionary<TKey, TValue, TDictionary> {
        /// <summary>
        /// Returns the dictionary containing all keys present in one of the
        /// dictionaries with the same values. If a key is present in both dictionaries, 
        /// the value from <c>this</c> dictionary is used.
        /// </summary>
        /// <param name="otherDict">
        /// The other dictionary.
        /// </param>
        /// <returns>
        /// The union of two dictionaries.
        /// </returns>
        TDictionary UnionLeftBiased(TDictionary otherDict);

        /// <summary>
        /// Returns the dictionary containing all keys present in one of the
        /// dictionaries with the same values. If a key is present in both dictionaries, 
        /// the value is obtained by using the <paramref name="combiner"/>.
        /// </summary>
        /// <param name="otherDict">
        /// The other dictionary.
        /// </param>
        /// <param name="combiner">
        /// The function used for combining values of duplicate keys.
        /// </param>
        /// <returns>
        /// The union of two dictionaries.
        /// </returns>
        TDictionary Union(TDictionary otherDict, Func<TKey, TValue, TValue, TValue> combiner);

        /// <summary>
        /// Returns the dictionary containing all keys present in this
        /// dictionary, but not in the other.
        /// </summary>
        /// <param name="otherDict">
        /// The other dictionary.
        /// </param>
        /// <param name="combiner">
        /// The function used for combining values of duplicate keys.
        /// </param>
        /// <returns>
        /// The difference of two dictionaries.
        /// </returns>
        TDictionary Difference(TDictionary otherDict);

        /// <summary>
        /// Returns the dictionary containing all keys present in both
        /// dictionaries. The value from <c>this</c> dictionary is used.
        /// </summary>
        /// <param name="otherDict">
        /// The other dictionary.
        /// </param>
        /// <param name="combiner">
        /// The function used for combining values of duplicate keys.
        /// </param>
        /// <returns>
        /// The difference of two dictionaries.
        /// </returns>
        TDictionary IntersectionLeftBiased(TDictionary otherDict);

        /// <summary>
        /// Returns the dictionary containing all keys present in both
        /// dictionaries. The value is obtained by using the 
        /// <paramref name="combiner"/>.
        /// </summary>
        /// <param name="otherDict">
        /// The other dictionary.
        /// </param>
        /// <param name="combiner">
        /// The function used for combining values of duplicate keys.
        /// </param>
        /// <returns>
        /// The difference of two dictionaries.
        /// </returns>
        TDictionary Intersection(TDictionary otherDict, Func<TKey, TValue, TValue, TValue> combiner);
    }
}
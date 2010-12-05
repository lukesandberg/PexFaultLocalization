/*
* Strings.cs is part of functional-dotnet project
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FP.Core;
using FP.HaskellNames;

namespace FP.Text {
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for <see cref="string"/>s.
    /// </summary>
    /// <remarks>
    /// The source of each method includes the Haskell version as a comment at the end.
    /// Some methods are not safe to use with mutable collections. These cases are mentioned in the 
    /// documentation for the method. The order of arguments has generally been inverted compared 
    /// to the Haskell versions to facilitate method chaining.
    /// 
    /// See also the Remarks for <see cref="Enumerable"/>.
    /// </remarks>
    public static class Strings {
        /// <summary>
        /// Determines whether the string is null or empty. This is merely <c>string.IsNullOrEmpty(s)</c>
        /// as an extension method.
        /// </summary>
        public static bool IsNullOrEmpty(this string s) {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Clears the specified <see cref="StringBuilder"/>.
        /// This is equivalent to calling <c>sb.Count = 0</c>.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/>.</param>
        public static void Clear(this StringBuilder sb) {
            sb.Length = 0;
        }

        /// <summary>
        /// Converts a sequence of <see cref="char"/>s to a <see cref="string"/>.
        /// This should be called <c>ToString</c>, but this is impossible for the
        /// obvious reason.
        /// </summary>
        /// <param name="charSequence">The char sequence.</param>
        /// <returns></returns>
        public static string AsString(this IEnumerable<char> charSequence) {
            return Switch.ExprOn<IEnumerable<char>, string>(charSequence)
                .Case<string>(s => s)
                .Default(cs => {
                             var builder = new StringBuilder();
                             foreach (char c in charSequence)
                                 builder.Append(c);
                             return builder.ToString();
                         });
        }

        /// <summary>
        /// Breaks the specified char sequence into a sequence of strings at newline
        /// characters. The resulting strings do not contain newlines.
        /// </summary>
        /// <param name="charSequence">The char sequence.</param>
        /// <returns>The sequence of lines.</returns>
        public static IEnumerable<string> Lines(this IEnumerable<char> charSequence) {
            var sb = new StringBuilder();
            char lastChar = '\0';
            foreach (char c in charSequence) {
                if (c != '\n')
                    sb.Append(c);
                else {
                    if (lastChar == '\r')
                        sb.Length -= 1;
                    yield return sb.ToString();
                    sb.Clear();
                }
                lastChar = c;
            }
            if (lastChar != '\n') yield return sb.ToString();

            //lines			:: String -> [String]
            //lines ""		=  []
            //lines s			=  let (l, s') = break (== '\n') s
            //			   in  l : case s' of
            //					[]     	-> []
            //					(_:s'') -> lines s''
        }

        /// <summary>
        /// An inverse to <see cref="Lines"/>. It joins lines, after appending a terminating newline to each.
        /// </summary>
        /// <param name="lines">A sequence of sequences of chars.</param>
        public static IEnumerable<char> UnLinesCS(this IEnumerable<IEnumerable<char>> lines) {
            return lines.Intercalate(Environment.NewLine);

            //unlines			:: [String] -> String
            //unlines [] = []
            //unlines (l:ls) = l ++ '\n' : unlines ls
        }

        /// <summary>
        /// An inverse to <see cref="Lines"/>. Joins lines, after appending a terminating newline to each.
        /// </summary>
        /// <param name="lines">A sequence of strings.</param>
        public static IEnumerable<char> UnLines(this IEnumerable<string> lines) {
            return lines.Cast<IEnumerable<char>>().UnLinesCS();
        }

        /// <summary>
        /// Equivalent to <c>UnLines().AsString()</c>, but more efficient.
        /// </summary>
        /// <param name="lines">A sequence of strings.</param>
        public static string UnLinesAsString(this IEnumerable<string> lines) {
            var sb = new StringBuilder();
            foreach (string s in lines.Intersperse(Environment.NewLine))
                sb.Append(s);
            return sb.ToString();
        }

        /// <summary>
        /// Breaks the specified char sequence into a sequence of words delimited by 
        /// whitespace. The resulting strings do not contain whitespace.
        /// </summary>
        /// <param name="charSequence">The char sequence.</param>
        /// <returns>The sequence of words.</returns>
        public static IEnumerable<string> Words(this IEnumerable<char> charSequence) {
            var sb = new StringBuilder();
            bool lastCharWasWhiteSpace = true;

            foreach (char c in charSequence) {
                if (!char.IsWhiteSpace(c)) sb.Append(c);
                else {
                    if (!lastCharWasWhiteSpace) {
                        yield return sb.ToString();
                        sb.Clear();
                    }
                }
                lastCharWasWhiteSpace = char.IsWhiteSpace(c);
            }

            if (!lastCharWasWhiteSpace)
                yield return sb.ToString();

            //words			:: String -> [String]
            //words s			=  case dropWhile {-partain:Char.-}isSpace s of
            //				"" -> []
            //				s' -> w : words s''
            //				      where (w, s'') = 
            //                                             break {-partain:Char.-}isSpace s'
        }

        /// <summary>
        /// An inverse to <see cref="Words"/>. It joins words, separating them by a single space.
        /// </summary>
        /// <param name="words">A sequence of sequences of chars.</param>
        /// <returns></returns>
        public static IEnumerable<char> UnWordsCS(this IEnumerable<IEnumerable<char>> words) {
            return words.Intercalate(" ");

            //unwords			:: [String] -> String
            //unwords []		=  ""
            //unwords [w]		= w
            //unwords (w:ws)		= w ++ ' ' : unwords ws
        }

        /// <summary>
        /// An inverse to <see cref="Words"/>. It joins words, separating them by a single space.
        /// </summary>
        /// <param name="words">A sequence of strings.</param>
        /// <returns></returns>
        public static IEnumerable<char> UnWords(this IEnumerable<string> words) {
            return words.Map(word => word.AsEnumerable()).Intercalate(" ");
        }

        /// <summary>
        /// Equivalent to <c>UnWords().AsString()</c>, but more efficient.
        /// </summary>
        /// <param name="words">A sequence of strings.</param>
        /// <returns></returns>
        public static string UnWordsAsString(this IEnumerable<string> words) {
            var sb = new StringBuilder();
            foreach (string s in words.Intersperse(" "))
                sb.Append(s);
            return sb.ToString();
        }

        private static readonly char[] _whitespaceChars =
            new[] {
                      '\t', '\n', '\v', '\f', '\r', ' ',
                      '\u0085', '\u00a0', '\u1680', '\u2000', '\u2001',
                      '\u2002', '\u2003', '\u2004', '\u2005', '\u2006',
                      '\u2007', '\u2008', '\u2009', '\u200a', '\u200b', '\u2028',
                      '\u2029', '\u3000', '\ufeff'
                  };

        /// <summary>
        /// Gets the array of all whitespace <see cref="char"/>s.
        /// </summary>
        /// <value>The whitespace <see cref="char"/>s.</value>
        public static char[] WhitespaceChars {
            get { return _whitespaceChars; }
        }

        /// <summary>
        /// Enumerates the specified string in the reverse order.
        /// </summary>
        public static IEnumerable<char> ReverseIterator(this string s) {
            for (int i = s.Length - 1; i >= 0; i--)
                yield return s[i];
        }
    }
}
// <copyright file="OriginalTest.cs">Copyright ©  2010</copyright>
using System;
using Edu.Unl.Sir.Siemens.PrintTokens.Orig;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Edu.Unl.Sir.Siemens.PrintTokens.Orig
{
    /// <summary>This class contains parameterized unit tests for Original</summary>
    [PexClass(typeof(Original))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class OriginalTest
    {
        /// <summary>Test stub for check_delimiter(Byte)</summary>
        [PexMethod]
        public int check_delimiter(byte ch)
        {
            int result = Original.check_delimiter(ch);
            return result;
            // TODO: add assertions to method OriginalTest.check_delimiter(Byte)
        }

        /// <summary>Test stub for constant(Int32, Byte[], Int32)</summary>
        [PexMethod]
        public int constant(
            int state,
            byte[] token_str,
            int token_ind
        )
        {
            PexAssume.IsNotNull(token_str);
            PexAssume.IsTrue(token_ind >= 0);
            PexAssume.IsTrue(token_str.Length > token_ind);
            PexAssume.IsTrue(token_ind > 1);
            int result = Original.constant(state, token_str, token_ind);
            return result;
            // TODO: add assertions to method OriginalTest.constant(Int32, Byte[], Int32)
        }

        /// <summary>Test stub for error_or_eof_case(TokenStream, Token, Int32, Byte[], Int32, Byte)</summary>
        [PexMethod]
        public Token error_or_eof_case(
            TokenStream tstream_ptr,
            Token token_ptr,
            int cu_state,
            byte[] token_str,
            int token_ind,
            byte ch
        )
        {
            PexAssume.IsNotNull(tstream_ptr);
            PexAssume.IsNotNull(tstream_ptr.ch_stream);
            PexAssume.IsNotNull(tstream_ptr.ch_stream.fp);
            PexAssume.IsNotNullOrEmpty(tstream_ptr.ch_stream.stream);
            PexAssume.IsNotNull(token_ptr);
            PexAssume.IsNotNullOrEmpty(token_ptr.token_string);
            PexAssume.IsTrue(tstream_ptr.ch_stream.stream_ind > 0 && tstream_ptr.ch_stream.stream_ind < tstream_ptr.ch_stream.stream.Length);
            PexAssume.IsNotNullOrEmpty(token_str);
            PexAssume.IsTrue(token_str.Length > token_ind);
            PexAssume.IsTrue(token_ind > 1 || (token_ind > 0 && cu_state == 0));
            PexAssume.IsTrue(token_str.Length == token_ptr.token_string.Length);

            Token result = Original.error_or_eof_case
                               (tstream_ptr, token_ptr, cu_state, token_str, token_ind, ch);
            return result;
            // TODO: add assertions to method OriginalTest.error_or_eof_case(TokenStream, Token, Int32, Byte[], Int32, Byte)
        }

        /// <summary>Test stub for get_actual_token(Byte[], Int32)</summary>
        [PexMethod]
        public void get_actual_token(byte[] token_str, int token_ind)
        {
            PexAssume.IsNotNullOrEmpty(token_str);
            PexAssume.IsTrue(token_str.Length > token_ind);
            PexAssume.IsTrue(token_ind >= 0);
            Original.get_actual_token(token_str, token_ind);
            // TODO: add assertions to method OriginalTest.get_actual_token(Byte[], Int32)
        }

        /// <summary>Test stub for get_char(CharacterStream)</summary>
        [PexMethod]
        public byte get_char(CharacterStream stream_ptr)
        {
            PexAssume.IsNotNull(stream_ptr);
            PexAssume.IsNotNull(stream_ptr.fp);
            PexAssume.IsNotNullOrEmpty(stream_ptr.stream);
            PexAssume.IsTrue(stream_ptr.stream_ind < stream_ptr.stream.Length);
            PexAssume.IsTrue(stream_ptr.stream_ind >= 0);
            PexAssume.IsTrue(stream_ptr.stream.Length > 1);
            byte result = Original.get_char(stream_ptr);
            return result;
            // TODO: add assertions to method OriginalTest.get_char(CharacterStream)
        }

        /// <summary>Test stub for get_token(TokenStream)</summary>
        [PexMethod, PexAllowedException(typeof(IndexOutOfRangeException))]
        public Token get_token(TokenStream tstream_ptr)
        {
            PexAssume.IsNotNull(tstream_ptr);
            PexAssume.IsNotNull(tstream_ptr.ch_stream);
            PexAssume.IsNotNull(tstream_ptr.ch_stream.fp);
            PexAssume.IsNotNullOrEmpty(tstream_ptr.ch_stream.stream);
            PexAssume.IsTrue(tstream_ptr.ch_stream.stream_ind < tstream_ptr.ch_stream.stream.Length - 1);
            PexAssume.IsTrue(tstream_ptr.ch_stream.stream_ind >= 0);
            PexAssume.IsTrue(tstream_ptr.ch_stream.stream.Length > 1);
            PexAssume.IsTrue(tstream_ptr.ch_stream.stream[tstream_ptr.ch_stream.stream.Length - 1] == 127);
            Token result = Original.get_token(tstream_ptr);
            return result;
            // TODO: add assertions to method OriginalTest.get_token(TokenStream)
        }

        /// <summary>Test stub for is_eof_token(Token)</summary>
        [PexMethod]
        public int is_eof_token(Token t)
        {
            PexAssume.IsNotNull(t);
            int result = Original.is_eof_token(t);
            return result;
            // TODO: add assertions to method OriginalTest.is_eof_token(Token)
        }

        /// <summary>Test stub for keyword(Int32)</summary>
        [PexMethod]
        public int keyword(int state)
        {
            int result = Original.keyword(state);
            return result;
            // TODO: add assertions to method OriginalTest.keyword(Int32)
        }

        /// <summary>Test stub for next_state(Int32, Byte)</summary>
        [PexMethod, PexAllowedException(typeof(IndexOutOfRangeException))]
        public int next_state(int state, byte ch)
        {
            PexAssume.IsTrue(state < Original.baseArray.Length);
            PexAssume.IsTrue(state < Original.default1.Length);
            PexAssume.IsTrue(Original.baseArray[state] + ch < Original.check.Length);
            PexAssume.IsTrue(Original.baseArray[state] + ch < Original.next.Length);

            PexAssume.IsTrue(Original.default1[state] < Original.baseArray.Length);
            PexAssume.IsTrue(Original.default1[state] < Original.default1.Length);
            PexAssume.IsTrue(Original.baseArray[Original.default1[state]] + ch < Original.check.Length);
            PexAssume.IsTrue(Original.baseArray[Original.default1[state]] + ch < Original.next.Length);

            int result = Original.next_state(state, ch);
            return result;
            // TODO: add assertions to method OriginalTest.next_state(Int32, Byte)
        }

        /// <summary>Test stub for numeric_case(TokenStream, Token, Byte, Byte[], Int32)</summary>
        [PexMethod, PexAllowedException(typeof(IndexOutOfRangeException))]
        public Token numeric_case(
            TokenStream tstream_ptr,
            Token token_ptr,
            byte ch,
            byte[] token_str,
            int token_ind
        )
        {
            PexAssume.IsNotNull(tstream_ptr);
            PexAssume.IsNotNull(tstream_ptr.ch_stream);
            PexAssume.IsNotNull(tstream_ptr.ch_stream.fp);
            PexAssume.IsNotNullOrEmpty(tstream_ptr.ch_stream.stream);
            PexAssume.IsTrue(tstream_ptr.ch_stream.stream_ind > 0 && tstream_ptr.ch_stream.stream_ind < tstream_ptr.ch_stream.stream.Length - 1);
            PexAssume.IsTrue(tstream_ptr.ch_stream.stream.Length > 1);

            PexAssume.IsNotNull(token_str);
            PexAssume.IsTrue(token_ind < token_str.Length - 1);
            PexAssume.IsTrue(token_ind > 0);
            PexAssume.IsTrue(token_str.Length > 1);
            
            PexAssume.IsNotNull(token_ptr);
            PexAssume.IsNotNull(token_ptr.token_string);

            PexAssume.IsTrue(token_str.Length <= token_ptr.token_string.Length);

            Token result
               = Original.numeric_case(tstream_ptr, token_ptr, ch, token_str, token_ind);
            return result;
            // TODO: add assertions to method OriginalTest.numeric_case(TokenStream, Token, Byte, Byte[], Int32)
        }

        /// <summary>Test stub for open_character_stream(String)</summary>
        [PexMethod]
        public CharacterStream open_character_stream(string filename)
        {   
            PexAssume.IsNotNullOrEmpty(filename);
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            PexAssume.IsTrue(filename.IndexOfAny(invalidChars) == -1);
            PexAssume.IsFalse(String.IsNullOrWhiteSpace(filename));
            try
            {
                CharacterStream result = Original.open_character_stream(filename);
                return result;
            }
            catch (FileNotFoundException e)
            {
                return null;
            }
            // TODO: add assertions to method OriginalTest.open_character_stream(String)
        }

        /// <summary>Test stub for open_token_stream(String)</summary>
        [PexMethod]
        public TokenStream open_token_stream(string FILENAME)
        {
            PexAssume.IsNotNullOrEmpty(FILENAME);
            PexAssume.IsFalse(String.IsNullOrWhiteSpace(FILENAME));
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            PexAssume.IsTrue(FILENAME.IndexOfAny(invalidChars) == -1);
            try
            {
                TokenStream result = Original.open_token_stream(FILENAME);
                return result;
            }
            catch (FileNotFoundException e)
            {
                return null;
            }

            // TODO: add assertions to method OriginalTest.open_token_stream(String)
        }

        /// <summary>Test stub for print_token(Token)</summary>
        [PexMethod]
        public int print_token(Token token_ptr)
        {
            PexAssume.IsNotNull(token_ptr);
            int result = Original.print_token(token_ptr);
            return result;
            // TODO: add assertions to method OriginalTest.print_token(Token)
        }

        /// <summary>Test stub for skip(CharacterStream)</summary>
        [PexMethod, PexAllowedException(typeof(IndexOutOfRangeException))]
        public void skip(CharacterStream stream_ptr)
        {
            PexAssume.IsNotNull(stream_ptr);
            PexAssume.IsNotNull(stream_ptr.fp);
            PexAssume.IsNotNullOrEmpty(stream_ptr.stream);
            PexAssume.IsTrue(stream_ptr.stream_ind >= 0 && stream_ptr.stream_ind < stream_ptr.stream.Length);
            PexAssume.IsTrue(stream_ptr.stream.Length > 1);
            PexAssume.IsTrue(stream_ptr.stream[stream_ptr.stream.Length - 1] == 127);
            Original.skip(stream_ptr);
            // TODO: add assertions to method OriginalTest.skip(CharacterStream)f
        }

        /// <summary>Test stub for special(Int32)</summary>
        [PexMethod]
        public int special(int state)
        {
            int result = Original.special(state);
            return result;
            // TODO: add assertions to method OriginalTest.special(Int32)
        }

        /// <summary>Test stub for unget_char(Byte, CharacterStream)</summary>
        [PexMethod]
        public void unget_char(byte ch, CharacterStream stream_ptr)
        {
            PexAssume.IsNotNull(stream_ptr);
            PexAssume.IsNotNull(stream_ptr.fp);
            PexAssume.IsNotNullOrEmpty(stream_ptr.stream);
            PexAssume.IsTrue(stream_ptr.stream_ind > 0 && stream_ptr.stream_ind < stream_ptr.stream.Length);
            Original.unget_char(ch, stream_ptr);
            // TODO: add assertions to method OriginalTest.unget_char(Byte, CharacterStream)
        }
    }
}

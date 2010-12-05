// <copyright file="originalTest.cs" company="Authorized User">Copyright © Authorized User 2010</copyright>
using System;
using System.IO;
using Edu.Unl.Sir.Siemens.PrintTokens2;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edu.Unl.Sir.Siemens.PrintTokens2
{
    /// <summary>This class contains parameterized unit tests for original</summary>
    [PexClass(typeof(original))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class originalTest
    {
        /// <summary>Test stub for get_char(Stream)</summary>
        [PexMethod]
        public char get_char(Stream fp)
        {
            PexAssume.IsNotNull(fp);
            char result = original.get_char(fp);
            return result;
            // TODO: add assertions to method originalTest.get_char(Stream)
        }

        /// <summary>Test stub for get_token(Stream)</summary>
        [PexMethod]
        public string get_token(Stream tp)
        {
            PexAssume.IsNotNull(tp);
            PexAssume.IsTrue(tp is FileStream);
            string result = original.get_token(tp);
            return result;
            // TODO: add assertions to method originalTest.get_token(Stream)
        }

        /// <summary>Test stub for is_char_constant(String)</summary>
        [PexMethod]
        public int is_char_constant(string str)
        {
            PexAssume.IsNotNull(str);
            PexAssume.IsTrue(str.Length > 1);
            int result = original.is_char_constant(str);
            return result;
            // TODO: add assertions to method originalTest.is_char_constant(String)
        }

        /// <summary>Test stub for is_comment(String)</summary>
        [PexMethod]
        public int is_comment(string ident)
        {
            PexAssume.IsNotNullOrEmpty(ident);
            int result = original.is_comment(ident);
            return result;
            // TODO: add assertions to method originalTest.is_comment(String)
        }

        /// <summary>Test stub for is_eof_token(String)</summary>
        [PexMethod]
        public int is_eof_token(string tok)
        {
            PexAssume.IsNotNullOrEmpty(tok);
            int result = original.is_eof_token(tok);
            return result;
            // TODO: add assertions to method originalTest.is_eof_token(String)
        }

        /// <summary>Test stub for is_identifier(String)</summary>
        [PexMethod]
        public int is_identifier(string str)
        {
            PexAssume.IsNotNull(str);
            PexAssume.IsTrue(str.Length > 1);
            PexAssume.IsTrue(str.LastIndexOf('\0') == str.Length - 1);
            int result = original.is_identifier(str);
            return result;
            // TODO: add assertions to method originalTest.is_identifier(String)
        }

        /// <summary>Test stub for is_keyword(String)</summary>
        [PexMethod]
        public int is_keyword(string str)
        {
            PexAssume.IsNotNull(str);
            int result = original.is_keyword(str);
            return result;
            // TODO: add assertions to method originalTest.is_keyword(String)
        }

        /// <summary>Test stub for is_num_constant(String)</summary>
        [PexMethod]
        public int is_num_constant(string str)
        {
            PexAssume.IsNotNull(str);
            PexAssume.IsTrue(str.Length > 1);
            PexAssume.IsTrue(str.LastIndexOf('\0') == str.Length - 1);
            int result = original.is_num_constant(str);
            return result;
            // TODO: add assertions to method originalTest.is_num_constant(String)
        }

        /// <summary>Test stub for is_spec_symbol(String)</summary>
        [PexMethod]
        public int is_spec_symbol(string str)
        {
            PexAssume.IsNotNull(str);
            int result = original.is_spec_symbol(str);
            return result;
            // TODO: add assertions to method originalTest.is_spec_symbol(String)
        }

        /// <summary>Test stub for is_str_constant(String)</summary>
        [PexMethod]
        public int is_str_constant(string str)
        {
            PexAssume.IsNotNull(str);
            PexAssume.IsTrue(str.Length > 1);
            PexAssume.IsTrue(str.LastIndexOf('\0') == str.Length - 1);
            int result = original.is_str_constant(str);
            return result;
            // TODO: add assertions to method originalTest.is_str_constant(String)
        }

        /// <summary>Test stub for is_token_end(Int32, Char)</summary>
        [PexMethod]
        public int is_token_end(int str_com_id, char ch)
        {
            int result = original.is_token_end(str_com_id, ch);
            return result;
            // TODO: add assertions to method originalTest.is_token_end(Int32, Char)
        }

        /// <summary>Test stub for open_character_stream(String)</summary>
        [PexMethod]
        public Stream open_character_stream(string fname)
        {
            PexAssume.IsNotNullOrEmpty(fname);
            PexAssume.IsFalse(String.IsNullOrWhiteSpace(fname));
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            PexAssume.IsTrue(fname.IndexOfAny(invalidChars) == -1);
            try
            {
                Stream result = original.open_character_stream(fname);
                return result;
            }
            catch (FileNotFoundException e)
            {
                return null;
            }

            // TODO: add assertions to method originalTest.open_character_stream(String)
        }

        /// <summary>Test stub for open_token_stream(String)</summary>
        [PexMethod]
        public Stream open_token_stream(string fname)
        {
            PexAssume.IsNotNullOrEmpty(fname);
            PexAssume.IsFalse(String.IsNullOrWhiteSpace(fname));
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            PexAssume.IsTrue(fname.IndexOfAny(invalidChars) == -1);

            try
            {
                Stream result = original.open_token_stream(fname);
                return result;
            }
            catch (FileNotFoundException e)
            {
                return null;
            }


            // TODO: add assertions to method originalTest.open_token_stream(String)
        }

        /// <summary>Test stub for print_spec_symbol(String)</summary>
        [PexMethod]
        public void print_spec_symbol(string str)
        {
            PexAssume.IsNotNull(str);
            original.print_spec_symbol(str);
            // TODO: add assertions to method originalTest.print_spec_symbol(String)
        }

        /// <summary>Test stub for print_token(String)</summary>
        [PexMethod]
        public int print_token(string tok)
        {
            PexAssume.IsNotNullOrEmpty(tok);
            PexAssume.IsTrue(tok.LastIndexOf('\0') == tok.Length - 1);
            int result = original.print_token(tok);
            return result;
            // TODO: add assertions to method originalTest.print_token(String)
        }

        /// <summary>Test stub for token_type(String)</summary>
        [PexMethod]
        public int token_type(string tok)
        {
            PexAssume.IsNotNullOrEmpty(tok);
            PexAssume.IsTrue(tok.LastIndexOf('\0') == tok.Length - 1);
            int result = original.token_type(tok);
            return result;
            // TODO: add assertions to method originalTest.token_type(String)
        }

        /// <summary>Test stub for unget_char(Char, Stream)</summary>
        [PexMethod]
        public char unget_char(char ch, Stream fp)
        {
            PexAssume.IsNotNull(fp);
            PexAssume.IsTrue(fp is FileStream);
            char result = original.unget_char(ch, fp);
            return result;
            // TODO: add assertions to method originalTest.unget_char(Char, Stream)
        }
    }
}

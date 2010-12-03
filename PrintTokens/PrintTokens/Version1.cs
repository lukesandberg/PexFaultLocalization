using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Edu.Unl.Sir.Siemens.PrintTokens.V1
{
    public class CharacterStream
    {
        public Stream fp;  /* File pointer to stream */
        public int stream_ind; /* buffer index */
        public byte[] stream;  /* buffer for the file*/

        public CharacterStream()
        {
            stream = new byte[80];
        }
    }

    public class Token
    {
        public int token_id;
        public byte[] token_string;

        public Token()
        {
            token_string = new byte[80];
        }
    }

    public class TokenStream
    {
        public CharacterStream ch_stream;
    }

    public class Original
    {
        public static int[] default1 ={
                 54, 17, 17, 17, 17, 17, 17, 17, 17, 17,
                 17, 17, 17, 17, 17, 17, 17, 51, -2, -1,
                 -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                 -1, -1, -1, -1, -1 ,-1, -1, -1, -1, -1,
                 -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 
                 -1, 52, -3, -1 ,-1, -1, -1, -1, -1, -1
               };
        public static int[] baseArray ={
                  -32, -96,-105, -93, -94, -87, -1,  -97, -86, -1,
                  -99, -1,  -72, -1,  -80, -82, -1,   53,  43, -1,
                  -1,  -1,  -1,  -1,  -1,  -1,  133, -1,  233, -1,
                  -1,  0,   -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,
                  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,
                  -1,  46,  40,  -1, 251,  -1,  -1,  -1,  -1,  -1
              };
        public static int[] next = {
                  0,  2, 26, 28,  3,  4,  5, 23, 19, 20,
                  6, -1, 25,  8,  9, 11, 18, 18, 18, 18,
                 18, 18, 18, 18, 18, 18, -1, 30, -1, 31,
                 13, 15, 16, 17, 17, 17, 17, 17, 17, 17,
                 17, 17, 17, 17, 17, 17, 17, 17, 17, 17,
                 17, 17, 17, 17, 17, 17, 17, 17, 17, 21,
                 -1, 22, 32, -1, 24,  7, 17, 17, 17, 17,
                 17, 17, 17, 12, 17, 17,  1, 17, 17, 10,
                 17, 17, 17, 17, 17, 17, 17, 17, 14, 17,
                 17, 18, 18, 18, 18, 18, 18, 18, 18, 18,
                 18, 17, 17, 17, 17, 17, 17, 17, 17, 17,
                 17, 17, 17, 17, 17, 17, 17, 17, 17, 17,
                 17, 17, 17, 17, 17, 17, 17, 17, 17, 17,
                 17, 17, 17, 17, 17, 17, 17, 17, 17, 17,
                 17, 17, 17, 17, 17, 17, 17, 17, 17, 17,
                 17, 17, 17, 17, 17, 17, 17, 17, 17, 17,
                 17, 17, 17, -1, -1, 26, 26, 27, 26, 26,
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                  0,  0, -1, -1, -1, 29, 29, 29, 29, 29,
                 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                 29, 29, 29, 29, 29, 29, 29, 29, 29, 29
            };
        public static int[] check = {   0,  1,  0,  0,  2,  3,  4,  0,  0,  0,
                  5, -1,  0,  7,  8, 10,  0,  0,  0,  0,
                  0,  0,  0,  0,  0,  0, -1,  0, -1,  0,
                 12, 14, 15,  0,  0,  0,  0,  0,  0,  0,
                  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                 -1,  0, 31, -1,  0,  0,  0,  0,  0,  0,
                  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                  0, 18, 18, 18, 18, 18, 18, 18, 18, 18,
                 18, 17, 17, 17, 17, 17, 17, 17, 17, 17,
                 17, 51, 51, 51, 51, 51, 51, 51, 51, 51,
                 51, 51, 51, 51, 51, 51, 51, 51, 51, 51,
                 51, 51, 51, 51, 51, 51, 51, 52, 52, 52,
                 52, 52, 52, 52, 52, 52, 52, 52, 52, 52,
                 52, 52, 52, 52, 52, 52, 52, 52, 52, 52,
                 52, 52, 52, -1, -1, 26, 26, 26, 26, 26,
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,  
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,  
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,  
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,  
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,  
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,  
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,  
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,  
                 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                 54, 54, -1, -1, -1, 28, 28, 28, 28, 28,
                 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                 28, 28, 28, 28, 28, 28, 28, 28, 28, 28
              };

        private const int EOF = 127;
        private const int START = 0;
        private const int TRUE = 1;
        private const int FALSE = 0;
        private const int EOTSTREAM = 0;
        private const int NUMERIC = 18;
        private const int IDENTIFIER = 17;
        private const int LAMBDA = 6;
        private const int AND = 9;
        private const int OR = 11;
        private const int IF = 13;
        private const int XOR = 16;
        private const int LPAREN = 19;
        private const int RPAREN = 20;
        private const int LSQUARE = 21;
        private const int RSQUARE = 22;
        private const int QUOTE = 23;
        private const int BQUOTE = 24;
        private const int COMMA = 25;
        private const int EQUALGREATER = 32;
        private const int STRING_CONSTANT = 27;
        private const int CHARACTER_CONSTANT = 29;
        private const int ERROR = -1;

        static void Main(string[] args)
        {
            Token token_ptr;
            TokenStream stream_ptr;

            if (args.Length > 2)
            {
                Console.Write("The format is print_tokens filename(optional)\n");
                System.Environment.Exit(-1);
            }
            stream_ptr = open_token_stream(args[1]);

            while (is_eof_token((token_ptr = get_token(stream_ptr))) == FALSE)
            {
                print_token(token_ptr);
                print_token(token_ptr);
                System.Environment.Exit(0);
            }
        }



        /* *********************************************************************
               Function name : open_character_stream
               Input         : filename 
               Output        : charactre stream.
               Exceptions    : If file name doesn't exists it will
                               exit from the program.
               Description   : The function first allocates the memory for 
                               the structure and initilizes it. The constant
                               START gives the first character available in
                               the stream. It ckecks whether the filename is
                               empty string. If it is it assigns file pointer
                               to stdin else it opens the respective file as input.                   
         * * ******************************************************************* */

        public static CharacterStream open_character_stream(String filename)
        {
            CharacterStream stream_ptr = new CharacterStream();
            stream_ptr.stream_ind = START;
            stream_ptr.stream[START] = (byte)'\0';
            if (filename == null)
                stream_ptr.fp = Console.OpenStandardInput();
            else if ((stream_ptr.fp = new FileStream(filename, FileMode.Open, FileAccess.Read)) == null)
            {
                Console.Write("The file %s doesn't exists\n", filename);
                System.Environment.Exit(0);
            }
            return (stream_ptr);
        }

        /* *********************************************************************
           Function name : get_char
           Input         : charcter_stream.
           Output        : character.
           Exceptions    : None.
           Description   : This function takes character_stream type variable 
                           as input and returns one character. If the stream is
                           empty then it reads the next line from the file and
                           returns the character.       
         * ****************************************************************** */

        public static byte get_char(CharacterStream stream_ptr)
        {
            // <pex>
            if ((uint)(stream_ptr.stream_ind) >= (uint)(stream_ptr.stream.Length))
                throw new ArgumentException("complex reason", "stream_ptr");
            if (stream_ptr.fp == (Stream)null)
                throw new ArgumentNullException("stream_ptr");
            if (stream_ptr.stream.Length < 80)
                throw new ArgumentException("stream_ptr.stream.Length < 80", "stream_ptr");
            // </pex>
            if (stream_ptr.stream[stream_ptr.stream_ind] == '\0')
            {
                try
                {
                    stream_ptr.fp.Read(stream_ptr.stream, START, 80 - START);
                }
                catch (ArgumentException e)
                {
                    stream_ptr.stream[START] = (byte)EOF; //no EOF in C#
                }

                stream_ptr.stream_ind = START;
            }
            return (stream_ptr.stream[(stream_ptr.stream_ind)++]);
        }

        /* *******************************************************************
           Function name : is_end_of_character_stream.
           Input         : character_stream.
           Output        : Boolean value.
           Description   : This function checks whether it is end of character
                           stream or not. It returns BOOLEANvariable which is 
                           true or false. The function checks whether the last 
                           read character is end file character or not and
                           returns the value according to it.
         * ****************************************************************** */

        static int is_end_of_character_stream(CharacterStream stream_ptr)
        {
            if (stream_ptr.stream[stream_ptr.stream_ind - 1] == (byte)EOF)
                return (TRUE);
            else
                return (FALSE);
        }

        /* *********************************************************************
           Function name : unget_char
           Input         : character,character_stream.
           Output        : void.
           Description   : This function adds the character ch to the stream. 
                           This is accomplished by decrementing the stream_ind
                           and storing it in the stream. If it is not possible
                           to unget the character then it returns
         * ******************************************************************* */

        public static void unget_char(byte ch, CharacterStream stream_ptr)
        {
            if (stream_ptr.stream_ind == 0)
                return;
            else
                stream_ptr.stream[--(stream_ptr.stream_ind)] = ch;
            return;
        }


        /* *******************************************************************
           Function name : open_token_stream
           Input         : filename
           Output        : token_stream
           Exceptions    : Exits if the file specified by filename not found.
           Description   : This function takes filename as input and opens the
                           token_stream which is nothing but the character stream.
                           This function allocates the memory for token_stream 
                           and calls open_character_stream to open the file as
                           input. This function returns the token_stream.
         * ****************************************************************** */

        public static TokenStream open_token_stream(string FILENAME)
        {
            TokenStream token_ptr = new TokenStream();
            token_ptr.ch_stream = open_character_stream(FILENAME);/* Get character
                                                                 stream  */
            return (token_ptr);
        }

        /* ********************************************************************
           Function name : get_token
           Input         : token_stream
           Output        : token
           Exceptions    : none.
           Description   : This function returns the next token from the
                           token_stream.The type of token is integer and specifies 
                           only the type of the token. DFA is used for finding the
                           next token. cu_state is initialized to zero and charcter
                           are read until the the is the final state and it
                           returns the token type.
        * ******************************************************************* */

        public static Token get_token(TokenStream tstream_ptr)
        {
            byte[] token_str = new byte[80]; /* This buffer stores the current token */
            int token_ind;      /* Index to the token_str  */
            Token token_ptr;
            byte ch;
            int cu_state, next_st, token_found;

            token_ptr = new Token();
            ch = get_char(tstream_ptr.ch_stream);
            cu_state = token_ind = token_found = 0;
            while (token_found == 0)
            {
                if (token_ind < 80) /* ADDED ERROR CHECK - hf */
                {
                    token_str[token_ind++] = ch;
                    next_st = next_state(cu_state, ch);
                }
                else
                {
                    next_st = -1; /* - hf */
                }
                if (next_st == -1)
                { /* ERROR or EOF case */
                    return (error_or_eof_case(tstream_ptr,
                                 token_ptr, cu_state, token_str, token_ind, ch));
                }
                else if (next_st == -2)
                {/* This is numeric case. */
                    return (numeric_case(tstream_ptr, token_ptr, ch,
                            token_str, token_ind));
                }
                else if (next_st == -3)
                {/* This is the IDENTIFIER case */
                    token_ptr.token_id = IDENTIFIER;
                    unget_char(ch, tstream_ptr.ch_stream);
                    token_ind--;
                    get_actual_token(token_str, token_ind);

                    token_str.CopyTo(token_ptr.token_string, 0);
                    return (token_ptr);
                }

                switch (next_st)
                {
                    default: break;
                    case 6: /* These are all KEYWORD cases. */
                    case 9:
                    case 11:
                    case 13:
                    case 16:
                    case 32: ch = get_char(tstream_ptr.ch_stream);
                        if (check_delimiter(ch) == TRUE)
                        {
                            token_ptr.token_id = keyword(next_st);
                            unget_char(ch, tstream_ptr.ch_stream);
                            token_ptr.token_string[0] = (byte)'\0';
                            return (token_ptr);
                        }
                        unget_char(ch, tstream_ptr.ch_stream);
                        break;
                    case 19: /* These are all special SPECIAL character */
                    case 20: /* cases */
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25: token_ptr.token_id = special(next_st);
                        token_ptr.token_string[0] = (byte)'\0';
                        return (token_ptr);
                    case 27: /* These are constant cases */
                    case 29: token_ptr.token_id = constant(next_st, token_str, token_ind);
                        get_actual_token(token_str, token_ind);
                        token_str.CopyTo(token_ptr.token_string, 0);
                        return (token_ptr);
                    case 30:  /* This is COMMENT case */
                        skip(tstream_ptr.ch_stream);
                        token_ind = next_st = 0;
                        break;
                }
                cu_state = next_st;
                ch = get_char(tstream_ptr.ch_stream);
            }
            throw new ApplicationException("Execution should not have reached this line");
        }

        /* ******************************************************************
           Function name : numeric_case
           Input         : tstream_ptr,token_ptr,ch,token_str,token_ind
           Output        : token_ptr;
           Exceptions    : none 
           Description   : It checks for the delimiter, if it is then it
                           forms numeric token else forms error token.
         * ****************************************************************** */

        public static Token numeric_case(TokenStream tstream_ptr, Token token_ptr, byte ch, byte[] token_str, int token_ind)
        {
            if (check_delimiter(ch) != TRUE)
            {   /* Error case */
                token_ptr.token_id = ERROR;
                while (check_delimiter(ch) == FALSE)
                {
                    if (token_ind >= 80) break; /* Added protection - hf */
                    token_str[token_ind++] = ch = get_char(tstream_ptr.ch_stream);
                }
                unget_char(ch, tstream_ptr.ch_stream);
                token_ind--;
                get_actual_token(token_str, token_ind);
                token_str.CopyTo(token_ptr.token_string, 0);
                return (token_ptr);
            }
            token_ptr.token_id = NUMERIC; /* Numeric case */
            unget_char(ch, tstream_ptr.ch_stream);
            token_ind--;
            get_actual_token(token_str, token_ind);
            token_str.CopyTo(token_ptr.token_string, 0);
            return (token_ptr);
        }

        /* *****************************************************************
           Function name : error_or_eof_case 
           Input         : tstream_ptr,token_ptr,cu_state,token_str,token_ind,ch
           Output        : token_ptr 
           Exceptions    : none 
           Description   : This function checks whether it is EOF or not.
                           If it is it returns EOF token else returns ERROR 
                           token.
         * *****************************************************************/

        public static Token error_or_eof_case(TokenStream tstream_ptr, Token token_ptr, int cu_state, byte[] token_str, int token_ind, byte ch)
        {
            if (is_end_of_character_stream(tstream_ptr.ch_stream) == TRUE)
            {
                token_ptr.token_id = EOTSTREAM;
                token_ptr.token_string[0] = (byte)'\0';
                return (token_ptr);
            }
            if (cu_state != 0)
            {
                unget_char(ch, tstream_ptr.ch_stream);
                token_ind--;
            }
            token_ptr.token_id = ERROR;
            get_actual_token(token_str, token_ind);
            token_str.CopyTo(token_ptr.token_string, 0);
            return (token_ptr);
        }

        /* *********************************************************************
           Function name : check_delimiter
           Input         : character
           Output        : boolean
           Exceptions    : none.
           Description   : This function checks for the delimiter. If ch is not
                           alphabet and non numeric then it returns TRUE else 
                           it returns FALSE. 
         * ******************************************************************* */

        public static int check_delimiter(byte ch)
        {

            if (!Char.IsLetterOrDigit((char)ch)) /* Check for digit and alpha */
                return (TRUE);
            return (FALSE);
        }

        /* ********************************************************************
           Function name : keyword
           Input         : state of the DFA
           Output        : Keyword.
           Exceptions    : If the state doesn't represent a keyword it exits.
           Description   : According to the final state specified by state the
                           respective token_id is returned.
         * ***************************************************************** */

        public static int keyword(int state)
        {
            switch (state)
            {   /* Return the respective macro for the Keyword. */
                case 6: return (LAMBDA);
                case 9: return (AND);
                case 11: return (OR);
                case 13: return (IF);
                case 16: return (XOR);
                case 32: return (EQUALGREATER);
                default: Console.Write("error\n"); return ERROR; break;
            }
            System.Environment.Exit(0);
        }

        /* ********************************************************************
           Function name : special
           Input         : The state of the DFA.
           Output        : special symbol.
           Exceptions    : if the state doesn't belong to a special character
                           it exits.
           Description   : This function returns the token_id according to the
                           final state given by state.
         * ****************************************************************** */

        public static int special(int state)
        {
            switch (state)
            {   /* return the respective macro for the special character. */
                case 19: return (LPAREN);
                case 20: return (RPAREN);
                case 21: return (LSQUARE);
                case 22: return (RSQUARE);
                case 23: return (QUOTE);
                case 24: return (BQUOTE);
                case 25: return (COMMA);
                default: Console.Write("error\n"); return (ERROR); break;
            }
            System.Environment.Exit(0);
        }

        /* **********************************************************************
           Function name : skip
           Input         : character_stream
           Output        : void.
           Exceptions    : none.
           Description   : This function skips the comment part of the program.
                           It takes charcter_stream as input and reads character
                           until it finds new line character or
                           end_of_character_stream.                   
         * ******************************************************************* */

        public static void skip(CharacterStream stream_ptr)
        {
            byte c;

            while ((c = get_char(stream_ptr)) != '\n' &&
                   is_end_of_character_stream(stream_ptr) == FALSE)
                ; /* Skip the characters until EOF or EOL found. */
            if (c == (byte)EOF) unget_char(c, stream_ptr); /* Put back to leave gracefully - hf */
            return;
        }

        /* *********************************************************************
           Function name : constant
           Input         : state of DFA, Token string, Token id.
           Output        : constant token.
           Exceptions    : none.
           Description   : This function returns the token_id for the constatnts
                           speccified by  the final state. 
         * ****************************************************************** */

        public static int constant(int state, byte[] token_str, int token_ind)
        {
            switch (state)
            {   /* Return the respective CONSTANT macro. */
                case 27: return (STRING_CONSTANT);
                case 29: token_str[token_ind - 2] = (byte)' '; return (CHARACTER_CONSTANT);
                default: return ERROR; break;
            }
        }


        /* *******************************************************************
           Function name : next_state
           Input         : current state, character
           Output        : next state of the DFA
           Exceptions    : none.
           Description   : This function returns the next state in the transition
                           diagram. The next state is determined by the current
                           state state and the inpu character ch.
         * ****************************************************************** */

        public static int next_state(int state, byte ch)
        {
            if (state < 0)
                return (state);
            if (baseArray[state] + ch >= 0)
            {
                if (check[baseArray[state] + ch] == state) /* Check for the right state */
                    return (next[baseArray[state] + ch]);
                else
                    return (next_state(default1[state], ch));
            }
            else
                return (next_state(default1[state], ch));
        }

        /* *********************************************************************
           Function name : is_eof_token
           Input         : token
           Output        : Boolean
           Exceptions    : none.
           Description   : This function checks whether the token t is eof_token 
                           or not. If the integer value stored in the t is
                           EOTSTREAM then it is eof_token.
         * ***************************************************************** */

        public static int is_eof_token(Token t)
        {
            if (t.token_id == EOTSTREAM)
                return (TRUE);
            return (FALSE);
        }

        /* ********************************************************************
           Function name : print_token
           Input         : token
           Output        : Boolean
           Exceptions    : none.
           Description   : This function  prints the token. The token_id gives 
                           the type of token not the token itself. So, in the
                           case of identifier,numeric,  string,character it is
                           required to print the actual token  from token_str. 
                           So, precaution must be taken when printing the token.
                           This function is able to print the current token only
                           and it is the limitation of the program.
         * ******************************************************************** */

        public static int print_token(Token token_ptr)
        {
            switch (token_ptr.token_id)
            {    /* Print the respective tokens. */
                case ERROR: Console.Write("error,\t\""); Console.Write("%s", token_ptr.token_string);
                    Console.Write("\".\n"); return (TRUE);
                case EOTSTREAM: Console.Write("eof.\n"); return (TRUE);
                case 6: Console.Write("keyword,\t\"lambda\".\n"); return (TRUE);
                case 9: Console.Write("keyword,\t\"and\".\n"); return (TRUE);
                case 11: Console.Write("keyword,\t\"or\".\n"); return (TRUE);
                case 13: Console.Write("keyword,\t\"if\".\n"); return (TRUE);
                case 16: Console.Write("keyword,\t\"xor\".\n"); return (TRUE);
                case 17: Console.Write("identifier,\t\""); Console.Write("%s", token_ptr.token_string);
                    Console.Write("\".\n"); return (TRUE);
                case 18: Console.Write("numeric,\t"); Console.Write("%s", token_ptr.token_string);
                    Console.Write(".\n"); return (TRUE);
                case 19: Console.Write("lparen.\n"); return (TRUE);
                case 20: Console.Write("rparen.\n"); return (TRUE);
                case 21: Console.Write("lsquare.\n"); return (TRUE);
                case 22: Console.Write("rsquare.\n"); return (TRUE);
                case 23: Console.Write("quote.\n"); return (TRUE);
                case 24: Console.Write("bquote.\n"); return (TRUE);
                case 25: Console.Write("comma.\n"); return (TRUE);
                case 27: Console.Write("string,\t"); Console.Write("%s", token_ptr.token_string);
                    Console.Write(".\n"); return (TRUE);
                case 29: Console.Write("character,\t\""); Console.Write("%s", token_ptr.token_string);
                    Console.Write("\".\n"); return (TRUE);
                case 32: Console.Write("keyword,\t\"=>\".\n"); return (TRUE);
                default: break;
            }
            return (FALSE);
        }

        /* **********************************************************************
           Function name : get_actual_token
           Input         : token string and token id.
           Output        : void.
           Exceptions    : none.
           Description   : This function prints the actual token in the case of
                           identifier,numeric,string and character. It removes
                           the leading and trailing  spaces and prints the token.
         * ****************************************************************** */

        public static void get_actual_token(byte[] token_str, int token_ind)
        {
            int ind, start;

            for (ind = token_ind; ind > 0 && Char.IsWhiteSpace((char)token_str[ind - 1]); --ind) ;
            /* Delete the trailing white spaces & protect - hf */
            token_str[ind] = (byte)'\0'; token_ind = ind;
            for (ind = 0; ind < token_ind; ++ind)
                if (!Char.IsWhiteSpace((char)token_str[ind]))
                    break;
            for (start = 0; ind <= token_ind; ++start, ++ind) /* Delete the leading
                                                           white spaces. */
                token_str[start] = token_str[ind];
            return;
        }
    }
}

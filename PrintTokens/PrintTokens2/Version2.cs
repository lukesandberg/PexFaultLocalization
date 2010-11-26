using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Edu.Unl.Sir.Siemens.PrintTokens2.V2
{
    class original
    {
        static const int TRUE = 1;
        static const int FALSE = 1;

        static const int SUCCEED = 1;
        static const int FAIL = 0;
        static const int error = 0;
        static const int keyword = 1;
        static const int spec_symbol = 2;
        static const int identifier = 3;
        static const int num_constant = 41;
        static const int str_constant = 42;
        static const int char_constant = 43;
        static const int comment = 5;
        static const int end = 6;

        static void main(string[] args)
        {
            string fname;
            string tok;
            Stream tp;
            if (args.Length == 1)                  /* if not given filename,take as '""' */
            {
                fname = "\0";
            }
            else if (args.Length == 2)
                fname = args[1];
            else
            {
                fname = "";
                Console.Write("Error!,please give the token stream\n");
                System.Environment.Exit(0);
            }
            tp = open_token_stream(fname);  /* open token stream */
            tok = get_token(tp);
            while (is_eof_token(tok) == FALSE) /* take one token each time until eof */
            {
                print_token(tok);
                tok = get_token(tp);
            }
            print_token(tok); /* print eof signal */
            System.Environment.Exit(0);
        }

        /* stream.c code */

        /***********************************************/
        /* NMAE:	open_character_stream          */
        /* INPUT:       a filename                     */
        /* OUTPUT:      a pointer to chacracter_stream */
        /* DESCRIPTION: when not given a filename,     */
        /*              open stdin,otherwise open      */
        /*              the existed file               */
        /***********************************************/
        static Stream open_character_stream(string fname)
        {

            Stream fp;
            if (fname == null)
                fp = Console.OpenStandardInput();
            else if ((fp = new FileStream(fname, FileMode.Open, FileAccess.Read)) == null)
            {
                Console.Write("The file %s doesn't exists\n", fname);
                System.Environment.Exit(0);
            }
            return (fp);
        }

        /**********************************************/
        /* NAME:	get_char                      */
        /* INPUT:       a pointer to char_stream      */
        /* OUTPUT:      a character                   */
        /**********************************************/
        static char get_char(Stream fp)
        {
            char ch;
            ch = (char)fp.ReadByte();
            return (ch);
        }

        /***************************************************/
        /* NAME:      unget_char                           */
        /* INPUT:     a pointer to char_stream,a character */
        /* OUTPUT:    a character                          */
        /* DESCRIPTION:when unable to put back,return '\0'  */
        /***************************************************/
        static char unget_char(char ch, Stream fp)
        {
            fp.WriteByte((byte)ch);
            return ch;
        }

        /* tokenizer.c code */


        static char[] buffer = new char[81];  /* fixed array length MONI */ /* to store the token temporar */

        /********************************************************/
        /* NAME:	open_token_stream                       */
        /* INPUT:       a filename                              */
        /* OUTPUT:      a pointer to a token_stream             */
        /* DESCRIPTION: when filename is EMPTY,choice standard  */
        /*              input device as input source            */
        /********************************************************/
        static Stream open_token_stream(string fname)
        {
            Stream fp;
            if (fname.Equals(""))
                fp = open_character_stream(null);
            else
                fp = open_character_stream(fname);
            return (fp);
        }

        /********************************************************/
        /* NAME :	get_token                               */
        /* INPUT: 	a pointer to the tokens_stream          */
        /* OUTPUT:      a token                                 */
        /* DESCRIPTION: according the syntax of tokens,dealing  */
        /*              with different case  and get one token  */
        /********************************************************/
        static string get_token(Stream tp)
        {
            int i = 0, j;
            int id = 0;
            char ch;
            char[] ch1 = new char[2];
            for (j = 0; j <= 80; j++)          /* initial the buffer   */
            { buffer[j] = '\0'; }
            ch1[0] = '\0';
            ch1[1] = '\0';
            ch = get_char(tp);
            while (ch == ' ' || ch == '\n')      /* strip all blanks until meet characters */
            {
                ch = get_char(tp);
            }
            buffer[i] = ch;
            if (is_eof_token(buffer.ToString()) == TRUE) return (buffer.ToString());
            if (is_spec_symbol(buffer.ToString()) == TRUE) return (buffer.ToString());
            if (ch == '"') id = 1;    /* prepare for string */
            if (ch == 59) id = 2;    /* prepare for comment */
            ch = get_char(tp);

            while (is_token_end(id, ch) == FALSE)/* until meet the end character */
            {
                i++;
                buffer[i] = ch;
                ch = get_char(tp);
            }
            ch1[0] = ch;                        /* hold the end charcater          */
            if (is_eof_token(ch1.ToString()) == TRUE)       /* if end character is eof token    */
            {
                ch = unget_char(ch, tp);        /* then put back eof on token_stream */
                if (ch == '\0') unget_error(tp);
                return (buffer.ToString());
            }
            if (is_spec_symbol(ch1.ToString()) == TRUE)     /* if end character is special_symbol */
            {
                ch = unget_char(ch, tp);        /* then put back this character       */
                if (ch == '\0') unget_error(tp);
                return (buffer.ToString());
            }
            if (id == 1)                  /* if end character is " and is string */
            {
                i++;                     /* case,hold the second " in buffer    */
                buffer[i] = ch;
                return (buffer.ToString());
            }
            if (id == 0 && ch == 59)
            /* when not in string or comment,meet ";" */
            {
                ch = unget_char(ch, tp);       /* then put back this character         */
                /*if (ch == '\0') */
                    unget_error(tp);
                return (buffer.ToString());
            }
            return (buffer.ToString());                   /* return nomal case token             */
        }

        /*******************************************************/
        /* NAME:	is_token_end                           */
        /* INPUT:       a character,a token status             */
        /* OUTPUT:	a int value                        */
        /*******************************************************/
        static int is_token_end(int str_com_id, char ch)
        {
            char[] ch1 = new char[2];  /* fixed array declaration MONI */
            ch1[0] = ch;
            ch1[1] = '\0';
            if (is_eof_token(ch1.ToString()) == TRUE) return (TRUE); /* is eof token? */
            if (str_com_id == 1)          /* is string token */
            {
                if (ch == '"' | ch == '\n')   /* for string until meet another " */
                    return (TRUE);
                else
                    return (FALSE);
            }

            if (str_com_id == 2)    /* is comment token */
            {
                if (ch == '\n')     /* for comment until meet end of line */
                    return (TRUE);
                else
                    return (FALSE);
            }

            if (is_spec_symbol(ch1.ToString()) == TRUE) return (TRUE); /* is special_symbol? */
            if (ch == ' ' || ch == '\n' || ch == 59) return (TRUE);
            /* others until meet blank or tab or 59 */
            return (FALSE);               /* other case,return FALSE */
        }

        /****************************************************/
        /* NAME :	token_type                          */
        /* INPUT:       a pointer to the token              */
        /* OUTPUT:      an integer value                    */
        /* DESCRIPTION: the integer value is corresponding  */
        /*              to the different token type         */
        /****************************************************/
        static int token_type(string tok)
        {
            if (is_keyword(tok) == TRUE) return (keyword);
            if (is_spec_symbol(tok) == TRUE) return (spec_symbol);
            if (is_identifier(tok) == TRUE) return (identifier);
            if (is_num_constant(tok) == TRUE) return (num_constant);
            if (is_str_constant(tok) == TRUE) return (str_constant);
            if (is_char_constant(tok) == TRUE) return (char_constant);
            if (is_comment(tok) == TRUE) return (comment);
            if (is_eof_token(tok) == TRUE) return (end);
            return (error);                    /* else look as error token */
        }

        /****************************************************/
        /* NAME:	print_token                         */
        /* INPUT:	a pointer to the token              */
        /* OUTPUT:      a int value,print out the token */
        /*              according the forms required        */
        /****************************************************/
        static int print_token(string tok)
        {
            int type;
            type = token_type(tok);
            if (type == error)
            {
                Console.Write("error,\"%s\".\n", tok);
            }
            if (type == keyword)
            {
                Console.Write("keyword,\"%s\".\n", tok);
            }
            if (type == spec_symbol) print_spec_symbol(tok);
            if (type == identifier)
            {
                Console.Write("identifier,\"%s\".\n", tok);
            }
            if (type == num_constant)
            {
                Console.Write("numeric,%s.\n", tok);
            }
            if (type == str_constant)
            {
                Console.Write("string,%s.\n", tok);
            }
            if (type == char_constant)
            {
                tok = tok + 1;
                Console.Write("character,\"%s\".\n", tok);
            }
            if (type == end)
                Console.Write("eof.\n");
            return type;
        }

        /* the code for tokens judgment function */

        /*************************************/
        /* NAME:	is_eof_token         */
        /* INPUT: 	a pointer to a token */
        /* OUTPUT:      a int value      */
        /*************************************/
        static int is_eof_token(string tok)
        {
            if (tok.ToCharArray()[0] == '\0')
                return (TRUE);
            else
                return (FALSE);
        }

        /*************************************/
        /* NAME:	is_comment           */
        /* INPUT: 	a pointer to a token */
        /* OUTPUT:      a int value      */
        /*************************************/
        static int is_comment(string ident)
        {
            if ((ident.ToCharArray()[0]) == 59)   /* the char is 59   */
                return (TRUE);
            else
                return (FALSE);
        }

        /*************************************/
        /* NAME:	is_keyword           */
        /* INPUT: 	a pointer to a token */
        /* OUTPUT:      a int value      */
        /*************************************/
        static int is_keyword(string str)
        {
            if (str.Equals("and") || str.Equals("or") || str.Equals("if") ||
               str.Equals("xor") || str.Equals("lambda") || str.Equals("=>"))
                return (TRUE);
            else
                return (FALSE);
        }

        /*************************************/
        /* NAME:	is_char_constant     */
        /* INPUT: 	a pointer to a token */
        /* OUTPUT:      a int value      */
        /*************************************/
        static int is_char_constant(string str)
        {
            if ((str.ToCharArray()[0]) == '#' && Char.IsLetter(str.ToCharArray()[1]))
                return (TRUE);
            else
                return (FALSE);
        }

        /*************************************/
        /* NAME:	is_num_constant      */
        /* INPUT: 	a pointer to a token */
        /* OUTPUT:      a int value      */
        /*************************************/
        static int is_num_constant(string str)
        {
            int i = 1;

            if (Char.IsNumber(str.ToCharArray()[0]))
            {
                while (str.ToCharArray()[i] != '\0')   /* until meet token end sign */
                {
                    if (Char.IsNumber(str.ToCharArray()[i]))
                        i++;
                    else
                        return (FALSE);
                }                         /* end WHILE */
                return (TRUE);
            }
            else
                return (FALSE);               /* other return FALSE */
        }

        /*************************************/
        /* NAME:	is_str_constant      */
        /* INPUT: 	a pointer to a token */
        /* OUTPUT:      a int value      */
        /*************************************/
        static int is_str_constant(string str)
        {
            int i = 1;

            if (str.ToCharArray()[0] == '"')
            {
                while (str.ToCharArray()[i] != '\0')  /* until meet the token end sign */
                {
                    if (str.ToCharArray()[i] == '"')
                        return (TRUE);        /* meet the second '"'           */
                    else
                        i++;
                }               /* end WHILE */
                return (FALSE);
            }
            else
                return (FALSE);       /* other return FALSE */
        }
        /*************************************/
        /* NAME:	is_identifier         */
        /* INPUT: 	a pointer to a token */
        /* OUTPUT:      a int value      */
        /*************************************/
        static int is_identifier(string str)
        {
            int i = 1;

            if (Char.IsLetter(str.ToCharArray()[0]))
            {
                while (str.ToCharArray()[i] != '\0')   /* unti meet the end token sign */
                {
                    if (Char.IsLetter(str.ToCharArray()[i]) || Char.IsNumber(str.ToCharArray()[i]))
                        i++;
                    else
                        return (FALSE);
                }      /* end WHILE */
                return (TRUE);
            }
            else
                return (FALSE);
        }

        /******************************************/
        /* NAME:	unget_error               */
        /* INPUT:       a pointer to token stream */
        /* OUTPUT: 	print error message       */
        /******************************************/
        static void unget_error(Stream fp)
        {
            Console.Write("It can not get charcter\n");
        }

        /*************************************************/
        /* NAME:        print_spec_symbol                */
        /* INPUT:       a pointer to a spec_symbol token */
        /* OUTPUT :     print out the spec_symbol token  */
        /*              according to the form required   */
        /*************************************************/
        static void print_spec_symbol(string str)
        {
            if (str.Equals("("))
            {
                Console.Write("%s\n", "lparen.");
                return;
            }
            if (str.Equals(")"))
            {
                Console.Write("%s\n", "rparen.");
                return;
            }
            if (str.Equals("["))
            {
                Console.Write("%s\n", "lsquare.");
                return;
            }
            if (str.Equals("]"))
            {
                Console.Write("%s\n", "rsquare.");
                return;
            }
            if (str.Equals("'"))
            {
                Console.Write("%s\n", "quote.");
                return;
            }
            if (str.Equals("`"))
            {
                Console.Write("%s\n", "bquote.");
                return;
            }

            Console.Write("%s\n", "comma.");
        }


        /*************************************/
        /* NAME:        is_spec_symbol       */
        /* INPUT:       a pointer to a token */
        /* OUTPUT:      a int value      */
        /*************************************/
        static int is_spec_symbol(string str)
        {
            if (str.Equals("("))
            {
                return (TRUE);
            }
            if (str.Equals(")"))
            {
                return (TRUE);
            }
            if (str.Equals("["))
            {
                return (TRUE);
            }
            if (str.Equals("]"))
            {
                return (TRUE);
            }
            if (str.Equals("'"))
            {
                return (TRUE);
            }
            if (str.Equals("`"))
            {
                return (TRUE);
            }
            if (str.Equals(","))
            {
                return (TRUE);
            }
            return (FALSE);     /* others return FALSE */
        }
    }
}


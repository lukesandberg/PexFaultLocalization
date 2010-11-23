using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edu.Nlu.Sir.Siemens.Replace
{
    /// <summary>
    /// /* -*- Last-Edit: Mon Dec 7 10:31:51 1992 by Tarak S. Goradia; -*- */
    /// </summary>
    public class BaseVersion
    {
        private const int NULL = 0;

        private const int MAXSTR = 100;
        private const int MAXPAT = MAXSTR;

        private const char ENDSTR = '\0';
        private const char ESCAPE = '@';
        private const char CLOSURE = '*';
        private const char BOL = '%';
        private const char EOL = '$';
        private const char ANY = '?';
        private const char CCL = '[';
        private const char CCLEND = ']';
        private const char NEGATE = '^';
        private const char NCCL = '!';
        private const char LITCHAR = 'c';
        private const char DITTO = unchecked((char)-1);
        private const char DASH = '-';

        private const char TAB = (char)9;
        private const char NEWLINE = (char)10;

        private const int CLOSIZE = 1;


        public static bool
        getline(out string s,
        int maxsize)
        {
            string result;
            result = Console.ReadLine();
            s = result;
            return (result != null);
        }

        public static bool
        addstr(char c,
        ref string outset,
        ref int j,
        int maxset)
        {
            bool result;
            if (j >= maxset)
                result = false;
            else
            {
                outset = outset.Substring(0, j) + c + outset.Substring(j + 1);
                j = j + 1;
                result = true;
            }
            return result;
        }

        public static char
        esc(string s, ref int i)
        {
            char result;
            if (s[i] != ESCAPE)
                result = s[i];
            else
                if (s[i + 1] == ENDSTR)
                    result = ESCAPE;
                else
                {
                    i = i + 1;
                    if (s[i] == 'n')
                        result = NEWLINE;
                    else
                        if (s[i] == 't')
                            result = TAB;
                        else
                            result = s[i];
                }
            return result;
        }

        public static void
        dodash(char delim,
        string src,
        ref int i,
        string dest,
        ref int j,
        int maxset)
        {
            int k;
            bool junk;
            char escjunk;

            while ((src[i] != delim) && (src[i] != ENDSTR))
            {
                if (src[i - 1] == ESCAPE)
                {
                    escjunk = esc(src, ref i);
                    junk = addstr(escjunk, ref dest, ref j, maxset);
                }
                else
                    if (src[i] != DASH)
                        junk = addstr(src[i], ref dest, ref j, maxset);
                    else if (j <= 1 || src[i + 1] == ENDSTR)
                        junk = addstr(DASH, ref dest, ref j, maxset);
                    else if ((Char.IsLetterOrDigit(src[i - 1])) && (Char.IsLetterOrDigit(src[i + 1]))
                       && (src[i - 1] <= src[i + 1]))
                    {
                        for (k = src[i - 1] + 1; k <= src[i + 1]; k++)
                        {
                            junk = addstr(src[i], ref dest, ref j, maxset);
                        }
                        i = i + 1;
                    }
                    else
                        junk = addstr(DASH, ref dest, ref j, maxset);
                (i) = (i) + 1;
            }
        }

        public static bool
        getccl(string arg,
        ref int i,
        string pat,
        ref int j)
        {
            int jstart;
            bool junk;

            i = i + 1;
            if (arg[i] == NEGATE)
            {
                junk = addstr(NCCL, ref pat, ref j, MAXPAT);
                i = i + 1;
            }
            else
                junk = addstr(CCL, ref pat, ref j, MAXPAT);
            jstart = j;
            junk = addstr((char)0, ref pat, ref j, MAXPAT);
            dodash(CCLEND, arg, ref i, pat, ref j, MAXPAT);
            pat = pat.Substring(0,jstart) + (char)(j - jstart - 1) + pat.Substring(jstart+1);
            return (arg[i] == CCLEND);
        }

        public static void
        stclose(string pat,
        ref int j,
        int lastj)
        {
            int jt;
            int jp;
            bool junk;


            for (jp = j - 1; jp >= lastj; jp--)
            {
                jt = jp + CLOSIZE;
                junk = addstr(pat[jp], ref pat, ref jt, MAXPAT);
            }
            j = j + CLOSIZE;
            pat = pat.Substring(0,lastj) + CLOSURE + pat.Substring(lastj+1);
        }

        public static bool in_set_2(char c)
        {
            return (c == BOL || c == EOL || c == CLOSURE);
        }

        public static bool in_pat_set(char c)
        {
            return (c == LITCHAR || c == BOL || c == EOL || c == ANY
            || c == CCL || c == NCCL || c == CLOSURE);
        }

        public static int
        makepat(string arg,
         int start,
        char delim,
        out string pat)
        {
            int result;
            int i, j, lastj, lj;
            bool done, junk;
            bool getres;
            char escjunk;

            j = 0;
            i = start;
            lastj = 0;
            done = false;
            pat = "";
            while ((!done) && (arg[i] != delim) && (arg[i] != ENDSTR))
            {
                lj = j;
                if ((arg[i] == ANY))
                    junk = addstr(ANY, ref pat, ref j, MAXPAT);
                else if ((arg[i] == BOL) && (i == start))
                    junk = addstr(BOL, ref pat, ref j, MAXPAT);
                else if ((arg[i] == EOL) && (arg[i + 1] == delim))
                    junk = addstr(EOL, ref pat, ref j, MAXPAT);
                else if ((arg[i] == CCL))
                {
                    getres = getccl(arg, ref i, pat, ref j);
                    done = (bool)(getres == false);
                }
                else if ((arg[i] == CLOSURE) && (i > start))
                {
                    lj = lastj;
                    if (in_set_2(pat[lj]))
                        done = true;
                    else
                        stclose(pat, ref j, lastj);
                }
                else
                {
                    junk = addstr(LITCHAR, ref pat, ref j, MAXPAT);
                    escjunk = esc(arg, ref i);
                    junk = addstr(escjunk, ref pat, ref j, MAXPAT);
                }
                lastj = lj;
                if ((!done))
                    i = i + 1;
            }
            junk = addstr(ENDSTR, ref pat, ref j, MAXPAT);
            if ((done) || (arg[i] != delim))
                result = 0;
            else
                if ((!junk))
                    result = 0;
                else
                    result = i;
            return result;
        }

        public static bool
        getpat(string arg,
        out string pat)
        {
            int makeres;

            makeres = makepat(arg, 0, ENDSTR, out pat);
            return (makeres > 0);
        }

        public static int
        makesub(string arg,
            int from,
            char delim,
            out string sub)
        {
            int result;
            int i, j;
            bool junk;
            char escjunk;
            sub = "";
            j = 0;
            i = from;
            while ((arg[i] != delim) && (arg[i] != ENDSTR))
            {
                if ((arg[i] == ('&')))
                    junk = addstr(unchecked((char)DITTO), ref sub, ref j, MAXPAT);
                else
                {
                    escjunk = esc(arg, ref i);
                    junk = addstr(escjunk, ref sub, ref j, MAXPAT);
                }
                i = i + 1;
            }
            if (arg[i] != delim)
                result = 0;
            else
            {
                junk = addstr(ENDSTR, ref sub, ref j, MAXPAT);
                if ((!junk))
                    result = 0;
                else
                    result = i;
            }
            return result;
        }

        public static bool
        getsub(string arg,
            out string sub)
        {
            int makeres;

            makeres = makesub(arg, 0, ENDSTR, out sub);
            return (makeres > 0);
        }

        public static bool
        locate(char c,
            string pat,
            int offset)
        {
            int i;
            bool flag;

            flag = false;
            i = offset + pat[offset];
            while ((i > offset))
            {
                if (c == pat[i])
                {
                    flag = true;
                    i = offset;
                }
                else
                    i = i - 1;
            }
            return flag;
        }

        public static bool
        omatch(string lin,
            ref int i,
            string pat,
            int j)
        {
            sbyte advance;
            bool result;

            advance = -1;
            if ((lin[i] == ENDSTR))
                result = false;
            else
            {
                if (!in_pat_set(pat[j]))
                {
                    Console.Write("in omatch: can't happen\n");
                    Environment.Exit(-1);
                }
                else
                {
                    switch (pat[j])
                    {
                        case LITCHAR:
                            if (lin[i] == pat[j + 1])
                                advance = 1;
                            break;
                        case BOL:
                            if (i == 0)
                                advance = 0;
                            break;
                        case ANY:
                            if (lin[i] != NEWLINE)
                                advance = 1;
                            break;
                        case EOL:
                            if (lin[i] == NEWLINE)
                                advance = 0;
                            break;
                        case CCL:
                            if (locate(lin[i], pat, j + 1))
                                advance = 1;
                            break;
                        case NCCL:
                            if ((lin[i] != NEWLINE) && (!locate(lin[i], pat, j + 1)))
                                advance = 1;
                            break;
                        default:
                            Caseerror(pat[j]);
                            break;
                    };
                }
            }
            if ((advance >= 0))
            {
                i = i + advance;
                result = true;
            }
            else
                result = false;
            return result;
        }

        public static int
        patsize(string pat,
            int n)
        {
            int size = -1;
            if (!in_pat_set(pat[n]))
            {
                Console.Write("in patsize: can't happen\n");
                Environment.Exit(-1);
            }
            else
                switch (pat[n])
                {
                    case LITCHAR: size = 2; break;

                    case BOL:
                    case EOL:
                    case ANY:
                        size = 1;
                        break;
                    case CCL:
                    case NCCL:
                        size = pat[n + 1] + 2;
                        break;
                    case CLOSURE:
                        size = CLOSIZE;
                        break;
                    default:
                        Caseerror(pat[n]);
                        break;
                }
            return size;
        }

        public static int
        amatch(string lin,
            int offset,
            string pat,
            int j)
        {
            int i = 0, k = 0;
            bool result, done;

            done = false;
            while ((!done) && (pat[j] != ENDSTR))
                if ((pat[j] == CLOSURE))
                {
                    j = j + patsize(pat, j);
                    i = offset;
                    while ((!done) && (lin[i] != ENDSTR))
                    {
                        result = omatch(lin, ref i, pat, j);
                        if (!result)
                            done = true;
                    }
                    done = false;
                    while ((!done) && (i >= offset))
                    {
                        k = amatch(lin, i, pat, j + patsize(pat, j));
                        if ((k >= 0))
                            done = true;
                        else
                            i = i - 1;
                    }
                    offset = k;
                    done = true;
                }
                else
                {
                    result = omatch(lin, ref offset, pat, j);
                    if ((!result))
                    {
                        offset = -1;
                        done = true;
                    }
                    else
                        j = j + patsize(pat, j);
                }
            return offset;
        }

        public static void
        putsub(string lin,
         int s1, int s2,
         string sub)
        {
            int i;
            int j;

            i = 0;
            while ((sub[i] != ENDSTR))
            {
                if ((sub[i] == unchecked((char)DITTO)))
                    for (j = s1; j < s2; j++)
                    {
                        Console.Write(lin[j]);
                    }
                else
                {
                    Console.Write(sub[i]);
                }
                i = i + 1;
            }
        }

        public static void
        subline(string lin,
         string pat,
         string sub)
        {
            int i, lastm, m;

            lastm = -1;
            i = 0;
            while ((lin[i] != ENDSTR))
            {
                m = amatch(lin, i, pat, 0);
                if ((m >= 0) && (lastm != m))
                {
                    putsub(lin, i, m, sub);
                    lastm = m;
                }
                if ((m == -1) || (m == i))
                {
                    Console.Write(lin[i]);
                    i = i + 1;
                }
                else
                    i = m;
            }
        }

        public static void
        change(string pat, string sub)
        {
            string line;
            bool result;

            result = getline(out line, MAXSTR);
            while ((result))
            {
                subline(line, pat, sub);
                result = getline(out line, MAXSTR);
            }
        }

        public static void Main(string[] args)
        {
            string pat, sub;
            bool result;

            if (args.Length < 2)
            {
                Console.Write("usage: change from [to]\n");
                Environment.Exit(1);
            };

            result = getpat(args[1], out pat);
            if (!result)
            {
                Console.Write("change: illegal \"from\" pattern\n");
                Environment.Exit(2);
            }

            if (args.Length >= 3)
            {
                result = getsub(args[2], out sub);
                if (!result)
                {
                    Console.Write("change: illegal \"to\" string\n");
                    Environment.Exit(3);
                }
            }
            else
            {
                sub = '\0'.ToString();
            }

            change(pat, sub);
            return;
        }

        public static void
        Caseerror(int n)
        {
            Console.Write("Missing case limb: line %d\n", n);
            Environment.Exit(4);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edu.Nlu.Sir.Siemens.Shared;

namespace Edu.Nlu.Sir.Siemens.Replace
{
    public class Version3: IReplace, FaultyVersion
    {
        public int[] FaultLines { get { return new int[] { 503 }; } }
        public FaultType FaultType { get { return FaultType.IF_CONDITION_CHANGE; } }

        public const int NULL = 0;

        public const int MAXSTR = 100;
        public const int MAXPAT = MAXSTR;

        public const char ENDSTR = '\0';
        public const char ESCAPE = '@';
        public const char CLOSURE = '*';
        public const char BOL = '%';
        public const char EOL = '$';
        public const char ANY = '?';
        public const char CCL = '[';
        public const char CCLEND = ']';
        public const char NEGATE = '^';
        public const char NCCL = '!';
        public const char LITCHAR = 'c';
        public const char DITTO = unchecked((char)-1);
        public const char DASH = '-';

        public const char TAB = (char)9;
        public const char NEWLINE = (char)10;

        public const int CLOSIZE = 1;


        public override bool
        getline(out char[] s,
        int maxsize)
        {
            
            Console.In.ReadLine(out s, maxsize);
            
            return (s != null);
        }

        public override bool
        addstr(char c,
        ref char[] outset,
        ref int j,
        int maxset)
        {
            bool result;
            if (j >= maxset)
                result = false;
            else
            {
                outset[j] = c;
                j = j + 1;
                result = true;
            }
            return result;
        }

        public override char
        esc(char[] s, ref int i)
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

        public override void
        dodash(char delim,
        char[] src,
        ref int i,
        char[] dest,
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

        public override bool
        getccl(char[] arg,
        ref int i,
        char[] pat,
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
            pat[jstart] = (char)(j - jstart - 1);
            return (arg[i] == CCLEND);
        }

        public override void
        stclose(char[] pat,
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
            pat[lastj] = CLOSURE;
        }

        public override bool in_set_2(char c)
        {
            return (c == BOL || c == EOL || c == CLOSURE);
        }

        public override bool in_pat_set(char c)
        {
            return (c == LITCHAR || c == BOL || c == EOL || c == ANY
            || c == CCL || c == NCCL || c == CLOSURE);
        }

        public override int
        makepat(char[] arg,
         int start,
        char delim,
        out char[] pat)
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
            pat = new char[MAXSTR];
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

        public override bool
        getpat(char[] arg,
        out char[] pat)
        {
            int makeres;

            makeres = makepat(arg, 0, ENDSTR, out pat);
            return (makeres > 0);
        }

        public override int
        makesub(char[] arg,
            int from,
            char delim,
            out char[] sub)
        {
            int result;
            int i, j;
            bool junk;
            char escjunk;
            sub = new char[MAXSTR];
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

        public override bool
        getsub(char[] arg,
            out char[] sub)
        {
            int makeres;

            makeres = makesub(arg, 0, ENDSTR, out sub);
            return (makeres > 0);
        }

        public override bool
        locate(char c,
            char[] pat,
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

        public override bool
        omatch(char[] lin,
            ref int i,
            char[] pat,
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
                    throw new Exception("(-1)");
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

        public override int
        patsize(char[] pat,
            int n)
        {
            int size = -1;
            if (!in_pat_set(pat[n]))
            {
                Console.Write("in patsize: can't happen\n");
                throw new Exception("(-1)");
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

        public override int
        amatch(char[] lin,
            int offset,
            char[] pat,
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

        public override void
        putsub(char[] lin,
         int s1, int s2,
         char[] sub)
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

        public override void
        subline(char[] lin,
         char[] pat,
         char[] sub)
        {
            int i, lastm, m;

            lastm = -1;
            i = 0;
            while ((lin[i] != ENDSTR))
            {
                m = amatch(lin, i, pat, 0);
                if ((m >= 0)  /* && (lastm != m) */) //FAULT: Missing Condition
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

        public override void
        change(char[] pat, char[] sub)
        {
            char[] line = new char[MAXSTR];
            bool result;

            result = getline(out line, MAXSTR);
            while ((result))
            {
                subline(line, pat, sub);
                result = getline(out line, MAXSTR);
            }
        }

        public override void Main(char[][] args)
        {
            char[] pat = new char[MAXSTR], sub = new char[MAXSTR];
            bool result;

            if (args.Length < 2)
            {
                Console.Write("usage: change from [to]\n");
                throw new Exception("(1)");
            };

            result = getpat(args[1], out pat);
            if (!result)
            {
                Console.Write("change: illegal \"from\" pattern\n");
                throw new Exception("(2)");
            }

            if (args.Length >= 3)
            {
                result = getsub(args[2], out sub);
                if (!result)
                {
                    Console.Write("change: illegal \"to\" char[]\n");
                    throw new Exception("(3)");
                }
            }
            else
            {
                sub = new char[]{'\0'};
            }

            change(pat, sub);
            return;
        }

        public override void
        Caseerror(int n)
        {
            Console.Write("Missing case limb: line %d\n", n);
            throw new Exception("(4)");
        }

    }
}

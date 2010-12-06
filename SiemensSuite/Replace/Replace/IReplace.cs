using System;
using System.Text;
namespace Edu.Nlu.Sir.Siemens.Replace
{
    public abstract class IReplace
    {
        public abstract bool addstr(char c, ref char[] outset, ref int j, int maxset);
        public abstract int amatch(char[] lin, int offset, char[] pat, int j);
        public abstract void Caseerror(int n);
        public abstract void change(char[] pat, char[] sub);
        public abstract void dodash(char delim, char[] src, ref int i, char[] dest, ref int j, int maxset);
        public abstract char esc(char[] s, ref int i);
        public abstract bool getccl(char[] arg, ref int i, char[] pat, ref int j);
        public abstract bool getline(out char[] s, int maxsize);
        public abstract bool getpat(char[] arg, out char[] pat);
        public abstract bool getsub(char[] arg, out char[] sub);
        public abstract bool in_pat_set(char c);
        public abstract bool in_set_2(char c);
        public abstract bool locate(char c, char[] pat, int offset);
        public abstract void Main(char[][] args);
        public abstract int makepat(char[] arg, int start, char delim, out char[] pat);
        public abstract int makesub(char[] arg, int from, char delim, out char[] sub);
        public abstract bool omatch(char[] lin, ref int i, char[] pat, int j);
        public abstract int patsize(char[] pat, int n);
        public abstract void putsub(char[] lin, int s1, int s2, char[] sub);
        public abstract void stclose(char[] pat, ref int j, int lastj);
        public abstract void subline(char[] lin, char[] pat, char[] sub);
    }
}

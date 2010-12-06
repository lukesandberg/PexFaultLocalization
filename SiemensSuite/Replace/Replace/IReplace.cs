using System;
using System.Text;
namespace Edu.Nlu.Sir.Siemens.Replace
{
    public interface IReplace
    {
        bool addstr(char c, ref char[] outset, ref int j, int maxset);
        int amatch(char[] lin, int offset, char[] pat, int j);
        void Caseerror(int n);
        void change(char[] pat, char[] sub);
        void dodash(char delim, char[] src, ref int i, char[] dest, ref int j, int maxset);
        char esc(char[] s, ref int i);
        bool getccl(char[] arg, ref int i, char[] pat, ref int j);
        bool getline(out char[] s, int maxsize);
        bool getpat(char[] arg, out char[] pat);
        bool getsub(char[] arg, out char[] sub);
        bool in_pat_set(char c);
        bool in_set_2(char c);
        bool locate(char c, char[] pat, int offset);
        void Main(char[][] args);
        int makepat(char[] arg, int start, char delim, out char[] pat);
        int makesub(char[] arg, int from, char delim, out char[] sub);
        bool omatch(char[] lin, ref int i, char[] pat, int j);
        int patsize(char[] pat, int n);
        void putsub(char[] lin, int s1, int s2, char[] sub);
        void stclose(char[] pat, ref int j, int lastj);
        void subline(char[] lin, char[] pat, char[] sub);
    }
}

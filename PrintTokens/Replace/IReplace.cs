using System;
namespace Edu.Nlu.Sir.Siemens.Replace
{
    public interface IReplace
    {
        bool addstr(char c, ref string outset, ref int j, int maxset);
        int amatch(string lin, int offset, string pat, int j);
        void Caseerror(int n);
        void change(string pat, string sub);
        void dodash(char delim, string src, ref int i, string dest, ref int j, int maxset);
        char esc(string s, ref int i);
        bool getccl(string arg, ref int i, string pat, ref int j);
        bool getline(out string s, int maxsize);
        bool getpat(string arg, out string pat);
        bool getsub(string arg, out string sub);
        bool in_pat_set(char c);
        bool in_set_2(char c);
        bool locate(char c, string pat, int offset);
        void Main(string[] args);
        int makepat(string arg, int start, char delim, out string pat);
        int makesub(string arg, int from, char delim, out string sub);
        bool omatch(string lin, ref int i, string pat, int j);
        int patsize(string pat, int n);
        void putsub(string lin, int s1, int s2, string sub);
        void stclose(string pat, ref int j, int lastj);
        void subline(string lin, string pat, string sub);
    }
}

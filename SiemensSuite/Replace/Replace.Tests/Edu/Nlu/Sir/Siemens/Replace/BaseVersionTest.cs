// <copyright file="BaseVersionTest.cs" company="General Electric">Copyright © General Electric 2010</copyright>
using System;
using Edu.Nlu.Sir.Siemens.Replace;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Edu.Nlu.Sir.Siemens.Replace
{
    /// <summary>This class contains parameterized unit tests for BaseVersion</summary>
    [PexClass(typeof(BaseVersion))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class BaseVersionTest
    {
        /// <summary>Test stub for Caseerror(Int32)</summary>
        [PexMethod]
        public void Caseerror(int n)
        {
            ReplaceVersionFactory.getReplaceVersion().Caseerror(n);
            // TODO: add assertions to method BaseVersionTest.Caseerror(Int32)
        }

        /// <summary>Test stub for addstr(Char, String&amp;, Int32&amp;, Int32)</summary>
        [PexMethod]
        public bool addstr(
            char c,
            ref char[] outset,
            ref int j,
            int maxset
        )
        {
            PexAssume.IsNotNull(outset);
            PexAssume.IsTrue(j >= 0 && j+1 < outset.Length);
            PexAssume.IsTrue(maxset >= 0);
            PexAssume.IsTrue(outset.Length == BaseVersion.MAXSTR);
            bool result = ReplaceVersionFactory.getReplaceVersion().addstr(c, ref outset, ref j, maxset);
            return result;
            // TODO: add assertions to method BaseVersionTest.addstr(Char, String&, Int32&, Int32)
        }

        [PexMethod(MaxBranches = 40000, MaxConstraintSolverTime = 2)]
        public int amatch(
            char[] lin,
            int offset,
            char[] pat,
            int j
        )
        {
            PexAssume.IsTrue(offset >= 0);
            PexAssume.IsNotNull(pat);
            PexAssume.IsNotNull(lin);
            PexAssume.IsTrue(lin.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(pat.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(offset < Array.LastIndexOf(lin, BaseVersion.ENDSTR));
            PexAssume.IsTrue(offset < Array.LastIndexOf(pat, BaseVersion.ENDSTR));
            PexAssume.IsTrue(j < Array.LastIndexOf(lin, BaseVersion.ENDSTR));
            PexAssume.IsTrue(j < Array.LastIndexOf(pat, BaseVersion.ENDSTR));
            int result = ReplaceVersionFactory.getReplaceVersion().amatch(lin, offset, pat, j);
            return result;
            // TODO: add assertions to method BaseVersionTest.amatch(String, Int32, String, Int32)
        }

        ///// <summary>Test stub for change(String, String)</summary>
        //[PexMethod(MaxBranches = 20000)]
        //public void change(char[] pat, char[] sub)
        //{
        //    PexAssume.IsNotNull(pat);
        //    PexAssume.IsTrue(pat.Contains(BaseVersion.ENDSTR.ToString()));
        //    PexAssume.IsNotNull(sub);
           
        //    ReplaceVersionFactory.getReplaceVersion().change(pat, sub);
        //    // TODO: add assertions to method BaseVersionTest.change(String, String)
        //}

        /// <summary>Test stub for dodash(Char, String, Int32&amp;, String, Int32&amp;, Int32)</summary>
        [PexMethod(MaxRunsWithoutNewTests = 200, MaxConditions = 1000, MaxConstraintSolverTime = 5)]
        public void dodash(
            char delim,
            char[] src,
            ref int i,
            char[] dest,
            ref int j,
            int maxset
        )
        {
            PexAssume.IsNotNull(dest);
            PexAssume.IsNotNull(src);
            PexAssume.IsTrue(src.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(dest.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(i > 0 && i+1 < src.Length);
            PexAssume.IsTrue(i < Array.LastIndexOf(src, BaseVersion.ENDSTR));
            PexAssume.IsTrue(j >= 0 && j < dest.Length);
            PexAssume.IsTrue(maxset < src.Length && maxset < dest.Length);
            ReplaceVersionFactory.getReplaceVersion().dodash(delim, src, ref i, dest, ref j, maxset);
            // TODO: add assertions to method BaseVersionTest.dodash(Char, String, Int32&, String, Int32&, Int32)
        }

        /// <summary>Test stub for esc(String, Int32&amp;)</summary>
        [PexMethod]
        public char esc(char[] s, ref int i)
        {
            PexAssume.IsNotNull(s);
            PexAssume.IsTrue(i >= 0);
            PexAssume.IsTrue(i  < s.Length);
            PexAssume.IsTrue(i + 1 < s.Length);
            PexAssume.IsTrue(s.Length == BaseVersion.MAXSTR);
            char result = ReplaceVersionFactory.getReplaceVersion().esc(s, ref i);
            return result;
            // TODO: add assertions to method BaseVersionTest.esc(String, Int32&)
        }

        /// <summary>Test stub for getccl(String, Int32&amp;, String, Int32&amp;)</summary>
        [PexMethod(MaxConditions = 4000, MaxConstraintSolverTime = 5, MaxRunsWithoutNewTests = 200, MaxBranches = 20000)]
        public bool getccl(
            char[] arg,
            ref int i,
            char[] pat,
            ref int j
        )
        {
            PexAssume.IsNotNull(arg);
            PexAssume.IsNotNull(pat);
            PexAssume.IsTrue(arg.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(pat.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(i > 0 && i + 1 < arg.Length);
            PexAssume.IsTrue(i < Array.LastIndexOf(arg, BaseVersion.ENDSTR));
            PexAssume.IsTrue(j < Array.LastIndexOf(arg, BaseVersion.ENDSTR));
            PexAssume.IsTrue(j >= 0 && j < pat.Length);
            
            bool result = ReplaceVersionFactory.getReplaceVersion().getccl(arg, ref i, pat, ref j);
            return result;
            // TODO: add assertions to method BaseVersionTest.getccl(String, Int32&, String, Int32&)
        }

        ///// <summary>Test stub for getline(String&amp;, Int32)</summary>
        //[PexMethod, PexAllowedException(typeof(IOException))]
        //public bool getline(out char[] s, int maxsize)
        //{
        //    bool result = ReplaceVersionFactory.getReplaceVersion().getline(out s, maxsize);
        //    return result;
        //    // TODO: add assertions to method BaseVersionTest.getline(String&, Int32)
        //}

        /// <summary>Test stub for getpat(String, String&amp;)</summary>
        [PexMethod]
        public bool getpat(char[] arg, out char[] pat)
        {
            PexAssume.IsNotNull(arg);
            PexAssume.IsTrue(arg.Length == BaseVersion.MAXSTR);
            PexAssume.TrueForAny(arg, a => a == BaseVersion.ENDSTR);
            bool result = ReplaceVersionFactory.getReplaceVersion().getpat(arg, out pat);
            return result;
            // TODO: add assertions to method BaseVersionTest.getpat(String, String&)
        }

        /// <summary>Test stub for getsub(String, String&amp;)</summary>
        [PexMethod(MaxConditions = 8000, MaxRunsWithoutNewTests = 400, MaxBranches = 20000, MaxConstraintSolverTime = 2)]
        public bool getsub(char[] arg, out char[] sub)
        {
            PexAssume.IsNotNull(arg);
            PexAssume.IsTrue(arg.Length == BaseVersion.MAXSTR);
            PexAssume.TrueForAny(arg, a => a == BaseVersion.ENDSTR);
            bool result = ReplaceVersionFactory.getReplaceVersion().getsub(arg, out sub);
            return result;
            // TODO: add assertions to method BaseVersionTest.getsub(String, String&)
        }

        /// <summary>Test stub for in_pat_set(Char)</summary>
        [PexMethod]
        public bool in_pat_set(char c)
        {
            bool result = ReplaceVersionFactory.getReplaceVersion().in_pat_set(c);
            return result;
            // TODO: add assertions to method BaseVersionTest.in_pat_set(Char)
        }

        /// <summary>Test stub for in_set_2(Char)</summary>
        [PexMethod]
        public bool in_set_2(char c)
        {
            bool result = ReplaceVersionFactory.getReplaceVersion().in_set_2(c);
            return result;
            // TODO: add assertions to method BaseVersionTest.in_set_2(Char)
        }

        /// <summary>Test stub for locate(Char, String, Int32)</summary>
        [PexMethod]
        public bool locate(
            char c,
            char[] pat,
            int offset
        )
        {
            PexAssume.IsNotNull(pat);
            PexAssume.IsTrue(pat.Length == BaseVersion.MAXSTR);
            PexAssume.IsFalse ((uint)offset >= (uint)(pat.Length));
            PexAssume.IsFalse ((uint)((int)((ushort)(pat[offset])) + offset) >= (uint)(pat.Length));
            PexAssume.IsFalse ((uint)(offset + (int)((ushort)(pat[offset]))) >= (uint)(pat.Length));
            bool result = ReplaceVersionFactory.getReplaceVersion().locate(c, pat, offset);
            return result;
            // TODO: add assertions to method BaseVersionTest.locate(Char, String, Int32)
        }

        /// <summary>Test stub for makepat(String, Int32, Char, String&amp;)</summary>
        [PexMethod(MaxConditions = 1000)]
        public int makepat(
            char[] arg,
            int start,
            char delim,
            out char[] pat
        )
        {
            PexAssume.IsNotNull(arg);
            PexAssume.IsTrue(start >= 0 && start < arg.Length);
            PexAssume.IsTrue(arg.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(start < Array.LastIndexOf(arg, BaseVersion.ENDSTR));
            int result = ReplaceVersionFactory.getReplaceVersion().makepat(arg, start, delim, out pat);
            return result;
            // TODO: add assertions to method BaseVersionTest.makepat(String, Int32, Char, String&)
        }

        /// <summary>Test stub for makesub(String, Int32, Char, String&amp;)</summary>
        [PexMethod(MaxConstraintSolverTime = 5, MaxConditions = 1000)]
        public int makesub(
            char[] arg,
            int from,
            char delim,
            out char[] sub
        )
        {
            PexAssume.IsNotNull(arg);
            PexAssume.IsTrue(arg.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(from >= 0 && from < arg.Length);
            PexAssume.IsTrue(from < Array.LastIndexOf(arg, BaseVersion.ENDSTR));
                
            int result = ReplaceVersionFactory.getReplaceVersion().makesub(arg, from, delim, out sub);
            return result;
            // TODO: add assertions to method BaseVersionTest.makesub(String, Int32, Char, String&)
        }

        /// <summary>Test stub for omatch(String, Int32&amp;, String, Int32)</summary>
        [PexMethod(MaxRunsWithoutNewTests = 200)]
        public bool omatch(
            char[] lin,
            ref int i,
            char[] pat,
            int j
        )
        {
            PexAssume.IsNotNull(lin);
            PexAssume.IsNotNull(pat);
            PexAssume.IsTrue(lin.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(pat.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(i >= 0 && i < lin.Length);
            PexAssume.IsTrue(j + 1 > 0 && j+1 < pat.Length);
            PexAssume.IsFalse((uint)((int)((ushort)(pat[j+1])) + j+1) >= (uint)(pat.Length));
            PexAssume.IsFalse((uint)(j+1 + (int)((ushort)(pat[j+1]))) >= (uint)(pat.Length));
            bool result = ReplaceVersionFactory.getReplaceVersion().omatch(lin, ref i, pat, j);
            return result;
            // TODO: add assertions to method BaseVersionTest.omatch(String, Int32&, String, Int32)
        }

        /// <summary>Test stub for patsize(String, Int32)</summary>
        [PexMethod]
        public int patsize(char[] pat, int n)
        {
            PexAssume.IsNotNull(pat);
            PexAssume.IsTrue(pat.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(n >= 0 && n < pat.Length && n+1 < pat.Length);
            int result = ReplaceVersionFactory.getReplaceVersion().patsize(pat, n);
            return result;
            // TODO: add assertions to method BaseVersionTest.patsize(String, Int32)
        }

        /// <summary>Test stub for putsub(String, Int32, Int32, String)</summary>
        [PexMethod(MaxConditions = 1000)]
        public void putsub(
            char[] lin,
            int s1,
            int s2,
            char[] sub
        )
        {
            PexAssume.IsNotNull(lin);
            PexAssume.IsNotNull(sub);
            PexAssume.IsTrue(lin.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(sub.Length == BaseVersion.MAXSTR);
            PexAssume.TrueForAny(sub, s => s == BaseVersion.ENDSTR);
            PexAssume.IsTrue(s1 >= 0 && s1 < lin.Length);
            PexAssume.IsTrue(s2 > s1 && s2 < lin.Length);
                
            ReplaceVersionFactory.getReplaceVersion().putsub(lin, s1, s2, sub);
            // TODO: add assertions to method BaseVersionTest.putsub(String, Int32, Int32, String)
        }

        /// <summary>Test stub for stclose(String, Int32&amp;, Int32)</summary>
        [PexMethod]
        public void stclose(
            char[] pat,
            ref int j,
            int lastj
        )
        {
            PexAssume.IsNotNull(pat);
            PexAssume.IsTrue(pat.Length == BaseVersion.MAXSTR);
            PexAssume.IsTrue(pat.Length > 3);
            PexAssume.IsTrue(lastj < pat.Length);
            PexAssume.IsTrue(j < pat.Length);
            PexAssume.IsTrue(j >= lastj && lastj > 0);
            ReplaceVersionFactory.getReplaceVersion().stclose(pat, ref j, lastj);
            // TODO: add assertions to method BaseVersionTest.stclose(String, Int32&, Int32)
        }

        [PexMethod(MaxBranches = 80000, MaxConstraintSolverTime = 2), PexAllowedException(typeof(ArgumentException))]
        public void subline(
            char[] lin,
            char[] pat,
            char[] sub
        )
        {
            PexAssume.IsNotNull(lin);
            PexAssume.IsTrue(lin.Length == BaseVersion.MAXSTR);
            PexAssume.TrueForAny(lin, s => s == BaseVersion.ENDSTR);
            
            PexAssume.IsNotNull(pat);
            PexAssume.IsTrue(pat.Length == BaseVersion.MAXSTR);
            PexAssume.TrueForAny(pat, s => s == BaseVersion.ENDSTR);
            
            PexAssume.IsNotNull(sub);
            PexAssume.IsTrue(sub.Length == BaseVersion.MAXSTR);
            PexAssume.TrueForAny(sub, s => s == BaseVersion.ENDSTR);

            PexAssume.IsTrue(Array.LastIndexOf(pat, BaseVersion.ENDSTR) <= Array.LastIndexOf(lin, BaseVersion.ENDSTR));
            PexAssume.IsTrue(Array.LastIndexOf(sub, BaseVersion.ENDSTR) <= Array.LastIndexOf(lin, BaseVersion.ENDSTR));

            ReplaceVersionFactory.getReplaceVersion().subline(lin, pat, sub);
            // TODO: add assertions to method BaseVersionTest.subline(String, String, String)
        }
    }
}

// <copyright file="ProgramTest.cs" company="Authorized User">Copyright © Authorized User 2010</copyright>
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using schedule;
using System.Text.RegularExpressions;

namespace schedule
{
    /// <summary>This class contains parameterized unit tests for Program</summary>
    [PexClass(typeof(Program))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ProgramTest
    {
        /// <summary>Test stub for Main(String[])</summary>
		[PexMethod(MaxRuns = 10000, MaxConditions = 10000, MaxRunsWithoutNewTests = 1000000000, MaxConstraintSolverTime = 1000000000)]
        public void Main(string[] args)
		{
			PexAssume.IsNotNullOrEmpty(args);
			PexAssume.IsTrue(args.Length == 4);
			PexAssume.TrueForAll(0, 4, i => !String.IsNullOrEmpty(args[i]));
			PexAssume.TrueForAll(0, 3, i => int.Parse(args[i]) > 0 && int.Parse(args[i]) < 100);
			PexAssume.IsNotNull(args[3]);
			PexAssume.IsFalse(args[3].Contains("\0"));
			String re = @"^(((1 [123])|(2 [12] \.[0-9]{2})|3|(4 \.[0-9]{2})|5|6)
)+7$";
			PexAssume.IsTrue(Regex.IsMatch(args[3], re, RegexOptions.Multiline | RegexOptions.Compiled));
			Program.Main(args);
            // TODO: add assertions to method ProgramTest.Main(String[])
        }

        /// <summary>Test stub for addProcess(Int32)</summary>
        [PexMethod]
        public void addProcess(int prio)
        {
            PexAssume.IsTrue(prio < (Program.MAXPRIO + 1));
            PexAssume.IsTrue(prio >= 0);

            Program.addProcess(prio);
            // TODO: add assertions to method ProgramTest.addProcess(Int32)
        }

        /// <summary>Test stub for append_ele(LinkedList`1&lt;Ele&gt;, Ele)</summary>
        [PexMethod]
        public LinkedList<Ele> append_ele(LinkedList<Ele> a_list, Ele a_ele)
        {
            LinkedList<Ele> result = Program.append_ele(a_list, a_ele);
            return result;
            // TODO: add assertions to method ProgramTest.append_ele(LinkedList`1<Ele>, Ele)
        }

        /// <summary>Test stub for block_process()</summary>
        [PexMethod]
        public void block_process()
        {
            Program.block_process();
            // TODO: add assertions to method ProgramTest.block_process()
        }

        /// <summary>Test stub for del_ele(LinkedList`1&lt;Ele&gt;, LinkedListNode`1&lt;Ele&gt;)</summary>
        [PexMethod]
        public LinkedList<Ele> del_ele(LinkedList<Ele> d_list, LinkedListNode<Ele> d_ele)
        {
            //PexAssume.IsNotNullOrEmpty(d_list);
            //PexAssume.IsNotNull(d_ele);
            if (d_list != null)
            {
                PexAssume.IsFalse(d_list.Contains(null));
                if (d_ele != null)
                {
                    PexAssume.IsTrue(d_list.Contains(d_ele.Value));
                    PexAssume.IsNotNull(d_ele.List);
                    if (d_ele.List != null)
                    {
                        PexAssume.IsTrue(d_ele.List.Equals(d_list));
                    }
                }                
            }

            LinkedList<Ele> result = Program.del_ele(d_list, d_ele);
            return result;
            // TODO: add assertions to method ProgramTest.del_ele(LinkedList`1<Ele>, LinkedListNode`1<Ele>)
        }

        /// <summary>Test stub for find_nth(LinkedList`1&lt;Ele&gt;, Int32)</summary>
        [PexMethod]
        public LinkedListNode<Ele> find_nth(LinkedList<Ele> f_list, int n)
        {
            LinkedListNode<Ele> result = Program.find_nth(f_list, n);
            return result;
            // TODO: add assertions to method ProgramTest.find_nth(LinkedList`1<Ele>, Int32)
        }

        /// <summary>Test stub for finish_all_processes()</summary>
        [PexMethod]
        public void finish_all_processes()
        {
            Program.finish_all_processes();
            // TODO: add assertions to method ProgramTest.finish_all_processes()
        }

        /// <summary>Test stub for finish_process()</summary>
        [PexMethod]
        public void finish_process()
        {
            Program.finish_process();
            // TODO: add assertions to method ProgramTest.finish_process()
        }

        /// <summary>Test stub for init_prio_queue(Int32, Int32)</summary>
        [PexMethod]
        public void init_prio_queue(int prio, int num_proc)
        {
            PexAssume.IsTrue(prio < (Program.MAXPRIO + 1));
            PexAssume.IsTrue(prio >= 0);

            Program.init_prio_queue(prio, num_proc);
            // TODO: add assertions to method ProgramTest.init_prio_queue(Int32, Int32)
        }

        /// <summary>Test stub for initialize()</summary>
        [PexMethod]
        public void initialize()
        {
            Program.initialize();
            // TODO: add assertions to method ProgramTest.initialize()
        }

        /// <summary>Test stub for new_list()</summary>
        [PexMethod]
        public LinkedList<Ele> new_list()
        {
            LinkedList<Ele> result = Program.new_list();
            return result;
            // TODO: add assertions to method ProgramTest.new_list()
        }

        /// <summary>Test stub for new_process(Int32)</summary>
        [PexMethod]
        public Ele new_process(int prio)
        {
            Ele result = Program.new_process(prio);
            return result;
            // TODO: add assertions to method ProgramTest.new_process(Int32)
        }

        /// <summary>Test stub for quantum_expire()</summary>
        [PexMethod]
        public void quantum_expire()
        {
            Program.quantum_expire();
            // TODO: add assertions to method ProgramTest.quantum_expire()
        }

        /// <summary>Test stub for readFile(String)</summary>
        [PexMethod(MaxRuns=10000)]
        public ArrayList readFile(string path)
        {
            PexAssume.IsNotNullOrEmpty(path);
            /*string[] filePaths = Directory.GetFiles(@"input");
            ArrayList fp = new ArrayList(filePaths);
            PexAssume.IsTrue(fp.Contains(path));
			*/
			PexAssume.IsTrue(Regex.IsMatch(path, @"(\d+(\s+\d+)*)\n+", RegexOptions.Multiline));
            ArrayList result = Program.readFile(path);
			PexAssert.IsNotNull(result);
			
            return result;
            // TODO: add assertions to method ProgramTest.readFile(String)
        }

        /// <summary>Test stub for schedule()</summary>
        [PexMethod]
        public void schedule()
        {
            PexAssume.IsNotNull(Program.prio_queue);
            for (int i = 0; i < Program.prio_queue.Length; i++)
            {
                PexAssume.IsNotNull(Program.prio_queue[i]);
            }

            Program.schedule();
            // TODO: add assertions to method ProgramTest.schedule()
        }

        /// <summary>Test stub for unblock_process(Single)</summary>
        [PexMethod]
        public void unblock_process(float ratio)
        {
            Program.unblock_process(ratio);
            // TODO: add assertions to method ProgramTest.unblock_process(Single)
        }

        /// <summary>Test stub for upgrade_process_prio(Int32, Single)</summary>
        [PexMethod]
        public void upgrade_process_prio(int prio, float ratio)
        {
            PexAssume.IsTrue(prio < (Program.MAXPRIO + 1));
            PexAssume.IsTrue(prio >= 0);
            PexAssume.IsNotNull(Program.prio_queue);
            for (int i = 0; i < Program.prio_queue.Length; i++)
            {
                PexAssume.IsNotNull(Program.prio_queue[i]);
            }

            Program.upgrade_process_prio(prio, ratio);
            // TODO: add assertions to method ProgramTest.upgrade_process_prio(Int32, Single)
        }
    }
}

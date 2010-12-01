// <copyright file="ProgramTest.cs" company="Authorized User">Copyright © Authorized User 2010</copyright>
using System;
using System.Collections;
using System.IO;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using schedule2;

namespace schedule2
{
    /// <summary>This class contains parameterized unit tests for Program</summary>
    [PexClass(typeof(Program))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ProgramTest
    {
        /// <summary>Test stub for Main(String[])</summary>
        [PexMethod]
        public void Main(string[] args)
        {
            short x;
            string[] filePaths = Directory.GetFiles(@"input");
            ArrayList fp = new ArrayList(filePaths);
            PexAssume.IsNotNullOrEmpty(args);
            PexAssume.IsTrue(args.Length == 4);
            for (int i = 0; i < args.Length; i++)
            {
                PexAssume.IsNotNullOrEmpty(args[i]);
                if (i != 3)
                {
                    PexAssume.IsTrue(Int16.TryParse(args[i], out x));
                    PexAssume.IsTrue(Int16.Parse(args[i]) > 0);
                    PexAssume.IsFalse(args[i].Contains("\0"));
                }
                else
                {
                    PexAssume.IsTrue(fp.Contains(args[3]));
                }
            }

            Program.Main(args);
            // TODO: add assertions to method ProgramTest.Main(String[])
        }

        /// <summary>Test stub for block()</summary>
        [PexMethod]
        public int block()
        {
            int result = Program.block();
            return result;
            // TODO: add assertions to method ProgramTest.block()
        }

        /// <summary>Test stub for enqueue(Int32, Process)</summary>
        [PexMethod]
        public int enqueue(int prio, Process new_process)
        {
            PexAssume.IsTrue(prio < (Program.MAXPRIO + 1));
            PexAssume.IsTrue(prio >= 0);
            PexAssume.IsNotNull(new_process);

            int result = Program.enqueue(prio, new_process);
            return result;
            // TODO: add assertions to method ProgramTest.enqueue(Int32, Process)
        }

        /// <summary>Test stub for finish()</summary>
        [PexMethod]
        public int finish()
        {
            int result = Program.finish();
            return result;
            // TODO: add assertions to method ProgramTest.finish()
        }

        /// <summary>Test stub for flush()</summary>
        [PexMethod]
        public int flush()
        {
            int result = Program.flush();
            return result;
            // TODO: add assertions to method ProgramTest.flush()
        }

        /// <summary>Test stub for getCurrent()</summary>
        [PexMethod]
        public Process getCurrent()
        {
            Process result = Program.getCurrent();
            return result;
            // TODO: add assertions to method ProgramTest.getCurrent()
        }

        /// <summary>Test stub for get_process(Int32, Single, Process&amp;)</summary>
        [PexMethod]
        public int get_process(
            int prio,
            float ratio,
            ref Process job
        )
        {
            PexAssume.IsTrue(prio < (Program.MAXPRIO + 1));
            PexAssume.IsTrue(prio >= 0);
            PexAssume.IsNotNull(Program.prio_queue[prio]);

            int result = Program.get_process(prio, ratio, ref job);
            return result;
            // TODO: add assertions to method ProgramTest.get_process(Int32, Single, Process&)
        }

        /// <summary>Test stub for new_job(Int32)</summary>
        [PexMethod]
        public int new_job(int prio)
        {
            PexAssume.IsTrue(prio < (Program.MAXPRIO + 1));
            PexAssume.IsTrue(prio >= 0);

            int result = Program.new_job(prio);
            return result;
            // TODO: add assertions to method ProgramTest.new_job(Int32)
        }

        /// <summary>Test stub for put_end(Int32, Process)</summary>
        [PexMethod]
        public int put_end(int prio, Process process)
        {
            PexAssume.IsTrue(prio < (Program.MAXPRIO + 1));
            PexAssume.IsTrue(prio >= 0);

            int result = Program.put_end(prio, process);
            return result;
            // TODO: add assertions to method ProgramTest.put_end(Int32, Process)
        }

        /// <summary>Test stub for quantum_expire()</summary>
        [PexMethod]
        public int quantum_expire()
        {
            int result = Program.quantum_expire();
            return result;
            // TODO: add assertions to method ProgramTest.quantum_expire()
        }

        /// <summary>Test stub for readFile(String)</summary>
        [PexMethod]
        public ArrayList readFile(string path)
        {
            PexAssume.IsNotNullOrEmpty(path);            
            string[] filePaths = Directory.GetFiles(@"input");
            ArrayList fp = new ArrayList(filePaths);
            PexAssume.IsTrue(fp.Contains(path) && !path.Contains("\0"));            

            ArrayList result = Program.readFile(path);
            return result;
            // TODO: add assertions to method ProgramTest.readFile(String)
        }

        /// <summary>Test stub for reschedule(Int32)</summary>
        [PexMethod]
        public int reschedule(int prio)
        {
            int result = Program.reschedule(prio);
            return result;
            // TODO: add assertions to method ProgramTest.reschedule(Int32)
        }

        /// <summary>Test stub for schedule(Int32, Int32, Single)</summary>
        [PexMethod]
        public int schedule(
            int command,
            int prio,
            float ratio
        )
        {
            PexAssume.IsTrue(prio < (Program.MAXPRIO + 1));
            PexAssume.IsTrue(prio >= 0);

            int result = Program.schedule(command, prio, ratio);
            return result;
            // TODO: add assertions to method ProgramTest.schedule(Int32, Int32, Single)
        }

        /// <summary>Test stub for unblock(Single)</summary>
        [PexMethod]
        public int unblock(float ratio)
        {
            int result = Program.unblock(ratio);
            return result;
            // TODO: add assertions to method ProgramTest.unblock(Single)
        }

        /// <summary>Test stub for upgrade_prio(Int32, Single)</summary>
        [PexMethod]
        public int upgrade_prio(int prio, float ratio)
        {
            int result = Program.upgrade_prio(prio, ratio);
            return result;
            // TODO: add assertions to method ProgramTest.upgrade_prio(Int32, Single)
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FaultLocalization
{
    class ReflectionTestRunner : AbstractTestRunner
    {
        private UTCore.TestRunner TestRunner { get; set; }

        public ReflectionTestRunner(TestSuite testSuite)
            : base(testSuite)
        {
            TestRunner = new UTCore.TestRunner();
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        }

        public override void RunTests()
        {
            foreach (string testDllPath in tests.TestDllPaths)
            {
                TestRunner.LoadAssembly(testDllPath);
            }
            TestRunner.ParseAssemblies();
            TestRunner.RunTests();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaultLocalization
{
    class ReflectionTestRunner : AbstractTestRunner
    {
        private UTCore.TestRunner TestRunner { get; set; }

        public ReflectionTestRunner(TestSuite testSuite)
            : base(testSuite)
        {
            TestRunner = new UTCore.TestRunner();
        }

        public override void RunTests()
        {
            foreach (string testDllPath in tests.TestDllPaths)
            {
                TestRunner.LoadAssembly(testDllPath);
            }
            TestRunner.RunTests();
        }

    }
}

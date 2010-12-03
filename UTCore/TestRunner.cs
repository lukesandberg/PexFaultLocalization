using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Pex.Framework.Generated;

using KALib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UTCore
{
	public delegate void TestNotificationDelegate(UTCore.TestAttribute ta);

	public class TestRunner
	{
		private AssemblyCollection assemblyCollection;
		private IList<TestFixture> testFixtureList;
		public event TestNotificationDelegate testNotificationEvent;
		private int numTests;

		public AssemblyCollection AssemblyCollection
		{
			get
			{
				return assemblyCollection;
			}
		}

		public IEnumerable<TestFixture> TestFixtures
		{
			get
			{
				return testFixtureList;
			}
		}

		public IEnumerable<TestAttribute> Tests
		{
			get
			{
				return testFixtureList.SelectMany(tf => tf.Tests);
			}
		}

		public int NumTests
		{
			get
			{
				return numTests;
			}
		}

		public TestRunner()
		{
			assemblyCollection=new AssemblyCollection();
			testFixtureList=new List<TestFixture>();
		}

		public void LoadAssembly(string file)
		{
			AssemblyItem ai=new AssemblyItem();
			ai.Load(file);
			ai.LoadNamespaces();
			ai.LoadClasses();
			ai.LoadMethods();
			assemblyCollection.Add(file, ai);
		}

		public void ParseAssemblies()
		{
			numTests=0;

			foreach (AssemblyItem ai in assemblyCollection.Values)
			{
				foreach (NamespaceItem ni in ai.NamespaceCollection.Values)
				{
					foreach (ClassItem ci in ni.ClassCollection.Values)
					{
						TestFixture tf=new TestFixture();
						foreach (Attribute attr in ci.Attributes)
						{
                            
							// verify that attribute class is "UnitTest"
							string attrStr=attr.ToString();
							attrStr=StringHelpers.RightOfRightmostOf(attrStr, '.');
							Trace.WriteLine("Class: "+ci.ToString()+", Attribute: "+attrStr);
                            
							try
							{
                                if (attr.GetType() ==typeof(TestClassAttribute))
                                {
                                    TestUnitAttribute tua = new TestFixtureAttribute();
                                    tua.Initialize(ci, null, attr);
                                    tua.SelfRegister(tf);
                                }
							}
							catch(Exception e)
							{
								Trace.WriteLine("Exception adding attribute: "+e.Message);
								Trace.WriteLine("Attribute "+attrStr+" is unknown");
							}
						}

						if (tf.HasTestFixture)
						{
							foreach(MethodItem mi in ci.MethodCollection.Values)
							{
								foreach (object attr in mi.Attributes)
								{
									// verify that attribute class is "UnitTest"
									string attrStr=attr.ToString();
									attrStr=StringHelpers.RightOfRightmostOf(attrStr, '.');
									Trace.WriteLine("Method: "+mi.ToString()+", Attribute: "+attrStr);
									try
									{
                                        if (attr.GetType() == typeof(TestMethodAttribute))
                                        {
                                            TestUnitAttribute tua = new TestAttribute();
                                            tua.Initialize(ci, mi, attr);
                                            tua.SelfRegister(tf);
                                        }
                                        else if (attr.GetType() == typeof(PexRaisedExceptionAttribute))
                                        {
                                            PexRaisedExceptionAttribute excAttr = (PexRaisedExceptionAttribute)attr;

                                            TestUnitAttribute tua = new ExpectedExceptionAttribute();
                                            tua.Initialize(ci, mi, attr);
                                            tua.SelfRegister(tf);
                                        }
									}
									catch(TypeLoadException)
									{
										Trace.WriteLine("Attribute "+attrStr+"is unknown");
									}
								}
							}
							testFixtureList.Add(tf);
							numTests+=tf.NumTests;
						}
					}
				}
			}
		}

		public void RunTests()
		{
			foreach(TestFixture tf in testFixtureList)
			{
				tf.RunTests(testNotificationEvent);
			}
		}

        public void RunTests(IEnumerable<string> TestNamePatterns)
        {
            var tests = from tf in TestFixtures
                    select tf.Tests.Where(t => TestNames.Contains(t.TestMethod.MethodName));
            foreach (var test in tests)
            {
                

            }
        }

        public void RunTest(string TestName)
        {

        }
	}
}

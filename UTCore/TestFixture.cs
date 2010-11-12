using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UTCore
{
	public class TestFixture
	{
		private TestFixtureAttribute tfa=null;		// one per TF
		private SetUpAttribute sua=null;			// one per TF
		private TearDownAttribute tda=null;			// one per TF
		private ArrayList testList=null;			// many per TF

		public bool HasTestFixture
		{
			get
			{
				return tfa != null;
			}
		}

		public Assembly Assembly
		{
			get
			{
				return tfa.TestClass.Assembly;
			}
		}

		public string Namespace
		{
			get
			{
				return tfa.TestClass.Namespace;
			}
		}

		public ArrayList Tests
		{
			get
			{
				return testList;
			}
		}

		public int NumTests
		{
			get
			{
				return testList.Count;
			}
		}

		public TestFixture()
		{
			testList=new ArrayList();
		}

		public void AddTestFixtureAttribute(TestFixtureAttribute tfa)
		{
			this.tfa=tfa;
		}

		public void AddSetUpAttribute(SetUpAttribute sua)
		{
			if (this.sua==null)
			{
				this.sua=sua;
			}
			else
			{
				Trace.WriteLine("Multiple setup methods are not allowed.");
			}
		}

		public void AddTearDownAttribute(TearDownAttribute tda)
		{
			if (this.tda==null)
			{
				this.tda=tda;
			}
			else
			{
				Trace.WriteLine("Multiple teardown methods are not allowed.");
			}
		}

		public void AddTestAttribute(TestAttribute ta)
		{
			testList.Add(ta);
		}

		// *****
		public void RunTests(TestNotificationDelegate testNotificationEvent)
		{
			object instance=tfa.CreateClass();
			foreach (TestAttribute ta in testList)
			{
				if (!ta.IgnoreTest())
				{
					try
					{
						if (sua != null) sua.Invoke(instance);
						ta.Invoke(instance);
						// If we get here, the test did not throw an exception.
						// Was it supposed too?
						if (ta.ExpectedExceptionType != null)
						{
							Trace.WriteLine("***Fail***: "+ta.TestMethod.ToString()+" Expected exception not encountered");
							ta.State=TestAttribute.TestState.Fail;
						}
						else
						{
							Trace.WriteLine("***Pass***: "+ta.TestMethod.ToString());
							ta.State=TestAttribute.TestState.Pass;
						}
					}

					catch(AssertFailedException e)
					{
						Trace.WriteLine("***Fail***: "+ta.TestMethod.ToString()+" Exception="+e.Message);
						ta.State=TestAttribute.TestState.Fail;
					}

					catch(Exception e)
					{
						if (e.GetType() != ta.ExpectedExceptionType)
						{
							Trace.WriteLine("***Fail***: "+ta.TestMethod.ToString()+" Exception="+e.Message);
							ta.State=TestAttribute.TestState.Fail;
						}
						else
						{
							Trace.WriteLine("***Pass***: "+ta.TestMethod.ToString()+" Exception="+e.Message);
							ta.State=TestAttribute.TestState.Pass;
						}
					}
					finally
					{
						if (tda != null) tda.Invoke(instance);
					}
				}
				else
				{
					Trace.WriteLine("***Ignore***: "+ta.TestMethod.ToString());
					ta.State=TestAttribute.TestState.Ignore;
				}
                if (testNotificationEvent != null)
                {
                    testNotificationEvent(ta);
                }
			}
		}
	}
}

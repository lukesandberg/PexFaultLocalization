using System;
using Microsoft.Pex.Framework.Generated;
namespace UTCore
{
	public abstract class TestUnitAttribute
	{
		protected ClassItem ci;
		protected MethodItem mi;
		protected object attr;

		public ClassItem TestClass
		{
			get
			{
				return ci;
			}
		}

		public MethodItem TestMethod
		{
			get
			{
				return mi;
			}
		}

		public TestUnitAttribute()
		{
		}

		public abstract void SelfRegister(TestFixture tf);

		public void Initialize(ClassItem ci, MethodItem mi, object attr)
		{
			this.ci=ci;
			this.mi=mi;
			this.attr=attr;
		}

		public object CreateClass()
		{
			object obj=ci.Create();
			return obj;
		}

		public void Invoke(object classInstance)
		{
			mi.Invoke(classInstance);
		}

		public bool IgnoreTest()
		{
			return mi.Ignore==true;
		}

		public Type ExpectedExceptionType
		{
			get
			{
				if (mi.ExpectedException!=null && mi.ExpectedException.GetType() == typeof(PexRaisedExceptionAttribute))
				{

                    return ((PexRaisedExceptionAttribute)mi.ExpectedException).ExceptionType.GetType();
                    
				}
				else
				{
                    return null;
				}
			}
		}
	}

	public class TestFixtureAttribute : TestUnitAttribute
	{
		public TestFixtureAttribute()
		{
		}

		public override void SelfRegister(TestFixture tf)
		{
			tf.AddTestFixtureAttribute(this);
		}

	}

	public class TestAttribute : TestUnitAttribute
	{
		public enum TestState
		{
			Untested=0,
			Pass,
			Ignore,
			Fail,
		}

		TestState state;

		public TestState State
		{
			get
			{
				return state;
			}
			set
			{
				state=value;
			}
		}

		public TestAttribute()
		{
			state=TestState.Untested;
		}

		public override void SelfRegister(TestFixture tf)
		{
			tf.AddTestAttribute(this);
		}
	}

	public class SetUpAttribute : TestUnitAttribute
	{
		public SetUpAttribute()
		{
		}

		public override void SelfRegister(TestFixture tf)
		{
			tf.AddSetUpAttribute(this);
		}
	}

	public class TearDownAttribute : TestUnitAttribute
	{
		public TearDownAttribute()
		{
		}

		public override void SelfRegister(TestFixture tf)
		{
			tf.AddTearDownAttribute(this);
		}
	}

	public class ExpectedExceptionAttribute : TestUnitAttribute
	{
		public ExpectedExceptionAttribute()
		{
		}

		public override void SelfRegister(TestFixture tf)
		{
			mi.ExpectedException=(Attribute)attr;
		}
	}

	public class IgnoreAttribute : TestUnitAttribute
	{
		public IgnoreAttribute()
		{
		}

		public override void SelfRegister(TestFixture tf)
		{
			mi.Ignore=true;
		}
	}
}

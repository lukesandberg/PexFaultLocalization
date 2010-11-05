using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaultLocalization
{

	public interface IChain<T> : IEnumerable<T>
	{
		/// <summary>
		/// Get the next item in the chain
		/// </summary>
		IChain<T> Next { get; }

		/// <summary>
		/// Get the value of this Chain Link
		/// </summary>
		T Value { get; }
	}

	public static class Chains
	{
		public static IChain<T> Create<T>(IEnumerable<T> values)
		{
            // <pex>
            if (values == (IEnumerable<int>)null)
                throw new ArgumentNullException("values");
            // </pex>
			return Create<T>(values.GetEnumerator());
		}
		static IChain<T> Create<T>(IEnumerator<T> itr)
		{
			if(itr.MoveNext())
			{
				return new Chain<T>(Create<T>(itr), itr.Current);
			}
			return null;
		}

		public static IChain<T> CreateLazy<T>(IEnumerable<T> values)
		{
            // <pex>
            if (values == (IEnumerable<int>)null)
                throw new ArgumentNullException("values");
            // </pex>
			return CreateLazy<T>(values.GetEnumerator());
		}

		static IChain<T> CreateLazy<T>(IEnumerator<T> itr)
		{
			if(itr.MoveNext())
			{
				return new LazyChain<T>(itr.Current, itr);
			}
			return null;
		}
		private class LazyChain<T> : AbstractChain<T>
		{
			IChain<T> next;
			T value;
			IEnumerator<T> enumerator;

			public LazyChain(T value, IEnumerator<T> enumerator)
			{
				this.value = value;
				this.enumerator = enumerator;
			}
			
			public override IChain<T> Next
			{
				get
				{
					if(enumerator != null)
					{
						next = CreateLazy(enumerator);
						enumerator = null;
					}
					return next;
				}
			}

			public override T Value
			{
				get { return value; }
			}
		}
	}
	
	public abstract class AbstractChain<T> : IChain<T>
	{
		#region IChain<T> Members

		public abstract IChain<T> Next { get; }

		public abstract T Value { get; }

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			IChain<T> curr = this;
			while(curr != null)
			{
				yield return curr.Value;
				curr = curr.Next;
			}
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}

	/// <summary>
	/// Immutable Linked List implementation of the
	/// IChain interface
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Chain<T> : AbstractChain<T>
	{
		IChain<T> next;
		T value;
		
		public Chain(IChain<T> next, T value)
		{
			this.next = next;
			this.value = value;
		}

		public Chain(T value) : this(null, value)
		{ }

		public override IChain<T> Next
		{
			get { return next; }
		}

		public override T Value
		{
			get { return value; }
		}
	}
}

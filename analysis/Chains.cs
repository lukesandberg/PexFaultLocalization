using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaultLocalization
{
	interface IChain<T> : IEnumerable<T>
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
	abstract class AbstractChain<T> : IChain<T>
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

	public class LazyChain<T> : AbstractChain<T>
	{
		IChain<T> next;
		T value;
		IEnumerator<T> enumerator;

		LazyChain(T value, IEnumerator<T> enumerator)
		{
			this.value = value;
			this.enumerator = enumerator;
		}

		public static IChain<T> Create(IEnumerable<T> values)
		{
			return Create(values.GetEnumerator());
		}

		static IChain<T> Create(IEnumerator<T> itr)
		{
			if(itr.MoveNext())
			{
 				return new LazyChain<T>(itr.Current, itr);	
			}
			return null;
		}

		public override IChain<T> Next
		{
			get 
			{
				if(enumerator != null)
				{
					next = Create(enumerator);
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

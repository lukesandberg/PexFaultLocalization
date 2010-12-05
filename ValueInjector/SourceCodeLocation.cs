using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueInjector
{
	public sealed class SourceCodeLocation
	{
		private readonly String _FileName;
		private readonly int _StartLine;
		private readonly int _EndLine;
		private readonly int _StartColumn;
		private readonly int _EndColumn;
		public String FileName { get { return _FileName; } }
		public int StartLine { get { return _StartLine; } }
		public int EndLine { get { return _EndLine; } }
		public int StartColumn { get { return _StartColumn; } }
		public int EndColumn { get { return _EndColumn; } }

		public SourceCodeLocation(String fn, int sl, int el, int sc, int ec)
		{
			_FileName = fn;
			_StartLine = sl;
			_EndLine = el;
			_StartColumn = sc;
			_EndColumn = ec;
		}
		public override bool Equals(object obj)
		{
			if(obj == null)
				return false;
			if(!(obj is SourceCodeLocation))
				return false;
			var other = (SourceCodeLocation) obj;

			return StartLine == other.StartLine
				&& EndLine == other.EndLine
				&& StartColumn == other.StartColumn
				&& EndColumn == other.EndColumn
				&& String.Equals(FileName, other.FileName);
		}
		private int _hash_code;
		private bool calcHashCode = true;
		public override int GetHashCode()
		{
			if(calcHashCode)
			{
				int prime = 31;
				_hash_code = 1;
				_hash_code = prime * _hash_code + ((FileName == null) ? 0 : FileName.GetHashCode());
				_hash_code = prime * _hash_code + StartLine;
				_hash_code = prime * _hash_code + EndLine;
				_hash_code = prime * _hash_code + StartColumn;
				_hash_code = prime * _hash_code + EndColumn;
				calcHashCode = false;
			}
			return _hash_code;
		}

		public bool Contains(SourceCodeLocation other)
		{
			if(!String.Equals(FileName, other.FileName))
				return false;
			var startsBefore = (StartLine < other.StartLine || (StartLine == other.StartLine && StartColumn <= other.StartColumn));
			var endsAfter = (EndLine > other.EndLine || (EndLine == other.EndLine && EndColumn >= other.EndColumn));
			return startsBefore && endsAfter;
		}

		public override string ToString()
		{
			return FileName + "(" + StartLine + ", " + StartColumn + ") -> (" + EndLine + ", " + EndColumn + ")"; 
		}
	}
}

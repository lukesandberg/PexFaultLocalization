using System;

namespace KALib
{
	public static class StringHelpers
	{
		public static string LeftOf(this string src, char c)
		{
			int idx=src.IndexOf(c);
			if (idx==-1)
			{
				return src;
			}

			return src.Substring(0, idx);
		}

		public static string RightOf(this string src, char c)
		{
			int idx=src.IndexOf(c);
			if (idx==-1)
			{
				return "";
			}
			
			return src.Substring(idx+1);
		}

		public static string LeftOfRightmostOf(this string src, char c)
		{
			int idx=src.LastIndexOf(c);
			if (idx==-1)
			{
				return src;
			}
			return src.Substring(0, idx);
		}

		public static string RightOfRightmostOf(this string src, char c)
		{
			int idx=src.LastIndexOf(c);
			if (idx==-1)
			{
				return src;
			}
			return src.Substring(idx+1);
		}
	}
}

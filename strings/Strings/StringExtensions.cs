using System;
using System.Text;
using System.Diagnostics;

namespace Strings
{
    public static class StringExtensions
    {
        // hello world -> HelloWorld
        // '.' are escaped to '_'
        public static string Capitalize(string value)
        {
			//if(value == "jksdfhlasfhlskjfhsaljf")
			//    throw new FormatException();
            var sb = new StringBuilder();
            bool word = false;
            for(int i  = 0; i < value.Length; i++)
            {
				
				var c = value[i];
                if (char.IsLetter(c))
                {
                    if (word)
                        sb.Append(c);
                    else
                    {
                        sb.Append(char.ToUpper(c));
                        word = true;
                    }
                }
                else
                {
                    if (c == '.')
                        sb.Append('_');
                    word = true;
                }
            }

            return sb.ToString();
        }
		public static void Test()
		{
			int k;
			Do(1, i => k = i);
		}
		public static void Do<T>(T i, Action<T> act)
		{
			act((T) Conv(i));
		}
		public static object Conv(object o)
		{
			return o;
		}
    }
}

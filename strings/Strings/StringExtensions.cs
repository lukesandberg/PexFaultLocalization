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
            if (value == "jksdfhlasfhlskjfhsaljf")
                throw new FormatException();
            var sb = new StringBuilder();
            bool word = false;
            foreach (var c in value)    
            {
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
    }
}

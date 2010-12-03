using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Coverage.Analysis;
using System.Diagnostics;
using System.IO;
namespace FaultLocalization
{
	public static class Extensions
	{
		/// <summary>
		/// Generates a String consisting of two lines before and two lines after
		/// the given line in the source file.  All covered lines are proceeded
		/// with '>>'
		/// </summary>
		/// <param name="ds">The Coverage Data Set that this line is based in</param>
		/// <param name="line">The line we are interested in</param>
		/// <returns>The source code around the given line</returns>
		public static String GetCoveredLine(this CoverageDS ds, CoverageDS.LinesRow line)
		{
            // <pex>
            if (line == (CoverageDSPriv.LinesRow) null)
                throw new ArgumentNullException("line");
            // </pex>
			uint file_id = line.SourceFileID;
			String filename = ds.SourceFileNames.Where(fn => fn.SourceFileID == file_id).Select(f => f.SourceFileName).First();
			using(var reader = File.OpenText(filename))
			{
				uint ln = 1;
				uint start = Math.Max(1, line.LnStart -2);
				StringBuilder rval = new StringBuilder();
				foreach(var code in reader.Lines())
				{
					if(ln >=start)
					{
						var output = code;
						if(ln >= line.LnStart && ln <= line.LnEnd)
						{
							if(output.StartsWith("  "))
								output = output.Substring(2);
							if(output.StartsWith("\t"))
								output = "  " + output.Substring(2);
							output = ">>" + output;
						}
						rval.AppendLine(output);
					}
					if(ln > line.LnEnd + 2)
					{
						break;
					}
					ln++;
				}
				return rval.ToString();
			}
		}
		/// <summary>
		/// Enumerates the lines of the given file
		/// </summary>
		/// <param name="reader">The Stream to read</param>
		/// <returns>The lines in the file or resource</returns>
		public static IEnumerable<String> Lines(this TextReader reader)
		{
			String line;
			while((line = reader.ReadLine()) != null)
			{
				yield return line;
			}
		}


        /// <summary> 
        /// Returns all types in the current AppDomain implementing the interface or inheriting the type.  
        /// </summary> 
        /// <param name="onlyRealTypes">if true, only real, instantiable types will be returned</param>
        public static IEnumerable<Type> TypesImplementingInterface(this Type interfaceType, bool onlyRealTypes)
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => interfaceType.IsAssignableFrom(type) && (!onlyRealTypes || type.IsRealClass()));
        }


        /// <param name="testType">The type to test</param>
        /// <returns>true if this type is non-abstract, non-generic, and non-interface</returns>
        public static bool IsRealClass(this Type testType)
        {
            return testType.IsAbstract == false
                && testType.IsGenericTypeDefinition == false
                && testType.IsInterface == false;
        }


        public static int SumNoOverflow<T>(this IEnumerable<T> list, Func<T, int> func)
        {
            long sum = 0;
            foreach (T t in list)
            {
                try
                {
                    sum += func(t);
                }
                catch (OverflowException)
                {
                    sum = 0;
                }
            }
            return (int)(sum % int.MaxValue);
        }
	}

    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> equalityFunc;
        private Func<T, int> hashCodeFunc;

        public EqualityComparer(Func<T, T, bool> equalityFunc, Func<T, int> hashCodeFunc)
        {
            this.equalityFunc = equalityFunc;
            this.hashCodeFunc = hashCodeFunc;
        }

        public bool Equals(T t1, T t2)
        {
            return equalityFunc(t1, t2);
        }

        public int GetHashCode(T t)
        {
            return hashCodeFunc(t);
        }


    } 
}

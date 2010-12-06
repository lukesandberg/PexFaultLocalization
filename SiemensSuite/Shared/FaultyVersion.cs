using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Edu.Nlu.Sir.Siemens.Shared
{
    public enum FaultType
    {
        NO_FAULT,
        MISSING_CODE, 
        ADDED_CODE,
        NUMERIC_CHANGE,
        OPERATOR_CHANGE,
        IF_CONDITION_CHANGE,
        IF_OPERATOR_CHANGE,
        CONSTANT_VALUE_CHANGE,
        FORLOOP_PARAMETER_CHANGE,
        ARRAY_INDEX_CHANGE
    }

    public interface FaultyVersion
    {
        int[] FaultLines { get; }
        FaultType FaultType { get; }
    }

    public static partial class Extensions {
        public static bool ReadLine(this TextReader reader, out char[] result, int maxsize)
        {
            StringBuilder s = new StringBuilder();
            while (s.Length < maxsize && reader.Peek() != -1 && reader.Peek() != '\n')
            {
                s.Append(reader.Read());
            }
            if(reader.Peek() != -1) {
                s.Append(reader.Read());
            }
            if (s.Length == 0)
            {
                result = null;
                return false;
            }
            else
            {
                result = s.ToString().ToCharArray();
                return true;
            }
        }

        /// <summary> 
        /// Returns all types in the current AppDomain implementing the interface or inheriting the type.  
        /// </summary> 
        /// <param name="onlyRealTypes">if true, only real, instantiable types will be returned</param>
        public static IEnumerable<Type> TypesImplementingInterface(this Type interfaceType, bool onlyRealTypes)
        {
             var x = AppDomain
                .CurrentDomain
                .GetAssemblies();
             List<Type> types = new List<Type>();
             foreach (Assembly assembly in x)
             {
                 try
                 {
                     types.AddRange(assembly.GetTypes().AsEnumerable());
                 }
                 catch (Exception e)
                 {

                 }
             }
             return types.Where(type => interfaceType.IsAssignableFrom(type) && (!onlyRealTypes || type.IsRealClass()));
        }

        /// <param name="testType">The type to test</param>
        /// <returns>true if this type is non-abstract, non-generic, and non-interface</returns>
        public static bool IsRealClass(this Type testType)
        {
            return testType.IsAbstract == false
                && testType.IsGenericTypeDefinition == false
                && testType.IsInterface == false;
        }

    }
}

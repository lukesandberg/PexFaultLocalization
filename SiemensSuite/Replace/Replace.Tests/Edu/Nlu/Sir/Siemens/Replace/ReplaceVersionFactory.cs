using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edu.Nlu.Sir.Siemens.Replace;
using Edu.Nlu.Sir.Siemens.Shared;

namespace Edu.Nlu.Sir.Siemens.Replace
{
    internal class ReplaceVersionFactory
    {
        public static IReplace getReplaceVersion()
        {
            string VersionString = Environment.GetEnvironmentVariable("Version");
            int VersionNum;
            bool numeric = Int32.TryParse(VersionString, out VersionNum);
            if (numeric)
            {
                var x = typeof(IReplace).TypesImplementingInterface(true);
                x = x.Where(t => t.Name == ("Version" + VersionString));
                IReplace Version = (IReplace)x.First().GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
                return Version;
            }
            else
            {
                return new BaseVersion();
            }
        }

        

    }
}

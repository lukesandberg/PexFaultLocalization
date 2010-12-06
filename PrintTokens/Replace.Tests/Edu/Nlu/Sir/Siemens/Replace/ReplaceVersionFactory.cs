using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edu.Nlu.Sir.Siemens.Replace;

namespace Edu.Nlu.Sir.Siemens.Replace
{
    internal class ReplaceVersionFactory
    {
        public static IReplace getReplaceVersion()
        {
            return new BaseVersion();
        }

    }
}

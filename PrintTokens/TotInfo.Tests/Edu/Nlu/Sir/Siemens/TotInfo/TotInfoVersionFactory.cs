using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edu.Nlu.Sir.Siemens.TotInfo
{
    public partial class TotInfoVersionFactory
    {
        public static ITotInfo getTotInfoVersion()
        {
            return new BaseVersion();
        }
    }
}

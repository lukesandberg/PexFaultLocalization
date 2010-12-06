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
            string VersionString = System.Environment.GetEnvironmentVariable("Version");
            int VersionNum;
            bool numeric = Int32.TryParse(VersionString, out VersionNum);
            if (numeric)
            {
                string VersionName = "Edu.Nlu.Sir.Siemens.Replace.Version" + VersionString;
                ITotInfo Version = (ITotInfo)Type.GetType(VersionName).GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
                return Version;
            }
            else
            {
                return new BaseVersion();
            }
        }
    }
}

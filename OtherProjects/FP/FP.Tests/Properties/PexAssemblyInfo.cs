// <copyright file="PexAssemblyInfo.cs">Copyright ©  2008</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Moles;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;
using Microsoft.Pex.Linq;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "VisualStudioUnitTest")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("FP")]
[assembly: PexInstrumentAssembly("System.Data")]
[assembly: PexInstrumentAssembly("FP.Validation")]
[assembly: PexInstrumentAssembly("XunitExtensions")]
[assembly: PexInstrumentAssembly("System.Core")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Data")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "FP.Validation")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]

// Microsoft.Pex.Framework.Moles
[assembly: PexAssumeContractEnsuresFailureAtBehavedSurface]
[assembly: PexChooseAsBehavedCurrentBehavior]

// Microsoft.Pex.Linq
[assembly: PexLinqPackage]


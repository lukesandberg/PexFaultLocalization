// <copyright file="PexAssemblyInfo.cs" company="General Electric">Copyright © General Electric 2010</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Moles;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;
using Microsoft.Pex.Linq;
using System;
using Microsoft.Pex.Framework.Suppression;
using Microsoft.Pex.Engine.Symbols;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "VisualStudioUnitTest")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("TotInfo")]
[assembly: PexInstrumentAssembly("System.Core")]
[assembly: PexInstrumentAssembly("Shared")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Shared")]

// Microsoft.Pex.Framework.Moles
[assembly: PexAssumeContractEnsuresFailureAtBehavedSurface]
[assembly: PexChooseAsBehavedCurrentBehavior]

// Microsoft.Pex.Linq
[assembly: PexLinqPackage]

[assembly: PexInstrumentAssembly("Microsoft.Pex.Framework")]
[assembly: PexInstrumentAssembly("Microsoft.Pex.Framework")]
[assembly: PexInstrumentAssembly("Microsoft.Pex.Framework")]
[assembly: PexInstrumentType(typeof(Math))]
[assembly: PexInstrumentAssembly("Microsoft.Pex.Framework")]
[assembly: PexInstrumentType(typeof(Math))]
[assembly: PexInstrumentAssembly("Microsoft.Pex.Framework")]
[assembly: PexSuppressUninstrumentedMethodFromType(typeof(__TestabilityHelper))]
[assembly: PexInstrumentAssembly("Microsoft.Pex.Framework")]

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace FaultLocalization.Util
{
	public class CSProject
	{
		#region "Constants"
		private const string testFrameworkName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
		private const string testProjectGuid = "3AC096D0-A1C2-E12C-1390-A8335801FDAB";
		#endregion

		public String CSProj { get; private set; }
		protected String ProjDir
		{
			get
			{
				return Path.GetDirectoryName(CSProj);
			}
		}
		private string output_type;
		public String OutputType
		{
			get
			{
				if(output_type == null)
				{
					output_type = ProjectFile.Descendants(XmlNs + "PropertyGroup")
					.Where(n => !n.HasAttributes).Descendants(XmlNs + "OutputType").Single().Value;
				}
				return output_type;
			}
		}
		public String AssemblyLocation
		{
			get
			{
				return Path.Combine(ProjDir, OutputDirectory, AssemblyName+ (OutputType.Equals("Library")? ".dll" : ".exe"));
			}
		}

		private String outputDir;
		protected String OutputDirectory
		{
			get
			{
				if(outputDir == null)
					outputDir = ActiveConfigration.Descendants(XmlNs + "OutputPath").Single().Value;
				return outputDir;
			}
		}

		private String assemblyName;
		public String AssemblyName
		{
			get
			{
				if(assemblyName == null)
				{
					var query = ProjectFile.Descendants(XmlNs + "AssemblyName").Select(n => n.Value);
					assemblyName = query.First();
				}
				return assemblyName;
			}
		}
		private XElement _activeConfiguration;
		public XElement ActiveConfigration
		{
			get
			{
				if(_activeConfiguration == null)
				{
					_activeConfiguration = ProjectFile.Descendants(XmlNs + "PropertyGroup")
						.Where(n => n.Attribute("Condition") != null && n.Attribute("Condition").Value.Contains(Configuration + "|" + Platform))
						.Single();
				}
				return _activeConfiguration;
			}
		}
		private XElement _mainPropertyGroup;
		public XElement MainPropertyGroup
		{
			get
			{
				if(_mainPropertyGroup == null)
				{
					_mainPropertyGroup = ProjectFile.Descendants(XmlNs + "PropertyGroup").Where(n => !n.Attributes().Any()).Single();
				}
				return _mainPropertyGroup;
			}
		}
		private String _platform;
		public String Platform
		{
			get
			{
				if(_platform == null)
				{
					_platform = MainPropertyGroup.Descendants(XmlNs + "Platform").Single().Value;
				}
				return _platform;
			}
		}
		private String _configuration;
		public String Configuration
		{
			get
			{
				if(_configuration == null)
				{
					_configuration = MainPropertyGroup.Descendants(XmlNs + "Configuration").Single().Value;
				}
				return _configuration;
			}
		}

		private IChain<String> cachedAssemblyNames;
		public IEnumerable<String> ReferenceAssemblies
		{
			get
			{
				if(cachedAssemblyNames == null)
				{
					var query = ProjectFile.Descendants(XmlNs + "Reference").Select(n => n.Attribute("Include").Value);
					cachedAssemblyNames = Chains.CreateLazy(query);
				}
				return cachedAssemblyNames;
			}
		}

		private IChain<String> cachedFileNames;
		public IEnumerable<String> CSFiles
		{
			get
			{
				if(cachedFileNames == null)
				{
					var query = ProjectFile.Descendants(XmlNs + "Compile").Select(n => Path.Combine(ProjDir, n.Attribute("Include").Value));
					cachedFileNames = Chains.CreateLazy(query);
				}
				return cachedFileNames;
			}
		}

		public CSProject(String path)
		{
			CSProj = path;
		}

		private XDocument doc;
		protected XDocument ProjectFile
		{
			get
			{
				if(doc == null)
					doc = XDocument.Load(CSProj);
				return doc;
			}
		}
		private XNamespace _xmlns;
		protected XNamespace XmlNs
		{
			get
			{
				if(_xmlns == null)
					_xmlns = XNamespace.Get(ProjectFile.Elements().First().Attribute("xmlns").Value);
				return _xmlns;
			}
		}
	}
}

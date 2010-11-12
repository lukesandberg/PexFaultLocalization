using System;
using System.Xml;
using System.IO;
using System.Threading;

/*
	code originally from: http://www.codeproject.com/useritems/XmlRegistry.asp by Nadeem Ghias
		removed lazy write feature
		changed key creation from an indexer to a method
		some other minor cleanup
		changed XmlRegistryKey class to handle value as a get accessor
		removed extra unecessary constructors
		added default values for getters
*/	


namespace KALib
{
	public class XmlRegistryKey
	{
		private XmlElement Elem;
		private XmlRegistry Reg;

		public string Value
		{
			get
			{
				return Elem.Name;
			}
		}

		internal XmlRegistryKey(XmlElement e, XmlRegistry r)
		{
			Elem = e;
			Reg = r;
		}

		public void SetValue(string name, object val)
		{
			XmlAttribute at = Elem.Attributes[name];
			if (at == null)
			{
				Elem.Attributes.Append(at = Elem.OwnerDocument.CreateAttribute(name));
			}
			at.Value = val.ToString();
			Reg.Save();
		}

		public bool GetBooleanValue(string name)
		{
			return GetBooleanValue(name, false);
		}

		public bool GetBooleanValue(string name, bool defaultval)
		{
			XmlAttribute at = Elem.Attributes[name];
			bool ret=defaultval;
			if (at != null)
			{
				string val = at.Value.ToUpper();
				ret= val == "YES" || val == "TRUE";
			}
			return ret;
		}

		public string GetStringValue(string name)
		{
			return GetStringValue(name, "");
		}

		public string GetStringValue(string name, string defaultval)
		{
			XmlAttribute at = Elem.Attributes[name];
			string ret=defaultval;
			if (at != null)
			{
				ret=at.Value;
			}
			return ret;
		}

		public int GetIntValue(string name)
		{
			return GetIntValue(name, 0);
		}

		public int GetIntValue(string name, int defaultval)
		{
			XmlAttribute at = Elem.Attributes[name];
			int ret=defaultval;
			if (at != null)
			{
				ret= int.Parse(at.Value);
			}
			return ret;
		}


		public XmlRegistryKey[] GetSubKeys()
		{
			int keycount = Elem.ChildNodes.Count;
			XmlRegistryKey[] keys = new XmlRegistryKey[keycount];
			for (int i=0; i < keycount; i++)
			{
				keys[i] = new XmlRegistryKey((XmlElement)Elem.ChildNodes[i], Reg);
			}
			return keys;
		}


		public XmlRegistryKey GetSubKey(string path, bool createpath)
		{
			XmlElement e = Elem, parent = null;
			for (int len, start=0; start < path.Length; start += len+1)
			{
				len = path.IndexOf('/', start);
				if (len == -1)
				{
					len = path.Length;
				}
				len -= start;
				string node = path.Substring(start, len);
				parent = e;
				e = e[node];
				if (e == null)
				{
					if (createpath)
					{
						parent.AppendChild(e = Elem.OwnerDocument.CreateElement(node));
					}
					else
					{
						return null;
					}
				}
			}
			return new XmlRegistryKey(e, Reg);
		}
	}

	public class XmlRegistry
	{
		private FileInfo Fileinfo;
		private XmlDocument Doc = new XmlDocument();
		private const string MsgPrefix = "XmlRegistry:";

		public XmlRegistryKey RootKey
		{
			get
			{
				return new XmlRegistryKey(Doc.DocumentElement, this);
			}
		}

		public XmlRegistry(string filename) : this(filename, null, null)
		{
		}

		public XmlRegistry(string filename, string rootkeyname, string encoding)
		{
			Fileinfo = new FileInfo(filename);
			if (Fileinfo.Exists)
			{
				Doc.Load(Fileinfo.FullName);
				if (rootkeyname != null && Doc.DocumentElement.Name != rootkeyname)
				{
					string msg = string.Format("{0} Specified root node name '{1}' does not match root name '{2}' in loaded document.", MsgPrefix, rootkeyname, Doc.DocumentElement.Name);
					throw new Exception(msg);
				}
			}
			else
			{
				if (rootkeyname == null)
				{
					rootkeyname = "XmlRegistryRoot";
				}
				XmlDeclaration dec = Doc.CreateXmlDeclaration("1.0", encoding, null);
				Doc.AppendChild(dec);
				XmlElement root = Doc.CreateElement(rootkeyname);
				Doc.AppendChild(root);
			}
		}

		public void Save()
		{
			Doc.Save(Fileinfo.FullName);
		}
	}
}

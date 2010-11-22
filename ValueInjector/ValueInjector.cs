using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ValueInjector
{
	public static class ValueInjector
	{
		private static Dictionary<SourceCodeLocation, ValueProfile> profiles;

		public static String CurrentTestName { get; set; }
		[DefaultValue(true)]
		public static bool IsSaving { get; set; }

		private static SourceCodeLocation CurrentStatement;
		private static ValueMapping AlternateMapping;

		public static IEnumerable<ValueMapping> GetAlternateMappings(SourceCodeLocation stmt)
		{
			ValueProfile vp;
			if(!profiles.TryGetValue(stmt, out vp))
			{
				return Enumerable.Empty<ValueMapping>();
			}
			return vp.AlternateMappings(CurrentTestName);
		}

		public static void ApplyMapping(SourceCodeLocation stmt, ValueMapping vm)
		{
			IsSaving = false;
			CurrentStatement = stmt;
			AlternateMapping = vm;
		}

		static ValueInjector()
		{
			profiles = new Dictionary<SourceCodeLocation, ValueProfile>();
		}

		public static void SetCurrentTestName(String testName)
		{
			CurrentTestName = testName;
		}

		private static void SaveValue(SourceCodeLocation stmt, int key, Type t, object value)
		{
			ValueProfile profile;
			if(!profiles.TryGetValue(stmt, out profile))
			{
				profile = new ValueProfile();
				profiles[stmt] = profile;
			}

			profile.AddValue(CurrentTestName, key, new Value{Type = t, Val = value});
		}


		public static object Instrument(object value, String FileName, int lineStart, int lineEnd, int colStart, int colEnd, int id)
		{
			SourceCodeLocation location = new SourceCodeLocation(FileName, lineStart, lineEnd, colStart, colEnd);
			if(IsSaving || !CurrentStatement.Contains(location))
			{
				if(IsSaving)
					SaveValue(location, id, value.GetType(), value);
				return value;
			}
			else
			{
				Value saved;
				if(!AlternateMapping.TryGetValue(id, out saved))
				{
					return value;
				}
				else
				{
					return saved.Val;
				}
			}
		}
	}
}

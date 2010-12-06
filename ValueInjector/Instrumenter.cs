using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ValueInjector
{
	public static class Instrumenter
	{
		private static Dictionary<SourceCodeLocation, ValueProfile> profiles;
		private static Dictionary<SourceCodeLocation, uint> counts;
		public static String CurrentTestName { get; set; }
		[DefaultValue(true)]
		public static bool IsSaving { get; set; }

		private static SourceCodeLocation CurrentStatement;
		private static ValueMapping AlternateMapping;

		public static IEnumerable<SourceCodeLocation> AllStatements
		{
			get
			{
				return profiles.Select(kvp => kvp.Key);
			}
		}

		public static IEnumerable<SourceCodeLocation> CurrentTestStatements
		{
			get
			{
				return profiles.Where(kvp => kvp.Value.TestsCovered.Contains(CurrentTestName)).Select(kvp => kvp.Key);
			}
		}

		public static IEnumerable<ValueMapping> GetAlternateMappings(SourceCodeLocation stmt)
		{
			ValueProfile vp;
			if(!profiles.TryGetValue(stmt, out vp))
			{
				return Enumerable.Empty<ValueMapping>();
			}
			return vp.AlternateMappings(CurrentTestName);
		}

		public static ValueMapping GetCurrentMapping(SourceCodeLocation stmt)
		{
			ValueProfile vp;
			if(!profiles.TryGetValue(stmt, out vp))
			{
				return null;
			}
			return vp.GetMapping(CurrentTestName);
		}
		public static void ApplyMapping(SourceCodeLocation stmt, ValueMapping vm)
		{
			IsSaving = false;
			CurrentStatement = stmt;
			AlternateMapping = vm;
			counts = new Dictionary<SourceCodeLocation, uint>();
		}

		static Instrumenter()
		{
			profiles = new Dictionary<SourceCodeLocation, ValueProfile>();
			counts = new Dictionary<SourceCodeLocation, uint>();
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
			if(counts.ContainsKey(location))
			{
				counts[location]++;
			}
			else
			{
				counts[location] = 1;
			}

			if(counts[location] > 100000)
			{
				return value;
			}
			try
			{
				if(IsSaving || !CurrentStatement.Contains(location))
				{
					if(IsSaving)
						SaveValue(location, id, value == null ? null : value.GetType(), value);
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
			catch(Exception)
			{
				//if anything goes wrong just return the value
				return value;
			}
		}
	}
}

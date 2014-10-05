using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Magnetite
{
	public class Config
	{
		public static IniParser MagnetiteConfig;

		public static void Init()
		{
			string ConfigPath = Path.Combine(Util.GetPublicFolder(), "Magnetite.cfg");

			if (File.Exists(ConfigPath))
			{
				MagnetiteConfig = new IniParser(ConfigPath);
				Debug.Log("Config " + ConfigPath + " loaded!");
			}
			else
			{
				Debug.Log("Config " + ConfigPath + " NOT loaded!");
			}
		}

		public static string GetValue(string Section, string Setting)
		{
			return MagnetiteConfig.GetSetting(Section, Setting);
		}

		public static bool GetBoolValue(string Section, string Setting)
		{
			var val = MagnetiteConfig.GetSetting(Section, Setting);
			return val != null && val.ToLower() == "true";
		}
	}
}


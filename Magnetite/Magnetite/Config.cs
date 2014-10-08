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

		public static string GetValue(string section, string setting)
		{
			return MagnetiteConfig.GetSetting(section, setting);
		}

		public static bool GetBoolValue(string section, string setting)
		{
			var val = MagnetiteConfig.GetSetting(section, setting);
			return val != null && val.ToLower() == "true";
		}

		public IniParser ini;

		public Config(string path)
		{
			if (!File.Exists(path))
			{
				File.Create(path);
			}
			ini = new IniParser(path);
			Debug.Log("Config " + path + " loaded!");
		}

		public string Get(string section, string setting)
		{
			return ini.GetSetting(section, setting);
		}

		public bool GetBool(string section, string setting)
		{
			var val = ini.GetSetting(section, setting);
			return val != null && val.ToLower() == "true";
		}

		public int GetInt(string section, string setting)
		{
			var val = ini.GetSetting(section, setting);
			return val != null ? int.Parse(val) : 0;
		}
	}
}


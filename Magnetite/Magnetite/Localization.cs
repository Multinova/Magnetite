using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Magnetite
{
	public class Localization
	{
		public readonly IniParser ini;

		public static string default_lang = "french";

		public static DataStore data;

		private static Localization _instance;

		public static string GetLang(Player player)
		{
			return data.datastore.Contains(player.SteamID) ? (string)data.datastore[player.SteamID] : default_lang;
		}

		public static void SetLang(Player player, string lang)
		{
			if (data.datastore.Contains(player.SteamID))
			{
				data.datastore[player.SteamID] = lang;
			}
			else
			{
				data.datastore.Add(player.SteamID, lang);
			}
			data.Save();
		}

		public static void Init()
		{
			data = new DataStore("languages.ds");
		}

		public static Localization GetInstance()
		{
			if (_instance == null)
			{
				//_instance = new Localization();
			}
			return _instance;
		}
		
		public Localization(string path)
		{
			if (!File.Exists(path))
			{
				File.Create(path);
			}
			ini = new IniParser(path);
		}
		/*
		public void Load(string path)
		{
			if (!File.Exists(path))
			{
				return;
			}
			IniParser ini = new IniParser(path);
		}

		private readonly Hashtable keyPairs = new Hashtable();

		private readonly List<IniParser.SectionPair> tmpList = new List<IniParser.SectionPair>();*/

		#region Lang

		public string lang(string language, string setting)
		{
			string value = ini.GetSetting(language, setting);
			return value != null ? value : setting;
		}

		public string lang(string language, string setting, object arg0)
		{
			string value = lang(language, setting);
			return string.Format(value, arg0);
		}

		public string lang(string language, string setting, object arg0, object arg1)
		{
			string value = lang(language, setting);
			return string.Format(value, arg0, arg1);
		}

		public string lang(string language, string setting, object arg0, object arg1, object arg2)
		{
			string value = lang(language, setting);
			return string.Format(value, arg0, arg1, arg2);
		}

		public string lang(string language, string setting, object[] format)
		{
			string value = lang(language, setting);
			return string.Format(value, format);
		}

		#endregion

		#region Localization

		public string _(string setting)
		{
			string value = ini.GetSetting(default_lang, setting);
			return value != null ? value : setting;
		}

		public string _(string setting, object arg0)
		{
			string value = _(setting);
			return string.Format(value, arg0);
		}

		public string _(string setting, object arg0, object arg1)
		{
			string value = _(setting);
			return string.Format(value, arg0, arg1);
		}

		public string _(string setting, object arg0, object arg1, object arg2)
		{
			string value = _(setting);
			return string.Format(value, arg0, arg1, arg2);
		}

		public string _(string setting, object[] format)
		{
			string value = _(setting);
			return string.Format(value, format);
		}

		#endregion

		#region Player

		public string _(Player player, string setting)
		{
			string value = ini.GetSetting(GetLang(player), setting);
			return value != null ? value : _(setting);
		}

		public string _(Player player, string setting, object arg0)
		{
			string value = _(player, setting);
			return string.Format(value, arg0);
		}

		public string _(Player player, string setting, object arg0, object arg1)
		{
			string value = _(player, setting);
			return string.Format(value, arg0, arg1);
		}

		public string _(Player player, string setting, object arg0, object arg1, object arg2)
		{
			string value = _(player, setting);
			return string.Format(value, arg0, arg1, arg2);
		}

		public string _(Player player, string setting, object[] format)
		{
			string value = _(player, setting);
			return string.Format(value, format);
		}

		#endregion

		#region Message

		public void Message(Player player, string setting)
		{
			player.Message(_(player, setting));
		}

		public void Message(Player player, string setting, object arg0)
		{
			player.Message(_(player, setting, arg0));
		}

		public void Message(Player player, string setting, object arg0, object arg1)
		{
			player.Message(_(player, setting, arg0, arg1));
		}

		public void Message(Player player, string setting, object arg0, object arg1, object arg2)
		{
			player.Message(_(player, setting, arg0, arg1, arg2));
		}

		public void Message(Player player, string setting, object[] format)
		{
			player.Message(_(player, setting, format));
		}

		#endregion

		#region Broadcast

		public void Broadcast(string setting)
		{
			Server server = Server.GetServer();
			Hashtable localized = new Hashtable();
			foreach (Player player in server.ActivePlayers)
			{
				string language = GetLang(player);
				if (!localized.Contains(language))
				{
					localized.Add(language, lang(language, setting));
				}
				player.Message((string)localized[language]);
			}
		}

		public void Broadcast(string setting, object arg0)
		{
			Server server = Server.GetServer();
			foreach (Player player in server.ActivePlayers)
			{
				player.Message(_(player, setting, arg0));
			}
		}

		public void Broadcast(string setting, object arg0, object arg1)
		{
			Server server = Server.GetServer();
			foreach (Player player in server.ActivePlayers)
			{
				player.Message(_(player, setting, arg0, arg1));
			}
		}

		public void Broadcast(string setting, object arg0, object arg1, object arg2)
		{
			Server server = Server.GetServer();
			foreach (Player player in server.ActivePlayers)
			{
				player.Message(_(player, setting, arg0, arg1, arg2));
			}
		}

		public void Broadcast(string setting, object[] format)
		{
			Server server = Server.GetServer();
			foreach (Player player in server.ActivePlayers)
			{
				player.Message(_(player, setting, format));
			}
		}

		#endregion
	}
}

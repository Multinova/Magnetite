using System;
using System.IO;
using UnityEngine;

namespace Magnetite
{
	public class Bootstrap : MonoBehaviour
	{

		public static string Version = "0.9.2";

		public static void AttachBootstrap()
		{
			Logger.Init();
			try
			{
				if (!magnetite.enabled)
				{
					return;
				}
				Init();
				Logger.Log("[Logger] Magnetite Loaded!");
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				Debug.Log("[Bootstarp] Error while loading Magnetite!");
			}
		}

		public static void SaveAll()
		{
			try
			{
				Server.GetServer().OnShutdown();
				DataStore.GetInstance().Save();
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}

		public static void Init()
		{
			if (!Directory.Exists(Util.GetPublicFolder()))
				Directory.CreateDirectory(Util.GetPublicFolder());

			Config.Init();
			Server.GetServer();
			ModuleManager.LoadModules();

			server.official = false;

			/*
			if (!server.hostname.ToLower().Contains("magnetite"))
				server.hostname = String.Format("{0} [Magnetite v.{1}]", server.hostname, Version);
			*/
		}
	}
}

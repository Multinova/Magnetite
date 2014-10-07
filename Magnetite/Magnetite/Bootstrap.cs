using System;
using System.IO;
using UnityEngine;

namespace Magnetite
{
	public class Bootstrap : MonoBehaviour
	{
		public static string Version = "0.9.2";

		public static bool loaded = false;

		public static void AttachBootstrap()
		{
			if (loaded)
			{
				return;
			}
			loaded = true;
			Config.Init();
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
			{
				Directory.CreateDirectory(Util.GetPublicFolder());
			}

			//AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			
			Server.GetServer();
			ModuleManager.LoadModules();

			server.official = false;
		}

		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			Logger.Log("UnhandledExceptionEventArgs");
			if (args == null || args.ExceptionObject == null)
			{
				return;
			}
			Logger.LogException((Exception)args.ExceptionObject);
		}
	}
}

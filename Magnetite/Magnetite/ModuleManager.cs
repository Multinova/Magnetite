using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Magnetite
{
	public class ModuleManager
	{
		//
		// Static Fields
		//
		public static string PublicFolder = Util.GetPublicFolder ();

		public static readonly List<ModuleContainer> Modules = new List<ModuleContainer> ();

		public static readonly Version ApiVersion = new Version (1, 0, 0, 0);

		public static string ModulesFolder = Path.Combine (Util.GetPublicFolder (), "Plugins");

		public static Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly> ();

		//
		// Static Properties
		//
		public static ReadOnlyCollection<ModuleContainer> Plugins {
			get {
				return new ReadOnlyCollection<ModuleContainer> (ModuleManager.Modules);
			}
		}

		//
		// Constructors
		//
		static ModuleManager ()
		{
			// Note: this type is marked as 'beforefieldinit'.
			ModuleManager.LoadedAssemblies = new Dictionary<string, Assembly> ();
			ModuleManager.Modules = new List<ModuleContainer> ();
		}

		//
		// Static Methods
		//
		public static void LoadModules()
		{
			Logger.Log ("[Modules] Loading modules...");
			string path = Path.Combine (ModuleManager.ModulesFolder, "ignoredmodules.txt");
			List<string> list = new List<string> ();
			if (File.Exists (path)) {
				list.AddRange (File.ReadAllLines (path));
			}
			DirectoryInfo[] directories = new DirectoryInfo (ModuleManager.ModulesFolder).GetDirectories ();
			DirectoryInfo[] array = directories;
			for (int i = 0; i < array.Length; i++) {
				DirectoryInfo directoryInfo = array [i];
				FileInfo fileInfo = new FileInfo (Path.Combine (directoryInfo.FullName, directoryInfo.Name + ".dll"));
				if (fileInfo.Exists)
				{
					Logger.LogDebug("[Modules] Module Found: " + fileInfo.Name, null);
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
					if (list.Contains(fileNameWithoutExtension))
					{
						Logger.LogWarning(string.Format("[Modules] {0} was ignored from being loaded.", fileNameWithoutExtension), null);
					}
					else
					{
						try
						{
							Logger.LogDebug("[Modules] Loading assembly: " + fileInfo.Name, null);
							Assembly assembly;
							if (!ModuleManager.LoadedAssemblies.TryGetValue(fileNameWithoutExtension, out assembly))
							{
								assembly = Assembly.Load(File.ReadAllBytes(fileInfo.FullName));
								ModuleManager.LoadedAssemblies.Add(fileNameWithoutExtension, assembly);
							}
							Type[] exportedTypes = assembly.GetExportedTypes();
							for (int j = 0; j < exportedTypes.Length; j++)
							{
								Type type = exportedTypes[j];
								if (type.IsSubclassOf(typeof(Module)) && type.IsPublic && !type.IsAbstract)
								{
									Logger.LogDebug("[Modules] Checked " + type.FullName, null);
									Module module = null;
									try
									{
										module = (Module)Activator.CreateInstance(type);
										Logger.LogDebug("[Modules] Instance created: " + type.FullName, null);
									}
									catch (Exception arg)
									{
										Logger.LogError(string.Format("[Modules] Could not create an instance of plugin class \"{0}\". {1}", type.FullName, arg), null);
									}
									if (module != null)
									{
										ModuleContainer moduleContainer = new ModuleContainer(module);
										moduleContainer.Plugin.ModuleFolder = Path.Combine(ModuleManager.ModulesFolder, directoryInfo.Name);
										ModuleManager.Modules.Add(moduleContainer);
										Logger.LogDebug("[Modules] Module added: " + fileInfo.Name, null);
										break;
									}
								}
							}
						}
						catch (Exception arg2)
						{
							Logger.LogError(string.Format("[Modules] Failed to load assembly \"{0}\". {1}", fileInfo.Name, arg2), null);
						}
					}
				}
			}
			IOrderedEnumerable<ModuleContainer> orderedEnumerable = from x in ModuleManager.Plugins
			orderby x.Plugin.Order, x.Plugin.Name
			select x;
			foreach (ModuleContainer current in orderedEnumerable) {
				try {
					current.Initialize ();
				}
				catch (Exception arg3) {
					Logger.LogError (string.Format ("[Modules] Module \"{0}\" has thrown an exception during initialization. {1}", current.Plugin.Name, arg3), null);
				}
				Logger.Log (string.Format ("[Modules] Module {0} v{1} (by {2}) initiated.", current.Plugin.Name, current.Plugin.Version, current.Plugin.Author), null);
			}
			//Hooks.ModulesLoaded ();
		}

		public static void ReloadModules()
		{
			ModuleManager.UnloadModules ();
			ModuleManager.LoadModules ();
		}

		public static void UnloadModules()
		{
			foreach (ModuleContainer current in ModuleManager.Modules) {
				try {
					current.DeInitialize ();
				}
				catch (Exception arg) {
					Logger.LogError (string.Format ("[Modules] Module \"{0}\" has thrown an exception while being deinitialized: {1}", current.Plugin.Name, arg), null);
				}
			}
			foreach (ModuleContainer current2 in ModuleManager.Modules) {
				try {
					current2.Dispose ();
				}
				catch (Exception arg2) {
					Logger.LogError (string.Format ("[Modules] Module \"{0}\" has thrown an exception while being disposed: {1}", current2.Plugin.Name, arg2), null);
				}
			}
			ModuleManager.Modules.Clear ();
			Logger.LogDebug ("All modules unloaded!", null);
		}
	}
}

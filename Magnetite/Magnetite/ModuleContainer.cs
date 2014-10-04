using System;

namespace Magnetite
{
	public class ModuleContainer : IDisposable
	{
		//
		// Fields
		//
		public readonly Module Plugin;

		//
		// Properties
		//
		public bool Dll
		{
			get;
			set;
		}

		public bool Initialized
		{
			get;
			protected set;
		}

		//
		// Constructors
		//
		public ModuleContainer(Module plugin, bool dll)
		{
			this.Plugin = plugin;
			this.Initialized = false;
			this.Dll = dll;
		}

		public ModuleContainer(Module plugin)
			: this(plugin, true)
		{
		}

		//
		// Methods
		//
		public void DeInitialize()
		{
			this.Initialized = false;
			this.Plugin.DeInitialize();
		}

		public void Dispose()
		{
			this.Plugin.Dispose();
		}

		public void Initialize()
		{
			this.Plugin.Initialize();
			this.Initialized = true;
		}

		private void Invariant()
		{
		}
	}
}

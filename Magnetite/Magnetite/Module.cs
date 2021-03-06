﻿using System;
using System.IO;

namespace Magnetite
{
	public abstract class Module : IDisposable
	{
		//
		// Properties
		//
		public virtual string Author {
			get {
				return "None";
			}
		}

		public virtual string Description {
			get {
				return "None";
			}
		}

		public virtual string Help {
			get {
				return "";
			}
		}

		public virtual bool Enabled {
			get;
			set;
		}

		public virtual string ModuleFolder {
			get;
			set;
		}

		public virtual string Name {
			get {
				return "None";
			}
		}

		public int Order {
			get;
			set;
		}

		public virtual string UpdateURL {
			get {
				return "";
			}
		}

		public virtual Version Version {
			get {
				return new Version(1, 0);
			}
		}

		//
		// Constructors
		//
		~Module()
		{
			this.Dispose(false);
		}

		//
		// Methods
		//
		public virtual void DeInitialize()
		{
		}

		private Localization _local;

		public Localization Local {
			get {
				if (_local == null)
				{
					_local = new Localization(Path.Combine(this.ModuleFolder, "locals.cfg"));
				}
				return _local;
			}
		}

		protected DataStore DataStore(string path)
		{
			return new DataStore(Path.Combine(this.ModuleFolder, path));
		}

		protected Localization Localization(string path)
		{
			return new Localization(Path.Combine(this.ModuleFolder, path));
		}

		protected Config Config(string path)
		{
			return new Config(Path.Combine(this.ModuleFolder, path));
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public abstract void Initialize();
	}
}

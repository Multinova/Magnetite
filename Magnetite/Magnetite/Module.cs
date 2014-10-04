﻿using System;

namespace Magnetite
{
	public abstract class Module : IDisposable
	{
		//
		// Properties
		//
		public virtual string Author
		{
			get
			{
				return "None";
			}
		}

		public virtual string Description
		{
			get
			{
				return "None";
			}
		}

		public virtual bool Enabled
		{
			get;
			set;
		}

		public virtual string ModuleFolder
		{
			get;
			set;
		}

		public virtual string Name
		{
			get
			{
				return "None";
			}
		}

		public int Order
		{
			get;
			set;
		}

		public virtual string UpdateURL
		{
			get
			{
				return "";
			}
		}

		public virtual Version Version
		{
			get
			{
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
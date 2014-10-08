using System;
using Magnetite;

namespace History
{
	public class History : Module
	{
		public override string Name {
			get {
				return "History";
			}
		}

		public override string Author {
			get {
				return "Yohann Seris";
			}
		}

		public override Version Version {
			get {
				return new Version(0, 1, 0, 0);
			}
		}

		public override void Initialize()
		{

		}

		public override void DeInitialize()
		{

		}
    }
}

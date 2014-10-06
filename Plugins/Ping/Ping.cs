using System;
using Magnetite;

namespace Ping
{
	public class Ping : Module
	{
		public override string Name {
			get {
				return "Ping";
			}
		}

		public override string Author {
			get {
				return "Yohann Seris";
			}
		}

		public override string Help {
			get {
				return "/ping (Showyour ping)";
			}
		}

		public override Version Version {
			get {
				return new Version(0, 1, 0, 0);
			}
		}

		public override void Initialize()
		{
			Hooks.OnCommand += Hooks_OnCommand;
		}

		public override void DeInitialize()
		{
			Hooks.OnCommand -= Hooks_OnCommand;
		}

		void Hooks_OnCommand(Command command)
		{
			if (command.cmd == "ping")
			{
				command.User.Message("Your ping is " + command.User.Ping);
			}
		}
	}
}

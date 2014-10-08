using System;
using Magnetite;

namespace Suicide
{
    public class Suicide : Module
	{
		public override string Name {
			get {
				return "Suicide";
			}
		}

		public override string Author {
			get {
				return "John Smith";
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
			if (command.cmd == "suicide")
			{
				Player player = command.User;
				player.Kill();
			}
		}
    }
}

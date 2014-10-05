using System;
using Magnetite;

namespace Location
{
    public class Location : Module
    {
		public override string Name {
			get {
				return "Location";
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
			Hooks.OnCommand += Hooks_OnCommand;
		}

		public override void DeInitialize()
		{
			Hooks.OnCommand -= Hooks_OnCommand;
		}

		void Hooks_OnCommand(Command command)
		{
			if (command.cmd == "location")
			{
				Player player = command.User;
				if (command.quotedArgs.Length == 1 && command.User.Admin)
				{
					Player target = Player.Find(command.quotedArgs[0]);
					if (target != null)
					{
						player.Message(command.User.Location.ToString());
					}
					else
					{
						player.Message("No players found with that name!");
					}
				}
				else
				{
					player.Message(command.User.Location.ToString());
				}
			}
		}
    }
}

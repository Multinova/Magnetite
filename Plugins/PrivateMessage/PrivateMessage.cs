using System;
using Magnetite;

namespace PrivateMessage
{
    public class PrivateMessage : Module
	{
		public override string Name {
			get {
				return "PrivateMessage";
			}
		}

		public override string Author {
			get {
				return "Yohann Seris";
			}
		}

		public override string Help {
			get {
				return "/pm <player> <message to send> (Send a private message)";
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
			if (command.cmd == "pm")
			{
				Player player = command.User;
				if (command.quotedArgs.Length < 2)
				{
					int founds;
					Player target = Player.FindByName(command.quotedArgs[0], out founds);
					if (target != null)
					{
						string[] message = new string[command.quotedArgs.Length - 1];
						Array.Copy(command.quotedArgs, 1, message, 0, command.quotedArgs.Length - 1);

						target.MessageFrom("[PM] " + player.Name, String.Join(" ", message));
						player.MessageFrom("[PM] " + player.Name, String.Join(" ", message));
					}
					else if (founds == 0)
					{
						player.Message("No players found with that name!");
					}
					else
					{
						player.Message("Multiple players found with that name!");
					}
				}
				else
				{
					player.Message("Wrong number of arguments.");
				}
			}
		}
    }
}

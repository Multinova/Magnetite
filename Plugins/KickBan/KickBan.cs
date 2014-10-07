using System;
using Magnetite;

namespace KickBan
{
	public class KickBan : Module
	{
		public override string Name {
			get {
				return "KickBan";
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
			Player player = command.User;
			if (command.cmd == "kick")
			{
				if (player.Moderator || player.Owner || player.Admin)
				{
					if (command.quotedArgs.Length > 0)
					{
						int founds;
						Player target = Player.FindByName(command.quotedArgs[0], out founds);
						if (target != null)
						{
							if (target.Owner || target.Admin || target.Moderator)
							{
								player.Message("You can't ban this player!");
								return;
							}
							string[] reason = new string[command.quotedArgs.Length - 1];
							Array.Copy(command.quotedArgs, 1, reason, 0, command.quotedArgs.Length - 1);

							Server.GetServer().Broadcast(target.Name + " has been kicked by " + player.Name);

							target.Kick(String.Join(" ", reason));
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
				else
				{
					player.Message("You don't have permission to use this command!");
				}
			}
			else if (command.cmd == "ban")
			{
				if (player.Moderator || player.Owner || player.Admin)
				{
					if (command.quotedArgs.Length > 0)
					{
						int founds;
						Player target = Player.FindByName(command.quotedArgs[0], out founds);
						if (target != null)
						{
							if (target.Owner || target.Admin || target.Moderator)
							{
								player.Message("You can't ban this player!");
								return;
							}
							string[] reason = new string[command.quotedArgs.Length - 1];
							Array.Copy(command.quotedArgs, 1, reason, 0, command.quotedArgs.Length - 1);

							Server.GetServer().Broadcast(target.Name + " has been banned by " + player.Name);

							target.Ban(String.Join(" ", reason));
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
				else
				{
					player.Message("You don't have permission to use this command!");
				}
			}
		}
	}
}

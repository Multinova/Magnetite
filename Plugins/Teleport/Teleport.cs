using System;
using System.Collections;
using Magnetite;

namespace Teleport
{
    public class Teleport : Module
	{
		public override string Name {
			get {
				return "Teleport";
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

		public Hashtable Requests = new Hashtable();

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
			if (command.cmd == "tp")
			{
				Player player = command.User;

				if (player.Admin)
				{
					if (command.quotedArgs.Length == 1)
					{
						Player target = Player.FindByName(command.quotedArgs[0]);
						if (target != null)
						{
							player.TeleportToPlayer(target);
						}
						else
						{
							player.Message("No players found with that name!");
						}
					}
					else if (command.quotedArgs.Length == 2)
					{
						Player target = Player.FindByName(command.quotedArgs[0]);
						Player target2 = Player.FindByName(command.quotedArgs[1]);
						if (target != null && target2 != null)
						{
							target.TeleportToPlayer(target2);
						}
						else
						{
							player.Message("No players found with that name!");
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
			else if (command.cmd == "teleport")
			{
				Player player = command.User;

				if (command.quotedArgs.Length == 1)
				{
					Player target = Player.FindByName(command.quotedArgs[0]);
					if (target != null)
					{
						if (!Requests.Contains(player.SteamID))
						{
							target.Message(player.Name + " want to be teleported to you /accept for comfirm the teleportation or /refuse");
							player.Message("teleportation request sended to " + target.Name);
							Requests.Add(target.SteamID, player.SteamID);
						}
						else
						{
							player.Message(target.Name + " has already been requested!");
						}
					}
					else
					{
						player.Message("No players found with that name!");
					}
				}
				else
				{
					player.Message("Wrong number of arguments.");
					player.Message("/teleport \"player\"");
				}
			}
			else if (command.cmd == "accept")
			{
				Player player = command.User;
				if (Requests.Contains(player.SteamID))
				{
					Player target = Player.FindBySteamID((string)Requests[player.SteamID]);
					if (target != null)
					{
						target.TeleportToPlayer(player);
					}
					else
					{
						player.Message("Player not connected!");
					}
					Requests.Remove(player.SteamID);
				}
				else
				{
					player.Message("No requests found.");
				}
			}
			else if (command.cmd == "refuse")
			{
				Player player = command.User;
				if (Requests.Contains(player.SteamID))
				{
					Requests.Remove(player.SteamID);
					player.Message("Request denied.");
				}
				else
				{
					player.Message("No requests found.");
				}
			}
		}
    }
}

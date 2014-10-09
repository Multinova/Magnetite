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
				return new Version(0, 1, 2, 0);
			}
		}

		public override string Help {
			get {
				return "/tpa \"player\" (Send a teleport request)";
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
						int founds;
						Player target = Player.FindByName(command.quotedArgs[0], out founds);
						if (target != null)
						{
							player.TeleportToPlayer(target);
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
					else if (command.quotedArgs.Length == 2)
					{
						int founds;
						int founds2;
						Player target = Player.FindByName(command.quotedArgs[0], out founds);
						Player target2 = Player.FindByName(command.quotedArgs[1], out founds2);
						if (target != null && target2 != null)
						{
							target.TeleportToPlayer(target2);
						}
						else if (founds == 0 || founds2 == 0)
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
			else if (command.cmd == "tpto")
			{
				Player player = command.User;
				if (player.Admin)
				{
					if (command.quotedArgs.Length == 3)
					{
						int x = int.Parse(command.quotedArgs[0]);
						int y = int.Parse(command.quotedArgs[1]);
						int z = int.Parse(command.quotedArgs[2]);
						player.Teleport(x, y, z);
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
			else if (command.cmd == "teleport" || command.cmd == "tpa")
			{
				Player player = command.User;

				if (command.quotedArgs.Length == 1)
				{
					int founds;
					Player target = Player.FindByName(command.quotedArgs[0], out founds);
					if (target != null)
					{
						if (player.SteamID == target.SteamID)
						{
							player.Message("Cannot teleport to yourself!");
						}
						else if (!Requests.Contains(player.SteamID))
						{
							target.Message(String.Format("{0} want to be teleported to you.", player.Name));
							target.Message("Use /tpaccept for comfirm the teleportation or /tprefuse");
							player.Message("Teleportation request sent to " + target.Name);
							Requests.Add(target.SteamID, player.SteamID);
						}
						else
						{
							target.Message(String.Format("{0} tried to send you a teleport request, but you've already been asked.", player.Name));
							target.Message("Use /tprefuse to clear previous request.");
							player.Message(target.Name + " has already been requested!");
						}
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
					player.Message("/tpa \"player\"");
				}
			}
			else if (command.cmd == "accept" || command.cmd == "tpaccept")
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
			else if (command.cmd == "refuse" || command.cmd == "tprefuse")
			{
				Player player = command.User;
				if (Requests.Contains(player.SteamID))
				{
					Player target = Player.FindBySteamID((string)Requests[player.SteamID]);
					if (target != null)
					{
						player.Message(String.Format("{0} has denied your request.", player.Name));
					}
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

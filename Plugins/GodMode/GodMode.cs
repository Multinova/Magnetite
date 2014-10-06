using System;
using System.Collections.Generic;
using Magnetite;

namespace GodMode
{
    public class GodMode : Module
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

		public List<string> GodUsers = new List<string>();

		public override void Initialize()
		{
			Hooks.OnPlayerTakeDamage += Hooks_OnPlayerTakeDamage;
			Hooks.OnCommand += Hooks_OnCommand;
		}

		public override void DeInitialize()
		{
			Hooks.OnPlayerTakeDamage -= Hooks_OnPlayerTakeDamage;
			Hooks.OnCommand -= Hooks_OnCommand;
		}

		void Hooks_OnCommand(Command command)
		{
			if (command.cmd == "god")
			{
				Player player = command.User;
				if (player.Admin || player.Owner || player.Moderator)
				{
					if (GodUsers.Contains(player.SteamID))
					{
						GodUsers.Remove(player.SteamID);
						player.Message("You are no longer in god mode.");
					}
					else
					{
						GodUsers.Add(player.SteamID);
						player.Message("You are now in god mode.");
					}
				}
				else
				{
					player.Message("You don't have permission to use this command!");
				}
			}
		}

		void Hooks_OnPlayerTakeDamage(Magnetite.Events.PlayerTakedmgEvent ptde)
		{
			if (GodUsers.Contains(ptde.Victim.SteamID))
			{
				ptde.Amount = 0;
			}
		}
    }
}

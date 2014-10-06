using System;
using System.Collections.Generic;
using Magnetite;

namespace Players
{
    public class Players : Module
    {
		public override string Name {
			get {
				return "Players";
			}
		}

		public override string Author {
			get {
				return "Yohann Seris";
			}
		}

		public override string Help {
			get {
				return "/players (Show list of players)";
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
			if (command.cmd == "players")
			{
				Player player = command.User;
				Server server = Server.GetServer();

				string msg = server.Players.Count == 1 ? "You are alone!" : String.Format("There are: {0} players online!", server.Players.Count);
				player.Message(msg);

				if (server.Players.Count == 1)
				{
					return;
				}

				List<string> players = new List<string>();
				foreach (Player pl in server.ActivePlayers)
				{
					players.Add(pl.Name);
					if (players.Count == 5)
					{
						player.Message(string.Join(", ", players.ToArray()));
						players.Clear();
					}
				}
				if (players.Count > 0)
				{
					player.Message(string.Join(", ", players.ToArray()));
				}
			}
		}
    }
}

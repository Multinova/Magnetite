using System;
using Magnetite;

namespace JoinLeftMessage
{
	public class JoinLeftMessage : Module
	{
		public override string Name {
			get {
				return "JoinLeftMessage";
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
			Hooks.OnPlayerConnected += Hooks_OnPlayerConnected;
			Hooks.OnPlayerDisconnected += Hooks_OnPlayerDisconnected;
		}

		public override void DeInitialize()
		{
			Hooks.OnPlayerConnected -= Hooks_OnPlayerConnected;
			Hooks.OnPlayerDisconnected -= Hooks_OnPlayerDisconnected;
		}

		void Hooks_OnPlayerConnected(Player player)
		{
			Server.GetServer().Broadcast(player.Name + " has join the game");
		}

		void Hooks_OnPlayerDisconnected(Player player)
		{
			Server.GetServer().Broadcast(player.Name + " has left the game");
		}
	}
}

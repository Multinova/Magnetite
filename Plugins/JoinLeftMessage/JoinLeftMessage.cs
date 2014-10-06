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
			if (joinleft.enabled)
			{
				Server.GetServer().Broadcast(String.Format(joinleft.join, player.Name));
			}
		}

		void Hooks_OnPlayerDisconnected(Player player)
		{
			if (joinleft.enabled)
			{
				Server.GetServer().Broadcast(String.Format(joinleft.left, player.Name));
			}
		}
	}

	[ConsoleSystem.Factory("joinleft")]
	public class joinleft : ConsoleSystem
	{
		static joinleft() { }

		public joinleft() : base() { }

		[ConsoleSystem.Admin]
		public static bool enabled = true;

		[ConsoleSystem.Admin]
		public static string join = "{0} has joined the game";

		[ConsoleSystem.Admin]
		public static string left = "{0} has left the game";
	}
}

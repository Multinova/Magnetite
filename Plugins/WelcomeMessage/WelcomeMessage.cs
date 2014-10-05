using System;
using Magnetite;

namespace WelcomeMessage
{
	public class WelcomeMessage : Module
	{
		public override string Name {
			get {
				return "WelcomeMessage";
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
		}

		public override void DeInitialize()
		{
			Hooks.OnPlayerConnected -= Hooks_OnPlayerConnected;
		}

		void Hooks_OnPlayerConnected(Player player)
		{
			if (welcome.enabled)
			{
				player.Message(welcome.message);
			}
		}
	}

	[ConsoleSystem.Factory("welcome")]
	public class welcome : ConsoleSystem
	{
		static welcome() { }

		public welcome() : base() { }

		[ConsoleSystem.Admin]
		public static bool enabled = true;

		[ConsoleSystem.Admin]
		public static string message = "Welcome on our server";
	}
}

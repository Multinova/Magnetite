using System;
using Magnetite;

namespace Magnetite
{
	public class AutoMessage : Module
	{
		public static TimedEvent timer;

		public override string Name {
			get {
				return "AutoMessage";
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
			Start();
		}

		public override void DeInitialize()
		{
			timer.OnFire -= DisplayMessage;
			timer.Stop();
		}

		public static void Start()
		{
			if (timer == null)
			{
				timer = new TimedEvent("automessage", (double)(automessage.interval * 1000));
				timer.OnFire += DisplayMessage;
			}
			timer.Start();
		}

		public static void Restart()
		{
			timer.Stop();
			timer.Interval = (double)(automessage.interval * 1000);
			timer.Start();
		}

		public static void Stop()
		{
			timer.Stop();
		}

		static void DisplayMessage(string name)
		{
			if (automessage.enabled && automessage.message != null)
			{
				Server.GetServer().Broadcast(automessage.message);
			}
		}
	}

	[ConsoleSystem.Factory("automessage")]
	public class automessage : ConsoleSystem
	{
		static automessage() { }

		public automessage() : base() { }

		[ConsoleSystem.Admin]
		public static bool enabled = true;

		[ConsoleSystem.Admin]
		public static int interval = 300;

		[ConsoleSystem.Admin]
		public static string message = "Teamspeak: ts.multinova.fr";
	}
}

using System;
using Magnetite;

namespace AirDrop
{
    public class AirDrop : Module
	{
		public override string Name {
			get {
				return "AirDrop";
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

		public static TimedEvent timer;

		public override void Initialize()
		{
			Start();
			Hooks.OnCommand += Hooks_OnCommand;
		}

		public override void DeInitialize()
		{
			timer.OnFire -= Drop;
			timer.Stop();
			Hooks.OnCommand -= Hooks_OnCommand;
		}

		public static void Start()
		{
			if (timer == null)
			{
				timer = new TimedEvent("airdrop", (double)(airdrop.interval * 1000));
				timer.OnFire += Drop;
			}
			timer.Start();
		}

		public static void Restart()
		{
			timer.Stop();
			timer.Interval = (double)(airdrop.interval * 1000);
			timer.Start();
		}

		public static void Stop()
		{
			timer.Stop();
		}

		static void Drop(string name)
		{
			if (airdrop.enabled)
			{
				World.GetWorld().AirDrop();
			}
		}

		void Hooks_OnCommand(Command command)
		{
			if (command.cmd == "airdrop")
			{
				Player player = command.User;
				if (player.Admin || player.Owner || player.Moderator)
				{
					if (command.quotedArgs.Length == 1)
					{
						int founds;
						Player target = Player.FindByName(command.quotedArgs[0], out founds);
						if (target != null)
						{
							World.GetWorld().AirDropAtPlayer(target);
							player.Message("Air drop called on player " + target.Name);
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
						World.GetWorld().AirDrop();
						player.Message("Air drop called");
					}
				}
				else
				{
					player.Message("You don't have permission to use this command!");
				}
			}
		}
    }

	[ConsoleSystem.Factory("airdrop")]
	public class airdrop : ConsoleSystem
	{
		static airdrop() { }

		public airdrop() : base() { }

		[ConsoleSystem.Admin]
		public static bool enabled = true;

		[ConsoleSystem.Admin]
		public static int interval = 1800;
	}
}

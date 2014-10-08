using System;
using Magnetite;

namespace DeathMessage
{
    public class DeathMessage : Module
	{
		public override string Name {
			get {
				return "DeathMessage";
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
			Hooks.OnPlayerDied += Hooks_OnPlayerDied;
		}

		public override void DeInitialize()
		{
			Hooks.OnPlayerDied -= Hooks_OnPlayerDied;
		}

		void Hooks_OnPlayerDied(Magnetite.Events.PlayerDeathEvent pde)
		{
			if (deathmessages.enabled)
			{
				Player victim = pde.Victim;
				if (pde.Attacker is BasePlayer)
				{
					if (!deathmessages.showsuicide && pde.DamageType != Rust.DamageType.Bleeding)
					{
						return;
					}
					Player attacker = new Player((BasePlayer)pde.Attacker);
					if (attacker.SteamID == victim.SteamID)
					{
						switch (pde.DamageType)
						{
							case Rust.DamageType.Bleeding:
								Server.GetServer().Broadcast(String.Format(deathmessages.bleeding, victim.Name));
								break;
							case Rust.DamageType.Cold:
								Server.GetServer().Broadcast(String.Format(deathmessages.cold, victim.Name));
								break;
							case Rust.DamageType.Radiation:
								Server.GetServer().Broadcast(String.Format(deathmessages.radiation, victim.Name));
								break;
							case Rust.DamageType.Poison:
								Server.GetServer().Broadcast(String.Format(deathmessages.poison, victim.Name));
								break;
							case Rust.DamageType.Hunger:
								Server.GetServer().Broadcast(String.Format(deathmessages.hunger, victim.Name));
								break;
							case Rust.DamageType.Fall:
								Server.GetServer().Broadcast(String.Format(deathmessages.fall, victim.Name));
								break;
							default:
								Server.GetServer().Broadcast(String.Format(deathmessages.suicide, victim.Name));
								break;
						}
						
					}
					else
					{
						int distance = (int)Util.GetUtil().GetVectorsDistance(attacker.Location, victim.Location);
						Server.GetServer().Broadcast(String.Format(deathmessages.player, victim.Name, attacker.Name, pde.WeaponName, distance));
					}
				}
				else if (pde.Attacker is BaseAnimal)
				{

					if (!deathmessages.showanimal)
					{
						return;
					}
					NPC npc = new NPC((BaseAnimal)pde.Attacker);
					Server.GetServer().Broadcast(String.Format(deathmessages.animal, victim.Name, npc.Name));
				}
				else
				{
					string type = pde.Attacker == null ? "null" : pde.Attacker.GetType().ToString();
					Logger.Log("Death message attacker type " + type);
					switch (pde.DamageType)
					{
						case Rust.DamageType.Bleeding:
							Server.GetServer().Broadcast(String.Format(deathmessages.bleeding, victim.Name));
							break;
						case Rust.DamageType.Cold:
							Server.GetServer().Broadcast(String.Format(deathmessages.cold, victim.Name));
							break;
						case Rust.DamageType.Radiation:
							Server.GetServer().Broadcast(String.Format(deathmessages.radiation, victim.Name));
							break;
						case Rust.DamageType.Poison:
							Server.GetServer().Broadcast(String.Format(deathmessages.poison, victim.Name));
							break;
						case Rust.DamageType.Hunger:
							Server.GetServer().Broadcast(String.Format(deathmessages.hunger, victim.Name));
							break;
						case Rust.DamageType.Fall:
							Server.GetServer().Broadcast(String.Format(deathmessages.fall, victim.Name));
							break;
						default:
							Server.GetServer().Broadcast(String.Format(deathmessages.suicide, victim.Name));
							break;
					}
				}
			}
		}
    }

	[ConsoleSystem.Factory("deathmessages")]
	public class deathmessages : ConsoleSystem
	{
		static deathmessages() { }

		public deathmessages() : base() { }

		[ConsoleSystem.Admin]
		public static bool enabled = true;

		[ConsoleSystem.Admin]
		public static bool showsuicide = true;

		[ConsoleSystem.Admin]
		public static bool showanimal = true;

		[ConsoleSystem.Admin]
		public static string player = "{0} was killed by {1} using {2} at {3} meters";

		[ConsoleSystem.Admin]
		public static string suicide = "{0} has committed suicide";

		[ConsoleSystem.Admin]
		public static string bleeding = "{0} has succumbed to his injuries";

		[ConsoleSystem.Admin]
		public static string radiation = "{0} died from radiations";

		[ConsoleSystem.Admin]
		public static string poison = "{0} died from poisoning";

		[ConsoleSystem.Admin]
		public static string hunger = "{0} died from hunger";

		[ConsoleSystem.Admin]
		public static string fall = "{0} died from fall";

		[ConsoleSystem.Admin]
		public static string cold = "{0} froze to death";

		[ConsoleSystem.Admin]
		public static string animal = "{0} was killed by a {1}";
	}
}

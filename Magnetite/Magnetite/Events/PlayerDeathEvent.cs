using System;

namespace Magnetite.Events
{
	public class PlayerDeathEvent : DeathEvent
	{
		public readonly Player Victim;

		public PlayerDeathEvent(Player player, HitInfo info)
			: base(info)
		{
			Victim = player;
		}
	}
}


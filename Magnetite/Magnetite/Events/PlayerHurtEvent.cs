using System;

namespace Magnetite.Events
{
	public class PlayerHurtEvent : HurtEvent
	{
		public readonly Player Victim;

		public PlayerHurtEvent(Player player, HitInfo info)
			: base(info)
		{
			Victim = player;
		}
	}
}


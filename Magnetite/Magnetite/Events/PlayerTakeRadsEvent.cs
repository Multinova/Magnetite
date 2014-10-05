using System;

namespace Magnetite.Events
{
	public class PlayerTakeRadsEvent
	{
		public readonly Player Victim;

		public float Amount;

		public PlayerTakeRadsEvent(Player player, float amount)
		{
			Amount = amount;
			Victim = player;
		}
	}
}


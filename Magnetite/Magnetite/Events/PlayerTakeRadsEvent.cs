using System;

namespace Magnetite.Events
{
	public class PlayerTakeRadsEvent
	{

		public readonly Player Victim;
		public float Amount;

		public PlayerTakeRadsEvent(Player p, float amount)
		{
			Amount = amount;
			Victim = p;
		}
	}
}


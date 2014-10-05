using System;

namespace Magnetite.Events
{
	public class PlayerTakedmgEvent
	{
		public readonly Player Victim;

		public float Amount;

		public Rust.DamageType Type;

		public PlayerTakedmgEvent(Player player, float amount, Rust.DamageType type)
		{
			Type = type;
			Amount = amount;
			Victim = player;
		}
	}
}


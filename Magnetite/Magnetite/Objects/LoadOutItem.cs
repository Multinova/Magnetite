using System;

namespace Magnetite
{
	public class LoadOutItem
	{
		public readonly int Amount;
		public readonly string Name;

		public LoadOutItem(string name)
		{
			Amount = 1;
			Name = name;
		}

		public LoadOutItem(string name, int amount)
		{
			Amount = amount;
			Name = name;
		}

		public InventoryItem invItem {
			get {
				return new InventoryItem(Name, Amount);
			}
		}
	}
}


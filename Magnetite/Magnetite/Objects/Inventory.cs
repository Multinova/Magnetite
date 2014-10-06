using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magnetite
{
	public class Inventory
	{
		public readonly PlayerInventory _inv;

		public readonly Player owner;

		public Inventory(PlayerInventory inv)
		{
			_inv = inv;
			owner = new Player(inv.GetComponent<BasePlayer>());
		}

		public bool Add(InventoryItem item)
		{
			ItemContainer con;
			if (item.containerPref == InventoryItem.ContainerPreference.Belt)
				con = InnerBelt;
			else if (item.containerPref == InventoryItem.ContainerPreference.Wear)
				con = InnerWear;
			else
				con = InnerMain;

			bool flag = _inv.GiveItem(item._item, con);
			if (!flag)
			{
				flag = _inv.GiveItem(item._item);
			}

			return flag;
		}

		public bool Add(InventoryItem item, ItemContainer con)
		{
			return _inv.GiveItem(item._item, con);
		}

		public bool Add(int itemID)
		{
			return Add(itemID, 1);
		}

		public bool Add(int itemID, int amount)
		{
			return _inv.GiveItem(itemID, amount, true);
		}

		public ItemContainer InnerBelt
		{
			get
			{
				return _inv.containerBelt;
			}
		}

		public ItemContainer InnerMain
		{
			get
			{
				return _inv.containerMain;
			}
		}

		public ItemContainer InnerWear
		{
			get
			{
				return _inv.containerWear;
			}
		}

		public List<InventoryItem> AllItems()
		{
			return (from item in _inv.AllItems()
					select new InventoryItem(item)).ToList();
		}

		public List<InventoryItem> BeltItems()
		{
			return (from item in _inv.containerBelt.itemList
					select new InventoryItem(item)).ToList();
		}

		public List<InventoryItem> MainItems()
		{
			return (from item in _inv.containerMain.itemList
					select new InventoryItem(item)).ToList();
		}

		public List<InventoryItem> WearItems()
		{
			return (from item in _inv.containerWear.itemList
					select new InventoryItem(item)).ToList();
		}

		public void Notice(LoadOutItem loItem)
		{
			string msg = String.Format("{0} {1}", InventoryItem.GetItemID(loItem.Name), loItem.Amount);
			Notice(msg);
		}

		public void Notice(InventoryItem item)
		{
			string msg = String.Format("{0} {1}", item.ItemID, item.Quantity);
			Notice(msg);
		}

		public void Notice(string msg)
		{
			owner.basePlayer.Command("note.inv " + msg);
		}
	}
}

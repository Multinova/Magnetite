using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Magnetite;

namespace Kits
{
	public class Kits : Module
	{
		public static List<string> ItemsNames = new List<string>() {
			"Waterpipe Shotgun", "Blood", "Sulfur", "Wooden Door Key", "Stones", "Tuna Can", "Spoiled Wolf Meat", "Metal Ore", "Shotgun Cartridge", "Low Grade Fuel", "Burlap Shirt", "Paper", "Burned Wolf Meat", "Human Skull", "Floor Plan", "Thompson", "Red Jacket", "Granola Bar", "Bear Trap", "Wooden Door", "Sleeping Bag", "Stone Hatchet", "Bone Fragments", "Cooked Chicken", "Rotten Apple", "Raw Wolf Meat", "Bandage", "Bed", "Can of Beans", "Raw Human Meat", "Lantern", "Burned Human Meat", "Doorway Plan", "Blueberries", "Hunting Bow", "Can of Tuna", "Pistol Bullet", "Cooked Wolf Meat", "Bolt Action Rifle", "Chocolate Bar", "Burned Chicken", "Wolf Skull", "Black Raspberries", "Camp Fire", "Wood", "Charcoal", "Metal Chest Plate", "Lock", "Small Water Bottle", "Anti-Radiation Pills", "Burlap Shoes", "Torch", "Bone Knife", "Pick Axe", "Window Plan", "Foundation Plan", "Raw Chicken Breast", "Spoiled Human Meat", "Eoka Pistol", "Wooden Arrow", "Animal Fat", "Railing Plan", "Large Wood Storage", "Cooked Human Meat", "Hammer", "Wooden Door Plan", "Burlap Trousers", "Rock", "Metal Fragments", "Apple", "Flare", "Hatchet", "Cloth", "Wood Storage Box", "Revolver", "5.56 Rifle Cartridge", "Bear Meat", "Wall Plan", "Sulfur Ore", "Stairs", "Furnace", "Empty Can Of Beans", "Small Medkit", "Large Medkit", "Spoiled Chicken"
		};

		private static DataStore data;
		
		public override string Name {
			get {
				return "Kits";
			}
		}

		public override string Author {
			get {
				return "Yohann Seris";
			}
		}

		public override string Help {
			get {
				return "/kit \"kit name\"";
			}
		}

		public override Version Version {
			get {
				return new Version(0, 1, 0, 0);
			}
		}

		public static Hashtable kits {
			get {
				return data.datastore;
			}
		}

		public static Hashtable users {
			get {
				return (Hashtable)data.datastore["users_data"];
			}
		}

		public override void Initialize()
		{
			data = this.DataStore("kits.ds");
			data.Load();
			if (!data.datastore.Contains("users_data"))
			{
				data.datastore.Add("users_data", new Hashtable());
			}
			Hooks.OnCommand += Hooks_OnCommand;
		}

		public override void DeInitialize()
		{
			Hooks.OnCommand -= Hooks_OnCommand;
		}

		public static void Save()
		{
			data.Save();
		}

		public static int CurrentTime()
		{
			return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		}

		public static Dictionary<string, int> Items(string kit)
		{
			Hashtable kitTable = (Hashtable)kits[kit];
			if (kitTable != null)
			{
				return (Dictionary<string, int>)kitTable["items"];
			}
			return null;
		}

		public static int Time(string kit)
		{
			Hashtable kitTable = (Hashtable)kits[kit];
			if (kitTable != null)
			{
				return (int)kitTable["time"];
			}
			return 0;
		}

		public static int Limit(string kit)
		{
			Hashtable kitTable = (Hashtable)kits[kit];
			if (kitTable != null)
			{
				return (int)kitTable["limit"];
			}
			return 0;
		}

		public static Hashtable User(Player player)
		{
			//Hashtable users = (Hashtable)data.datastore["users_data"];
			if (!users.Contains(player.SteamID))
			{
				users[player.SteamID] = new Hashtable();
				Save();
			}
			return (Hashtable)users[player.SteamID];
		}

		public static int LastClaim(Player player, string kit)
		{
			Hashtable user = User(player);
			if (user.Contains(kit))
			{
				Hashtable userKit = (Hashtable)user[kit];
				return (int)userKit["last"];
			}
			return 0;
		}

		public static int NumberClaim(Player player, string kit)
		{
			Hashtable user = User(player);
			if (user.Contains(kit))
			{
				Hashtable userKit = (Hashtable)user[kit];
				return (int)userKit["number"];
			}
			return 0;
		}

		public static void SetClaim(Player player, string kit)
		{
			Hashtable user = User(player);
			if (!user.Contains(kit))
			{
				Hashtable userKit = new Hashtable();
				userKit["number"] = 1;
				userKit["last"] = CurrentTime();
				user[kit] = userKit;
			}
			else
			{
				Hashtable userKit = (Hashtable)user[kit];
				userKit["number"] = ((int)userKit["number"]) + 1;
				userKit["last"] = CurrentTime();
			}
			Save();
		}

		public bool HasWait(Player player, string kit)
		{
			return ((LastClaim(player, kit) + Time(kit)) < CurrentTime());
		}

		public bool HasReach(Player player, string kit)
		{
			int limit = Limit(kit);

			return (NumberClaim(player, kit) >= limit && limit > 0);
		}

		void Hooks_OnCommand(Command command)
		{
			if (command.cmd == "kit")
			{
				Player player = command.User;
				if (command.quotedArgs.Length == 1)
				{
					string kit = command.quotedArgs[0];

					if (kits.Contains(kit))
					{
						if (HasReach(player, kit))
						{
							player.Message("You have reach the maximum number of this kit");
							return;
						}
						if (!HasWait(player, kit))
						{
							player.Message("You need to wait for claim this kit again");
							return;
						}

						Dictionary<string, int> items = Items(kit);
						foreach (KeyValuePair<string, int> item in items)
						{
							if (ItemsNames.Contains(item.Key))
							{
								player.Inventory.Add(new InventoryItem(item.Key, item.Value));
							}
						}
						SetClaim(player, kit);
					}
					else
					{
						player.Message("Kit not found!");
					}
				}
				else
				{
					player.Message("Wrong number of arguments.");
				}
			}
		}
    }

	[ConsoleSystem.Factory("kits")]
	public class kits : ConsoleSystem
	{
		static kits() { }

		public kits() : base() { }

		[ConsoleSystem.Admin]
		public static bool enabled = true;

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.list", "")]
		public static void list(ConsoleSystem.Arg arg)
		{
			List<string> list = new List<string>();

			foreach (string kit in Kits.kits.Keys)
			{
				if (kit != "users_data")
				{
					list.Add(kit);
				}
			}

			arg.ReplyWith(String.Join(", ", list.ToArray()));
		}

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.items <kit name>", "")]
		public static void items(ConsoleSystem.Arg arg)
		{
			if (arg.Args != null && arg.Args.Length == 1)
			{
				string kit = arg.Args[0];

				if (Kits.kits.Contains(kit))
				{
					List<string> items = new List<string>();

					Dictionary<string, int> itemsList = Kits.Items(kit);

					foreach (KeyValuePair<string, int> item in itemsList)
					{
						items.Add(item.Key + ": " + item.Value);
					}

					arg.ReplyWith(String.Join(", ", items.ToArray()));
				}
			}
			else
			{
				arg.ReplyWith("Wrong number of arguments.");
			}
		}

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.settime <kit name> <time elapse claim>", "")]
		public static void settime(ConsoleSystem.Arg arg)
		{
			if (arg.Args != null && arg.Args.Length == 2)
			{
				string kit = arg.Args[0];
				int time = int.Parse(arg.Args[1]);

				if (Kits.kits.Contains(kit))
				{
					Hashtable kitTable = (Hashtable)Kits.kits[kit];
					kitTable["time"] = time;
					Kits.Save();
					arg.ReplyWith("time of " + kit + " set to " + time);
				}
				else
				{
					arg.ReplyWith("kit not exists!");
				}
			}
			else
			{
				arg.ReplyWith("Wrong number of arguments.");
			}
		}

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.limit <kit name> <limit of claim>", "")]
		public static void limit(ConsoleSystem.Arg arg)
		{
			if (arg.Args != null && arg.Args.Length == 2)
			{
				string kit = arg.Args[0];
				int limit = int.Parse(arg.Args[1]);

				if (Kits.kits.Contains(kit))
				{
					Hashtable kitTable = (Hashtable)Kits.kits[kit];
					kitTable["limit"] = limit;
					Kits.Save();
					arg.ReplyWith("limit of claim fo " + kit + " set to " + limit);
				}
				else
				{
					arg.ReplyWith("kit not exists!");
				}
			}
			else
			{
				arg.ReplyWith("Wrong number of arguments.");
			}
		}

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.addkit <kit name> <time elapse claim> <limit of claim>", "")]
		public static void addkit(ConsoleSystem.Arg arg)
		{
			if (arg.Args != null && arg.Args.Length == 3)
			{
				string kit = arg.Args[0];
				int time = int.Parse(arg.Args[1]);
				int limit = int.Parse(arg.Args[2]);

				if (!Kits.kits.Contains(kit))
				{
					Hashtable kitTable = new Hashtable();
					kitTable.Add("items", new Dictionary<string, int>());
					kitTable.Add("time", time);
					kitTable.Add("limit", limit);

					Kits.kits.Add(kit, kitTable);
					Kits.Save();
					arg.ReplyWith("kit " + kit + " created!");
				}
				else
				{
					arg.ReplyWith("kit already exists!");
				}
			}
			else
			{
				arg.ReplyWith("Wrong number of arguments.");
			}
		}

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.removekit <kit name>", "")]
		public static void removekit(ConsoleSystem.Arg arg)
		{
			if (arg.Args != null && arg.Args.Length == 1)
			{
				string kit = arg.Args[0];

				if (Kits.kits.Contains(kit))
				{
					Kits.kits.Remove(kit);
					Kits.Save();
					arg.ReplyWith("kit " + kit + " removed!");
				}
				else
				{
					arg.ReplyWith("kit not exists!");
				}
			}
			else
			{
				arg.ReplyWith("Wrong number of arguments.");
			}
		}

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.additem <kit name> <item> <amount>", "")]
		public static void additem(ConsoleSystem.Arg arg)
		{
			if (arg.Args != null && arg.Args.Length == 3)
			{
				string kit = arg.Args[0];
				string item = arg.Args[1];
				int amount = int.Parse(arg.Args[2]);

				if (Kits.kits.Contains(kit))
				{
					Dictionary<string, int> itemsList = Kits.Items(kit);

					if (amount > 0)
					{
						if (itemsList.ContainsKey(item))
						{
							itemsList[item] = amount;
						}
						else
						{
							itemsList.Add(item, amount);
						}
						arg.ReplyWith(item + " set to " + amount + " in " + kit);
					}
					else
					{
						itemsList.Remove(item);
						arg.ReplyWith(item + " removed from " + kit);
					}
					Kits.Save();
				}
			}
			else
			{
				arg.ReplyWith("Wrong number of arguments.");
			}
		}

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.removeitem <kit name> <item>", "")]
		public static void removeitem(ConsoleSystem.Arg arg)
		{
			if (arg.Args != null && arg.Args.Length == 2)
			{
				string kit = arg.Args[0];
				string item = arg.Args[1];

				if (Kits.kits.Contains(kit))
				{
					Dictionary<string, int> itemsList = Kits.Items(kit);

					itemsList.Remove(item);
					Kits.Save();
					arg.ReplyWith(item + " removed from " + kit);
				}
			}
			else
			{
				arg.ReplyWith("Wrong number of arguments.");
			}
		}
	}
}

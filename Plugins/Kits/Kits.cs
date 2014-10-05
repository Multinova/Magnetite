using System;
using System.Collections;
using System.Collections.Generic;
using Magnetite;

namespace Kits
{
	public class Kits : Module
	{
		public static List<string> ItemsNames = new List<string>() {
			/*"shotgun_waterpipe", "blood", "sulfur", "woodendoorkey", "stones", "can_tuna_empty", "wolfmeat_spoiled", "metal_ore", "ammo_shotgun", "lowgradefuel", "burlap_shirt", "paper", "wolfmeat_burned", "skull_human", "c_floor", "smg_thompson", "jacket_urban_red", "granolabar", "trap_bear", "deployed_door", "sleepingbag", "stonehatchet", "bone_fragments", "chicken_cooked", "apple_spoiled", "wolfmeat_raw", "bandage", "burlap_gloves", "can_beans", "humanmeat_raw", "lantern", "humanmeat_burned", "c_door", "blueberries", "bow_hunting", "can_tuna", "ammo_pistol", "wolfmeat_cooked", "rifle_bolt", "chocholate", "chicken_burned", "skull_wolf", "black raspberries", "campfire", "wood", "charcoal", "metal_plate_torso", "lock", "smallwaterbottle", "antiradpills", "burlap_shoes", "torch", "knife_bone", "pickaxe", "c_window", "c_foundation", "chicken_raw", "humanmeat_spoiled", "pistol_eoka", "arrow_woodenb", "fat_animal", "c_railing", "box_wooden_large", "humanmeat_cooked", "hammer", "c_door_wood", "burlap_trousers", "rock", "metal_fragments", "apple", "flare", "hatchet", "cloth", "box_wooden", "pistol_revolver", "ammo_rifle", "bearmeat", "c_wall", "sulfur_ore", "c_stairs", "furnace", "can_beans_empty", "medkit", "largemedit", "chicken_spoiled"
			*/
			"Waterpipe Shotgun", "Blood", "Sulfur", "Wooden Door Key", "Stones", "Tuna Can", "Spoiled Wolf Meat", "Metal Ore", "Shotgun Cartridge", "Low Grade Fuel", "Burlap Shirt", "Paper", "Burned Wolf Meat", "Human Skull", "Floor Plan", "Thompson", "Red Jacket", "Granola Bar", "Bear Trap", "Wooden Door", "Sleeping Bag", "Stone Hatchet", "Bone Fragments", "Cooked Chicken", "Rotten Apple", "Raw Wolf Meat", "Bandage", "Bed", "Can of Beans", "Raw Human Meat", "Lantern", "Burned Human Meat", "Doorway Plan", "Blueberries", "Hunting Bow", "Can of Tuna", "Pistol Bullet", "Cooked Wolf Meat", "Bolt Action Rifle", "Chocolate Bar", "Burned Chicken", "Wolf Skull", "Black Raspberries", "Camp Fire", "Wood", "Charcoal", "Metal Chest Plate", "Lock", "Small Water Bottle", "Anti-Radiation Pills", "Burlap Shoes", "Torch", "Bone Knife", "Pick Axe", "Window Plan", "Foundation Plan", "Raw Chicken Breast", "Spoiled Human Meat", "Eoka Pistol", "Wooden Arrow", "Animal Fat", "Railing Plan", "Large Wood Storage", "Cooked Human Meat", "Hammer", "Wooden Door Plan", "Burlap Trousers", "Rock", "Metal Fragments", "Apple", "Flare", "Hatchet", "Cloth", "Wood Storage Box", "Revolver", "5.56 Rifle Cartridge", "Bear Meat", "Wall Plan", "Sulfur Ore", "Stairs", "Furnace", "Empty Can Of Beans", "Small Medkit", "Large Medkit", "Spoiled Chicken"
			/*
			"14099", "11955", "11966", "14046", "11964", "14091", "14087", "13994", "14100", "13985", "14048", "11963", "14089", "14094", "14028", "14102", "12540", "11910", "836", "14042", "11923", "11983", "188", "2", "14093", "13968", "11949", "11914", "11906", "13967", "14038", "14109", "14027", "1529", "6296", "11907", "14101", "14086", "11864", "11908", "3", "14095", "2537", "3533", "514", "11956", "14110", "14072", "11913", "11905", "14076", "11953", "373", "11980", "14030", "13886", "1", "14085", "14098", "11904", "11954", "14036", "11919", "13971", "13851", "14043", "14055", "13844", "13995", "10338", "11952", "11975", "6604", "11930", "14103", "13987", "2821", "13986", "13993", "14029", "11917", "14092", "11951", "11950", "4"*/
		};
		
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

		public override Version Version {
			get {
				return new Version(0, 1, 0, 0);
			}
		}

		public static Hashtable kits = new Hashtable();

		public override void Initialize()
		{
			Dictionary<string, int> starter = new Dictionary<string, int>();
			starter.Add("Stone Hatchet", 1);
			starter.Add("Apple", 2);
			starter.Add("Flare", 1);
			_kits.Add("starter", starter);
			Hooks.OnCommand += Hooks_OnCommand;
		}

		public override void DeInitialize()
		{
			Hooks.OnCommand -= Hooks_OnCommand;
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
						Dictionary<string, int> items = (Dictionary<string, int>)kits[kit];
						foreach (KeyValuePair<string, int> item in items)
						{
							if (ItemsNames.Contains(item.Key))
							{
								player.Inventory.Add(new InventoryItem(item.Key, item.Value));
							}
						}
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

			foreach (string kit in Kits.kits)
			{
				list.Add(kit);
			}

			arg.ReplyWith(String.Join(", ", list.ToArray()));
		}

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.items <kit name>", "")]
		public static void list(ConsoleSystem.Arg arg)
		{
			if (arg.Args.Length == 1)
			{
				string kit = arg.Args[0];

				if (Kits.kits.Contains(kit))
				{
					List<string> items = new List<string>();

					Dictionary<string, int> itemsList = (Dictionary<string, int>)Kits.kits[kit];

					foreach (KeyValuePair<string, int> item in itemsList)
					{
						items.Add(item.Key + ": " + item.Value);
					}

					arg.ReplyWith(String.Join(", ", items.ToArray()));
				}
			}
		}

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.additem <kit name> <item> <amount>", "")]
		public static void additem(ConsoleSystem.Arg arg)
		{
			if (arg.Args.Length == 3)
			{
				string kit = arg.Args[0];
				string item = arg.Args[1];
				int amount = int.Parse(arg.Args[2]);

				if (Kits.kits.Contains(kit))
				{
					Dictionary<string, int> itemsList = (Dictionary<string, int>)Kits.kits[kit];

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
						arg.ReplyWith(item + " has been to " + amount);
					}
					else
					{
						itemsList.Remove(item);
						arg.ReplyWith(item + " has been remove from " + kit);
					}
				}
			}
		}

		[ConsoleSystem.Admin]
		[ConsoleSystem.Help("kits.removeitem <kit name> <item>", "")]
		public static void removeitem(ConsoleSystem.Arg arg)
		{
			if (arg.Args.Length == 2)
			{
				string kit = arg.Args[0];
				string item = arg.Args[1];

				if (Kits.kits.Contains(kit))
				{
					Dictionary<string, int> itemsList = (Dictionary<string, int>)Kits.kits[kit];

					itemsList.Remove(item);
					arg.ReplyWith(item + " has been remove from " + kit);
				}
			}
		}
	}
}

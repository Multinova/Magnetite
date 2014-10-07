using System;
using System.Collections;
using System.Collections.Generic;
using Magnetite;

namespace Give
{
	public class Give : Module
	{
		public static List<string> ItemsNames = new List<string>() {
			"Waterpipe Shotgun", "Blood", "Sulfur", "Wooden Door Key", "Stones", "Tuna Can", "Spoiled Wolf Meat", "Metal Ore", "Shotgun Cartridge", "Low Grade Fuel", "Burlap Shirt", "Paper", "Burned Wolf Meat", "Human Skull", "Floor Plan", "Thompson", "Red Jacket", "Granola Bar", "Bear Trap", "Wooden Door", "Sleeping Bag", "Stone Hatchet", "Bone Fragments", "Cooked Chicken", "Rotten Apple", "Raw Wolf Meat", "Bandage", "Bed", "Can of Beans", "Raw Human Meat", "Lantern", "Burned Human Meat", "Doorway Plan", "Blueberries", "Hunting Bow", "Can of Tuna", "Pistol Bullet", "Cooked Wolf Meat", "Bolt Action Rifle", "Chocolate Bar", "Burned Chicken", "Wolf Skull", "Black Raspberries", "Camp Fire", "Wood", "Charcoal", "Metal Chest Plate", "Lock", "Small Water Bottle", "Anti-Radiation Pills", "Burlap Shoes", "Torch", "Bone Knife", "Pick Axe", "Window Plan", "Foundation Plan", "Raw Chicken Breast", "Spoiled Human Meat", "Eoka Pistol", "Wooden Arrow", "Animal Fat", "Railing Plan", "Large Wood Storage", "Cooked Human Meat", "Hammer", "Wooden Door Plan", "Burlap Trousers", "Rock", "Metal Fragments", "Apple", "Flare", "Hatchet", "Cloth", "Wood Storage Box", "Revolver", "5.56 Rifle Cartridge", "Bear Meat", "Wall Plan", "Sulfur Ore", "Stairs", "Furnace", "Empty Can Of Beans", "Small Medkit", "Large Medkit", "Spoiled Chicken"
		};

		public override string Name {
			get {
				return "Give";
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
			Hooks.OnCommand += Hooks_OnCommand;
		}

		public override void DeInitialize()
		{
			Hooks.OnCommand -= Hooks_OnCommand;
		}

		void Hooks_OnCommand(Command command)
		{
			if (command.cmd == "give")
			{
				Player player = command.User;
				if (command.quotedArgs.Length == 2)
				{
					string item = command.quotedArgs[0];
					int amount = int.Parse(command.quotedArgs[1]);

					if (ItemsNames.Contains(item))
					{
						player.Inventory.Add(new InventoryItem(item, amount));
					}
					else
					{
						player.Message("Item not found!");
					}
				}
				else if (command.quotedArgs.Length == 3)
				{
					int founds;
					Player target = Player.FindByName(command.quotedArgs[0], out founds);
					if (target != null)
					{
						string item = command.quotedArgs[1];
						int amount = int.Parse(command.quotedArgs[2]);

						if (ItemsNames.Contains(item))
						{
							player.Inventory.Add(new InventoryItem(item, amount));
						}
						else
						{
							player.Message("Item not found!");
						}
					}
					else if (founds == 0)
					{
						player.Message("No players found with that name!");
					}
					else
					{
						player.Message("Multiple players found with that name!");
					}
				}
				else
				{
					player.Message("Wrong number of arguments.");
				}
			}
		}
	}
}

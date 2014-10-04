using System;
using Magnetite.Events;
using Network;
using ProtoBuf;
using UnityEngine;

namespace Magnetite
{
	public class Hooks
	{
		#region Events

		public delegate void BuildingPartAttackedHandlerDelagate(BuildingHurtEvent bhe);
		public delegate void BuildingPartDestroyedHandlerDelagate(BuildingHurtEvent bhe);
		public delegate void BuildingFrameDeployedHandlerDelagate(BuildingPart bp);
		public delegate void BuildingCompleteHandlerDelagate(BuildingPart bp);
		public delegate void BuildingUpdateHandlerDelagate(BuildingEvent be);
		public delegate void ChatHandlerDelagate(Magnetite.Chat chat);
		public delegate void ClientAuthHandlerDelagate(AuthEvent ae);
		public delegate void CommandHandlerDelagate(Magnetite.Command command);
		public delegate void CorpseAttackedHandlerDelagate(CorpseHurtEvent che);
		public delegate void CorpseDroppedHandlerDelagate(CorpseInitEvent che);
		public delegate void NPCDiedHandlerDelagate(NPCDeathEvent nde);
		public delegate void NPCHurtHandlerDelagate(NPCHurtEvent nhe);
		public delegate void PlayerConnectedHandlerDelagate(Player player);
		public delegate void PlayerDisconnectedHandlerDelagate(Player player);
		public delegate void PlayerDiedHandlerDelagate(PlayerDeathEvent pde);
		public delegate void PlayerHurtHandlerDelagate(PlayerHurtEvent phe);
		public delegate void PlayerTakeDamageHandlerDelagate(PlayerTakedmgEvent ptde);
		public delegate void PlayerTakeRadsHandlerDelagate(PlayerTakeRadsEvent ptre);
		public delegate void GatheringHandlerDelagate(GatherEvent ge);
		public delegate void LootingEntityHandlerDelagate(EntityLootEvent ele);
		public delegate void LootingPlayerHandlerDelagate(PlayerLootEvent ple);
		public delegate void LootingItemHandlerDelagate(ItemLootEvent ile);
		public delegate void ServerShutdownHandlerDelagate(string info);


		public static event Hooks.BuildingPartAttackedHandlerDelagate OnBuildingPartAttacked;
		public static event Hooks.BuildingPartDestroyedHandlerDelagate OnBuildingPartDestroyed;
		public static event Hooks.BuildingFrameDeployedHandlerDelagate OnBuildingFrameDeployed;
		public static event Hooks.BuildingCompleteHandlerDelagate OnBuildingComplete;
		public static event Hooks.BuildingUpdateHandlerDelagate OnBuildingUpdate;
		public static event Hooks.ChatHandlerDelagate OnChat;
		public static event Hooks.ClientAuthHandlerDelagate OnClientAuth;
		public static event Hooks.CommandHandlerDelagate OnCommand;
		public static event Hooks.CorpseAttackedHandlerDelagate OnCorpseAttacked;
		public static event Hooks.CorpseDroppedHandlerDelagate OnCorpseDropped;
		public static event Hooks.NPCDiedHandlerDelagate OnNPCDied;
		public static event Hooks.NPCHurtHandlerDelagate OnNPCHurt;
		public static event Hooks.PlayerConnectedHandlerDelagate OnPlayerConnected;
		public static event Hooks.PlayerDisconnectedHandlerDelagate OnPlayerDisconnected;
		public static event Hooks.PlayerDiedHandlerDelagate OnPlayerDied;
		public static event Hooks.PlayerHurtHandlerDelagate OnPlayerHurt;
		public static event Hooks.PlayerTakeDamageHandlerDelagate OnPlayerTakeDamage;
		public static event Hooks.PlayerTakeRadsHandlerDelagate OnPlayerTakeRads;
		public static event Hooks.GatheringHandlerDelagate OnGathering;
		public static event Hooks.LootingEntityHandlerDelagate OnLootingEntity;
		public static event Hooks.LootingPlayerHandlerDelagate OnLootingPlayer;
		public static event Hooks.LootingItemHandlerDelagate OnLootingItem;
		public static event Hooks.ServerShutdownHandlerDelagate OnServerShutdown;

		#endregion

		#region Handlers

		// ConnectionAuth.Approve()
		public static void ClientAuth(ConnectionAuth ca, Connection connection)
		{
			var ae = new Events.AuthEvent(connection);

			if (OnClientAuth != null)
			{
				OnClientAuth(ae);
			}

			ca.m_AuthConnection.Remove(connection);
			if (!ae.approved)
				ConnectionAuth.Reject(connection, ae._reason);

			Approval instance = new Approval();
			instance.level = Application.loadedLevelName;
			instance.levelSeed = TerrainGenerator.Seed;
			instance.hostname = server.hostname;
			Net.sv.Approve(connection, Approval.SerializeToBytes(instance));
		}

		// chat.say().Hooks.Chat()
		public static void Command(ConsoleSystem.Arg arg)
		{

			Player player = new Player(arg.Player());
			string[] args = arg.ArgsStr.Substring(2, arg.ArgsStr.Length - 3).Replace("\\", "").Split(new string[] { " " }, StringSplitOptions.None);

			Command cmd = new Command(player, args);

			if (cmd.cmd == "")
				return;

			if (Config.GetBoolValue("Commands", "enabled"))
			{
				/*
				if (cmd.cmd == Config.GetValue("Commands", "ShowMyStats"))
				{
					PlayerStats stats = player.Stats;
					player.Message(String.Format("You have {0} kills and {1} deaths!", stats.Kills, stats.Deaths));
					player.Message(String.Format("You have taken {0} dmg, and caused {1} in total!", stats.TotalDamageTaken, stats.TotalDamageDone));
					return;
				}
				if (cmd.cmd == Config.GetValue("Commands", "ShowStatsOther"))
				{
					Player pOther = Player.Find(String.Join(" ", cmd.args));
					if (pOther != null)
					{
						PlayerStats stats2 = pOther.Stats;
						player.Message(String.Format("You have {0} kills and {1} deaths!", stats2.Kills, stats2.Deaths));
						player.Message(String.Format("You have taken {0} dmg, and caused {1} in total!", stats2.TotalDamageTaken, stats2.TotalDamageDone));
						return;
					}
					player.Message("Can't find player: " + String.Join(" ", cmd.args));
					return;
				}*/
				if (cmd.cmd == Config.GetValue("Commands", "ShowLocation"))
				{
					player.Message(player.Location.ToString());
					return;
				}
				if (cmd.cmd == Config.GetValue("Commands", "ShowOnlinePlayers"))
				{
					string msg = Server.GetServer().Players.Count == 1 ? "You are alone!" : String.Format("There are: {0} players online!", Server.GetServer().Players.Count);
					player.Message(msg);
					return;
				}
			}

			if (cmd.cmd == "login" && cmd.args[0] == "12345")
			{
				Debug.Log("making you an admin");
				player.MakeModerator("Just cause!");
				return;
			}
			if ((cmd.cmd == "magnetite.reload") && player.Admin)
			{
				//PluginLoader.GetInstance().ReloadPlugins();
				return;
			}
			if (OnCommand != null)
			{
				OnCommand(cmd);
			}

			if (cmd.ReplyWith != "")
				arg.ReplyWith(cmd.ReplyWith);

		}

		// chat.say()
		public static void Chat(ConsoleSystem.Arg arg)
		{
			if (arg.ArgsStr.StartsWith("\"/"))
			{
				Command(arg);
				return;
			}

			if (!chat.enabled)
			{
				arg.ReplyWith("Chat is disabled.");
			}
			else
			{
				if (arg.ArgsStr == "\"\"")
				{
					return;
				}

				BasePlayer basePlayer = ArgExtension.Player(arg);
				if (!(bool)((UnityEngine.Object)basePlayer))
					return;

				Chat pChat = new Chat(new Player(basePlayer), arg);

				string str = arg.GetString(0, "text");

				if (str.Length > 128)
					str = str.Substring(0, 128);

				if (chat.serverlog)
					Debug.Log((object)(basePlayer.displayName + ": " + str));

				if (OnChat != null)
				{
					OnChat(pChat);
				}

				if (pChat.FinalText != "")
				{
					ConsoleSystem.Broadcast("chat.add " + StringExtensions.QuoteSafe(pChat.BroadcastName) + " " + StringExtensions.QuoteSafe(pChat.FinalText));
					arg.ReplyWith(pChat.ReplyWith);
				}

				Logger.ChatLog(pChat.BroadcastName, pChat.FinalText);
			}
		}

		// BaseResource.OnAttacked()
		public static void Gathering(BaseResource res, HitInfo info)
		{
			if (!Realm.Server())
				return;

			if (OnGathering != null)
			{
				OnGathering(new Events.GatherEvent(res, info));
			}

			res.health -= info.damageAmount * info.resourceGatherProficiency;
			if ((double)res.health <= 0.0)
				res.Kill(ProtoBuf.EntityDestroy.Mode.None, 0, 0.0f, new Vector3());
			else
				res.Invoke("UpdateNetworkStage", 0.1f);
		}

		// BaseAnimal.OnAttacked()
		public static void NPCHurt(BaseAnimal animal, HitInfo info)
		{
			// works
			var npc = new NPC(animal);

			if (!Realm.Server() || (double)animal.myHealth <= 0.0)
				return;

			if (OnNPCHurt != null && (animal.myHealth - info.damageAmount) > 0.0f)
				OnNPCHurt(new Events.NPCHurtEvent(npc, info));

			animal.myHealth -= info.damageAmount;
			if ((double)animal.myHealth > 0.0)
				return;
			animal.Die(info);
		}

		// BaseAnimal.Die()
		public static void NPCDied(BaseAnimal animal, HitInfo info)
		{
			var npc = new NPC(animal);
			if (OnNPCDied != null)
			{
				OnNPCDied(new Events.NPCDeathEvent(npc, info));
			}
		}

		// BasePlayer.PlayerInit()
		public static void PlayerConnected(Network.Connection connection)
		{
			var player = connection.player as BasePlayer;
			var p = new Player(player);
			if (Server.GetServer().OfflinePlayers.ContainsKey(player.userID))
				Server.GetServer().OfflinePlayers.Remove(player.userID);
			if (!Server.GetServer().Players.ContainsKey(player.userID))
				Server.GetServer().Players.Add(player.userID, p);

			if (OnPlayerConnected != null)
			{
				OnPlayerConnected(p);
			}
		}

		// BasePlayer.Die()
		public static void PlayerDied(BasePlayer player, HitInfo info)
		{
			// works
			if (info == null)
			{
				info = new HitInfo();
				info.damageType = player.metabolism.lastDamage;
				info.Initiator = player as BaseEntity;
			}

			Player victim = new Player(player);

			Events.PlayerDeathEvent pde = new Events.PlayerDeathEvent(victim, info);

			if (OnPlayerDied != null)
			{
				OnPlayerDied(pde);
			}

			if (!pde.dropLoot)
				player.inventory.Strip();
		}

		// BasePlayer.OnDisconnected()
		public static void PlayerDisconnected(BasePlayer player)
		{
			// works
			var p = new Player(player);

			if (Server.GetServer().serverData.ContainsKey("OfflinePlayers", p.SteamID))
			{
				OfflinePlayer op = (Server.GetServer().serverData.Get("OfflinePlayers", p.SteamID) as OfflinePlayer);
				op.Update(p);
				Server.GetServer().OfflinePlayers[player.userID] = op;
			}
			else
			{
				OfflinePlayer op = new OfflinePlayer(p);
				Server.GetServer().OfflinePlayers.Add(player.userID, op);
			}

			if (Server.GetServer().Players.ContainsKey(player.userID))
				Server.GetServer().Players.Remove(player.userID);
			if (OnPlayerDisconnected != null)
			{
				OnPlayerDisconnected(p);
			}
		}

		// BasePlayer.OnAttacked()
		public static void PlayerHurt(BasePlayer player, HitInfo info)
		{
			// not tested
			var p = new Player(player);

			if (info == null)
			{ // it should never accour, but just in case
				info = new HitInfo();
				info.damageAmount = 0.0f;
				info.damageType = player.metabolism.lastDamage;
				info.Initiator = player as BaseEntity;
			}

			if (!player.TestAttack(info) || !Realm.Server() || (info.damageAmount <= 0.0f))
				return;
			player.metabolism.bleeding.Add(Mathf.InverseLerp(0.0f, 100f, info.damageAmount));
			player.metabolism.SubtractHealth(info.damageAmount);
			player.TakeDamageIndicator(info.damageAmount, player.transform.position - info.PointStart);
			player.CheckDeathCondition(info);

			if (!player.IsDead() && OnPlayerHurt != null)
				OnPlayerHurt(new Events.PlayerHurtEvent(p, info));

			player.SendEffect("takedamage_hit");
		}

		// BasePlayer.TakeDamage()
		public static void PlayerTakeDamage(BasePlayer player, float dmgAmount, Rust.DamageType dmgType)
		{
			// works?
			var ptd = new PlayerTakedmgEvent(new Player(player), dmgAmount, dmgType);
			if (OnPlayerTakeDamage != null)
			{
				OnPlayerTakeDamage(ptd);
			}
		}

		public static void PlayerTakeDamageOverload(BasePlayer player, float dmgAmount)
		{
			PlayerTakeDamage(player, dmgAmount, Rust.DamageType.Generic);
		}

		// BasePlayer.TakeRadiation()
		public static void PlayerTakeRadiation(BasePlayer player, float dmgAmount)
		{
			var ptr = new PlayerTakeRadsEvent(new Player(player), dmgAmount);
			if (OnPlayerTakeRads != null)
			{
				OnPlayerTakeRads(ptr);
			}
		}

		// BuildingBlock.OnAttacked()
		public static void EntityAttacked(BuildingBlock bb, HitInfo info)
		{
			// works, event needed

			var bp = new BuildingPart(bb);
			// if entity will be destroyed call the method below
			if ((bb.health - info.damageAmount) <= 0.0f)
			{
				BuildingPartDestroyed(bp, info);
				if ((bb.health - info.damageAmount) <= 0.0f)
					return;
			}
			if (OnBuildingPartAttacked != null)
			{
				OnBuildingPartAttacked(new BuildingHurtEvent(bp, info));
			}
		}

		public static void BuildingPartDestroyed(BuildingPart bp, HitInfo info)
		{
			if (OnBuildingPartDestroyed != null)
			{
				OnBuildingPartDestroyed(new BuildingHurtEvent(bp, info));
			}
		}

		// BuildingBlock.BecomeFrame()
		public static void EntityFrameDeployed(BuildingBlock bb)
		{
			// blockDefinition is null in this hook, but works

			var bp = new BuildingPart(bb);
			if (OnBuildingFrameDeployed != null)
			{
				OnBuildingFrameDeployed(bp);
			}
		}

		// BuildingBlock.BecomeBuilt()
		public static void EntityBuilt(BuildingBlock bb)
		{
			var bp = new BuildingPart(bb);
			if (OnBuildingComplete != null)
			{
				OnBuildingComplete(bp);
			}
		}

		// BuildingBlock.DoBuild()
		public static void EntityBuildingUpdate(BuildingBlock bb, HitInfo info)
		{
			// hammer prof = 1
			// works
			// called anytime you hit a building block with a constructor item (hammer)
			BasePlayer player = info.Initiator as BasePlayer;
			float proficiency = info.resourceGatherProficiency;

			var bp = new BuildingPart(bb);
			var p = new Player(player);
			var ebe = new Events.BuildingEvent(bp, p, proficiency);
			if (OnBuildingUpdate != null)
			{
				OnBuildingUpdate(ebe);
			}
		}

		// BaseCorpse.InitCorpse()
		public static void CorpseInit(BaseCorpse corpse, BaseEntity parent)
		{
			// works
			if (OnCorpseDropped != null)
			{
				OnCorpseDropped(new CorpseInitEvent(corpse, parent));
			}
		}

		// BaseCorpse.OnAttacked()
		public static void CorpseHit(BaseCorpse corpse, HitInfo info)
		{
			// works

			CorpseHurtEvent che = new CorpseHurtEvent(corpse, info);
			if (OnCorpseAttacked != null)
			{
				OnCorpseAttacked(che);
			}
		}

		// PlayerLoot.StartLootingEntity()
		public static void StartLootingEntity(PlayerLoot playerLoot, BasePlayer looter, BaseEntity entity)
		{
			// not tested, what is a lootable entity anyway?

			var ele = new Events.EntityLootEvent(playerLoot, new Player(looter), new Entity(entity));

			if (OnLootingEntity != null)
			{
				OnLootingEntity(ele);
			}
		}

		// PlayerLoot.StartLootingPlayer()
		public static void StartLootingPlayer(PlayerLoot playerLoot, BasePlayer looter, BasePlayer looted)
		{
			// not tested

			var ple = new Events.PlayerLootEvent(playerLoot, new Player(looter), new Player(looted));

			if (OnLootingPlayer != null)
			{
				OnLootingPlayer(ple);
			}
		}

		// PlayerLoot.StartLootingItem()
		public static void StartLootingItem(PlayerLoot playerLoot, BasePlayer looter, Item item)
		{
			// works, event needed

			var ile = new Events.ItemLootEvent(playerLoot, new Player(looter), item);

			if (OnLootingItem != null)
			{
				OnLootingItem(ile);
			}
		}

		public static void ServerShutdown()
		{
			if (OnServerShutdown != null)
			{
				OnServerShutdown("");
			}
			Bootstrap.SaveAll();
		}

		public static void Respawn(BasePlayer player, bool newpos)
		{
			++ServerPerformance.spawns;
			if (newpos)
			{
				BasePlayer.SpawnPoint spawnPoint = ServerMgr.FindSpawnPoint();
				player.transform.position = spawnPoint.pos;
				player.transform.rotation = spawnPoint.rot;
			}
			player.supressSnapshots = true;
			player.StopSpectating();
			player.UpdateNetworkGroup();
			player.StartSleeping();
			player.metabolism.Reset();
			player.inventory.GiveDefaultItems();
			player.SendFullSnapshot();
		}

		public static void SetModded()
		{
			try
			{
				if (magnetite.enabled)
				{
					string pchGameTags = String.Format("mp{0},cp{1},v1140,modded", server.maxplayers, BasePlayer.activePlayerList.Count);
					Steamworks.SteamGameServer.SetGameTags(pchGameTags);
				}
			}
			catch (Exception ex)
			{
				Debug.Log("[Hooks] Error while setting the server modded.");
				Logger.LogException(ex);
			}
		}

		#endregion

		public Hooks()
		{
		}
	}
}

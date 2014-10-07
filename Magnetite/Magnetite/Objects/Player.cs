using System;
using System.Collections.Generic;
using UnityEngine;

namespace Magnetite
{
	public class Player
	{
		#region Variables

		public readonly BasePlayer basePlayer;

		public bool IsBleeding {
			get {
				return Convert.ToBoolean(basePlayer.metabolism.bleeding.value); ;
			}
			set {
				basePlayer.metabolism.bleeding.value = (float)Convert.ToInt32(value);
			}
		}

		public bool Admin {
			get {
				return basePlayer.IsAdmin();
			}
		}

		public string AuthStatus {
			get {
				return basePlayer.net.connection.authStatus;
			}
		}

		public ulong GameID {
			get {
				return basePlayer.userID;
			}
		}

		public float Health {
			get {
				return basePlayer.Health();
			}
			set {
				basePlayer.metabolism.health.value = value;
			}
		}

		public Inventory Inventory {
			get {
				return new Inventory(basePlayer.inventory);
			}
		}

		public string IP {
			get {
				return basePlayer.net.connection.ipaddress;
			}
		}

		public Vector3 Location {
			get {
				return basePlayer.transform.position;
			}
			set {
				basePlayer.transform.position.Set(value.x, value.y, value.z);
			}
		}

		public bool Moderator {
			get {
				return ServerUsers.Is(GameID, ServerUsers.UserGroup.Moderator);
			}
			set {
				if (value)
				{
					MakeModerator();
				}
				else
				{
					MakeNone();
					ServerUsers.Clear(GameID);
				}
			}
		}

		public string Name {
			get {
				return basePlayer.displayName;
			}
		}

		public bool Owner {
			get {
				return ServerUsers.Is(GameID, ServerUsers.UserGroup.Owner);
			}
			set {
				if (value)
				{
					MakeOwner();
				}
				else
				{
					MakeNone();
				}
			}
		}

		public string OS {
			get {
				return basePlayer.net.connection.os;
			}
		}

		public int Ping {
			get {
				return basePlayer.net.connection.ping;
			}
		}

		public string SteamID {
			get {
				return basePlayer.userID.ToString();
			}
		}

		public float TimeOnline {
			get {
				return basePlayer.net.connection.connectionTime;
			}
		}

		public float X {
			get {
				return basePlayer.transform.position.x;
			}
			set {
				basePlayer.transform.position.Set(value, Y, Z);
			}
		}

		public float Y {
			get {
				return basePlayer.transform.position.y;
			}
			set {
				basePlayer.transform.position.Set(X, value, Z);
			}
		}

		public float Z {
			get {
				return basePlayer.transform.position.z;
			}
			set {
				basePlayer.transform.position.Set(X, Y, value);
			}
		}

		#endregion

		#region Methodes

		public Player(BasePlayer player)
		{
			basePlayer = player;
		}

		public static Player Find(string nameOrSteamidOrIP)
		{
			BasePlayer player = BasePlayer.Find(nameOrSteamidOrIP);
			if (player != null)
			{
				return new Player(player);
			}
			Logger.LogDebug("[Player] Couldn't find player!");
			return null;
		}

		public static Player FindByGameID(ulong steamID)
		{
			BasePlayer player = BasePlayer.FindByID(steamID);
			if (player != null)
			{
				return new Player(player);
			}
			Logger.LogDebug("[Player] Couldn't find player!");
			return null;
		}

		public static Player FindBySteamID(string steamID)
		{
			return FindByGameID(UInt64.Parse(steamID));
		}

		public static Player FindByName(string name)
		{
			BasePlayer player = BasePlayer.activePlayerList.Find((BasePlayer x) => x.displayName.Contains(name));
			if (player != null)
			{
				return new Player(player);
			}
			return null;
		}

		public static Player FindByName(string name, out int number)
		{
			List<BasePlayer> players = BasePlayer.activePlayerList.FindAll((BasePlayer x) => x.displayName.Contains(name));
			number = players.Count;
			if (players.Count == 1)
			{
				return new Player(players[0]);
			}
			return null;
		}

		public void Ban(string reason = "no reason")
		{
			ServerUsers.Set(GameID, ServerUsers.UserGroup.Banned, Name, reason);
			ServerUsers.Save();
		}

		public void Kick(string reason = "no reason")
		{
			Network.Net.sv.Kick(basePlayer.net.connection, reason);
		}

		public void Reject(string reason = "no reason")
		{
			ConnectionAuth.Reject(basePlayer.net.connection, reason);
		}

		public void Kill()
		{
			Kill(Rust.DamageType.Suicide);
		}

		public void Kill(Rust.DamageType type)
		{
			var info = new HitInfo();
			info.damageType = type;
			info.Initiator = basePlayer as BaseEntity;
			Kill(info);
		}

		public void Kill(HitInfo info)
		{
			basePlayer.Die(info);
		}

		public void MakeNone(string reason = "no reason")
		{
			ServerUsers.Set(GameID, ServerUsers.UserGroup.None, Name, reason);
			ServerUsers.Save();
		}

		public void MakeModerator(string reason = "no reason")
		{
			ServerUsers.Set(GameID, ServerUsers.UserGroup.Moderator, Name, reason);
			ServerUsers.Save();
		}

		public void MakeOwner(string reason = "no reason")
		{
			ServerUsers.Set(GameID, ServerUsers.UserGroup.Owner, Name, reason);
			ServerUsers.Save();
		}

		public void Message(string msg)
		{
			basePlayer.SendConsoleCommand("chat.add " + StringExtensions.QuoteSafe(Server.server_message_name) + " " + StringExtensions.QuoteSafe(msg));
		}

		public void MessageFrom(string from, string msg)
		{
			basePlayer.SendConsoleCommand("chat.add " + StringExtensions.QuoteSafe(from) + " " + StringExtensions.QuoteSafe(msg));
		}

		public void TeleportTo(float x, float y, float z)
		{
			Teleport(x, y, z);
		}

		public void TeleportToPlayer(Player player)
		{
			Teleport(player.X, player.Y, player.Z);
		}

		public void SafeTeleport(Vector3 v3)
		{
			SafeTeleport(v3.x, v3.y, v3.z);
		}

		public void SafeTeleport(float x, float y, float z)
		{
			basePlayer.supressSnapshots = true;
			basePlayer.transform.position = UnityEngine.Vector3.zero;
			basePlayer.UpdateNetworkGroup();

			basePlayer.transform.position = new UnityEngine.Vector3(x, y, z);
			basePlayer.UpdateNetworkGroup();
			basePlayer.UpdatePlayerCollider(true, false);
			basePlayer.SendFullSnapshot();
			throw new NotImplementedException("SafeTeleport is not yet implemented.");
		}

		public void Teleport(Player player)
		{
			Teleport(player.X, player.Y, player.Z);
		}

		public void Teleport(Vector3 v3)
		{
			Teleport(v3.x, v3.y, v3.z);
		}

		public void Teleport(float x, float y, float z)
		{
			basePlayer.supressSnapshots = true;
			basePlayer.transform.position = UnityEngine.Vector3.zero;
			basePlayer.UpdateNetworkGroup();

			basePlayer.transform.position = new UnityEngine.Vector3(x, y, z);
			basePlayer.UpdateNetworkGroup();
			basePlayer.UpdatePlayerCollider(true, false);
			basePlayer.SendFullSnapshot();
		}

		#endregion
	}
}

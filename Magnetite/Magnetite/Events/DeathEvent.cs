﻿using System;

namespace Magnetite.Events
{
	public class DeathEvent
	{
		public HitInfo _info;

		public bool dropLoot = true;

		public DeathEvent(HitInfo info)
		{
			_info = info;
		}

		public float DamageAmount {
			get {
				return _info.damageAmount;
			}
		}

		public Rust.DamageType DamageType {
			get {
				return _info.damageType;
			}
		}

		public string WeaponName {
			get {
				return _info.Weapon.info.displayname;
			}
		}

		public BaseEntity Attacker {
			get {
				return _info.Initiator;
			}
		}
	}
}


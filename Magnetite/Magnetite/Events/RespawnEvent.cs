using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magnetite.Events
{
	public class RespawnEvent
	{
		public bool GiveDefault;
		public bool ChangePos;
		public UnityEngine.Vector3 SpawnPos;
		public Player Player;

		public RespawnEvent(Player p)
		{
			SpawnPos = UnityEngine.Vector3.zero;
			ChangePos = false;
			GiveDefault = true;
			Player = p;
		}
	}
}

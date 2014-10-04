using System;

namespace Magnetite.Events
{
	public class BuildingHurtEvent : HurtEvent
	{
		public readonly BuildingPart Victim;

		public BuildingHurtEvent(BuildingPart bp, HitInfo info)
			: base(info)
		{
			Victim = bp;
		}
	}
}


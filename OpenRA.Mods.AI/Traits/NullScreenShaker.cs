using OpenRA.Traits;

namespace OpenRA.Mods.AI.Traits
{
	[TraitLocation(SystemActors.World)]
	public class NullScreenShakerInfo : TraitInfo
	{
		public override object Create(ActorInitializer init) { return new NullScreenShaker(); }
	}

	public class NullScreenShaker : IScreenShaker
	{
		public NullScreenShaker() { }

		public void AddEffect(int time, WPos position, int intensity) { }

		public void AddEffect(int time, WPos position, int intensity, float2 multiplier) { }
	}
}

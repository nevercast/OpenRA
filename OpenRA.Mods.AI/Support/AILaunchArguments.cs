using System;
using OpenRA.Network;

namespace OpenRA.Mods.AI.Support
{
	public class AILaunchArguments
	{
		[Desc("The map to start playing on.")]
		public string Map;

		[Desc("Specify a list of bots to use.")]
		public string Bots;

		[Desc("Specify the seed to use for the random number generator.")]
		public string Seed;

		public AILaunchArguments(Arguments args)
		{
			if (args == null)
				return;

			foreach (var f in GetType().GetFields())
				if (args.Contains("AI" + "." + f.Name))
					FieldLoader.LoadField(this, f.Name, args.GetValue("AI" + "." + f.Name, ""));
		}
	}
}

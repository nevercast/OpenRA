using System;
using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.AI.Traits
{
	[TraitLocation(SystemActors.World)]
	[Desc("Trait for exiting the game on GameOver. Attach this to the world actor.")]
	public class ExitOnGameOverInfo : TraitInfo<ExitOnGameOver> { }
	public class ExitOnGameOver : IGameOver
	{
		void IGameOver.GameOver(World world)
		{
			var bots = world.Players.Where(p => p.IsBot && p.InternalName.StartsWith("Multi"));
			foreach (var bot in bots)
			{
				var experience = bot.PlayerActor.TraitOrDefault<PlayerExperience>()?.Experience;
				var playerStats = bot.PlayerActor.TraitOrDefault<PlayerStatistics>();
				Console.WriteLine("Bot {0}:[{1}], score: {2}, winstate: {3}".F(bot.BotType, bot.InternalName, experience, bot.WinState));
				if (playerStats != null)
				{
					Console.WriteLine("Bot {0}:[{1}], units killed: {2}, units lost: {3}, buildings killed: {4}, buildings lost: {5}".F(bot.BotType, bot.InternalName, playerStats.UnitsKilled, playerStats.UnitsDead, playerStats.BuildingsKilled, playerStats.BuildingsDead));
				}
			}

			// Exit game.
			Game.RunAfterDelay(1000, () => Game.Exit());
		}
	}
}

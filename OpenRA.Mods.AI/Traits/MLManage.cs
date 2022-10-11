using System;
using System.Diagnostics;
using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.AI.Traits
{
	[TraitLocation(SystemActors.World)]
	[Desc("Trait for watching over the game. Attach this to the world actor.")]
	public class MLManageInfo : TraitInfo<MLManage> { }
	public class MLManage : IGameOver, ITick
	{
		static void BotReport(World world)
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
		}

		// Use a stopwatch to report ticks per second, once per second
		readonly Stopwatch stopwatch = new Stopwatch();
		int lastReportTicks = 0;
		float tpsAverage = 0;
		void ITick.Tick(Actor self)
		{
			if (self.World.WorldTick == 1)
			{
				stopwatch.Start();
				return;
			}

			var deltaTicks = self.World.WorldTick - lastReportTicks;
			if (deltaTicks >= 1000 && stopwatch.ElapsedMilliseconds > 1000)
			{
				var ticksPerSecond = deltaTicks / (stopwatch.ElapsedMilliseconds / 1000f);
				tpsAverage = (tpsAverage + ticksPerSecond) / 2;
				BotReport(self.World);
				Console.WriteLine($"Ticks per second: {tpsAverage}, total ticks: {self.World.WorldTick}");
				stopwatch.Restart();
				lastReportTicks = self.World.WorldTick;
			}
		}

		void IGameOver.GameOver(World world)
		{
			BotReport(world);

			// Exit game.
			Game.RunAfterDelay(1000, () => Game.Exit());
		}
	}
}

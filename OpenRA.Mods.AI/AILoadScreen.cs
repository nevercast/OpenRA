using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRA.Mods.AI.Support;

namespace OpenRA.Mods.AI
{
	public sealed class AILoadScreen : ILoadScreen
	{
		ModData ModData { get; set; }

		public void Init(ModData m, Dictionary<string, string> info)
		{
			ModData = m;
		}

		public void Display() { }

		public bool BeforeLoad()
		{
			return InstallModContent();
		}

		public void StartGame(Arguments args)
		{
			var launchArgs = new AILaunchArguments(args);
			if (string.IsNullOrWhiteSpace(launchArgs.Map))
			{
				// Display a list of maps and their slot counts
				Console.WriteLine("Map not specified. Use AI.Map=\"Title or ID\" command line argument.");
				Console.WriteLine("Available maps:");
				var lobbyMaps = ModData.MapCache
					.Where(m => m.Status == MapStatus.Available)
					.Where(m => m.Visibility.HasFlag(MapVisibility.Lobby))
					.Select(m => new { m.Title, m.Uid, m.PlayerCount });

				foreach (var map in lobbyMaps)
					Console.WriteLine("{0} ({1}): {2} slots", map.Title, map.Uid, map.PlayerCount);

				Game.Exit();
				return;
			}
			else
			{
				var map = launchArgs.Map;
				var bots = (launchArgs.Bots ?? "rush,rush").Split(',');
				var seed = launchArgs.Seed ?? "0";

				Game.BotSkirmish(map, seed, bots);
			}
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		bool InstallModContent()
		{
			// TODO: Make this headless. Just assume quick install without asking.
			if (!ModData.Manifest.Contains<ModContent>())
				return true;

			var content = ModData.Manifest.Get<ModContent>();
			var contentInstalled = content.Packages
				.Where(p => p.Value.Required)
				.All(p => p.Value.TestFiles.All(f => File.Exists(Platform.ResolvePath(f))));

			if (contentInstalled)
				return true;

			Game.InitializeMod(content.ContentInstallerMod, new Arguments(new[] { "Content.Mod=" + ModData.Manifest.Id }));
			return false;
		}
	}
}

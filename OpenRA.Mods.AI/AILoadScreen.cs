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
			var map = launchArgs.Map ?? "7d5076d43a1849e6961263df547aea27680f470b";
			var bots = (launchArgs.Bots ?? "rush,rush").Split(',');
			var seed = launchArgs.Seed ?? "0";

			Game.BotSkirmish(map, seed, bots);
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

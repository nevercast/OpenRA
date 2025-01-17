#region Copyright & License Information
/*
 * Copyright 2007-2022 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System;
using System.IO;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Video;
using OpenRA.Widgets;

namespace OpenRA.Mods.Common.Scripting
{
	public static class Media
	{
		public static void PlayFMVFullscreen(World w, string movie, Action onComplete)
		{
			var playerRoot = Game.OpenWindow(w, "FMVPLAYER");
			var player = playerRoot.Get<VideoPlayerWidget>("PLAYER");

			try
			{
				player.Load(movie);
			}
			catch (FileNotFoundException)
			{
				Ui.CloseWindow();
				onComplete();
				return;
			}

			w.SetPauseState(true);

			player.PlayThen(() =>
			{
				Ui.CloseWindow();
				w.SetPauseState(false);
				onComplete();
			});
		}

		public static void PlayFMVInRadar(IVideo movie, Action onComplete)
		{
			var player = Ui.Root.Get<VideoPlayerWidget>("PLAYER");
			player.Open(movie);

			player.PlayThen(() =>
			{
				onComplete();
				player.CloseVideo();
			});
		}

		public static void StopFMVInRadar()
		{
			var player = Ui.Root.Get<VideoPlayerWidget>("PLAYER");
			player.Stop();
		}

		public static IVideo LoadVideo(Stream s)
		{
			return VideoLoader.GetVideo(s, true, Game.ModData.VideoLoaders);
		}
	}
}

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
using OpenRA.GameRules;
using OpenRA.Mods.Common.Traits;
using OpenRA.Widgets;

namespace OpenRA.Mods.Common.Widgets.Logic
{
	public class MusicPlayerLogic : ChromeLogic
	{
		[TranslationReference]
		static readonly string SoundMuted = "sound-muted";

		[TranslationReference]
		static readonly string NoSongPlaying = "no-song-playing";

		readonly ScrollPanelWidget musicList;
		readonly ScrollItemWidget itemTemplate;

		readonly MusicPlaylist musicPlaylist;
		MusicInfo currentSong = null;

		[ObjectCreator.UseCtor]
		public MusicPlayerLogic(Widget widget, World world, ModData modData, Action onExit)
		{
			var panel = widget;

			musicList = panel.Get<ScrollPanelWidget>("MUSIC_LIST");
			itemTemplate = musicList.Get<ScrollItemWidget>("MUSIC_TEMPLATE");
			musicPlaylist = world.WorldActor.Trait<MusicPlaylist>();

			BuildMusicTable();

			Func<bool> noMusic = () => !musicPlaylist.IsMusicAvailable || musicPlaylist.CurrentSongIsBackground || currentSong == null;
			panel.Get("NO_MUSIC_LABEL").IsVisible = () => !musicPlaylist.IsMusicAvailable;

			if (musicPlaylist.IsMusicAvailable)
			{
				panel.Get<LabelWidget>("MUTE_LABEL").GetText = () =>
				{
					if (Game.Settings.Sound.Mute)
						return modData.Translation.GetString(SoundMuted);

					return "";
				};
			}

			var playButton = panel.Get<ButtonWidget>("BUTTON_PLAY");
			playButton.OnClick = Play;
			playButton.IsDisabled = noMusic;
			playButton.IsVisible = () => false;

			var pauseButton = panel.Get<ButtonWidget>("BUTTON_PAUSE");
			pauseButton.IsDisabled = noMusic;
			pauseButton.IsVisible = () => false;

			var stopButton = panel.Get<ButtonWidget>("BUTTON_STOP");
			stopButton.OnClick = () => { musicPlaylist.Stop(); };
			stopButton.IsDisabled = noMusic;

			var nextButton = panel.Get<ButtonWidget>("BUTTON_NEXT");
			nextButton.OnClick = () => { currentSong = musicPlaylist.GetNextSong(); Play(); };
			nextButton.IsDisabled = noMusic;

			var prevButton = panel.Get<ButtonWidget>("BUTTON_PREV");
			prevButton.OnClick = () => { currentSong = musicPlaylist.GetPrevSong(); Play(); };
			prevButton.IsDisabled = noMusic;

			var shuffleCheckbox = panel.Get<CheckboxWidget>("SHUFFLE");
			shuffleCheckbox.IsDisabled = () => musicPlaylist.CurrentSongIsBackground;

			var repeatCheckbox = panel.Get<CheckboxWidget>("REPEAT");
			repeatCheckbox.IsDisabled = () => musicPlaylist.CurrentSongIsBackground;

			panel.Get<LabelWidget>("TIME_LABEL").GetText = () => "";
			var noSongPlaying = modData.Translation.GetString(NoSongPlaying);
			var musicTitle = panel.GetOrNull<LabelWidget>("TITLE_LABEL");
			if (musicTitle != null)
				musicTitle.GetText = () => currentSong != null ? currentSong.Title : noSongPlaying;

			var musicSlider = panel.Get<SliderWidget>("MUSIC_SLIDER");

			var songWatcher = widget.GetOrNull<LogicTickerWidget>("SONG_WATCHER");
			if (songWatcher != null)
			{
				songWatcher.OnTick = () =>
				{
					currentSong = null;
				};
			}

			var backButton = panel.GetOrNull<ButtonWidget>("BACK_BUTTON");
			if (backButton != null)
				backButton.OnClick = () => { Game.Settings.Save(); Ui.CloseWindow(); onExit(); };
		}

		public void BuildMusicTable()
		{
			if (!musicPlaylist.IsMusicAvailable)
				return;

			var music = musicPlaylist.AvailablePlaylist();
			currentSong = musicPlaylist.CurrentSong();

			musicList.RemoveChildren();
			foreach (var song in music)
			{
				var item = ScrollItemWidget.Setup(song.Filename, itemTemplate, () => currentSong == song, () => { currentSong = song; Play(); }, () => { });
				var label = item.Get<LabelWithTooltipWidget>("TITLE");
				WidgetUtils.TruncateLabelToTooltip(label, song.Title);

				item.Get<LabelWidget>("LENGTH").GetText = () => SongLengthLabel(song);
				musicList.AddChild(item);
			}

			if (currentSong != null && !musicPlaylist.CurrentSongIsBackground)
				musicList.ScrollToItem(currentSong.Filename);
		}

		void Play()
		{
			if (currentSong == null)
				return;

			musicList.ScrollToItem(currentSong.Filename);
			musicPlaylist.Play(currentSong);
		}

		static string SongLengthLabel(MusicInfo song)
		{
			return $"{song.Length / 60:D1}:{song.Length % 60:D2}";
		}
	}
}

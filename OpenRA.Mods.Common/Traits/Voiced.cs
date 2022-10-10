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

using OpenRA.Traits;

namespace OpenRA.Mods.Common.Traits
{
	[Desc("This actor has a voice.")]
	public class VoicedInfo : TraitInfo
	{
		[VoiceSetReference]
		[FieldLoader.Require]
		[Desc("Which voice set to use.")]
		public readonly string VoiceSet = null;

		[Desc("Multiply volume with this factor.")]
		public readonly float Volume = 1f;

		public override object Create(ActorInitializer init) { return new Voiced(this); }
	}

	public class Voiced : IVoiced
	{
		public readonly VoicedInfo Info;

		public Voiced(VoicedInfo info)
		{
			Info = info;
		}

		string IVoiced.VoiceSet => Info.VoiceSet;

		bool IVoiced.PlayVoice(Actor self, string phrase, string variant)
		{
			return false;
		}

		bool IVoiced.PlayVoiceLocal(Actor self, string phrase, string variant, float volume)
		{
			return false;
		}

		bool IVoiced.HasVoice(Actor self, string voice)
		{
			return false;
		}
	}
}

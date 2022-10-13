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
using OpenRA.Primitives;

namespace OpenRA.Platforms.Headless
{
	public class HeadlessPlatform : IPlatform
	{
		public IPlatformWindow CreateWindow(Size size, WindowMode windowMode, float scaleModifier, int batchSize, int videoDisplay, GLProfile profile, bool enableLegacyGL)
		{
			return new Sdl2PlatformWindow(size, windowMode, scaleModifier, batchSize, videoDisplay, profile, enableLegacyGL);
		}

		public ISoundEngine CreateSound(string device)
		{
			return new DummySoundEngine();
		}

		public IFont CreateFont(byte[] data)
		{
			return new FreeTypeFont(data);
		}
	}
}

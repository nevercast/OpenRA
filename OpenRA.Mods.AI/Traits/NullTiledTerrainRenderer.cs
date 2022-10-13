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
using System.Collections.Generic;
using System.IO;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Terrain;
using OpenRA.Mods.Common.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.AI.Traits
{
	[TraitLocation(SystemActors.World | SystemActors.EditorWorld)]
	public class NullTerrainRendererInfo : TraitInfo, ITiledTerrainRendererInfo
	{
		bool ITiledTerrainRendererInfo.ValidateTileSprites(ITemplatedTerrainInfo terrainInfo, Action<string> onError) => false;
		public override object Create(ActorInitializer init) { return new NullTerrainRenderer(init.World); }
	}

	public sealed class NullTerrainRenderer : IRenderTerrain, INotifyActorDisposing, ITiledTerrainRenderer
	{
		readonly Map map;
		readonly DefaultTerrain terrainInfo;
		readonly DefaultTileCache tileCache;
		bool disposed;

		public NullTerrainRenderer(World world)
		{
			map = world.Map;
			terrainInfo = map.Rules.TerrainInfo as DefaultTerrain;
			if (terrainInfo == null)
				throw new InvalidDataException("TerrainRenderer can only be used with the DefaultTerrain parser");

			tileCache = new DefaultTileCache(terrainInfo);
		}

		void IRenderTerrain.RenderTerrain(WorldRenderer wr, Viewport viewport) { }

		void INotifyActorDisposing.Disposing(Actor self)
		{
			if (disposed)
				return;

			tileCache.Dispose();
			disposed = true;
		}

		Sprite ITiledTerrainRenderer.MissingTile => tileCache.MissingTile;

		Sprite ITiledTerrainRenderer.TileSprite(TerrainTile r, int? variant)
		{
			return tileCache.TileSprite(r, variant);
		}

		Rectangle ITiledTerrainRenderer.TemplateBounds(TerrainTemplateInfo template)
		{
			return Rectangle.Empty;
		}

		IEnumerable<IRenderable> ITiledTerrainRenderer.RenderUIPreview(WorldRenderer wr, TerrainTemplateInfo t, int2 origin, float scale)
		{
			yield break;
		}

		IEnumerable<IRenderable> ITiledTerrainRenderer.RenderPreview(WorldRenderer wr, TerrainTemplateInfo t, WPos origin)
		{
			yield break;
		}
	}
}

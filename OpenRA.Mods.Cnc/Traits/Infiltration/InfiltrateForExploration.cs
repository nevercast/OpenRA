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

using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.Cnc.Traits
{
	[Desc("Steal and reset the owner's exploration.")]
	class InfiltrateForExplorationInfo : TraitInfo
	{
		[Desc("The `TargetTypes` from `Targetable` that are allowed to enter.")]
		public readonly BitSet<TargetableType> Types = default;

		[NotificationReference("Speech")]
		[Desc("Sound the victim will hear when they get sabotaged.")]
		public readonly string InfiltratedNotification = null;

		[Desc("Text notification the victim will see when they get sabotaged.")]
		public readonly string InfiltratedTextNotification = null;

		[NotificationReference("Speech")]
		[Desc("Sound the perpetrator will hear after successful infiltration.")]
		public readonly string InfiltrationNotification = null;

		[Desc("Text notification the perpetrator will see after successful infiltration.")]
		public readonly string InfiltrationTextNotification = null;

		public override object Create(ActorInitializer init) { return new InfiltrateForExploration(this); }
	}

	class InfiltrateForExploration : INotifyInfiltrated
	{
		readonly InfiltrateForExplorationInfo info;

		public InfiltrateForExploration(InfiltrateForExplorationInfo info)
		{
			this.info = info;
		}

		void INotifyInfiltrated.Infiltrated(Actor self, Actor infiltrator, BitSet<TargetableType> types)
		{
			if (!info.Types.Overlaps(types))
				return;

			TextNotificationsManager.AddTransientLine(info.InfiltratedTextNotification, self.Owner);
			TextNotificationsManager.AddTransientLine(info.InfiltrationTextNotification, infiltrator.Owner);

			infiltrator.Owner.Shroud.Explore(self.Owner.Shroud);
			var preventReset = self.Owner.PlayerActor.TraitsImplementing<IPreventsShroudReset>()
				.Any(p => p.PreventShroudReset(self));
			if (!preventReset)
				self.Owner.Shroud.ResetExploration();
		}
	}
}

using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using GrooveSharedUtils.Attributes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2.Items;
using System.Collections.ObjectModel;

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _Warbanner : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            IL.RoR2.TeleporterInteraction.ChargingState.OnEnter += ChargingState_OnEnter;
        }

        private void ChargingState_OnEnter(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchCallOrCallvirt(typeof(ReadOnlyCollection<TeamComponent>).GetProperty(nameof(ReadOnlyCollection<TeamComponent>.Count)).GetGetMethod()),
                x => x.MatchBlt(out _)
                );
            if (ilfound)
            {
                ILLabel iLLabel = c.DefineLabel();
                c.MarkLabel(iLLabel);
                ilfound = c.TryGotoPrev(MoveType.Before,
                x => x.MatchLdcI4((int)TeamIndex.Player),
                x => x.MatchCallOrCallvirt<TeamComponent>(nameof(TeamComponent.GetTeamMembers))
                );
                if (ilfound)
                {
                    c.Emit(OpCodes.Br, iLLabel);
                }
            }
        }
    }
}

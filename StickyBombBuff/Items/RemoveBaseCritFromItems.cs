using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using GrooveSharedUtils.Attributes;

namespace StickyBombBuff.Items
{
    [Configurable]
    public class RemoveBaseCritFromItems : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            int locCritIndex = -1;
            ilfound = c.TryGotoNext(
                x => x.MatchLdloc(out locCritIndex),
                x => x.MatchCallOrCallvirt<CharacterBody>("set_crit")
                );
            if (!ilfound)
            {
                return;
            }
            c.Index = 0;
            if(ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.HealOnCrit), out int locHealOnCritIndex, true))
            {
                TryRemoveBaseCrit(c, locCritIndex, locHealOnCritIndex);
            }
            c.Index = 0;
            if (ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.AttackSpeedOnCrit), out int locAttackSpeedOnCritIndex, true))
            {
                TryRemoveBaseCrit(c, locCritIndex, locAttackSpeedOnCritIndex);
            }
            c.Index = 0;
            if (ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.BleedOnHitAndExplode), out int locBleedOnHitAndExplodeIndex, true))
            {
                TryRemoveBaseCrit(c, locCritIndex, locBleedOnHitAndExplodeIndex);
            }
        }
        public void TryRemoveBaseCrit(ILCursor c, int locCritIndex, int locStackIndex)
        {
            ILLabel breakLabel = null;
            ilfound = c.TryGotoNext(MoveType.Before,
                x => x.MatchLdloc(locStackIndex),
                x => x.MatchLdcI4(0),
                x => x.MatchBle(out breakLabel),
                x => x.MatchLdloc(locCritIndex),
                x => x.MatchLdcR4(out _),
                x => x.MatchAdd(),
                x => x.MatchStloc(locCritIndex)
                );
            if (ilfound)
            {
                c.Emit(OpCodes.Br, breakLabel);
            }
        }
    }
}

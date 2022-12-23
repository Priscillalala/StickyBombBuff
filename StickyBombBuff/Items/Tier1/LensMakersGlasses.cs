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

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _LensMakersGlasses : StickyBombBuffModule
    {
        public static float critChance = 7f;
        public override void OnModInit()
        {
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if(ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.CritGlasses), out int locStackIndex))
            {
                ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchLdloc(locStackIndex),
                x => x.MatchConvR4(),
                x => x.MatchLdcR4(out _)
                );
                if (ilfound)
                {
                    c.Prev.Operand = critChance;
                }
            }
        }
    }
}

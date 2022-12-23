using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using GrooveSharedUtils.Attributes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2.Orbs;
using static RoR2.RoR2Content.Items;
using static RoR2.DLC1Content.Items;

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _TriTipDagger : StickyBombBuffModule
    {
        public static float bleedChance = 15f;
        public override void OnModInit()
        {
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.BleedOnHit), out int locStackIndex))
            {
                ilfound = c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcR4(out _),
                x => x.MatchLdloc(locStackIndex),
                x => x.MatchConvR4(),
                x => x.MatchMul(),
                x => x.MatchCallOrCallvirt<CharacterBody>("set_bleedChance")
                );
                if (ilfound)
                {
                    c.Next.Operand = bleedChance;
                }
            }
        }
    }
}

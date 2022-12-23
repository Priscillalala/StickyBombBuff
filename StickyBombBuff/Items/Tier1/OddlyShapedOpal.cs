using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using GrooveSharedUtils.Attributes;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _OddlyShapedOpal : StickyBombBuffModule
    {
        public static float armor = 20f;
        public override void OnModInit()
        {
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool found = c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld(typeof(DLC1Content.Buffs).GetField(nameof(DLC1Content.Buffs.OutOfCombatArmorBuff))),
                x => x.MatchCall<CharacterBody>(nameof(CharacterBody.HasBuff)),
                x => x.MatchBrtrue(out _),
                x => x.MatchLdcR4(out _),
                x => x.MatchBr(out _),
                x => x.MatchLdcR4(out _)
                );
            if (found)
            {
                c.Prev.Operand = armor;
            }
        }
    }
}

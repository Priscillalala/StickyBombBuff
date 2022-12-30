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
using RoR2.Projectile;
using RoR2.Orbs;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using HG;
using static RoR2.RoR2Content.Buffs;

namespace StickyBombBuff.Items.Tier3
{

    [Configurable]
    public class _ShatteringJustice : StickyBombBuffModule
    {
        public static float armorReduction = 40;
        public override void OnModInit()
        {
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld(typeof(RoR2Content.Buffs).GetField(nameof(Pulverized))),
                x => x.MatchCallOrCallvirt<CharacterBody>(nameof(CharacterBody.HasBuff)),
                x => x.MatchBrfalse(out _),
                x => x.MatchLdarg(0),
                x => x.MatchLdarg(0),
                x => x.MatchCallOrCallvirt<CharacterBody>("get_armor"),
                x => x.MatchLdcR4(out _)
                );
            if (ilfound)
            {
                c.Prev.Operand = armorReduction;
            }

        }
    }
}

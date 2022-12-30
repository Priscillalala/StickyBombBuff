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

namespace StickyBombBuff.Items.Tier2
{
    [Configurable]
    public class _HuntersHarpoon : StickyBombBuffModule
    {
        public static float speedPerBuff = 0.2f;
        public override void OnModInit()
        {
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.Before,
                    x => x.MatchLdcR4(out _),
                    x => x.MatchLdarg(0),
                    x => x.MatchLdsfld(typeof(DLC1Content.Buffs).GetField(nameof(DLC1Content.Buffs.KillMoveSpeed))),
                    x => x.MatchCallOrCallvirt<CharacterBody>(nameof(CharacterBody.GetBuffCount))
                );
            if (ilfound)
            {
                c.Next.Operand = speedPerBuff;
            }
        }
    }
}

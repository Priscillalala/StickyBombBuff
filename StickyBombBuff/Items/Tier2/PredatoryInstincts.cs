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

namespace StickyBombBuff.Items.Tier2
{

    [Configurable]
    public class _PredatoryInstincts : StickyBombBuffModule
    {
        public static float duration = 2f;
        public override void OnModInit()
        {
            IL.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
        }
        private void GlobalEventManager_OnCrit(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.Before,
                x => x.MatchLdsfld(typeof(RoR2Content.Buffs).GetField(nameof(RoR2Content.Buffs.AttackSpeedOnCrit))),
                x => x.MatchLdcR4(out _),
                x => x.MatchLdarg(out _),
                x => x.MatchMul(),
                x => x.MatchCallOrCallvirt<CharacterBody>(nameof(CharacterBody.AddTimedBuff))
            );
            if (ilfound)
            {
                c.Index++;
                c.Next.Operand = duration;
            }

        }
    }
}

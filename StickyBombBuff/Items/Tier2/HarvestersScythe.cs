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
    public class _HarvestersScythe : StickyBombBuffModule
    {
        public static float healing = 2;
        public static float healingPerStack = 2;
        public override void OnModInit()
        {
            IL.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
        }

        private void GlobalEventManager_OnCrit(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.HealOnCrit), out int locStackIndex, true))
            {
                ilfound = c.TryGotoNext(MoveType.Before,
                    x => x.MatchCallOrCallvirt<CharacterBody>("get_healthComponent"),
                    x => x.MatchLdcR4(out _),
                    x => x.MatchLdloc(out _),
                    x => x.MatchConvR4(),
                    x => x.MatchLdcR4(out _),
                    x => x.MatchMul(),
                    x => x.MatchAdd()
                );
                if (ilfound)
                {
                    c.Index++;
                    c.Next.Operand = healing - healingPerStack;
                    c.Index += 3;
                    c.Next.Operand = healingPerStack;
                }
            }
        }
    }
}

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

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _RollOfPennies : StickyBombBuffModule
    {
        public static int goldGained = 1;
        public override void OnModInit()
        {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchLdflda<HealthComponent>(nameof(HealthComponent.itemCounts)),
                x => x.MatchLdfld<HealthComponent.ItemCounts>(nameof(HealthComponent.ItemCounts.goldOnHurt))
                ) && c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcI4(out _),
                x => x.MatchStloc(out _),
                x => x.MatchNewobj<GoldOrb>()
                );
            if (ilfound)
            {
                c.Next.Operand = goldGained;
            }
        }
    }
}

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
                x => x.MatchLdfld<HealthComponent.ItemCounts>(nameof(HealthComponent.ItemCounts.goldOnHurt)),
                x => x.MatchLdloc(out _),
                x => x.MatchMul()
                );
            if (ilfound)
            {
                c.Index--;
                c.EmitDelegate<Func<int, int>>((_) => goldGained);
            }
        }
    }
}

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
    public class _RepulsionArmorPlate : StickyBombBuffModule
    {
        public static int flatDamageReduction = 3;
        public override void OnModInit()
        {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcR4(out _),
                x => x.MatchLdarg(0),
                x => x.MatchLdflda<HealthComponent>(nameof(HealthComponent.itemCounts)),
                x => x.MatchLdfld<HealthComponent.ItemCounts>(nameof(HealthComponent.ItemCounts.armorPlate)),
                x => x.MatchConvR4(),
                x => x.MatchMul()
                );
            if (ilfound)
            {
                c.Index++;
                c.EmitDelegate<Func<float, float>>((_) => flatDamageReduction);
            }
        }
    }
}

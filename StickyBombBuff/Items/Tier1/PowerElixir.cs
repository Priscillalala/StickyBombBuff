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
    public class _PowerElixir : StickyBombBuffModule
    {
        public static float healing = 75f;
        public override void OnModInit()
        {
            IL.RoR2.HealthComponent.UpdateLastHitTime += HealthComponent_UpdateLastHitTime;
        }

        private void HealthComponent_UpdateLastHitTime(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.After, x => x.MatchLdfld<HealthComponent.ItemCounts>(nameof(HealthComponent.ItemCounts.healingPotion)))
            && c.TryGotoNext(MoveType.Before,
            x => x.MatchLdcR4(out _),
            x => x.MatchLdloca(out _),
            x => x.MatchInitobj<ProcChainMask>(),
            x => x.MatchLdloc(out _),
            x => x.MatchCallOrCallvirt<HealthComponent>(nameof(HealthComponent.HealFraction))
            );
            if (ilfound)
            {
                c.Index++;
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate<Func<float, HealthComponent, float>>((_, healthComponent) => healing / healthComponent.fullHealth);
            }
        }
    }
}

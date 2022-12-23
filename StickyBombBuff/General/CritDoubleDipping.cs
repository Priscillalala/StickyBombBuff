using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using GrooveSharedUtils.Attributes;

namespace StickyBombBuff.General
{
    [Configurable]
    public class CritDoubleDipping : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.Before,
                x => x.MatchCallOrCallvirt<CharacterBody>("get_critMultiplier"),
                x => x.MatchMul(),
                x => x.MatchStloc(out _)
                );
            if (ilfound)
            {
                c.Index++;
                c.Emit(OpCodes.Dup);
                c.Emit(OpCodes.Ldarg, 1);
                c.EmitDelegate<Action<float, DamageInfo>>((critMultiplier, damageInfo) => damageInfo.damage *= critMultiplier);
            }
        }
    }
}

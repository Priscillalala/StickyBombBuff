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

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _StickyBomb : StickyBombBuffModule
    {
        public static float chance = 5f;
        public static float chancePerStack = 2.5f;
        public static float damageCoefficient = 1.8f;
        public static float damageCoefficientPerStack = 0.9f;
        public override void OnModInit()
        {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if(ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.StickyBomb), out int locStackIndex, true))
            {
                ilfound = c.TryGotoNext(MoveType.After,
                    x => x.MatchLdcR4(out _),
                    x => x.MatchLdloc(locStackIndex),
                    x => x.MatchConvR4(),
                    x => x.MatchMul()
                );
                if (ilfound)
                {
                    c.Emit(OpCodes.Ldloc, locStackIndex);
                    c.EmitDelegate<Func<float, int, float>>((_, stack) => GSUtil.StackScaling(chance, chancePerStack, stack));
                }
                ilfound = c.TryGotoNext(MoveType.Before,
                    x => x.MatchCallOrCallvirt(typeof(Util).GetMethod(nameof(Util.OnHitProcDamage))),
                    x => x.MatchStloc(out _),
                    x => x.MatchCallOrCallvirt(typeof(ProjectileManager).GetProperty(nameof(ProjectileManager.instance)).GetGetMethod()),
                    x => x.MatchLdstr("Prefabs/Projectiles/StickyBomb")
                );
                if (ilfound)
                {
                    c.Emit(OpCodes.Ldloc, locStackIndex);
                    c.EmitDelegate<Func<float, int, float>>((_, stack) => GSUtil.StackScaling(damageCoefficient, damageCoefficientPerStack, stack));
                }
            }
        }
    }
}

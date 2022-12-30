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
    public class _IgnitionTank : StickyBombBuffModule
    {
        public static float bonusDamage = 1f;
        public override void OnModInit()
        {
            IL.RoR2.StrengthenBurnUtils.CheckDotForUpgrade += StrengthenBurnUtils_CheckDotForUpgrade;
        }

        private void StrengthenBurnUtils_CheckDotForUpgrade(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(DLC1Content), nameof(DLC1Content.Items.StrengthenBurn), out int locStackIndex, true))
            {
                ilfound = c.TryGotoNext(MoveType.Before,
                    x => x.MatchLdcI4(out _),
                    x => x.MatchLdloc(locStackIndex),
                    x => x.MatchMul()
                );
                if (ilfound)
                {
                    c.Next.Operand = bonusDamage;
                }
            }
        }
    }
}

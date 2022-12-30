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
    public class _Infusion : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;        
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.Infusion), out int locStackIndex, true))
            {
                ilfound = c.TryGotoNext(MoveType.Before,
                    x => x.MatchLdloc(locStackIndex),
                    x => x.MatchStfld<InfusionOrb>(nameof(InfusionOrb.maxHpValue))
                );
                if (ilfound)
                {
                    c.Index++;
                    c.EmitDelegate<Func<int, int>>(_ => 1);
                }
            }
        }
    }
}

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
    public class _DeathMark : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if(ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.DeathMark), out int locStackIndex, true))
            {
                ilfound = c.TryGotoNext(MoveType.Before,
                    x => x.MatchCallOrCallvirt<CharacterBody>(nameof(CharacterBody.AddTimedBuff))
                );
                if (ilfound)
                {
                    c.EmitDelegate<Func<float, float>>(_ => 7);
                }
            }
        }
    }
}

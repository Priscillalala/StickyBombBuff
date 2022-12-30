using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Linq;
using GrooveSharedUtils.Attributes;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace StickyBombBuff.General
{
    [Configurable]
    public class Jellyfish : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            IL.EntityStates.JellyfishMonster.JellyNova.Detonate += JellyNova_Detonate;
        }

        private void JellyNova_Detonate(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if(ilfound = c.TryGotoNext(MoveType.Before, x => x.MatchLdcR4(out _), x => x.MatchStfld<BlastAttack>(nameof(BlastAttack.procCoefficient))))
            {
                c.Next.Operand = 1f;
            }
        }
    }
}

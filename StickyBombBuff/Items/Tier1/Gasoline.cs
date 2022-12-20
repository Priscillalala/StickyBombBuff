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
    public class _Gasoline : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            IL.RoR2.GlobalEventManager.ProcIgniteOnKill += GlobalEventManager_ProcIgniteOnKill;
        }

        private void GlobalEventManager_ProcIgniteOnKill(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.After, x => x.MatchCallOrCallvirt<BlastAttack>(nameof(BlastAttack.Fire)), x => x.MatchPop());             
            if (ilfound)
            {
                int lastIndex = c.Index;
                ilfound = c.TryGotoPrev(MoveType.Before, x => x.MatchNewobj<BlastAttack>());
                if (ilfound)
                {
                    c.RemoveRange(lastIndex - c.Index);
                }
            }
        }
    }
}

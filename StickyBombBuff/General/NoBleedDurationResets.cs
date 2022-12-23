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
    public class NoBleedDurationResets : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            IL.RoR2.DotController.AddDot += DotController_AddDot;
        }

        private void DotController_AddDot(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ILLabel[] switchLabels = null;
            ILLabel breakLabel = null;
            ilfound = c.TryGotoNext(MoveType.Before,
                x => x.MatchLdarg(3),
                x => x.MatchSwitch(out switchLabels),
                x => x.MatchBr(out breakLabel)
                );
            if (ilfound)
            {
                c.Index++;
                switchLabels[(int)DotController.DotIndex.Bleed] = breakLabel;
                c.Next.Operand = switchLabels;
            }
        }
    }
}

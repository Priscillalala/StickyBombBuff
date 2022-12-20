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

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _PersonalShieldGenerator : StickyBombBuffModule
    {
        public static float shield = 25f;
        public override void OnModInit()
        {
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if(ilfound = SBBUtil.TryFindStackLocIndex(c, nameof(RoR2Content), "PersonalShield", out int locStackIndex))
            {
                int locShieldIndex = -1;
                ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchLdloc(out locShieldIndex),
                x => x.MatchLdloc(locStackIndex)
                ) && c.TryGotoNext(MoveType.Before,
                x => x.MatchAdd(),
                x => x.MatchStloc(locShieldIndex)
                );
                if (ilfound)
                {
                    c.Emit(OpCodes.Ldloc, locStackIndex);
                    c.EmitDelegate<Func<float, int, float>>((_, stack) => shield * stack);
                }
            }
        }
    }
}

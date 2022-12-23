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
    public class _CautiousSlug : StickyBombBuffModule
    {
        public static float regenMultipler = 1.5f;
        public override void OnModInit()
        {
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.HealWhileSafe), out int locStackIndex))
            {
                int locSlugRegenIndex = -1;
                ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchLdcR4(out _),
                x => x.MatchLdloc(locStackIndex),
                x => x.MatchConvR4(),
                x => x.MatchMul(),
                x => x.MatchLdloc(out _),
                x => x.MatchMul(),
                x => x.MatchStloc(out locSlugRegenIndex)
                );
                if (ilfound)
                {
                    c.Emit(OpCodes.Ldc_R4, 0f);
                    c.Emit(OpCodes.Stloc, locSlugRegenIndex);
                }
                int locRegenIndex = -1;
                ilfound = c.TryGotoNext(MoveType.AfterLabel,
                x => x.MatchCallOrCallvirt<Mathf>(nameof(Mathf.Min)),
                x => x.MatchStloc(out locRegenIndex)
                );
                if (ilfound)
                {
                    c.Emit(OpCodes.Ldloc, locRegenIndex);
                    c.Emit(OpCodes.Ldloc, locStackIndex);
                    c.Emit(OpCodes.Ldarg, 0);
                    c.EmitDelegate<Func<float, int, CharacterBody, float>>((regen, stack, body) =>
                    {
                        if (stack > 0 && body.outOfDanger)
                        {
                            return regen * (1f + stack * regenMultipler);
                        }
                        return regen;
                    });
                    c.Emit(OpCodes.Stloc, locRegenIndex);
                }
            }
        }


    }
}

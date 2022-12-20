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
    public class _Medkit : StickyBombBuffModule
    {
        public static float healing = 10f;
        public static float healingPerStack = 10f;
        public static float delay = 1.1f;
        public override void OnModInit()
        {
            IL.RoR2.HealthComponent.UpdateLastHitTime += HealthComponent_UpdateLastHitTime;
            IL.RoR2.CharacterBody.RemoveBuff_BuffIndex += CharacterBody_RemoveBuff_BuffIndex; ;
        }

        private void CharacterBody_RemoveBuff_BuffIndex(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            int locStackIndex = -1;
            ilfound = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField(nameof(RoR2Content.Items.Medkit))),
            x => x.MatchCallOrCallvirt<Inventory>(nameof(Inventory.GetItemCount)),
            x => x.MatchStloc(out locStackIndex)) 
            && c.TryGotoNext(MoveType.Before, x => x.MatchCallOrCallvirt<HealthComponent>(nameof(HealthComponent.Heal)))
            && c.TryGotoPrev(MoveType.After,
            x => x.MatchCallOrCallvirt<CharacterBody>("get_healthComponent"),
            x => x.MatchLdloc(out _),
            x => x.MatchLdloc(out _),
            x => x.MatchAdd());
            if (ilfound)
            {
                c.Emit(OpCodes.Ldloc, locStackIndex);
                c.EmitDelegate<Func<float, int, float>>((_, stack) => GSUtil.StackScaling(healing, healingPerStack, stack));
            }
        }

        private void HealthComponent_UpdateLastHitTime(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld(typeof(RoR2Content.Buffs).GetField(nameof(RoR2Content.Buffs.MedkitHeal))),
            x => x.MatchLdcR4(out _),
            x => x.MatchCallOrCallvirt<CharacterBody>(nameof(CharacterBody.AddTimedBuff))
            );
            if (ilfound)
            {
                c.Index--;
                c.Prev.Operand = delay;
            }
        }
    }
}

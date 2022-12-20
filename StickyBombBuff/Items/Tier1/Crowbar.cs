using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using GrooveSharedUtils.Attributes;

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _Crowbar : StickyBombBuffModule
    {
        public static float damageBonus = 0.5f;
        public static float damageBonusPerStack = 0.3f;
        public override void OnModInit()
        {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if(ilfound = SBBUtil.HealthComponent_TakeDamage_TryFindLocDamageIndex(c, out int locDamageIndex))
            {
                int locStackIndex = -1;
                ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField(nameof(RoR2Content.Items.Crowbar))),
                x => x.MatchCallOrCallvirt<Inventory>(nameof(Inventory.GetItemCount)),
                x => x.MatchStloc(out locStackIndex)
                ) && c.TryGotoNext(MoveType.Before,
                x => x.MatchAdd(),
                x => x.MatchMul(),
                x => x.MatchStloc(locDamageIndex)
                );
                if (ilfound)
                {
                    c.Emit(OpCodes.Ldloc, locStackIndex);
                    c.EmitDelegate<Func<float, int, float>>((_, stack) => GSUtil.StackScaling(damageBonus, damageBonusPerStack, stack));
                }
            }
        }
    }
}

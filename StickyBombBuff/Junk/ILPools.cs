/*using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using GrooveSharedUtils.Attributes;

namespace StickyBombBuff.Junk
{
    [ModuleOrderPriority(LoadOrder.Delayed)]
    public class ILPools : StickyBombBuffModule
    {
        public delegate void TakeDamageContext(int locDamageIndex);
        public static event TakeDamageContext HealthComponent_TakeDamage;
        public override void OnModInit()
        {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }
        private void HealthComponent_TakeDamage(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField(nameof(RoR2Content.Items.BossDamageBonus))),
                x => x.MatchCallOrCallvirt<Inventory>(nameof(Inventory.GetItemCount)
                );
        }
    }
}*/

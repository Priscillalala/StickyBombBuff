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
    public class _MonsterTooth : StickyBombBuffModule
    {
        public static float healing = 4f;
        public static float healingPerStack = 4f;
        public override void OnModInit()
        {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            int locStackIndex = -1;
            ilfound = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField(nameof(RoR2Content.Items.Tooth))),
            x => x.MatchCallOrCallvirt<Inventory>(nameof(Inventory.GetItemCount)),
            x => x.MatchStloc(out locStackIndex)
            ) && c.TryGotoNext(MoveType.Before, x => x.MatchStfld<HealthPickup>(nameof(HealthPickup.flatHealing)));
            if (ilfound)
            {
                c.Emit(OpCodes.Ldloc, locStackIndex);
                c.EmitDelegate<Func<float, int, float>>((_, stack) => GSUtil.StackScaling(healing, healingPerStack, stack));
            }
            ilfound = c.TryGotoNext(MoveType.Before, x => x.MatchStfld<HealthPickup>(nameof(HealthPickup.fractionalHealing)));
            if (ilfound)
            {
                c.EmitDelegate<Func<float, float>>((_) => 0);

            }
        }
    }
}

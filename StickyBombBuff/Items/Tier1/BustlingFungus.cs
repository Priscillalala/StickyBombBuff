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
    public class _BustlingFungus : StickyBombBuffModule
    {
        public static float delay = 2f;
        public static float healingPerSecond = 15f;
        public static float healingPerSecondPerStack = 7.5f;
        public override void OnModInit()
        {
            IL.RoR2.Items.MushroomBodyBehavior.FixedUpdate += MushroomBodyBehavior_FixedUpdate;
        }

        private void MushroomBodyBehavior_FixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchCallOrCallvirt<BaseItemBodyBehavior>("get_body"),
                x => x.MatchCallOrCallvirt<CharacterBody>(nameof(CharacterBody.GetNotMoving))
                );
            if (ilfound)
            {
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate<Func<bool, BaseItemBodyBehavior, bool>>((_, behaviour) => behaviour.body.notMovingStopwatch >= delay);
            }
            if(ilfound = c.TryGotoNext(MoveType.Before, x => x.MatchStfld<HealingWard>(nameof(HealingWard.healFraction))))
            {
                c.EmitDelegate<Func<float, float>>((_) => 0);
            }
            if (ilfound = c.TryGotoNext(MoveType.Before, x => x.MatchStfld<HealingWard>(nameof(HealingWard.healPoints))))
            {
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate<Func<float, MushroomBodyBehavior, float>>((_, behaviour) => behaviour.mushroomHealingWard.interval * GSUtil.StackScaling(healingPerSecond, healingPerSecondPerStack, behaviour.stack));
            }
        }
    }
}

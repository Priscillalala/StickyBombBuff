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
using RoR2.Projectile;
using RoR2.Orbs;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using HG;
using static RoR2.RoR2Content.Items;

namespace StickyBombBuff.Items.Tier3
{

    [Configurable]
    public class _Brainstalks : StickyBombBuffModule
    {
        public static float duration = 3;
        public static float durationPerStack = 2;
        public override void OnModInit()
        {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if(ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(KillEliteFrenzy), out int locStackIndex, true))
            {
                ilfound = c.TryGotoNext(MoveType.After, x => x.MatchLdsfld(typeof(RoR2Content.Buffs).GetField(nameof(RoR2Content.Buffs.NoCooldowns))))
                    && c.TryGotoNext(MoveType.Before, x => x.MatchCallOrCallvirt<CharacterBody>(nameof(CharacterBody.AddTimedBuff)));
                if (ilfound)
                {
                    c.Emit(OpCodes.Ldloc, locStackIndex);
                    c.EmitDelegate<Func<float, int, float>>((_, stack) => GSUtil.StackScaling(duration, durationPerStack, stack));
                }
            }
        }
    }
}

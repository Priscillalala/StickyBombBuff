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
using System.Linq;
using static RoR2.RoR2Content.Items;
using System.Reflection;

namespace StickyBombBuff.Items.Tier3
{

    [Configurable]
    public class _HappiestMask : StickyBombBuffModule
    {
        public static float chance = 10;
        public static int damageBoost = 30;
        public override void OnModInit()
        {
            IL.RoR2.Util.TryToCreateGhost += Util_TryToCreateGhost;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void Util_TryToCreateGhost(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            int locMasterIndex = -1;
            ilfound = c.TryGotoNext(MoveType.After,
                    x => x.MatchCallOrCallvirt<MasterSummon>(nameof(MasterSummon.Perform)),
                    x => x.MatchStloc(out locMasterIndex)
                    );
            if (ilfound)
            {
                c.Emit(OpCodes.Ldloc, locMasterIndex);
                c.EmitDelegate<Action<CharacterMaster>>(master => 
                {
                    if (master)
                    {
                        master.inventory.ResetItem(BoostDamage);
                        master.inventory.GiveItem(BoostDamage, damageBoost);
                    }
                });
            }
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if(ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(GhostOnKill), out int locStackIndex, true))
            {
                ilfound = c.TryGotoNext(MoveType.Before,
                    x => x.MatchLdcR4(out _),
                    x => x.MatchLdloc(out _),
                    x => x.MatchCallOrCallvirt(typeof(Util).GetMethods().FirstOrDefault(m => m.Name == nameof(Util.CheckRoll) && m.GetParameters().Length == 2))
                    );
                if (ilfound)
                {
                    c.Next.Operand = chance;
                }
            }
        }
    }
}

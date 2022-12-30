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

namespace StickyBombBuff.Items.Tier2
{
    [Configurable]
    public class _Chronobauble : StickyBombBuffModule
    {
        public static float chance = 30f;
        public static float chancePerStack = 5f;
        public static float duration = 1;
        public override void OnModInit()
        {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if(ilfound = SBBUtil.TryFindStackLocIndex(c, typeof(RoR2Content), nameof(RoR2Content.Items.SlowOnHit), out int locStackIndex, true))
            {
                ILLabel breakLabel = null;
                int locAttackerMasterIndex = -1;
                ilfound = c.TryGotoPrev(MoveType.After,
                    x => x.MatchLdloc(out locAttackerMasterIndex),
                    x => x.MatchCallOrCallvirt<CharacterMaster>("get_inventory")
                ) && c.TryGotoNext(MoveType.After,
                    x => x.MatchBle(out breakLabel)
                );
                if (ilfound)
                {
                    c.Emit(OpCodes.Ldloc, locAttackerMasterIndex);
                    c.Emit(OpCodes.Ldloc, locStackIndex);
                    c.EmitDelegate<Func<CharacterMaster, int, bool>>((attackerMaster, stack) => Util.CheckRoll(GSUtil.StackScaling(chance, chancePerStack, stack), attackerMaster));
                    c.Emit(OpCodes.Brfalse, breakLabel);
                }
                ilfound = c.TryGotoNext(MoveType.After,
                    x => x.MatchLdsfld(typeof(RoR2Content.Buffs).GetField(nameof(RoR2Content.Buffs.Slow60))),
                    x => x.MatchLdcR4(out _)
                );
                if (ilfound)
                {
                    c.Prev.Operand = duration;
                }
            }
        }
    }
}

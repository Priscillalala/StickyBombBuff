using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace StickyBombBuff
{

    public static class SBBUtil
    {
        public static bool HealthComponent_TakeDamage_TryFindLocDamageIndex(ILCursor iLCursor, out int locDamageIndex)
        {
            int i = -1;
            bool found = iLCursor.TryGotoNext(MoveType.After,
                x => x.MatchLdarg(1),
                x => x.MatchLdfld<DamageInfo>(nameof(DamageInfo.damage)),
                x => x.MatchStloc(out i)
                );
            locDamageIndex = i;
            iLCursor.Index = 0;
            return found;
        }
        public static bool TryFindStackLocIndex(ILCursor iLCursor, string contentName, string itemName, out int locStackIndex)
        {
            int i = -1;
            bool found = iLCursor.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld(contentName + ".Items", itemName),
                x => x.MatchCallOrCallvirt<Inventory>(nameof(Inventory.GetItemCount)),
                x => x.MatchStloc(out i)
                );
            locStackIndex = i;
            iLCursor.Index = 0;
            return found;
        }
    }
}

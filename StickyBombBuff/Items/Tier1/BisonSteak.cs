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
    public class _BisonSteak : StickyBombBuffModule
    {
        public override void OnModInit()
        {
#pragma warning disable
            GSUtil.AddressablesLoad<ItemDef>("RoR2/Base/FlatHealth/FlatHealth.asset").deprecatedTier = ItemTier.NoTier;
#pragma warning restore
        }
    }
}

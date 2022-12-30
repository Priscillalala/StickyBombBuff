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

namespace StickyBombBuff.Items.Tier2
{

    [Configurable]
    public class _RedWhip : StickyBombBuffModule
    {

        public override void OnModInit()
        {
            On.RoR2.Items.RedWhipBodyBehavior.SetProvidingBuff += RedWhipBodyBehavior_SetProvidingBuff;
        }

        private void RedWhipBodyBehavior_SetProvidingBuff(On.RoR2.Items.RedWhipBodyBehavior.orig_SetProvidingBuff orig, BaseItemBodyBehavior self, bool shouldProvideBuff)
        {
            orig(self, shouldProvideBuff && self.body.outOfDanger);
        }
    }
}

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
    public class _LeptonDaisy : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            On.RoR2.HoldoutZoneController.Awake += HoldoutZoneController_Awake;        
        }

        private void HoldoutZoneController_Awake(On.RoR2.HoldoutZoneController.orig_Awake orig, HoldoutZoneController self)
        {
            orig(self);
            if (!self.GetComponent<TeleporterInteraction>())
            {
                self.applyHealingNova = false;
            }
        }
    }
}

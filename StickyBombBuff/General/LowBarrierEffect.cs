using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using GrooveSharedUtils.Attributes;
using RoR2.UI;

namespace StickyBombBuff.General
{
    [Configurable]
    public class LowBarrierEffect : StickyBombBuffModule
    {
        public float addition = 0.75f;
        public float multiplier = 0.25f;
        public float timeScalar = 15f;
        public override void OnModInit()
        {
            On.RoR2.UI.HealthBar.UpdateBarInfos += HealthBar_UpdateBarInfos;
        }

        private void HealthBar_UpdateBarInfos(On.RoR2.UI.HealthBar.orig_UpdateBarInfos orig, HealthBar self)
        {
            orig(self);
            if(self.source && self.barInfoCollection.barrierBarInfo.enabled && (self.source.barrier / self.source.fullCombinedHealth) <= HealthComponent.lowHealthFraction)
            {
                self.barInfoCollection.barrierBarInfo.color.a *= Mathf.Sin(Time.fixedTime * timeScalar) * multiplier + addition;
            }
        }
    }
}

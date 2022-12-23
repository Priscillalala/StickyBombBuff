using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using GrooveSharedUtils.Attributes;

namespace StickyBombBuff.Items
{
    [Configurable]
    public class TotalDamageToAdditiveDamage : StickyBombBuffModule
    {
        public static string TryAddString(string localizedTotalDamageString) => StickyBombBuffPlugin.instance && StickyBombBuffPlugin.instance.ModuleEnabled<TotalDamageToAdditiveDamage>() ? string.Empty : localizedTotalDamageString;
        public override void OnModInit()
        {
            On.RoR2.Util.OnHitProcDamage += Util_OnHitProcDamage;
        }

        private float Util_OnHitProcDamage(On.RoR2.Util.orig_OnHitProcDamage orig, float damageThatProccedIt, float damageStat, float damageCoefficient)
        {
            float a = Mathf.Max(1f, damageThatProccedIt + (damageCoefficient - 1f) * damageStat);
            return Mathf.Min(orig(damageThatProccedIt, damageStat, damageCoefficient), a);
        }
    }
}

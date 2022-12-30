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
    public class _StealthKit : StickyBombBuffModule
    {
        public static float duration = 3f;
        public static float durationPerStack = 1.5f;
        public override void OnModInit()
        {
            ItemBehaviourUnlinker.Add<PhasingBodyBehavior>();
        }
        public class OldPhasingBehaviour : BaseItemBodyBehavior, IOnTakeDamageServerReceiver
        {
            [ItemDefAssociation(useOnServer = true, useOnClient = false)]
            public static ItemDef GetItemDef() => StickyBombBuffPlugin.instance.ModuleEnabled<_StealthKit>() ? RoR2Content.Items.Phasing : null;
            private GameObject effectPrefab;
            private void Start()
            {
                this.effectPrefab = GSUtil.LegacyLoad<GameObject>("Prefabs/Effects/ProcStealthkit");
            }
            public void OnTakeDamageServer(DamageReport damageReport)
            {
                HealthComponent healthComponent = body.healthComponent;
                if (healthComponent && healthComponent.alive && Util.CheckRoll(damageReport.damageDealt / healthComponent.fullCombinedHealth * 100f, body.master))
                {
                    float buffDuration = GSUtil.StackScaling(duration, durationPerStack, stack);
                    body.AddTimedBuff(RoR2Content.Buffs.Cloak, buffDuration);
                    body.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, buffDuration);
                    EffectManager.SpawnEffect(this.effectPrefab, new EffectData
                    {
                        origin = base.transform.position,
                        rotation = Quaternion.identity
                    }, true);
                }
            }
        }
    }
}

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
    [ModuleOrderPriority(LoadOrder.Delayed)]
    public class _Shuriken : StickyBombBuffModule
    {
        public static float chance = 25f;
        public static float damageCoefficient = 2f; 
        public static float damageCoefficientPerStack = 2f;
        public override void OnModInit()
        {
            ItemBehaviourUnlinker.Add<PrimarySkillShurikenBehavior>();
            if (!StickyBombBuffPlugin.instance.ModuleEnabled<RemoveBaseCritFromItems>())
            {
                On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            }
            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self.HasItem(DLC1Content.Items.PrimarySkillShuriken))
            {
                self.crit += 5f;
            }
        }

        private void GlobalEventManager_OnCrit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, DamageInfo damageInfo, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask)
        {
            if (body && !damageInfo.procChainMask.HasProc(ProcType.MicroMissile) && body.HasItem(DLC1Content.Items.PrimarySkillShuriken, out int stack) && Util.CheckRoll(chance * procCoefficient, master))
            {
                Ray aimRay;
                if(body.TryGetComponent(out InputBankTest inputBank))
                {
                    aimRay = inputBank.GetAimRay();
                }
                else
                {
                    aimRay = new Ray(body.transform.position, body.transform.forward);
                }
                Quaternion lhs = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward);
                Quaternion rhs = Quaternion.AngleAxis(0f + UnityEngine.Random.Range(0f, 1f), Vector3.left);
                Quaternion randomRollPitch = lhs * rhs;
                ProcChainMask shurikenProcChainMask = procChainMask;
                shurikenProcChainMask.AddProc(ProcType.MicroMissile);
                float shurikenDamageCoefficient = GSUtil.StackScaling(damageCoefficient, damageCoefficientPerStack, stack);
                float damage = Util.OnHitProcDamage(damageInfo.damage, body.damage, shurikenDamageCoefficient);
                ProjectileManager.instance.FireProjectile(new FireProjectileInfo
                {
                    damage = damage,
                    crit = true,
                    damageColorIndex = DamageColorIndex.Item,
                    position = aimRay.origin,
                    procChainMask = shurikenProcChainMask,
                    force = 0f,
                    owner = damageInfo.attacker,
                    projectilePrefab = GSUtil.LegacyLoad<GameObject>("Prefabs/Projectiles/ShurikenProjectile"),
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction) * randomRollPitch,
                    target = null
                });
            }
            orig(self, body, damageInfo, master, procCoefficient, procChainMask);
        }
    }
}

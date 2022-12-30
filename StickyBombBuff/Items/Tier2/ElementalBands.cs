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
using static RoR2.RoR2Content.Items;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace StickyBombBuff.Items.Tier2
{

    [Configurable]
    public class _ElementalBands : StickyBombBuffModule
    {
		public static float iceRingDamageCoefficient = 2.5f;
		public static float iceRingDamageCoefficientPerStack = 1.25f;
		public static float fireRingDamageCoefficient = 5f;
		public static float fireRingDamageCoefficientPerStack = 2.5f;
		public override void OnModInit()
        {
			ItemBehaviourUnlinker.Add<CharacterBody.ElementalRingsBehavior>();
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElementalRings/FireTornado.prefab")).Completed += FireTornado_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElementalRings/FireTornadoGhost.prefab")).Completed += FireTornadoGhost_Completed;

			Common.Events.onHitEnemyServer += Events_onHitEnemyServer;
        }

        private void FireTornadoGhost_Completed(AsyncOperationHandle<GameObject> obj)
        {
			obj.Completed -= FireTornadoGhost_Completed;
			foreach(ParticleSystem particleSystem in obj.Result.GetComponentsInChildren<ParticleSystem>())
            {
				var main = particleSystem.main;
				main.scalingMode = ParticleSystemScalingMode.Local;
				particleSystem.transform.localScale *= 0.37f;
            }
			Light light = obj.Result.transform.Find("Point Light").GetComponent<Light>();
			light.range *= 0.37f;
		}

		private void FireTornado_Completed(AsyncOperationHandle<GameObject> obj)
        {
			obj.Completed -= FireTornado_Completed;
			if(obj.Result.TryGetComponent(out ProjectileSimple projectileSimple))
            {
				projectileSimple.desiredForwardSpeed = 5f;
            }
			Transform hitBox = obj.Result.transform.Find("HitBox");
			hitBox.localScale *= 0.37f;
			if(obj.Result.TryGetComponent(out SphereCollider sphereCollider))
            {
				sphereCollider.isTrigger = false;
            }

		}

        private void Events_onHitEnemyServer(DamageInfo damageInfo, GameObject victim)
        {
			if (!damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody)) return;
			CharacterMaster attackerMaster = attackerBody.master;
			if (!attackerMaster) return;
			int iceRingStack = attackerMaster.inventory.GetItemCount(IceRing);
			int fireRingStack = attackerMaster.inventory.GetItemCount(FireRing); 
			if ((iceRingStack | fireRingStack) > 0)
			{
				Vector3 position = damageInfo.position;
				if (Util.CheckRoll(8f * damageInfo.procCoefficient, attackerMaster))
				{
					ProcChainMask procChainMask = damageInfo.procChainMask;
					procChainMask.AddProc(ProcType.Rings);
					if (iceRingStack > 0 && victim.TryGetComponent(out CharacterBody victimBody))
					{
						float damageCoefficient = GSUtil.StackScaling(iceRingDamageCoefficient, iceRingDamageCoefficientPerStack, iceRingStack);
						float iceRingDamage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient);
						DamageInfo damageInfo2 = new DamageInfo
						{
							damage = iceRingDamage,
							damageColorIndex = DamageColorIndex.Item,
							damageType = DamageType.Generic,
							attacker = damageInfo.attacker,
							crit = damageInfo.crit,
							force = Vector3.zero,
							inflictor = null,
							position = position,
							procChainMask = procChainMask,
							procCoefficient = 1f
						};
						EffectManager.SimpleImpactEffect(GSUtil.LegacyLoad<GameObject>("Prefabs/Effects/ImpactEffects/IceRingExplosion"), position, Vector3.up, true);
						victimBody.AddTimedBuff(RoR2Content.Buffs.Slow80, 3f);
                        if (victimBody.healthComponent)
                        {
							victimBody.healthComponent.TakeDamage(damageInfo2);
                        }
					}
					if (fireRingStack > 0)
					{
						GameObject gameObject = GSUtil.LegacyLoad<GameObject>("Prefabs/Projectiles/FireTornado"); 
						float resetInterval = gameObject.GetComponent<ProjectileOverlapAttack>().resetInterval;
						float lifetime = gameObject.GetComponent<ProjectileSimple>().lifetime;
						float damageCoefficient = GSUtil.StackScaling(fireRingDamageCoefficient, fireRingDamageCoefficientPerStack, fireRingStack);
						float fireRingDamage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient) / lifetime * resetInterval;
						float speedOverride = 0f;
						Quaternion rotation2 = Quaternion.identity;
						Vector3 vector = position - attackerBody.aimOrigin;
						vector.y = 0f;
						if (vector != Vector3.zero)
						{
							speedOverride = -1f;
							rotation2 = Util.QuaternionSafeLookRotation(vector, Vector3.up);
						}
						ProjectileManager.instance.FireProjectile(new FireProjectileInfo
						{
							damage = fireRingDamage,
							crit = damageInfo.crit,
							damageColorIndex = DamageColorIndex.Item,
							position = position,
							procChainMask = procChainMask,
							force = 0f,
							owner = damageInfo.attacker,
							projectilePrefab = gameObject,
							rotation = rotation2,
							speedOverride = speedOverride,
							target = null
						});
					}
				}
			}
		}
    }
}

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
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using HG;
using static RoR2.DLC1Content.Items;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using Unity;
using EntityStates;
using static StickyBombBuff.Items.Tier3._ResonanceDisc;

namespace StickyBombBuff.Items.Tier3.EntityStates
{
    public class FireMainBeamState : OldLaserTurbineBaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (base.isAuthority)
			{
				this.initialAimRay = base.GetAimRay();
			}
			if (NetworkServer.active)
			{
				base.laserTurbineController.ExpendCharge();
				this.isCrit = base.ownerBody.RollCrit();
				this.FireBeamServer(this.initialAimRay, FireMainBeamState.forwardBeamTracerEffect, FireMainBeamState.mainBeamMaxDistance, true);
			}
			base.laserTurbineController.showTurbineDisplay = false;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority && base.fixedAge >= FireMainBeamState.baseDuration)
			{
				this.outer.SetNextState(new RechargeState());
			}
		}

		public override void OnExit()
		{
			if (NetworkServer.active && !this.outer.destroying)
			{
				Vector3 direction = this.initialAimRay.origin - this.beamHitPosition;
				Ray aimRay = new Ray(this.beamHitPosition, direction);
				this.FireBeamServer(aimRay, FireMainBeamState.backwardBeamTracerEffect, direction.magnitude, false);
			}
			base.laserTurbineController.showTurbineDisplay = true;
			base.OnExit();
		}

		private void FireBeamServer(Ray aimRay, GameObject tracerEffectPrefab, float maxDistance, bool isInitialBeam)
		{
			bool didHit = false;
			BulletAttack bulletAttack = new BulletAttack
			{
				origin = aimRay.origin,
				aimVector = aimRay.direction,
				bulletCount = 1U,
				damage = base.GetDamage() * FireMainBeamState.mainBeamDamageCoefficient,
				damageColorIndex = DamageColorIndex.Item,
				damageType = DamageType.Generic,
				falloffModel = BulletAttack.FalloffModel.None,
				force = FireMainBeamState.mainBeamForce,
				hitEffectPrefab = FireMainBeamState.mainBeamImpactEffect,
				HitEffectNormal = false,
				hitMask = LayerIndex.entityPrecise.mask,
				isCrit = this.isCrit,
				maxDistance = maxDistance,
				minSpread = 0f,
				maxSpread = 0f,
				muzzleName = "",
				owner = base.ownerBody.gameObject,
				procChainMask = default(ProcChainMask),
				procCoefficient = FireMainBeamState.mainBeamProcCoefficient,
				queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
				radius = FireMainBeamState.mainBeamRadius,
				smartCollision = true,
				sniper = false,
				spreadPitchScale = 1f,
				spreadYawScale = 1f,
				stopperMask = LayerIndex.world.mask,
				tracerEffectPrefab = (isInitialBeam ? tracerEffectPrefab : null),
				weapon = base.gameObject
			};
			TeamIndex teamIndex = base.ownerBody.teamComponent.teamIndex;
			bulletAttack.hitCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit info)
			{
				bool flag = BulletAttack.defaultHitCallback(_bulletAttack, ref info);
				if (!isInitialBeam)
				{
					return true;
				}
				if (flag)
				{
					HealthComponent healthComponent = info.hitHurtBox ? info.hitHurtBox.healthComponent : null;
					if (healthComponent && healthComponent.alive && info.hitHurtBox.teamIndex != teamIndex)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					didHit = true;
					this.beamHitPosition = info.point;
				}
				return flag;
			};
			/*bulletAttack.filterCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit info)
			{
				return (!info.entityObject || info.entityObject != _bulletAttack.owner) && BulletAttack.defaultFilterCallback(_bulletAttack, ref info);
			};*/
			bulletAttack.Fire();
			if (!didHit)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(aimRay, out raycastHit, FireMainBeamState.mainBeamMaxDistance, LayerIndex.world.mask, QueryTriggerInteraction.Ignore))
				{
					didHit = true;
					this.beamHitPosition = raycastHit.point;
				}
				else
				{
					this.beamHitPosition = aimRay.GetPoint(FireMainBeamState.mainBeamMaxDistance);
				}
			}
			if (didHit & isInitialBeam)
			{
				FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
				fireProjectileInfo.projectilePrefab = FireMainBeamState.secondBombPrefab;
				fireProjectileInfo.owner = base.ownerBody.gameObject;
				fireProjectileInfo.position = this.beamHitPosition - aimRay.direction * 0.5f;
				fireProjectileInfo.rotation = Quaternion.identity;
				fireProjectileInfo.damage = base.GetDamage() * FireMainBeamState.secondBombDamageCoefficient;
				fireProjectileInfo.damageColorIndex = DamageColorIndex.Item;
				fireProjectileInfo.crit = this.isCrit;
				ProjectileManager.instance.FireProjectile(fireProjectileInfo);
			}
			if (!isInitialBeam)
			{
				EffectData effectData = new EffectData
				{
					origin = aimRay.origin,
					start = base.transform.position
				};
				effectData.SetNetworkedObjectReference(base.gameObject);
				EffectManager.SpawnEffect(tracerEffectPrefab, effectData, true);
			}
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			Vector3 origin = this.initialAimRay.origin;
			Vector3 direction = this.initialAimRay.direction;
			writer.Write(origin);
			writer.Write(direction);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			Vector3 origin = reader.ReadVector3();
			Vector3 direction = reader.ReadVector3();
			this.initialAimRay = new Ray(origin, direction);
		}

		protected override bool shouldFollow => true;

		public static float baseDuration = 1f;

		public static float mainBeamDamageCoefficient = 3f;

		public static float mainBeamProcCoefficient = 1f;

		public static float mainBeamForce = 1000f;

		public static float mainBeamRadius = 1f;

		public static float mainBeamMaxDistance = 1000f;

		public static GameObject forwardBeamTracerEffect = GSUtil.AddressablesLoad<GameObject>("RoR2/Base/LaserTurbine/TracerLaserTurbine.prefab");

		public static GameObject backwardBeamTracerEffect = GSUtil.AddressablesLoad<GameObject>("RoR2/Base/LaserTurbine/TracerLaserTurbineReturn.prefab");

		public static GameObject mainBeamImpactEffect;

		public static GameObject secondBombPrefab = GSUtil.AddressablesLoad<GameObject>("RoR2/Base/LaserTurbine/LaserTurbineBomb.prefab");

		public static float secondBombDamageCoefficient = 10f;

		private Ray initialAimRay;

		private Vector3 beamHitPosition;

		private bool isCrit;
	}
}

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
    public class ChargeMainBeamState : OldLaserTurbineBaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.beamIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(ChargeMainBeamState.beamIndicatorPrefab, base.GetMuzzleTransform(), false);
			this.beamIndicatorChildLocator = this.beamIndicatorInstance.GetComponent<ChildLocator>();
			if (this.beamIndicatorChildLocator)
			{
				this.beamIndicatorEndTransform = this.beamIndicatorChildLocator.FindChild("End");
			}
		}

		public override void OnExit()
		{
			EntityState.Destroy(this.beamIndicatorInstance);
			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority && base.fixedAge >= ChargeMainBeamState.baseDuration)
			{
				this.outer.SetNextState(new FireMainBeamState());
			}
		}

		public override void Update()
		{
			base.Update();
			if (this.beamIndicatorInstance && this.beamIndicatorEndTransform)
			{
				float num = 1000f;
				Ray aimRay = base.GetAimRay();
				Vector3 position = this.beamIndicatorInstance.transform.parent.position;
				Vector3 point = aimRay.GetPoint(num);
				RaycastHit raycastHit;
				if (Util.CharacterRaycast(base.ownerBody.gameObject, aimRay, out raycastHit, num, LayerIndex.entityPrecise.mask | LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
				{
					point = raycastHit.point;
				}
				this.beamIndicatorEndTransform.transform.position = point;
			}
		}

		public override float GetChargeFraction() => 1f;

		protected override bool shouldFollow => false;

		public static float baseDuration = 0.5f;

		public static GameObject beamIndicatorPrefab = GSUtil.AddressablesLoad<GameObject>("RoR2/Base/LaserTurbine/TargetingLaserTurbine.prefab");

		private GameObject beamIndicatorInstance;

		private ChildLocator beamIndicatorChildLocator;

		private Transform beamIndicatorEndTransform;
	}
}

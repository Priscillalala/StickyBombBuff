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
    public class AimState : OldLaserTurbineBaseState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (base.isAuthority)
			{
				TeamMask mask = TeamMask.AllExcept(base.ownerBody.teamComponent.teamIndex);
				HurtBox[] hurtBoxes = new SphereSearch
				{
					radius = AimState.targetAcquisitionRadius,
					mask = LayerIndex.entityPrecise.mask,
					origin = base.transform.position,
					queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
				}.RefreshCandidates().FilterCandidatesByHurtBoxTeam(mask).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes();
				float blastRadius = FireMainBeamState.secondBombPrefab.GetComponent<ProjectileImpactExplosion>().blastRadius;
				int num = -1;
				int num2 = 0;
				for (int i = 0; i < hurtBoxes.Length; i++)
				{
					HurtBox[] hurtBoxes2 = new SphereSearch
					{
						radius = blastRadius,
						mask = LayerIndex.entityPrecise.mask,
						origin = hurtBoxes[i].transform.position,
						queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
					}.RefreshCandidates().FilterCandidatesByHurtBoxTeam(mask).FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes();
					if (hurtBoxes2.Length > num2)
					{
						num = i;
						num2 = hurtBoxes2.Length;
					}
				}
				if (num != -1)
				{
					base.simpleRotateToDirection.targetRotation = Quaternion.LookRotation(hurtBoxes[num].transform.position - base.transform.position);
					this.foundTarget = true;
				}
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority)
			{
				if (this.foundTarget)
				{
					this.outer.SetNextState(new ChargeMainBeamState());
					return;
				}
				this.outer.SetNextState(new ReadyState());
			}
		}

		protected override bool shouldFollow => false;

		public static float targetAcquisitionRadius = 100f;

		private bool foundTarget;
	}
}

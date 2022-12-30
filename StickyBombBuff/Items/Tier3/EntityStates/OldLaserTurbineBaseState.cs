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
    public class OldLaserTurbineBaseState : EntityState

	{
		private protected OldLaserTurbineController laserTurbineController { get; private set; }

		private protected SimpleRotateToDirection simpleRotateToDirection { get; private set; }

		protected CharacterBody ownerBody
		{
			get
			{
				GenericOwnership genericOwnership = this.genericOwnership;
				return this.bodyGetComponent.Get((genericOwnership != null) ? genericOwnership.ownerObject : null);
			}
		}

		public override void OnEnter()
		{
			base.OnEnter();
			this.genericOwnership = base.GetComponent<GenericOwnership>();
			this.simpleLeash = base.GetComponent<SimpleLeash>();
			this.simpleRotateToDirection = base.GetComponent<SimpleRotateToDirection>();
			this.laserTurbineController = base.GetComponent<OldLaserTurbineController>();
		}

		public virtual float GetChargeFraction()
		{
			return 0f;
		}

		protected InputBankTest GetInputBank()
		{
			CharacterBody ownerBody = this.ownerBody;
			if (ownerBody == null)
			{
				return null;
			}
			return ownerBody.inputBank;
		}

		protected Ray GetAimRay()
		{
			return new Ray(base.transform.position, base.transform.forward);
		}

		protected Transform GetMuzzleTransform()
		{
			return base.transform;
		}

		protected virtual bool shouldFollow
		{
			get
			{
				return true;
			}
		}

		public override void Update()
		{
			base.Update();
			if (this.ownerBody && this.shouldFollow)
			{
				this.simpleLeash.leashOrigin = this.ownerBody.corePosition;
				this.simpleRotateToDirection.targetRotation = Quaternion.LookRotation(this.ownerBody.inputBank.aimDirection);
			}
		}

		protected float GetDamage()
		{
			float num = 1f;
			if (this.ownerBody)
			{
				num = this.ownerBody.damage;
				if (this.ownerBody.inventory)
				{
					num *= (float)this.ownerBody.inventory.GetItemCount(RoR2Content.Items.LaserTurbine);
				}
			}
			return num;
		}

		private GenericOwnership genericOwnership;

		private SimpleLeash simpleLeash;

		private MemoizedGetComponent<CharacterBody> bodyGetComponent;
	}
}

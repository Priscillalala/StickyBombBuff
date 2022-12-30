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
    public class RechargeState : OldLaserTurbineBaseState
	{
		public Run.FixedTimeStamp startTime { get; private set; }

		public Run.FixedTimeStamp readyTime { get; private set; }

		public override void OnEnter()
		{
			base.OnEnter();
			if (base.isAuthority)
			{
				this.startTime = Run.FixedTimeStamp.now;
				this.readyTime = this.startTime + RechargeState.baseDuration;
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority && base.laserTurbineController.charge >= 1f)
			{
				this.outer.SetNextState(new ReadyState());
			}
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write(this.startTime);
			writer.Write(this.readyTime);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.startTime = reader.ReadFixedTimeStamp();
			this.readyTime = reader.ReadFixedTimeStamp();
		}

		public override float GetChargeFraction()
		{
			return Mathf.Clamp01(this.startTime.timeSince / (this.readyTime - this.startTime));
		}

		public static float baseDuration = 3f;
	}
}

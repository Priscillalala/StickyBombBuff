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
    public class ReadyState : OldLaserTurbineBaseState
	{
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority && base.fixedAge >= ReadyState.baseDuration)
			{
				this.outer.SetNextState(new AimState());
			}
		}

		public override float GetChargeFraction() => 1f;

		public static float baseDuration = 2f;
	}
}

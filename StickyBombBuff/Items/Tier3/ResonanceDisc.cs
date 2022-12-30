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
using StickyBombBuff.Items.Tier3.EntityStates;
using EntityStates;

namespace StickyBombBuff.Items.Tier3
{

    [Configurable]
    public class _ResonanceDisc : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LaserTurbine/LaserTurbineController.prefab")).Completed += _ResonanceDisc_Completed;
        }

        private void _ResonanceDisc_Completed(AsyncOperationHandle<GameObject> obj)
        {
			obj.Completed -= _ResonanceDisc_Completed;
			if(obj.Result.TryGetComponent(out LaserTurbineController laserTurbineController))
            {
				var oldController = obj.Result.AddComponent<OldLaserTurbineController>();
				oldController.chargeIndicator = laserTurbineController.chargeIndicator;
				oldController.spinIndicator = laserTurbineController.spinIndicator;
				oldController.turbineDisplayRoot = laserTurbineController.turbineDisplayRoot;
				DestroyImmediate(laserTurbineController);
			}
			if (obj.Result.TryGetComponent(out EntityStateMachine main))
			{
				main.initialStateType = new SerializableEntityStateType(typeof(RechargeState));
				main.mainStateType= new SerializableEntityStateType(typeof(RechargeState));
			}
		}

        public override void OnCollectContent(AssetStream sasset)
        {
			sasset.Add(typeof(AimState), typeof(ChargeMainBeamState), typeof(FireMainBeamState), typeof(OldLaserTurbineBaseState), typeof(ReadyState), typeof(RechargeState));
        }
        public static void _WriteSpinChargeState_LaserTurbineController(NetworkWriter writer, OldLaserTurbineController.SpinChargeState value)
		{
			writer.Write(value.initialCharge);
			writer.Write(value.initialSpin);
			GeneratedNetworkCode._WriteFixedTimeStamp_Run(writer, value.snapshotTime);
		}

		public static OldLaserTurbineController.SpinChargeState _ReadSpinChargeState_LaserTurbineController(NetworkReader reader)
		{
			return new OldLaserTurbineController.SpinChargeState
			{
				initialCharge = reader.ReadSingle(),
				initialSpin = reader.ReadSingle(),
				snapshotTime = GeneratedNetworkCode._ReadFixedTimeStamp_Run(reader)
			};
		}
		[RequireComponent(typeof(NetworkIdentity))]
		[RequireComponent(typeof(EntityStateMachine))]
		[RequireComponent(typeof(NetworkStateMachine))]
		[RequireComponent(typeof(GenericOwnership))]
		public class OldLaserTurbineController : NetworkBehaviour
		{
			public float charge { get; private set; }

			public CharacterBody ownerBody
			{
				get
				{
					return this.cachedOwnerBody;
				}
			}

			private void Awake()
			{
				this.genericOwnership = base.GetComponent<GenericOwnership>();
				this.genericOwnership.onOwnerChanged += this.OnOwnerChanged;
			}

			public override void OnStartServer()
			{
				base.OnStartServer();
				OldLaserTurbineController.SpinChargeState networkspinChargeState = this.spinChargeState;
				networkspinChargeState.initialSpin = this.minSpin;
				networkspinChargeState.snapshotTime = Run.FixedTimeStamp.now;
				this.NetworkspinChargeState = networkspinChargeState;
			}

			private void Update()
			{
				if (NetworkClient.active)
				{
					this.UpdateClient();
				}
			}

			private void FixedUpdate()
			{
				Run.FixedTimeStamp now = Run.FixedTimeStamp.now;
				this.spin = this.spinChargeState.CalcCurrentSpinValue(now, this.spinDecayRate, this.minSpin);
				this.charge = this.spinChargeState.CalcCurrentChargeValue(now, this.spinDecayRate, this.minSpin);
				if (this.turbineDisplayRoot)
				{
					this.turbineDisplayRoot.gameObject.SetActive(this.showTurbineDisplay);
				}
			}

			private void OnEnable()
			{
				if (NetworkServer.active)
				{
					GlobalEventManager.onCharacterDeathGlobal += this.OnCharacterDeathGlobalServer;
				}
			}

			private void OnDisable()
			{
				GlobalEventManager.onCharacterDeathGlobal -= this.OnCharacterDeathGlobalServer;
			}

			[Client]
			private void UpdateClient()
			{
				if (!NetworkClient.active)
				{
					Debug.LogWarning("[Client] function 'System.Void RoR2.LaserTurbineController::UpdateClient()' called on server");
					return;
				}
				float num = HGMath.CircleAreaToRadius(this.charge * HGMath.CircleRadiusToArea(1f));
				this.chargeIndicator.localScale = new Vector3(num, num, num);
				Vector3 localEulerAngles = this.spinIndicator.localEulerAngles;
				localEulerAngles.y += this.spin * Time.deltaTime * this.visualSpinRate;
				this.spinIndicator.localEulerAngles = localEulerAngles;
				AkSoundEngine.SetRTPCValue(this.spinRtpc, this.spin * this.spinRtpcScale, base.gameObject);
			}

			[Server]
			public void ExpendCharge()
			{
				if (!NetworkServer.active)
				{
					Debug.LogWarning("[Server] function 'System.Void RoR2.LaserTurbineController::ExpendCharge()' called on client");
					return;
				}
				Run.FixedTimeStamp now = Run.FixedTimeStamp.now;
				float num = this.spinChargeState.CalcCurrentSpinValue(now, this.spinDecayRate, this.minSpin);
				num += this.spinPerKill;
				OldLaserTurbineController.SpinChargeState networkspinChargeState = new OldLaserTurbineController.SpinChargeState
				{
					initialSpin = num,
					initialCharge = 0f,
					snapshotTime = now
				};
				this.NetworkspinChargeState = networkspinChargeState;
			}

			private void OnCharacterDeathGlobalServer(DamageReport damageReport)
			{
				if (damageReport.attacker == this.genericOwnership.ownerObject && damageReport.attacker != null)
				{
					this.OnOwnerKilledOtherServer();
				}
			}

			private void OnOwnerKilledOtherServer()
			{
				Run.FixedTimeStamp now = Run.FixedTimeStamp.now;
				float num = this.spinChargeState.CalcCurrentSpinValue(now, this.spinDecayRate, this.minSpin);
				float initialCharge = this.spinChargeState.CalcCurrentChargeValue(now, this.spinDecayRate, this.minSpin);
				num = Mathf.Min(num + this.spinPerKill, this.maxSpin);
				OldLaserTurbineController.SpinChargeState networkspinChargeState = new OldLaserTurbineController.SpinChargeState
				{
					initialSpin = num,
					initialCharge = initialCharge,
					snapshotTime = now
				};
				this.NetworkspinChargeState = networkspinChargeState;
			}

			private void OnOwnerChanged(GameObject newOwner)
			{
				this.cachedOwnerBody = (newOwner ? newOwner.GetComponent<CharacterBody>() : null);
			}

			private void UNetVersion()
			{
			}

			public OldLaserTurbineController.SpinChargeState NetworkspinChargeState
			{
				get
				{
					return this.spinChargeState;
				}
				[param: In]
				set
				{
					base.SetSyncVar<OldLaserTurbineController.SpinChargeState>(value, ref this.spinChargeState, 1U);
				}
			}

			public override bool OnSerialize(NetworkWriter writer, bool forceAll)
			{
				if (forceAll)
				{
					_WriteSpinChargeState_LaserTurbineController(writer, this.spinChargeState);
					return true;
				}
				bool flag = false;
				if ((base.syncVarDirtyBits & 1U) != 0U)
				{
					if (!flag)
					{
						writer.WritePackedUInt32(base.syncVarDirtyBits);
						flag = true;
					}
					_WriteSpinChargeState_LaserTurbineController(writer, this.spinChargeState);
				}
				if (!flag)
				{
					writer.WritePackedUInt32(base.syncVarDirtyBits);
				}
				return flag;
			}

			public override void OnDeserialize(NetworkReader reader, bool initialState)
			{
				if (initialState)
				{
					this.spinChargeState = _ReadSpinChargeState_LaserTurbineController(reader);
					return;
				}
				int num = (int)reader.ReadPackedUInt32();
				if ((num & 1) != 0)
				{
					this.spinChargeState = _ReadSpinChargeState_LaserTurbineController(reader);
				}
			}

			public float spinPerKill = 0.025f;

			public float spinDecayRate = 0.0125f;

			public float minSpin = 0f;

			public float maxSpin = 1f;

			public float visualSpinRate = 7200f;

			public Transform chargeIndicator;

			public Transform spinIndicator;

			public Transform turbineDisplayRoot;

			public bool showTurbineDisplay = true;

			public string spinRtpc = "item_laserTurbine_charge";

			public float spinRtpcScale = 100f;

			private GenericOwnership genericOwnership;

			[SyncVar]
			private OldLaserTurbineController.SpinChargeState spinChargeState = OldLaserTurbineController.SpinChargeState.zero;

			private float spin;

			private CharacterBody cachedOwnerBody;

			[Serializable]
			public struct SpinChargeState : IEquatable<OldLaserTurbineController.SpinChargeState>
			{
				public float CalcCurrentSpinValue(Run.FixedTimeStamp currentTime, float spinDecayRate, float minSpin)
				{
					return Mathf.Max(this.initialSpin - spinDecayRate * (currentTime - this.snapshotTime), minSpin);
				}

				public float CalcCurrentChargeValue(Run.FixedTimeStamp currentTime, float spinDecayRate, float minSpin)
				{
					float num = currentTime - this.snapshotTime;
					float num2 = minSpin * num;
					float num3 = this.initialSpin - minSpin;
					float t = Mathf.Min(Trajectory.CalculateFlightDuration(num3, -spinDecayRate) * 0.5f, num);
					float num4 = Trajectory.CalculatePositionYAtTime(0f, num3, t, -spinDecayRate);
					return Mathf.Min(this.initialCharge + num2 + num4, 1f);
				}

				public bool Equals(OldLaserTurbineController.SpinChargeState other)
				{
					return this.initialCharge.Equals(other.initialCharge) && this.initialSpin.Equals(other.initialSpin) && this.snapshotTime.Equals(other.snapshotTime);
				}

				public override bool Equals(object obj)
				{
					if (obj is OldLaserTurbineController.SpinChargeState)
					{
						OldLaserTurbineController.SpinChargeState other = (OldLaserTurbineController.SpinChargeState)obj;
						return this.Equals(other);
					}
					return false;
				}

				public override int GetHashCode()
				{
					return (this.initialCharge.GetHashCode() * 397 ^ this.initialSpin.GetHashCode()) * 397 ^ this.snapshotTime.GetHashCode();
				}

				public float initialCharge;

				public float initialSpin;

				public Run.FixedTimeStamp snapshotTime;

				public static readonly OldLaserTurbineController.SpinChargeState zero = new OldLaserTurbineController.SpinChargeState
				{
					initialCharge = 0f,
					initialSpin = 0f,
					snapshotTime = Run.FixedTimeStamp.negativeInfinity
				};
			}
		}
	}
}

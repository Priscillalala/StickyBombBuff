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
using static RoR2.RoR2Content.Items;

namespace StickyBombBuff.Items.Tier3
{

    [Configurable]
    public class _FrostRelic : StickyBombBuffModule
    {
        public static float radius = 10f;
        public static float radiusPerStack = 1.5f;
        public static float icicleDuration = 5f;
        public static int baseIcicleCount = 2;
        public static int maxIcicleCount = 5;
        public static float icicleAttackInterval = 0.25f;
        public static float procCoefficient = 0.05f;
        public static float dpsPerIcicle = 1f;
        public static float dpsPerIciclePerStack = 0.5f;

        public override void OnModInit()
        {
            IL.RoR2.IcicleAuraController.FixedUpdate += IcicleAuraController_FixedUpdate;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Icicle/IcicleAura.prefab")).Completed += _FrostRelic_Completed;

        }

        private void IcicleAuraController_FixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.Before, x => x.MatchStfld<BlastAttack>(nameof(BlastAttack.baseDamage)));
            if (ilfound)
            {
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate<Func<IcicleAuraController, float>>(controller => controller.cachedOwnerInfo.characterBody.HasItem(Icicle, out int stack) ? GSUtil.StackScaling(dpsPerIcicle, dpsPerIciclePerStack, stack) : 0f);
                c.Emit(OpCodes.Mul);
            }
        }

        private void _FrostRelic_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= _FrostRelic_Completed;
            if(obj.Result.TryGetComponent(out IcicleAuraController icicleAuraController))
            {
                icicleAuraController.icicleBaseRadius = radius;
                icicleAuraController.icicleRadiusPerIcicle = 0f;
                icicleAuraController.icicleDuration = icicleDuration;
                icicleAuraController.icicleCountOnFirstKill = baseIcicleCount;
                icicleAuraController.baseIcicleMax = maxIcicleCount;
                icicleAuraController.icicleMaxPerStack = 0;
                icicleAuraController.baseIcicleAttackInterval = icicleAttackInterval;
                icicleAuraController.icicleProcCoefficientPerTick = procCoefficient;
                icicleAuraController.icicleDamageCoefficientPerTick = 0f;
                icicleAuraController.icicleDamageCoefficientPerTickPerIcicle = icicleAuraController.baseIcicleAttackInterval;

            }
            if(obj.Result.TryGetComponent(out BuffWard buffWard))
            {
                DestroyImmediate(buffWard);
            }
        }
    }
}

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

namespace StickyBombBuff.Items.Tier3
{

    [Configurable]
    public class _Aegis : StickyBombBuffModule
    {
        public static float barrierCoefficient = 1;
        public static float barrierCoefficientPerStack = 0.5f;
        public override void OnModInit()
        {
            IL.RoR2.HealthComponent.Heal += HealthComponent_Heal;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<ItemDef>("RoR2/Base/BarrierOnOverHeal/BarrierOnOverHeal.asset")).Completed += _Aegis_Completed;
        }

        private void _Aegis_Completed(AsyncOperationHandle<ItemDef> obj)
        {
            obj.Completed -= _Aegis_Completed;
            if (obj.Result.DoesNotContainTag(ItemTag.LowHealth))
            {
                ArrayUtils.ArrayAppend(ref obj.Result.tags, ItemTag.LowHealth);
            }
        }

        private void HealthComponent_Heal(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ILLabel breakLabel = null;
            int locHealingIndex = -1;
            ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchLdflda<HealthComponent>(nameof(HealthComponent.itemCounts)),
                x => x.MatchLdfld<HealthComponent.ItemCounts>(nameof(HealthComponent.ItemCounts.barrierOnOverHeal)),
                x => x.MatchLdcI4(0),
                x => x.MatchBle(out breakLabel),
                x => x.MatchLdloc(out locHealingIndex)
                );
            if (ilfound)
            {
                c.Index--;
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate<Func<HealthComponent, bool>>(healthComponent => healthComponent.barrier < MaxBarrierFromOverheal(healthComponent));
                c.Emit(OpCodes.Brfalse, breakLabel);
            }
            ilfound = c.TryGotoNext(MoveType.Before, x => x.MatchCallOrCallvirt<HealthComponent>(nameof(HealthComponent.AddBarrier)));
            if (ilfound)
            {
                c.Emit(OpCodes.Ldloc, locHealingIndex);
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate<Func<float, float, HealthComponent, float>>((_, healing, healthComponent) =>
                {
                    float barrierGain = healing * GSUtil.StackScaling(barrierCoefficient, barrierCoefficientPerStack, healthComponent.itemCounts.barrierOnOverHeal);
                    float barrier = Mathf.Min(MaxBarrierFromOverheal(healthComponent) - healthComponent.barrier, barrierGain);
                    return barrier;
                });
            }
        }
        public float MaxBarrierFromOverheal(HealthComponent healthComponent) => healthComponent.fullCombinedHealth * HealthComponent.lowHealthFraction;
    }
}

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
    public class _BensRaincoat : StickyBombBuffModule
    {
        public static float chance = 20;
        public override void OnModInit()
        {
            ItemBehaviourUnlinker.Add<ImmuneToDebuffBehavior>();
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<ItemDef>("RoR2/DLC1/ImmuneToDebuff/ImmuneToDebuff.asset")).Completed += ImmuneToDebuff_Completed;
            On.RoR2.Items.ImmuneToDebuffBehavior.TryApplyOverride += ImmuneToDebuffBehavior_TryApplyOverride;
        }

        private bool ImmuneToDebuffBehavior_TryApplyOverride(On.RoR2.Items.ImmuneToDebuffBehavior.orig_TryApplyOverride orig, CharacterBody body)
        {
            if(body && body.HasItem(ImmuneToDebuff, out int stack) && Util.CheckRoll(Util.ConvertAmplificationPercentageIntoReductionPercentage(stack * chance), body.master))
            {
                EffectManager.SimpleImpactEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/ImmuneToDebuff/ImmuneToDebuffEffect.prefab").WaitForCompletion(), body.corePosition, Vector3.up, true);
                return true;
            }
            return orig(body);
        }

        private void ImmuneToDebuff_Completed(AsyncOperationHandle<ItemDef> obj)
        {
            obj.Completed -= ImmuneToDebuff_Completed;
#pragma warning disable
            obj.Result.deprecatedTier = ItemTier.Tier1;
#pragma warning restore
            obj.Result.pickupIconSprite = StickyBombBuffPlugin.assets.LoadAsset<Sprite>("texRaincoatIcon");
        }
    }
}

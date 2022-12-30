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
    public class _InterstellarDeskPlant : StickyBombBuffModule
    {
        public static float baseRadius = 1.5f;
        public static float radiusPerStack = 1.5f;
        public static float healingPerSecond = 16f;
        public static float healInterval = 0.5f;
        public override void OnModInit()
        {
            IL.RoR2.DeskPlantController.MainState.OnEnter += MainState_OnEnter;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Plant/InterstellarDeskPlant.prefab")).Completed += InterstellarDeskPlant_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Plant/DeskplantWard.prefab")).Completed += DeskPlantWard_Completed;
        
        }

        private void MainState_OnEnter(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (ilfound = c.TryGotoNext(MoveType.Before, x => x.MatchLdcR4(out _), x => x.MatchStfld<HealingWard>(nameof(HealingWard.healFraction))))
            {
                c.Next.Operand = 0f;
            }
            if (ilfound = c.TryGotoNext(MoveType.Before, x => x.MatchLdcR4(out _), x => x.MatchStfld<HealingWard>(nameof(HealingWard.healPoints))))
            {
                c.Next.Operand = healingPerSecond * healInterval;
            }
        }

        private void DeskPlantWard_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= DeskPlantWard_Completed;
            if (obj.Result.TryGetComponent(out HealingWard healingWard))
            {
                healingWard.interval = healInterval;
            }
        }

        private void InterstellarDeskPlant_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= InterstellarDeskPlant_Completed;
            if(obj.Result.TryGetComponent(out DeskPlantController deskPlantController))
            {
                deskPlantController.healingRadius = baseRadius;
                deskPlantController.radiusIncreasePerStack = radiusPerStack;
            }
        }
    }
}

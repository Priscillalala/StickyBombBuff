using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Linq;
using GrooveSharedUtils.Attributes;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using EntityStates.Gup;

namespace StickyBombBuff.General
{
    [Configurable]
    public class Gup : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GupBody.prefab")).Completed += GupBodyPrefab_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GeepBody.prefab")).Completed += GeepBodyPrefab_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Gup/GipBody.prefab")).Completed += GipBodyPrefab_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/DLC1/Gup/EntityStates.Gup.GupDeath.asset")).Completed += DeathState_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/DLC1/Gup/EntityStates.Gup.GeepDeath.asset")).Completed += DeathState_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/DLC1/Gup/EntityStates.Gup.GipDeath.asset")).Completed += DeathState_Completed;

        }

        private void DeathState_Completed(AsyncOperationHandle<EntityStateConfiguration> obj)
        {
            obj.Completed -= DeathState_Completed;
            obj.Result.TryModifyFieldValue<float>(nameof(BaseSplitDeath.moneyMultiplier), 0f);
        }

        private void GupBodyPrefab_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= GupBodyPrefab_Completed;
            if(obj.Result.TryGetComponent(out CharacterBody characterBody))
            {
                characterBody.baseMaxHealth = 1200f;
                characterBody.levelMaxHealth = 360f;
            }
        }
        private void GeepBodyPrefab_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= GeepBodyPrefab_Completed;
            if (obj.Result.TryGetComponent(out CharacterBody characterBody))
            {
                characterBody.baseMaxHealth = 600f;
                characterBody.levelMaxHealth = 180f;
            }
        }
        private void GipBodyPrefab_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= GipBodyPrefab_Completed;
            if (obj.Result.TryGetComponent(out CharacterBody characterBody))
            {
                characterBody.baseMaxHealth = 300f;
                characterBody.levelMaxHealth = 90f;
            }
        }
    }
}

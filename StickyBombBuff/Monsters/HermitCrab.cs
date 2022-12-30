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
using RoR2.Projectile;
using EntityStates.HermitCrab;

namespace StickyBombBuff.General
{
    [Configurable]
    public class HermitCrab : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/HermitCrab/HermitCrabBody.prefab")).Completed += BodyPrefab_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/HermitCrab/HermitCrabBombProjectile.prefab")).Completed += Projectile_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/Base/HermitCrab/EntityStates.HermitCrab.FireMortar.asset")).Completed += FireMortar_Completed; ;
        }

        private void FireMortar_Completed(AsyncOperationHandle<EntityStateConfiguration> obj)
        {
            obj.Completed -= FireMortar_Completed;
            obj.Result.TryModifyFieldValue<int>(nameof(FireMortar.mortarCount), 1);
        }

        private void Projectile_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= Projectile_Completed;
            if (obj.Result.TryGetComponent(out ProjectileController projectileController))
            {
                projectileController.procCoefficient = 3f;
            }
        }

        private void BodyPrefab_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= BodyPrefab_Completed;
            if(obj.Result.TryGetComponent(out CharacterBody characterBody))
            {
                characterBody.baseMaxHealth = 150f;
                characterBody.levelMaxHealth = 45f;
                characterBody.baseDamage = 4f;
                characterBody.levelDamage = 0.8f;
            }
        }
    }
}

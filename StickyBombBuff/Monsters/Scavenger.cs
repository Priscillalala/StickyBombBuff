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
using HG;

namespace StickyBombBuff.General
{
    [Configurable]
    public class Scavenger : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<ItemDef>("RoR2/Base/Dagger/Dagger.asset")).Completed += Dagger_Completed; ;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Scav/ScavBody.prefab")).Completed += BodyPrefab_Completed;
        }

        private void Dagger_Completed(AsyncOperationHandle<ItemDef> obj)
        {
            obj.Completed -= Dagger_Completed;
            if (obj.Result.ContainsTag(ItemTag.AIBlacklist))
            {
                ArrayUtils.ArrayRemoveAtAndResize(ref obj.Result.tags, Array.IndexOf(obj.Result.tags, ItemTag.AIBlacklist));
            }
        }

        private void BodyPrefab_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= BodyPrefab_Completed;
            if(obj.Result.TryGetComponent(out CharacterBody characterBody))
            {
                characterBody.baseMaxHealth = 5400f;
                characterBody.levelMaxHealth = 1620f;
            }
        }
    }
}

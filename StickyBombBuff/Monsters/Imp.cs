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

namespace StickyBombBuff.General
{
    [Configurable]
    public class Imp : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Imp/ImpBody.prefab")).Completed += BodyPrefab_Completed;
        }

        private void BodyPrefab_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= BodyPrefab_Completed;
            if(obj.Result.TryGetComponent(out CharacterBody characterBody))
            {
                characterBody.baseMaxHealth = 250f;
                characterBody.levelMaxHealth = 75f;
            }
        }
    }
}

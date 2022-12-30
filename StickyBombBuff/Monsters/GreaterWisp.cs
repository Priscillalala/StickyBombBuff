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
    public class GreaterWisp : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/GreaterWisp/GreaterWispBody.prefab")).Completed += GreaterWisp_Completed;
        }

        private void GreaterWisp_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= GreaterWisp_Completed;
            if(obj.Result.TryGetComponent(out CharacterBody characterBody))
            {
                characterBody.baseArmor = 1f;
            }
            if(obj.Result.TryGetComponent(out SetStateOnHurt setStateOnHurt))
            {
                DestroyImmediate(setStateOnHurt);
            }
        }
    }
}

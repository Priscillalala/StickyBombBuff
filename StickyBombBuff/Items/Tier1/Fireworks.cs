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
using UnityEngine.AddressableAssets;

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _Fireworks : StickyBombBuffModule
    {
        public static float damageCoefficient = 1f;
        public override void OnModInit()
        {
            Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Firework/FireworkLauncher.prefab").Completed += _Fireworks_Completed;
        }

        private void _Fireworks_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
        {
            GSUtil.Log("Fireworks completed!");
            obj.Completed -= _Fireworks_Completed;
            FireworkLauncher fireworkLauncher = obj.Result.GetComponent<FireworkLauncher>();
            fireworkLauncher.damageCoefficient = damageCoefficient;
        }
    }
}

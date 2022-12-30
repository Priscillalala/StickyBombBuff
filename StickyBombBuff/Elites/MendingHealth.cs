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
using static RoR2.RoR2Content.Elites;

namespace StickyBombBuff.General
{
    [Configurable]
    public class MendingHealth : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<EliteDef>("RoR2/DLC1/EliteEarth/edEarth.asset")).Completed += edEarth_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<EliteDef>("RoR2/DLC1/EliteEarth/edEarthHonor.asset")).Completed += edEarthHonor_Completed;

        }

        private void edEarthHonor_Completed(AsyncOperationHandle<EliteDef> obj)
        {
            obj.Completed -= edEarthHonor_Completed;
            obj.Result.healthBoostCoefficient = 2.5f;
        }

        private void edEarth_Completed(AsyncOperationHandle<EliteDef> obj)
        {
            obj.Completed -= edEarth_Completed;
            obj.Result.healthBoostCoefficient = 4f;
        }
    }
}

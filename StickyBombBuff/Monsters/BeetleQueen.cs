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
using EntityStates.BeetleQueenMonster;

namespace StickyBombBuff.General
{
    [Configurable]
    public class BeetleQueen : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            On.EntityStates.BeetleQueenMonster.SummonEggs.OnEnter += SummonEggs_OnEnter;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/Base/Beetle/EntityStates.BeetleQueenMonster.SummonEggs.asset")).Completed += SummonEggs_Completed;
        }

        private void SummonEggs_OnEnter(On.EntityStates.BeetleQueenMonster.SummonEggs.orig_OnEnter orig, SummonEggs self)
        {
            GSUtil.Log(BepInEx.Logging.LogLevel.Warning, SummonEggs.maxSummonCount);
            orig(self);
        }

        private void SummonEggs_Completed(AsyncOperationHandle<EntityStateConfiguration> obj)
        {
            obj.Completed -= SummonEggs_Completed;
            obj.Result.TryModifyFieldValue<int>(nameof(SummonEggs.maxSummonCount), 5);
            obj.Result.TryModifyFieldValue<SpawnCard>(nameof(SummonEggs.spawnCard), Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/Beetle/cscBeetle.asset").WaitForCompletion());
        }
    }
}

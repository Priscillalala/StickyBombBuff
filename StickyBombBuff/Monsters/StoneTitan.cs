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
using EntityStates.TitanMonster;

namespace StickyBombBuff.General
{
    [Configurable]
    public class StoneTitan : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/Base/Titan/EntityStates.TitanMonster.FireMegaLaser.asset")).Completed += FireMegaLaser_Completed; ;
        }

        private void FireMegaLaser_Completed(AsyncOperationHandle<EntityStateConfiguration> obj)
        {
            obj.Completed -= FireMegaLaser_Completed;
            obj.Result.TryModifyFieldValue<float>(nameof(FireMegaLaser.procCoefficientPerTick), 1f);
        }
        
    }
}

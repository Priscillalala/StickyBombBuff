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

namespace StickyBombBuff.Items.Tier2
{

    [Configurable]
    public class _SquidPolyp : StickyBombBuffModule
    {

        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<EntityStateConfiguration>("RoR2/Base/Squid/EntityStates.Squid.SquidWeapon.FireSpine.asset")).Completed += _SquidPolyp_Completed;
        }

        private void _SquidPolyp_Completed(AsyncOperationHandle<EntityStateConfiguration> obj)
        {
            obj.Completed -= _SquidPolyp_Completed;
            obj.Result.TryModifyFieldValue<float>(nameof(EntityStates.Squid.SquidWeapon.FireSpine.forceScalar), 0f);
        }
    }
}

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
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace StickyBombBuff.Items.Tier2
{

    [Configurable]
    public class _OldGuillotine : StickyBombBuffModule
    {
        public static float executeThreshold = 20f;
        public override void OnModInit()
        {
            IL.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<ItemDef>("RoR2/Base/ExecuteLowHealthElite/ExecuteLowHealthElite.asset")).Completed += _OldGuillotine_Completed;
        }

        private void CharacterBody_OnInventoryChanged(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ilfound = c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcR4(out _),
                x => x.MatchLdarg(0),
                x => x.MatchCallOrCallvirt<CharacterBody>("get_inventory"),
                x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField(nameof(RoR2Content.Items.ExecuteLowHealthElite)))
                );
            if (ilfound)
            {
                c.Next.Operand = executeThreshold;
            }
        }

        private void _OldGuillotine_Completed(AsyncOperationHandle<ItemDef> obj)
        {
            obj.Completed -= _OldGuillotine_Completed;
#pragma warning disable
            obj.Result.deprecatedTier = ItemTier.Tier1;
#pragma warning restore
            obj.Result.pickupIconSprite = StickyBombBuffPlugin.assets.LoadAsset<Sprite>("texGuillotineIcon");
        }
    }
}

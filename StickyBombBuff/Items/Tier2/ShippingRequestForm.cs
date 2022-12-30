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
using System.Collections.Generic;
using static RoR2.DLC1Content.Items;
using GrooveSharedUtils.Frames;
using RoR2.ExpansionManagement;

namespace StickyBombBuff.Items.Tier2
{

    [Configurable]
    public class _ShippingRequestForm : StickyBombBuffModule
    {
        public static ItemDef FreeChestConsumed;
        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/FreeChestTerminalShippingDrone/FreeChestTerminalShippingDrone.prefab")).Completed += _ShippingRequestForm_Completed;
            SceneDirector.onPostPopulateSceneServer += SceneDirector_onPostPopulateSceneServer;
        }

        private void SceneDirector_onPostPopulateSceneServer(SceneDirector obj)
        {
            using (IEnumerator<CharacterMaster> enumerator = CharacterMaster.readOnlyInstancesList.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.HasItem(FreeChest))
                    {
                        enumerator.Current.inventory.RemoveItem(FreeChest);
                        CharacterMasterNotificationQueue.SendTransformNotification(enumerator.Current, FreeChest.itemIndex, FreeChestConsumed.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                    }
                }
            }
        }

        private void _ShippingRequestForm_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= _ShippingRequestForm_Completed;
            obj.Result.GetComponent<ShopTerminalBehavior>().dropTable = Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Chest2/dtChest2.asset").WaitForCompletion();
        }
        public override void OnCollectContent(AssetStream sasset)
        {
            FreeChestConsumed = new ItemFrame
            {
                itemTier = ItemTier.NoTier,
                canRemove = false,
                itemTags = new[] { ItemTag.Utility },
                name = "FreeChestConsumed",
                requiredExpansion = GSUtil.AddressablesLoad<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset"),
                icon = StickyBombBuffPlugin.assets.LoadAsset<Sprite>("texShippingRequestFormConsumedIcon"),
            }.Build().ItemDef;
            sasset.Add(FreeChestConsumed);
        }
    }
}

using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using GrooveSharedUtils.Attributes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using GrooveSharedUtils.Frames;

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _FreshMeat : StickyBombBuffModule
    {
        public static ItemDef RegenOnKill;
        public static float duration = 3;
        public override void OnModInit()
        {
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport)
        {
            if(damageReport.attackerBody && damageReport.attackerBody.HasItem(RegenOnKill, out int stack))
            {
                damageReport.attackerBody.AddTimedBuff(JunkContent.Buffs.MeatRegenBoost, stack * duration);
            }
        }

        public override void OnCollectContent(AssetStream sasset)
        {
            RegenOnKill = new ItemFrame
            {
                name = "SSBRegenOnKill",
                itemTier = ItemTier.Tier1,
                itemTags = new[] { ItemTag.Healing, ItemTag.OnKillEffect },
                overrideLoreToken = "ITEM_FLATHEALTH_LORE",
                icon = StickyBombBuffPlugin.assets.LoadAsset<Sprite>("texFreshMeatIcon"),
                pickupModelPrefab = GSUtil.AddressablesLoad<GameObject>("RoR2/Base/FlatHealth/PickupSteak.prefab")
            }.Build().ItemDef;
            sasset.Add(RegenOnKill);
        }
    }
}

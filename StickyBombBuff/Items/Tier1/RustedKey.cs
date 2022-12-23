﻿using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using GrooveSharedUtils.Attributes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2.Orbs;
using static RoR2.RoR2Content.Items;
using static RoR2.DLC1Content.Items;
using System.Collections.Generic;
using RoR2.Items;
using UnityEngine.AddressableAssets;

namespace StickyBombBuff.Items.Tier1
{
    [Configurable]
    public class _RustedKey : StickyBombBuffModule
    {
        public static BasicPickupDropTable dropTable;
        public override void OnModInit()
        {
            dropTable = ScriptableObject.CreateInstance<BasicPickupDropTable>();
            dropTable.tier1Weight = 80f;
            dropTable.tier2Weight = 20f;
            dropTable.tier3Weight = 1f;
            Addressables.LoadAssetAsync<GameObject>("RoR2/Base/TreasureCache/Lockbox.prefab").Completed += _RustedKey_Completed;
            
            IL.RoR2.SceneDirector.PopulateScene += SceneDirector_PopulateScene;
            On.RoR2.CostTypeCatalog.Init += CostTypeCatalog_Init;
        }

        private void _RustedKey_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= _RustedKey_Completed;
            obj.Result.GetComponent<ChestBehavior>().dropTable = dropTable;
            ModelLocator modelLocator = obj.Result.GetComponent<ModelLocator>();
            Transform mdlTransform = modelLocator.modelTransform;
            mdlTransform.parent.localScale = Vector3.one;
            mdlTransform.Find("ActiveFX").gameObject.SetActive(false);
        }

        private void SceneDirector_PopulateScene(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            int locTotalStackIndex = -1;
            ilfound = c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField(nameof(TreasureCache))),
                x => x.MatchCallOrCallvirt<Inventory>(nameof(Inventory.GetItemCount)),
                x => x.MatchLdcI4(out _),
                x => x.MatchBle(out _),
                x => x.MatchLdloc(out locTotalStackIndex)
                ) && c.TryGotoNext(MoveType.After, x => x.MatchEndfinally());
            if (ilfound)
            {
                c.Emit(OpCodes.Ldloc, locTotalStackIndex);
                c.EmitDelegate<Func<int, int>>(totalStack =>
                {
                    dropTable.tier1Weight = 80f;
                    dropTable.tier2Weight = 20f * totalStack;
                    dropTable.tier3Weight = totalStack * totalStack;
                    dropTable.Regenerate(Run.instance);
                    return Mathf.Max(totalStack, 1);
                });
                c.Emit(OpCodes.Stloc, locTotalStackIndex);
            }

        }

        private void CostTypeCatalog_Init(On.RoR2.CostTypeCatalog.orig_Init orig)
        {
            orig();
            CostTypeCatalog.GetCostTypeDef(CostTypeIndex.TreasureCacheItem).payCost = delegate (CostTypeDef costTypeDef, CostTypeDef.PayCostContext context)
            {
                MultiShopCardUtils.OnNonMoneyPurchase(context);
            };
        }
    }
}

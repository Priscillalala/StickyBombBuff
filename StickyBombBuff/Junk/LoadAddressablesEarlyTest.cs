using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using HG;
using RoR2.ContentManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using RoR2.Skills;
using RoR2.ExpansionManagement;
using RoR2.EntitlementManagement;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.ResourceLocations;
using GrooveSharedUtils.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace StickyBombBuff.Junk
{
    [IgnoreModule]
    public class LoadAddressablesEarlyTest : BaseModModule<LoadAddressablesEarlyTest>
    {
        List<AsyncOperationHandle> addressableOperations = new List<AsyncOperationHandle>();
        Action<AsyncOperationHandle, Exception> oldResourceManagerExceptionHandler;
        public override void OnModInit()
        {
            /*IEnumerable<Type> contentTypes = typeof(ContentPack).GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(fi => typeof(NamedAssetCollection<>).IsAssignableFrom(fi.FieldType))
                .Select(fi => fi.FieldType.GetGenericArguments()[0])
                .Where(type => typeof(UnityEngine.Object).IsAssignableFrom(type));
            foreach(Type t in contentTypes)
            {
                Addressables.LoadAssetsAsync()
            }*/
            OverrideExceptions();
            //List<object> keys = new List<object>();// { "ContentPack:RoR2.BaseContent", "ContentPack:RoR2.DLC1", "ContentPack:RoR2.Junk" };
            //keys.AddRange(AddressablesLabels.assetTypeLabels.Values);
            //Addressables.LoadAssetsAsync<UnityEngine.Object>((IEnumerable)keys, null, Addressables.MergeMode.Union);
            this.AddLoadOperation<GameObject>(AddressablesLabels.characterBody);
            this.AddLoadOperation<GameObject>(AddressablesLabels.characterMaster);
            this.AddLoadOperation<GameObject>(AddressablesLabels.projectile);
            this.AddLoadOperation<GameObject>(AddressablesLabels.gameMode);
            this.AddLoadOperation<GameObject>(AddressablesLabels.networkedObject);
            this.AddLoadOperation<SkillFamily>(AddressablesLabels.skillFamily);
            this.AddLoadOperation<SkillDef>(AddressablesLabels.skillDef);
            this.AddLoadOperation<UnlockableDef>(AddressablesLabels.unlockableDef);
            this.AddLoadOperation<SurfaceDef>(AddressablesLabels.surfaceDef);
            this.AddLoadOperation<SceneDef>(AddressablesLabels.sceneDef);
            this.AddLoadOperation<NetworkSoundEventDef>(AddressablesLabels.networkSoundEventDef);
            this.AddLoadOperation<MusicTrackDef>(AddressablesLabels.musicTrackDef);
            this.AddLoadOperation<GameEndingDef>(AddressablesLabels.gameEndingDef);
            this.AddLoadOperation<ItemDef>(AddressablesLabels.itemDef);
            this.AddLoadOperation<ItemTierDef>(AddressablesLabels.itemTierDef);
            this.AddLoadOperation<ItemRelationshipProvider>(AddressablesLabels.itemRelationshipProvider);
            this.AddLoadOperation<ItemRelationshipType>(AddressablesLabels.itemRelationshipType);
            this.AddLoadOperation<EquipmentDef>(AddressablesLabels.equipmentDef);
            this.AddLoadOperation<MiscPickupDef>(AddressablesLabels.miscPickupDef);
            this.AddLoadOperation<BuffDef>(AddressablesLabels.buffDef);
            this.AddLoadOperation<EliteDef>(AddressablesLabels.eliteDef);
            this.AddLoadOperation<SurvivorDef>(AddressablesLabels.survivorDef);
            this.AddLoadOperation<ArtifactDef>(AddressablesLabels.artifactDef);
            this.AddLoadOperation<GameObject>(AddressablesLabels.effect);
            this.AddLoadOperation<EntityStateConfiguration>(AddressablesLabels.entityStateConfiguration);
            this.AddLoadOperation<ExpansionDef>(AddressablesLabels.expansionDef);
            this.AddLoadOperation<EntitlementDef>(AddressablesLabels.entitlementDef);
            /*Addressables.LoadAssetsAsync<UnityEngine.Object>("ContentPack:RoR2.BaseContent", null);
            Addressables.LoadAssetsAsync<UnityEngine.Object>("ContentPack:RoR2.DLC1", null);
            Addressables.LoadAssetsAsync<UnityEngine.Object>("ContentPack:RoR2.Junk", null);*/
            RestoreExceptions();
            ContentManager.onContentPacksAssigned += ContentManager_onContentPacksAssigned;
        }
        public void AddLoadOperation<TAsset>(string key) where TAsset : UnityEngine.Object
        {
            Addressables.LoadAssetsAsync<TAsset>(key, null);
        }
        public void OverrideExceptions()
        {
            oldResourceManagerExceptionHandler = ResourceManager.ExceptionHandler;
            ResourceManager.ExceptionHandler = (operationHandle, exception) =>
            {
                if (!(exception is InvalidKeyException))
                {
                    oldResourceManagerExceptionHandler?.Invoke(operationHandle, exception);
                }
            };
        }
        public void RestoreExceptions()
        {
            ResourceManager.ExceptionHandler = oldResourceManagerExceptionHandler;
        }
        private void ContentManager_onContentPacksAssigned(ReadOnlyArray<ReadOnlyContentPack> obj)
        {
        }

        public override void OnCollectContent(AssetStream sasset)
        {
        }

    }
}

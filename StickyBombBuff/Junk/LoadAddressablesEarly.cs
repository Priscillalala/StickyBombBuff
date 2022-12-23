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
using GrooveSharedUtils.Attributes;
using System.Diagnostics;

namespace StickyBombBuff.Junk
{
    [IgnoreModule]
    public class LoadAddressablesEarly : BaseModModule<LoadAddressablesEarly>
    {
        public class EarlyAddressablesLoadHelper
        {
            public static int maxConcurrentLoadActions = int.MaxValue;
            public EarlyAddressablesLoadHelper(params object[] requiredKeys)
            {
                ArrayUtils.CloneTo(requiredKeys, ref this.requiredKeys);
            }
			List<AsyncOperationHandle> asyncOperations = new List<AsyncOperationHandle>();
			public object[] requiredKeys = Array.Empty<object>();
            public Queue<Action> loadActionsQueue = new Queue<Action>();
            Stopwatch stopwatch = new Stopwatch();
            public void WaitForCompletion()
            {
                GSUtil.Log(BepInEx.Logging.LogLevel.Warning, $"{stopwatch.Elapsed} seconds were spent outside of async loading!");
                for(int i = 0; i < asyncOperations.Count; i++)
                {
                    asyncOperations[i].WaitForCompletion();
                }
                asyncOperations.Clear();
            }
            public void ContentPackLoadOperationAsync(ContentPack contentPack)
            {
                this.AddLoadOperation<GameObject>(AddressablesLabels.characterBody, (Action<GameObject[]>)contentPack.bodyPrefabs.Add, 1f);
                this.AddLoadOperation<GameObject>(AddressablesLabels.characterMaster, (Action<GameObject[]>)contentPack.masterPrefabs.Add, 1f);
                this.AddLoadOperation<GameObject>(AddressablesLabels.projectile, (Action<GameObject[]>)contentPack.projectilePrefabs.Add, 1f);
                this.AddLoadOperation<GameObject>(AddressablesLabels.gameMode, (Action<GameObject[]>)contentPack.gameModePrefabs.Add, 1f);
                this.AddLoadOperation<GameObject>(AddressablesLabels.networkedObject, (Action<GameObject[]>)contentPack.networkedObjectPrefabs.Add, 1f);
                this.AddLoadOperation<SkillFamily>(AddressablesLabels.skillFamily, (Action<SkillFamily[]>)contentPack.skillFamilies.Add, 1f);
                this.AddLoadOperation<SkillDef>(AddressablesLabels.skillDef, (Action<SkillDef[]>)contentPack.skillDefs.Add, 1f);
                this.AddLoadOperation<UnlockableDef>(AddressablesLabels.unlockableDef, (Action<UnlockableDef[]>)contentPack.unlockableDefs.Add, 1f);
                this.AddLoadOperation<SurfaceDef>(AddressablesLabels.surfaceDef, (Action<SurfaceDef[]>)contentPack.surfaceDefs.Add, 1f);
                this.AddLoadOperation<SceneDef>(AddressablesLabels.sceneDef, (Action<SceneDef[]>)contentPack.sceneDefs.Add, 1f);
                this.AddLoadOperation<NetworkSoundEventDef>(AddressablesLabels.networkSoundEventDef, (Action<NetworkSoundEventDef[]>)contentPack.networkSoundEventDefs.Add, 1f);
                this.AddLoadOperation<MusicTrackDef>(AddressablesLabels.musicTrackDef, (Action<MusicTrackDef[]>)contentPack.musicTrackDefs.Add, 1f);
                this.AddLoadOperation<GameEndingDef>(AddressablesLabels.gameEndingDef, (Action<GameEndingDef[]>)contentPack.gameEndingDefs.Add, 1f);
                this.AddLoadOperation<ItemDef>(AddressablesLabels.itemDef, (Action<ItemDef[]>)contentPack.itemDefs.Add, 1f);
                this.AddLoadOperation<ItemTierDef>(AddressablesLabels.itemTierDef, (Action<ItemTierDef[]>)contentPack.itemTierDefs.Add, 1f);
                this.AddLoadOperation<ItemRelationshipProvider>(AddressablesLabels.itemRelationshipProvider, (Action<ItemRelationshipProvider[]>)contentPack.itemRelationshipProviders.Add, 1f);
                this.AddLoadOperation<ItemRelationshipType>(AddressablesLabels.itemRelationshipType, (Action<ItemRelationshipType[]>)contentPack.itemRelationshipTypes.Add, 1f);
                this.AddLoadOperation<EquipmentDef>(AddressablesLabels.equipmentDef, (Action<EquipmentDef[]>)contentPack.equipmentDefs.Add, 1f);
                this.AddLoadOperation<MiscPickupDef>(AddressablesLabels.miscPickupDef, (Action<MiscPickupDef[]>)contentPack.miscPickupDefs.Add, 1f);
                this.AddLoadOperation<BuffDef>(AddressablesLabels.buffDef, (Action<BuffDef[]>)contentPack.buffDefs.Add, 1f);
                this.AddLoadOperation<EliteDef>(AddressablesLabels.eliteDef, (Action<EliteDef[]>)contentPack.eliteDefs.Add, 1f);
                this.AddLoadOperation<SurvivorDef>(AddressablesLabels.survivorDef, (Action<SurvivorDef[]>)contentPack.survivorDefs.Add, 1f);
                this.AddLoadOperation<ArtifactDef>(AddressablesLabels.artifactDef, (Action<ArtifactDef[]>)contentPack.artifactDefs.Add, 1f);
                this.AddLoadOperation<GameObject, EffectDef>(AddressablesLabels.effect, (Action<EffectDef[]>)contentPack.effectDefs.Add, (Func<GameObject, EffectDef>)((GameObject asset) => new EffectDef(asset)), 1f);
                this.AddLoadOperation<EntityStateConfiguration>(AddressablesLabels.entityStateConfiguration, (Action<EntityStateConfiguration[]>)contentPack.entityStateConfigurations.Add, 1f);
                this.AddLoadOperation<ExpansionDef>(AddressablesLabels.expansionDef, (Action<ExpansionDef[]>)contentPack.expansionDefs.Add, 1f);
                this.AddLoadOperation<EntitlementDef>(AddressablesLabels.entitlementDef, (Action<EntitlementDef[]>)contentPack.entitlementDefs.Add, 1f);
                for(int i = 0; i < Mathf.Min( maxConcurrentLoadActions, loadActionsQueue.Count); i++)
                {
                    InvokeNextLoadAction();
                }
            }
            public void AddLoadOperation<TAssetSrc>(string key, Action<TAssetSrc[]> onComplete, float weight = 1f) where TAssetSrc : UnityEngine.Object
            {
                AddLoadOperation<TAssetSrc, TAssetSrc>(key, onComplete, null, weight);
            }
            public void AddLoadOperation<TAssetSrc, TAssetDest>(string key, Action<TAssetDest[]> onComplete, Func<TAssetSrc, TAssetDest> selector = null, float weight = 1f) where TAssetSrc : UnityEngine.Object
            {

                stopwatch.Start();
                List<object> keys = new List<object>();
                ListUtils.AddRange(keys, requiredKeys);
                keys.Add(key);
                AsyncOperationHandle<IList<TAssetSrc>> asyncOperation;
                Action<AsyncOperationHandle, Exception> oldResourceManagerExceptionHandler = ResourceManager.ExceptionHandler;
                ResourceManager.ExceptionHandler = (operationHandle, exception) =>
                {
                    if (!(exception is InvalidKeyException))
                    {
                        oldResourceManagerExceptionHandler?.Invoke(operationHandle, exception);
                    }
                };
                try
                {
                    asyncOperation = Addressables.LoadAssetsAsync((IEnumerable)keys, (Action<TAssetSrc>)null, Addressables.MergeMode.Intersection);
                }
                finally
                {
                    ResourceManager.ExceptionHandler = oldResourceManagerExceptionHandler;
                }
                if (!asyncOperation.IsValid())
                {
                    return;
                }
                asyncOperations.Add(asyncOperation);
                /*asyncOperation.Completed += handle =>
                {
                    stopwatch.Start();
                    if (handle.Result != null)
                    {
                        TAssetSrc[] loadedAssets = handle.Result.Where((TAssetSrc asset) => (UnityEngine.Object)(object)asset).ToArray();
                        TAssetDest[] convertedAssets;
                        if (selector == null)
                        {
                            if (!(typeof(TAssetSrc) == typeof(TAssetDest)))
                            {
                                throw new ArgumentNullException("selector", "Converter must be provided when TAssetSrc and TAssetDest differ.");
                            }
                            convertedAssets = (TAssetDest[])(object)loadedAssets;
                        }
                        else
                        {
                            convertedAssets = new TAssetDest[loadedAssets.Length];
                            for (int i = 0; i < loadedAssets.Length; i++)
                            {
                                convertedAssets[i] = selector(loadedAssets[i]);
                            }
                        }
                        string[] assetNames = new string[loadedAssets.Length];
                        for (int k = 0; k < loadedAssets.Length; k++)
                        {
                            assetNames[k] = loadedAssets[k].name;
                        }
                        Array.Sort(assetNames, convertedAssets, StringComparer.Ordinal);
                        onComplete?.Invoke(convertedAssets);
                    }
                    stopwatch.Stop();
                    InvokeNextLoadAction();
                };*/
                stopwatch.Stop();

            }
            public void InvokeNextLoadAction()
            {
                if(loadActionsQueue.Count > 0)
                {
                    GSUtil.Log("Invoke next");
                    loadActionsQueue.Dequeue().Invoke();
                }
            }
        }

        const string ror2ContentKey = "ContentPack:RoR2.BaseContent";
        public EarlyAddressablesLoadHelper ror2ContentEarlyLoadHelper = null;
        public ContentPack ror2ContentPack;
        const string dlc1ContentKey = "ContentPack:RoR2.DLC1";
        public EarlyAddressablesLoadHelper dlc1ContentEarlyLoadHelper = null;
        public ContentPack dlc1ContentPack;
        const string junkContentKey = "ContentPack:RoR2.Junk";
        public EarlyAddressablesLoadHelper junkContentEarlyLoadHelper = null;
        public ContentPack junkContentPack;

        public override void OnModInit()
        {
            ror2ContentPack = new ContentPack();
            ror2ContentEarlyLoadHelper = new EarlyAddressablesLoadHelper(ror2ContentKey);
            ror2ContentEarlyLoadHelper.ContentPackLoadOperationAsync(ror2ContentPack);

            dlc1ContentPack = new ContentPack();
            dlc1ContentEarlyLoadHelper = new EarlyAddressablesLoadHelper(dlc1ContentKey);
            dlc1ContentEarlyLoadHelper.ContentPackLoadOperationAsync(dlc1ContentPack);

            junkContentPack = new ContentPack();
            junkContentEarlyLoadHelper = new EarlyAddressablesLoadHelper(junkContentKey);
            junkContentEarlyLoadHelper.ContentPackLoadOperationAsync(junkContentPack);

            /*On.RoR2.ContentManagement.AddressablesLoadHelper.AddContentPackLoadOperation += AddressablesLoadHelper_AddContentPackLoadOperation;
            On.RoR2.RoR2Content.ctor += RoR2Content_ctor;
            On.RoR2.DLC1Content.ctor += DLC1Content_ctor;
            On.RoR2.JunkContent.ctor += JunkContent_ctor;*/
        }

        private void AddressablesLoadHelper_AddContentPackLoadOperation(On.RoR2.ContentManagement.AddressablesLoadHelper.orig_AddContentPackLoadOperation orig, AddressablesLoadHelper self, ContentPack contentPack)
        {
            if(contentPack == ror2ContentPack || contentPack == dlc1ContentPack || contentPack == junkContentPack)
            {
                GSUtil.Log($"not adding {contentPack.identifier} load op");
                contentPack.entityStateTypes.Add((from v in contentPack.entityStateConfigurations
                                  select (Type)v.targetType into v
                                  where v != null
                                  select v).ToArray());
                return;
            }
            orig(self, contentPack);
        }

        private void RoR2Content_ctor(On.RoR2.RoR2Content.orig_ctor orig, RoR2Content self)
        {
            orig(self);
            ror2ContentEarlyLoadHelper.WaitForCompletion();
            ror2ContentEarlyLoadHelper = null;
            self.contentPack = ror2ContentPack;
            GSUtil.Log("ror2 content ctor");
        }
        private void DLC1Content_ctor(On.RoR2.DLC1Content.orig_ctor orig, DLC1Content self)
        {
            orig(self);
            dlc1ContentEarlyLoadHelper.WaitForCompletion();
            dlc1ContentEarlyLoadHelper = null;
            self.contentPack = dlc1ContentPack;
            GSUtil.Log("dlc1 content ctor");
        }
        private void JunkContent_ctor(On.RoR2.JunkContent.orig_ctor orig, JunkContent self)
        {
            orig(self);
            junkContentEarlyLoadHelper.WaitForCompletion();
            junkContentEarlyLoadHelper = null;
            self.contentPack = junkContentPack;
            GSUtil.Log("junk content ctor");
        }

        public override void OnCollectContent(AssetStream sasset)
        {

        }

    }
}

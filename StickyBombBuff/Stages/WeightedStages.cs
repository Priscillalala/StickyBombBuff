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
using HG;

namespace StickyBombBuff.Stages
{

    [Configurable]
    public class WeightedStages : StickyBombBuffModule
    {
        HashSet<SceneCollection> sceneCollectionBlacklist = new HashSet<SceneCollection>();
        public static class SceneWeight 
        {
            public const float ScorchedAcres = 0.9f;
            public const float Skills2_0 = 0.8f;
            public const float HiddenRealms = 0.7f;
            public const float Artifacts = 1f;
            public const float CU5 = 1f;
            public const float SunderedGrove = 0.6f;
            public const float DLC1 = 0.5f;
        }
        public static Dictionary<string, float> sceneToWeightMultiplier = new Dictionary<string, float>();
        public override void OnModInit()
        {
            sceneToWeightMultiplier["golemplains2"] = SceneWeight.HiddenRealms;
            sceneToWeightMultiplier["blackbeach2"] = SceneWeight.HiddenRealms;
            sceneToWeightMultiplier["snowyforest"] = SceneWeight.DLC1;

            sceneToWeightMultiplier["ancientloft"] = SceneWeight.DLC1;
            
            sceneToWeightMultiplier["wispgraveyard"] = SceneWeight.ScorchedAcres;
            sceneToWeightMultiplier["sulfurpools"] = SceneWeight.DLC1;
            
            sceneToWeightMultiplier["shipgraveyard"] = SceneWeight.Skills2_0;
            sceneToWeightMultiplier["rootjungle"] = SceneWeight.SunderedGrove;
            
            sceneToWeightMultiplier["skymeadow"] = SceneWeight.Artifacts;


            On.RoR2.SceneCollection.AddToWeightedSelection += SceneCollection_AddToWeightedSelection;
        }

        private void SceneCollection_AddToWeightedSelection(On.RoR2.SceneCollection.orig_AddToWeightedSelection orig, SceneCollection self, WeightedSelection<SceneDef> dest, Func<SceneDef, bool> canAdd)
        {

            if (!sceneCollectionBlacklist.Contains(self))
            {
                for(int i = 0; i < self.sceneEntries.Length; i++)
                {
                    SceneCollection.SceneEntry entry = self.sceneEntries[i];
                    if (sceneToWeightMultiplier.TryGetValue(entry.sceneDef.baseSceneName, out float multiplier))
                    {
                        entry.weight *= multiplier;
                        self._sceneEntries[i] = entry;
                    }
                }
                sceneCollectionBlacklist.Add(self);
            }
            orig(self, dest, canAdd);
        }
    }
}

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
using RoR2.CharacterAI;

namespace StickyBombBuff.General
{
    [Configurable]
    public class MagmaWorm : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/MagmaWorm/MagmaWormBody.prefab")).Completed += BodyPrefab_Completed;
            EnsureAsyncCompletion(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/MagmaWorm/MagmaWormMaster.prefab")).Completed += MasterPrefab_Completed; ;
        }

        private void MasterPrefab_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= MasterPrefab_Completed;
            AISkillDriver[] skillDrivers = obj.Result.GetComponents<AISkillDriver>();
            for (int i = skillDrivers.Length - 1; i >= 0; i--)
            {
                AISkillDriver driver = skillDrivers[i];
                switch (driver.customName)
                {
                    case "FollowTargetOnNodegraphAndChangeStance":
                    case "Blink":
                        DestroyImmediate(driver);
                        break;
                    case "ChaseAndSteerAtTarget":
                        driver.maxDistance = 10f;
                        driver.ignoreNodeGraph = true;
                        break;
                }
            }
        }

        private void BodyPrefab_Completed(AsyncOperationHandle<GameObject> obj)
        {
            obj.Completed -= BodyPrefab_Completed;
            if(obj.Result.TryGetComponent(out CharacterBody characterBody))
            {
                characterBody.baseArmor = 20f;
            }
            if(obj.Result.TryGetComponent(out WormBodyPositionsDriver wormBodyPositionsDriver))
            {
                wormBodyPositionsDriver.maxBreachSpeed = float.PositiveInfinity;
            }
        }
    }
}

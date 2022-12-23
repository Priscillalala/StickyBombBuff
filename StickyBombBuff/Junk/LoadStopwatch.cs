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
    public class LoadStopwatch : BaseModModule<LoadStopwatch>
    {
        
        public Stopwatch loadStopwatch;
        public override void OnModInit()
        {
            loadStopwatch = new Stopwatch();

            /*loadStopwatch.Start();
            Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").Completed += handle =>
            {
                loadStopwatch.Stop();
                GSUtil.Log(BepInEx.Logging.LogLevel.Warning, $"{loadStopwatch.Elapsed} seconds were spent loading DLC1!");
            };*/
            
            
            loadStopwatch.Restart();

            RoR2Application.onLoad = (Action)Delegate.Combine(RoR2Application.onLoad, new Action(PrintTimer));
        }

        private void PrintTimer()
        {
            loadStopwatch.Stop();
            GSUtil.Log(BepInEx.Logging.LogLevel.Warning, $"{loadStopwatch.Elapsed} seconds were spent loading!");
        }

        public override void OnCollectContent(AssetStream sasset)
        {

        }

    }
}

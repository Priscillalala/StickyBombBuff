using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2.ContentManagement;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;
using System.Reflection;

#pragma warning disable
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: HG.Reflection.SearchableAttribute.OptIn]
#pragma warning restore

namespace StickyBombBuff
{

    public class StickyBombBuffPlugin : BaseModPlugin<StickyBombBuffPlugin>
    {
        public override string PLUGIN_ModName => "StickyBombBuff";
        public override string PLUGIN_AuthorName => "groovesalad";
        public override string PLUGIN_VersionNumber => "1.0.0";
        public override string ENV_RelativeAssetBundleFolder => null;

        /*private bool hasGeneratedEnabledLanguage;
        public static List<string> partialEnabledLanguage;
        public static HashSet<string> enabledLanguage;*/
        public static List<AsyncOperationHandle> asyncOperations = new List<AsyncOperationHandle>();
        public static AssetBundle assets;

        public override void BeginModInit()
        {
            using (Stream manifestResourceStream = assembly.GetManifestResourceStream("StickyBombBuff.Assets.sbbassets"))
            {
                assets = AssetBundle.LoadFromStream(manifestResourceStream);
            }
            On.RoR2.RoR2Application.Awake += RoR2Application_Awake;
        }

        private void RoR2Application_Awake(On.RoR2.RoR2Application.orig_Awake orig, RoR2Application self)
        {
            int count = 0;
            for (int i = 0; i < asyncOperations.Count; i++)
            {
                if (!asyncOperations[i].IsDone)
                {
                    asyncOperations[i].WaitForCompletion();
                    count++;
                }
            }
            asyncOperations = null;
            if (count > 0)
            {
                GSUtil.Log(BepInEx.Logging.LogLevel.Warning, count + " async operations were not completed in time!");
            }
            On.RoR2.RoR2Application.Awake -= RoR2Application_Awake;
            orig(self);
        }

        public override void BeginCollectContent(AssetStream sasset)
        {
        }
        /*private void GenerateEnabledLanguage()
        {
            partialEnabledLanguage = new List<string>();
            enabledLanguage = new HashSet<string>();
            for(int i = 0; i < moduleOrder.Count; i++)
            {
                if(moduleOrder[i] is StickyBombBuffModule sbbmodule)
                {
                    GenerateModuleEnabledLanguage(sbbmodule);
                }
            }
            hasGeneratedEnabledLanguage = true;
        }
        private void GenerateModuleEnabledLanguage(StickyBombBuffModule sbbModule)
        {
            for(int i = 0; i < sbbModule.affectedLanguage.Length; i++)
            {
                object affected = sbbModule.affectedLanguage[i];
                if(affected is string s)
                {
                    partialEnabledLanguage.Add(s);
                }
                else if (affected is ItemDef itemDef)
                {
                    enabledLanguage.Add(itemDef.nameToken);
                    enabledLanguage.Add(itemDef.pickupToken);
                    enabledLanguage.Add(itemDef.descriptionToken);
                    enabledLanguage.Add(itemDef.loreToken);
                }
                else if (affected is EquipmentDef equipmentDef)
                {
                    enabledLanguage.Add(equipmentDef.nameToken);
                    enabledLanguage.Add(equipmentDef.pickupToken);
                    enabledLanguage.Add(equipmentDef.descriptionToken);
                    enabledLanguage.Add(equipmentDef.loreToken);
                }
            }
        }
        public static LanguageCollection FilterLanguageCollection(params (string token, string localizedString)[] strings)
        {
            if (!instance.hasGeneratedEnabledLanguage)
            {
                instance.GenerateEnabledLanguage();
            }
            return (LanguageCollection)strings.Where(s => enabledLanguage.Contains(s.token) || partialEnabledLanguage.Any(x => s.token.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
        }*/
    }
}

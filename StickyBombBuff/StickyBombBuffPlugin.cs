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

        private bool hasGeneratedEnabledLanguage;
        public static List<string> partialEnabledLanguage;
        public static HashSet<string> enabledLanguage;
        internal static List<ReadOnlyContentPack> loadedEarlyContentPacks = new List<ReadOnlyContentPack>(); 
        internal JobHandle loadContentEarly;
        internal static bool isLoadingContentEarly;
        public override void BeginModInit()
        {

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

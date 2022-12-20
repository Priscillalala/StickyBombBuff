using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;

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

        public override void BeginModInit()
        {
        }
        public override void BeginCollectContent(AssetStream sasset)
        {
        }
    }
}

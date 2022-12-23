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

namespace StickyBombBuff.Junk
{

    public struct LoadAddressableContentJob<TAssetSrc, TAssetDest> : IJob
    {
        public void Execute()
        {

        }
    }
}
/*StickyBombBuffPlugin.isLoadingContentEarly = true;
ContentManager.ContentPackLoader loader = new ContentManager.ContentPackLoader(new List<IContentPackProvider> { new RoR2Content(), new JunkContent(), new DLC1Content()});
foreach(ContentPackLoadInfo loadInfo in loader.contentPackLoadInfos)
{
    GSUtil.Log(loadInfo.contentPackProviderIdentifier);
}
ReadableProgress<float> contentLoadProgressReceiver = new ReadableProgress<float>();
Debug.Log("LoadContentEarly start");
while (loader.InitialLoad(contentLoadProgressReceiver).MoveNext()) { }
while (loader.LoadContentPacks(contentLoadProgressReceiver).MoveNext()) { }
while (loader.RunCleanup(contentLoadProgressReceiver).MoveNext()) {  }
Debug.Log("LoadContentEarly end");
ContentManager.SetContentPacks(loader.output);
StickyBombBuffPlugin.loadedEarlyContentPacks.AddRange(loader.output);
StickyBombBuffPlugin.isLoadingContentEarly = false;*/
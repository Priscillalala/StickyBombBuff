using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Diagnostics;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace StickyBombBuff
{

    public abstract class StickyBombBuffModule : BaseModModule
    {
        private byte ilindex;
        private bool _ilfound;
        protected bool ilfound
        {
            get => _ilfound;
            set
            {
                _ilfound = value;
                if (!_ilfound)
                {
                    //StackFrame frame = new StackFrame(1);
                    //(in method {frame.GetMethod().Name})
                    GSUtil.Log(BepInEx.Logging.LogLevel.Error, $"{GetType().Name}: IL failed to find match at index {ilindex}");
                }
                ilindex++;
            }
        }
        public AsyncOperationHandle<T> EnsureAsyncCompletion<T>(AsyncOperationHandle<T> handle)
        {
            StickyBombBuffPlugin.asyncOperations.Add(handle);
            return handle;
        }
        public override void OnModInit()
        {

        }

        public override void OnCollectContent(AssetStream sasset)
        {
            
        }
    }
}

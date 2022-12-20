using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Diagnostics;

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
                    StackFrame frame = new StackFrame(1);
                    GSUtil.Log(BepInEx.Logging.LogLevel.Error, $"{GetType().Name}: IL failed to find match at index {ilindex} (in method {frame.GetMethod().Name})");
                }
                ilindex++;
            }
        }
        public override void OnModInit()
        {

        }

        public override void OnCollectContent(AssetStream sasset)
        {
            
        }
    }
}

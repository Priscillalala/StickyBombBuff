using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using GrooveSharedUtils.Attributes;

namespace StickyBombBuff.Director
{
    [Configurable]
    public class NoFirstStagePreSpawns : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            SceneDirector.onPrePopulateMonstersSceneServer += SceneDirector_onPrePopulateMonstersSceneServer;
        }

        private void SceneDirector_onPrePopulateMonstersSceneServer(SceneDirector obj)
        {
            if(Run.instance.stageClearCount == 0)
            {
                obj.monsterCredit = 0;
            }
        }
    }
}

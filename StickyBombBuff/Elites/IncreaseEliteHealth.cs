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
using static RoR2.RoR2Content.Elites;

namespace StickyBombBuff.General
{
    [Configurable]
    public class IncreaseEliteHealth : StickyBombBuffModule
    {
        public static float t1HealthBoost = 0.7f;
        public static float honorHealthBoost = 0.25f;
        public static float t2HealthBoost = 10.2f;
        public static float lunarHealthBoost = 0.5f;
        public override void OnModInit()
        {
            On.RoR2.CombatDirector.Init += CombatDirector_Init;
        }

        private void CombatDirector_Init(On.RoR2.CombatDirector.orig_Init orig)
        {
            orig();
            Apply(Fire, t1HealthBoost);
            Apply(FireHonor, honorHealthBoost);
            Apply(Poison, t2HealthBoost);
            Apply(Lunar, lunarHealthBoost);
        }
        public void Apply(EliteDef targetElite, float tierHealthBoost)
        {
            CombatDirector.EliteTierDef tier = CombatDirector.eliteTiers.FirstOrDefault(t => Array.IndexOf(t.eliteTypes, targetElite) >= 0);
            if(tier != null)
            {
                for(int i = 0; i < tier.eliteTypes.Length; i++)
                {
                    tier.eliteTypes[i].healthBoostCoefficient += tierHealthBoost;
                }
            }
            else
            {
                string eliteName = targetElite ? targetElite.name : "null";
                GSUtil.Log(BepInEx.Logging.LogLevel.Error, $"{this}: failed to find elite tier that includes elite def \"{eliteName}\"");
            }
        }
    }
}

using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using System.Security;
using System.Security.Permissions;
using GrooveSharedUtils.Attributes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2.Items;
using RoR2.Projectile;
using RoR2.Orbs;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using HG;

namespace StickyBombBuff.Director
{

    [Configurable]
    public class AlwaysIncludeBaseDccs : StickyBombBuffModule
    {
        public override void OnModInit()
        {
            On.RoR2.DccsPool.GenerateWeightedSelection += DccsPool_GenerateWeightedSelection;
        }

        private WeightedSelection<DirectorCardCategorySelection> DccsPool_GenerateWeightedSelection(On.RoR2.DccsPool.orig_GenerateWeightedSelection orig, DccsPool self)
        {
            foreach(DccsPool.Category category in self.poolCategories)
            {
                if(category.includedIfNoConditionsMet.Length > 0)
                {
                    category.alwaysIncluded = ArrayUtils.Join(category.alwaysIncluded, category.includedIfNoConditionsMet);
                    Array.Resize(ref category.includedIfNoConditionsMet, 0);
                }
            }
            return orig(self);
        }
    }
}

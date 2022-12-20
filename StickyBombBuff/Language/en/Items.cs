using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using GrooveSharedUtils.Attributes;
using StickyBombBuff.Items.Tier1;
using static RoR2.RoR2Content.Items;
using static RoR2.DLC1Content.Items;

namespace StickyBombBuff.Languages.en
{
    public static partial class en
    {
        [LanguageCollectionProvider]
        public static LanguageCollection Items() => new LanguageCollection
        {
            (BossDamageBonus.descriptionToken, $"Deal an additional <style=cIsDamage>{_ArmorPiercingRounds.damageBonus:0%}</style> damage <style=cStack>(+{_ArmorPiercingRounds.damageBonusPerStack:0%} per stack)</style> to bosses."),
            (Crowbar.descriptionToken, $"Deal <style=cIsDamage>+{_Crowbar.damageBonus:0%}</style> <style=cStack>(+{_Crowbar.damageBonusPerStack:0%} per stack)</style> damage to enemies above <style=cIsDamage>90% health</style>."),
            (FragileDamageBonus.descriptionToken, $"Increase damage by <style=cIsDamage>{_DelicateWatch.damageBonus:0%}</style> <style=cStack>(+{_DelicateWatch.damageBonusPerStack:0%} per stack)</style>. Taking damage to below <style=cIsHealth>25% health</style> <style=cIsUtility>breaks</style> this item."),
            (NearbyDamageBonus.descriptionToken, $"Increase damage to enemies within <style=cIsDamage>13m</style> by <style=cIsDamage>{_FocusCrystal.damageBonus:0%}</style> <style=cStack>(+{_FocusCrystal.damageBonusPerStack:0%} per stack)</style>."),
            (Tooth.descriptionToken, $"Killing an enemy spawns a <style=cIsHealing>healing orb</style> that heals for <style=cIsHealing>{_MonsterTooth.healing} <style=cStack>(+{_MonsterTooth.healingPerStack} per stack)</style> health</style>."),
            (Medkit.descriptionToken, $"Heal for <style=cIsHealing>{_Medkit.healing} <style=cStack>(+{_Medkit.healingPerStack} per stack)</style> health</style> {_Medkit.delay} seconds after getting hurt."),
            (IgniteOnKill.descriptionToken, $"Killing an enemy <style=cIsDamage>ignites</style> all enemies within <style=cIsDamage>12m</style> <style=cStack>(+4m per stack)</style>. Enemies <style=cIsDamage>burn</style> for <style=cIsDamage>150%</style> <style=cStack>(+75% per stack)</style> base damage."),
            (Mushroom.pickupToken, $"Heal all nearby allies after standing still for {_BustlingFungus.delay} seconds."),
            (Mushroom.descriptionToken, $"After standing still for <style=cIsHealing>{_BustlingFungus.delay}</style> seconds, create a zone that <style=cIsHealing>heals</style> for <style=cIsHealing>{_BustlingFungus.healingPerSecond}</style> <style=cStack>(+{_BustlingFungus.healingPerSecondPerStack} per stack)</style> <style=cIsHealing>health</style> every second to all allies within <style=cIsHealing>3m</style> <style=cStack>(+1.5m per stack)</style>."),
            (PersonalShield.descriptionToken, $"Gain a <style=cIsHealing>{_PersonalShieldGenerator.shield} <style=cStack>(+{_PersonalShieldGenerator.shield} per stack)</style></style> health <style=cIsHealing>shield</style>. Recharges outside of danger."),
            (GoldOnHurt.descriptionToken, $"Gain <style=cIsUtility>{_RollOfPennies.goldGained} <style=cStack>(+{_RollOfPennies.goldGained} per stack)</style> gold</style> on <style=cIsDamage>taking damage</style> from an enemy. <style=cIsUtility>Scales over time.</style>"),
            (HealingPotion.descriptionToken, $"Taking damage to below <style=cIsHealth>25% health</style> <style=cIsUtility>consumes</style> this item, <style=cIsHealing>healing</style> you for <style=cIsHealing>{_PowerElixir.healing} health</style>."),
            (ArmorPlate.descriptionToken, $"Reduce all <style=cIsDamage>incoming damage</style> by <style=cIsDamage>{_RepulsionArmorPlate.flatDamageReduction}<style=cStack> (+{_RepulsionArmorPlate.flatDamageReduction} per stack)</style></style>. Cannot be reduced below <style=cIsDamage>1</style>."),
        };
    }
}

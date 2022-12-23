using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using GrooveSharedUtils.Attributes;
using StickyBombBuff.Items.Tier1;
using StickyBombBuff.Items;
using static StickyBombBuff.Language.SBBLanguage;
using static RoR2.RoR2Content.Items;
using static RoR2.DLC1Content.Items;

namespace StickyBombBuff.Language.en
{
    public static partial class en
    {

        [LanguageCollectionProvider]
        public static LanguageCollection Items() => new[]
        {
            Add<_ArmorPiercingRounds>(BossDamageBonus.descriptionToken, $"Deal an additional <style=cIsDamage>{_ArmorPiercingRounds.damageBonus:0%}</style> damage <style=cStack>(+{_ArmorPiercingRounds.damageBonusPerStack:0%} per stack)</style> to bosses."),
            Add<_Crowbar>(Crowbar.descriptionToken, $"Deal <style=cIsDamage>+{_Crowbar.damageBonus:0%}</style> <style=cStack>(+{_Crowbar.damageBonusPerStack:0%} per stack)</style> damage to enemies above <style=cIsDamage>90% health</style>."),
            Add<_DelicateWatch>(FragileDamageBonus.descriptionToken, $"Increase damage by <style=cIsDamage>{_DelicateWatch.damageBonus:0%}</style> <style=cStack>(+{_DelicateWatch.damageBonusPerStack:0%} per stack)</style>. Taking damage to below <style=cIsHealth>25% health</style> <style=cIsUtility>breaks</style> this item."),
            Add<_FocusCrystal>(NearbyDamageBonus.descriptionToken, $"Increase damage to enemies within <style=cIsDamage>13m</style> by <style=cIsDamage>{_FocusCrystal.damageBonus:0%}</style> <style=cStack>(+{_FocusCrystal.damageBonusPerStack:0%} per stack)</style>."),
            Add<_MonsterTooth>(Tooth.descriptionToken, $"Killing an enemy spawns a <style=cIsHealing>healing orb</style> that heals for <style=cIsHealing>{_MonsterTooth.healing} <style=cStack>(+{_MonsterTooth.healingPerStack} per stack)</style> health</style>."),
            Add<_Medkit>(Medkit.descriptionToken, $"Heal for <style=cIsHealing>{_Medkit.healing} <style=cStack>(+{_Medkit.healingPerStack} per stack)</style> health</style> {_Medkit.delay} seconds after getting hurt."),
            Add<_Gasoline>(IgniteOnKill.descriptionToken, $"Killing an enemy <style=cIsDamage>ignites</style> all enemies within <style=cIsDamage>12m</style> <style=cStack>(+4m per stack)</style>. Enemies <style=cIsDamage>burn</style> for <style=cIsDamage>150%</style> <style=cStack>(+75% per stack)</style> base damage."),
            Add<_BustlingFungus>(Mushroom.pickupToken, $"Heal all nearby allies after standing still for {_BustlingFungus.delay} seconds."),
            Add<_BustlingFungus>(Mushroom.descriptionToken, $"After standing still for <style=cIsHealing>{_BustlingFungus.delay}</style> seconds, create a zone that <style=cIsHealing>heals</style> for <style=cIsHealing>{_BustlingFungus.healingPerSecond}</style> <style=cStack>(+{_BustlingFungus.healingPerSecondPerStack} per stack)</style> <style=cIsHealing>health</style> every second to all allies within <style=cIsHealing>3m</style> <style=cStack>(+1.5m per stack)</style>."),
            Add<_PersonalShieldGenerator>(PersonalShield.descriptionToken, $"Gain a <style=cIsHealing>{_PersonalShieldGenerator.shield} <style=cStack>(+{_PersonalShieldGenerator.shield} per stack)</style></style> health <style=cIsHealing>shield</style>. Recharges outside of danger."),
            Add<_RollOfPennies>(GoldOnHurt.descriptionToken, $"Gain <style=cIsUtility>{_RollOfPennies.goldGained} <style=cStack>(+{_RollOfPennies.goldGained} per stack)</style> gold</style> on <style=cIsDamage>taking damage</style> from an enemy. <style=cIsUtility>Scales over time.</style>"),
            Add<_PowerElixir>(HealingPotion.descriptionToken, $"Taking damage to below <style=cIsHealth>25% health</style> <style=cIsUtility>consumes</style> this item, <style=cIsHealing>healing</style> you for <style=cIsHealing>{_PowerElixir.healing} health</style>."),
            Add<_RepulsionArmorPlate>(ArmorPlate.descriptionToken, $"Reduce all <style=cIsDamage>incoming damage</style> by <style=cIsDamage>{_RepulsionArmorPlate.flatDamageReduction}<style=cStack> (+{_RepulsionArmorPlate.flatDamageReduction} per stack)</style></style>. Cannot be reduced below <style=cIsDamage>1</style>."),
            Add<_FreshMeat>(_FreshMeat.RegenOnKill.nameToken, $"Fresh Meat"),
            Add<_FreshMeat>(_FreshMeat.RegenOnKill.pickupToken, $"Regenerate health after killing an enemy."),
            Add<_FreshMeat>(_FreshMeat.RegenOnKill.descriptionToken, $"Increases <style=cIsHealing>base health regeneration</style> by <style=cIsHealing>+2 hp/s</style> for <style=cIsUtility>{_FreshMeat.duration}s (+{_FreshMeat.duration}s per stack)</style> after killing an enemy."),
            Add<_LensMakersGlasses>(CritGlasses.pickupToken, $"Gain {_LensMakersGlasses.critChance}% chance for hits to 'Critically Strike', dealing double damage."),
            Add<_LensMakersGlasses>(CritGlasses.descriptionToken, $"Your attacks have a <style=cIsDamage>{_LensMakersGlasses.critChance}%</style> <style=cStack>(+{_LensMakersGlasses.critChance}% per stack)</style> chance to '<style=cIsDamage>Critically Strike</style>', dealing <style=cIsDamage>double damage</style>."),
            Add<_TriTipDagger>(BleedOnHit.pickupToken, $"Gain +{_TriTipDagger.bleedChance}% chance to bleed enemies on hit."),
            Add<_TriTipDagger>(BleedOnHit.descriptionToken, $"<style=cIsDamage>{_TriTipDagger.bleedChance}%</style> <style=cStack>(+{_TriTipDagger.bleedChance}% per stack)</style> chance to <style=cIsDamage>bleed</style> an enemy for <style=cIsDamage>240%</style> base damage."),
            Add<_Fireworks>(Firework.descriptionToken, $"Activating an interactable <style=cIsDamage>launches 8 <style=cStack>(+4 per stack)</style> fireworks</style> that deal <style=cIsDamage>{_Fireworks.damageCoefficient:0%}</style> base damage."),
            Add<_StickyBomb, TotalDamageToAdditiveDamage>(StickyBomb.descriptionToken, $"<style=cIsDamage>{(ModuleEnabled<_StickyBomb>() ? _StickyBomb.chance : 5)}%</style> <style=cStack>(+{(ModuleEnabled<_StickyBomb>() ? _StickyBomb.chancePerStack : 5)}% per stack)</style> chance on hit to attach a <style=cIsDamage>bomb</style> to an enemy, detonating for <style=cIsDamage>{(ModuleEnabled<_StickyBomb>() ? _StickyBomb.damageCoefficient : 1.8):0%}</style> " +
                (ModuleEnabled<_StickyBomb>() ? $"<style=cStack>(+{_StickyBomb.damageCoefficientPerStack:0%} per stack)</style> " : string.Empty) +
                $"{TotalDamageToAdditiveDamage.TryAddString("TOTAL ")}damage."),
            Add<_RustedKey>(TreasureCache.pickupToken, $"Gain access to a Rusty Lockbox that contains treasure."),
            Add<_RustedKey>(TreasureCache.descriptionToken, $"A hidden cache containing an item will appear in a random location in each stage.<style=cStack>Increases rarity of the item per stack.</style>"),
            Add<_Warbanner>(WardOnLevel.pickupToken, $"Drop a Warbanner on level up, granting allies attack and movement speed."),
            Add<_Warbanner>(WardOnLevel.pickupToken, $"On <style=cIsUtility>level up</style>, drop a banner that strengthens all allies within <style=cIsUtility>16m</style> <style=cStack>(+8m per stack)</style>. Raise <style=cIsDamage>attack</style> and <style=cIsUtility>movement speed</style> by <style=cIsDamage>30%</style>."),
            Add<_CautiousSlug>(HealWhileSafe.descriptionToken, $"Increases <style=cIsHealing>passive health regeneration</style> by <style=cIsHealing>+{_CautiousSlug.regenMultipler:0%} <style=cStack>(+{_CautiousSlug.regenMultipler:0%} per stack)</style></style> while outside of combat."),
            Add<_OddlyShapedOpal>(OutOfCombatArmor.descriptionToken, $"<style=cIsHealing>Increase armor</style> by <style=cIsHealing>{_OddlyShapedOpal.armor}</style> <style=cStack>(+{_OddlyShapedOpal.armor} per stack)</style> while out of danger."),

        };
    }
}

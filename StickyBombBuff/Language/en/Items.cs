using System;
using RoR2;
using GrooveSharedUtils;
using UnityEngine;
using GrooveSharedUtils.Attributes;
using StickyBombBuff.Items.Tier1;
using StickyBombBuff.Items.Tier2;
using StickyBombBuff.Items.Tier3;
using StickyBombBuff.Items;
using static StickyBombBuff.Language.SBBLanguage;
using static RoR2.RoR2Content.Items;
using static RoR2.DLC1Content.Items;

namespace StickyBombBuff.Language.en
{
    public static partial class en
    {

        [LanguageCollectionProvider]
        public static LanguageCollection Tier1() => new[]
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
            Add<_RustedKey>(TreasureCache.descriptionToken, $"A <style=cIsUtility>hidden cache</style> containing an item (79%/<style=cIsHealing>20%</style>/<style=cIsHealth>1%</style>) will appear in a random location <style=cIsUtility>on each stage</style>. <style=cStack>(Increases rarity of the item per stack)</style>."),
            Add<_Warbanner>(WardOnLevel.pickupToken, $"Drop a Warbanner on level up, granting allies attack and movement speed."),
            Add<_Warbanner>(WardOnLevel.descriptionToken, $"On <style=cIsUtility>level up</style>, drop a banner that strengthens all allies within <style=cIsUtility>16m</style> <style=cStack>(+8m per stack)</style>. Raise <style=cIsDamage>attack</style> and <style=cIsUtility>movement speed</style> by <style=cIsDamage>30%</style>."),
            Add<_CautiousSlug>(HealWhileSafe.descriptionToken, $"Increases <style=cIsHealing>passive health regeneration</style> by <style=cIsHealing>+{_CautiousSlug.regenMultipler:0%} <style=cStack>(+{_CautiousSlug.regenMultipler:0%} per stack)</style></style> while outside of combat."),
            Add<_OddlyShapedOpal>(OutOfCombatArmor.descriptionToken, $"<style=cIsHealing>Increase armor</style> by <style=cIsHealing>{_OddlyShapedOpal.armor}</style> <style=cStack>(+{_OddlyShapedOpal.armor} per stack)</style> while out of danger."),
        };
        [LanguageCollectionProvider]
        public static LanguageCollection Tier2() => new[]
        {
            Add<TotalDamageToAdditiveDamage>(Missile.descriptionToken, $"<style=cIsDamage>10%</style> chance to fire a missile that deals <style=cIsDamage>300%</style> <style=cStack>(+300% per stack)</style> damage."),
            Add<_Chronobauble>(SlowOnHit.descriptionToken, $"<style=cIsUtility>{_Chronobauble.chance}%</style> <style=cStack>(+{_Chronobauble.chancePerStack}% per stack)</style> chance to <style=cIsUtility>slow</style> enemies for <style=cIsUtility>-60% movement speed</style> for <style=cIsUtility>{_Chronobauble.duration}s</style> <style=cStack>(+{_Chronobauble.duration}s per stack)</style>."),
            Add<_DeathMark>(DeathMark.descriptionToken, $"Enemies with <style=cIsDamage>4</style> or more debuffs are <style=cIsDamage>marked for death</style>, increasing damage taken by <style=cIsDamage>50%</style> <style=cStack>(+50% per stack)</style> from all sources for <style=cIsUtility>7</style> seconds."),
            Add<_HarvestersScythe, RemoveBaseCritFromItems>(HealOnCrit.descriptionToken, $"{TryRemove<RemoveBaseCritFromItems>("Gain <style=cIsDamage>5% critical chance</style>. ")}<style=cIsDamage>Critical strikes</style> <style=cIsHealing>heal</style> for <style=cIsHealing>{TryAdd<_HarvestersScythe>(_HarvestersScythe.healing, "8")}</style> <style=cStack>(+{TryAdd<_HarvestersScythe>(_HarvestersScythe.healingPerStack, "4")} per stack)</style> <style=cIsHealing>health</style>."),
            Add<_HuntersHarpoon>(MoveSpeedOnKill.descriptionToken, $"Killing an enemy increases <style=cIsUtility>movement speed</style> by <style=cIsUtility>{_HuntersHarpoon.speedPerBuff * 5:0%}</style>, fading over <style=cIsUtility>1</style> <style=cStack>(+0.5 per stack)</style> seconds."),
            Add<_IgnitionTank>(StrengthenBurn.pickupToken, $"Your ignite effects deal double damage."),
            Add<_IgnitionTank>(StrengthenBurn.descriptionToken, $"Ignite effects deal <style=cIsDamage>+{_IgnitionTank.bonusDamage:0%}</style> <style=cStack>(+{_IgnitionTank.bonusDamage:0%} per stack)</style> more damage over time."),
            Add<_Infusion>(Infusion.descriptionToken, $"Killing an enemy increases your <style=cIsHealing>health permanently</style> by <style=cIsHealing>1</style>, up to a <style=cIsHealing>maximum</style> of <style=cIsHealing>100 <style=cStack>(+100 per stack)</style> health</style>."),
            Add<_StealthKit>(Phasing.pickupToken, $"Turn invisible on taking heavy damage."),
            Add<_StealthKit>(Phasing.descriptionToken, $"Chance on taking damage to gain <style=cIsUtility>40% movement speed</style> and <style=cIsUtility>invisibility</style> for <style=cIsUtility>{_StealthKit.duration}s</style> <style=cStack>(+{_StealthKit.durationPerStack}s per stack)</style>. Chance increases the more damage you take."),
            Add<_PredatoryInstincts>(AttackSpeedOnCrit.descriptionToken, $"<style=cIsDamage>Critical strikes</style> increase <style=cIsDamage>attack speed</style> by <style=cIsDamage>12%</style>. Maximum cap of <style=cIsDamage>36% <style=cStack>(+24% per stack)</style> attack speed</style>."),
            Add<_ElementalBands>(IceRing.pickupToken, $"Chance on hit to strike an enemy with a runic ice blast."),
            Add<_ElementalBands, TotalDamageToAdditiveDamage>(IceRing.descriptionToken, ModuleEnabled<_ElementalBands>() ? 
                $"<style=cIsDamage>8%</style> chance on hit to strike an enemy with a <style=cIsDamage>runic ice blast</style>, <style=cIsUtility>slowing</style> them by <style=cIsUtility>80%</style> and dealing <style=cIsDamage>{_ElementalBands.iceRingDamageCoefficient:0%}</style> <style=cStack>(+{_ElementalBands.iceRingDamageCoefficientPerStack:0%} per stack)</style> {TryRemove<TotalDamageToAdditiveDamage>("TOTAL ")}damage.": 
                $"Hits that deal <style=cIsDamage>more than 400% damage</style> also blasts enemies with a <style=cIsDamage>runic ice blast</style>, <style=cIsUtility>slowing</style> them by <style=cIsUtility>80%</style> for <style=cIsUtility>3s</style> <style=cStack>(+3s per stack)</style> and dealing <style=cIsDamage>250%</style> <style=cStack>(+250% per stack)</style> {TryRemove<TotalDamageToAdditiveDamage>("TOTAL ")}damage. Recharges every <style=cIsUtility>10</style> seconds."),
            Add<_ElementalBands>(FireRing.pickupToken, $"Chance on hit to strike an enemy with a runic flame tornado."),
            Add<_ElementalBands, TotalDamageToAdditiveDamage>(FireRing.descriptionToken, ModuleEnabled<_ElementalBands>() ?
                $"<style=cIsDamage>8%</style> chance on hit to strike an enemy with a <style=cIsDamage>runic flame tornado</style>, dealing <style=cIsDamage>{_ElementalBands.fireRingDamageCoefficient:0%}</style> <style=cStack>(+{_ElementalBands.fireRingDamageCoefficientPerStack:0%} per stack)</style> {TryRemove<TotalDamageToAdditiveDamage>("TOTAL ")}damage.":
                $"Hits that deal <style=cIsDamage>more than 400% damage</style> also blasts enemies with a <style=cIsDamage>runic flame tornado</style>, dealing <style=cIsDamage>300%</style> <style=cStack>(+300% per stack)</style> {TryRemove<TotalDamageToAdditiveDamage>("TOTAL ")}damage over time. Recharges every <style=cIsUtility>10</style> seconds."),
            Add<_ShippingRequestForm>(FreeChest.pickupToken, $"Get a delivery on the next stage that contains powerful items."),
            Add<_ShippingRequestForm>(FreeChest.descriptionToken, $"A <style=cIsUtility>delivery</style> containing 2 items (<style=cIsHealing>80%</style>/<style=cIsHealth>20%</style>) will appear in a random location on the next stage, <style=cIsUtility>consuming</style> this item."),
            Add<_ShippingRequestForm>(_ShippingRequestForm.FreeChestConsumed.nameToken, $"Shipping Request Form (Redeemed)"),
            Add<_ShippingRequestForm>(_ShippingRequestForm.FreeChestConsumed.pickupToken, $"<style=cMono>Thank you, valued customer!</style>"),
            Add<_ShippingRequestForm>(_ShippingRequestForm.FreeChestConsumed.descriptionToken, $"A spent item with no remaining power."),
            Add<_Shuriken>(PrimarySkillShuriken.pickupToken, $"Chance on 'Critical Strike' to also throw a shuriken."),
            Add<_Shuriken>(PrimarySkillShuriken.descriptionToken, $"{TryRemove<RemoveBaseCritFromItems>("Gain <style=cIsDamage>5% critical chance</style>. ")}<style=cIsDamage>{_Shuriken.chance}%</style> chance on <style=cIsDamage>critical strike</style> to throw a shuriken that deals <style=cIsDamage>{_Shuriken.damageCoefficient:0%}</style> <style=cStack>(+{_Shuriken.damageCoefficientPerStack:0%} per stack)</style> {TryRemove<TotalDamageToAdditiveDamage>("TOTAL ")}damage."),
            Add<_OldGuillotine>(ExecuteLowHealthElite.descriptionToken, $"Instantly kill Elite monsters below <style=cIsHealth>{_OldGuillotine.executeThreshold}% <style=cStack>(+{_OldGuillotine.executeThreshold}% per stack)</style> health</style>."),
            Add<TotalDamageToAdditiveDamage>(ChainLightning.descriptionToken, $"<style=cIsDamage>25%</style> chance to fire <style=cIsDamage>chain lightning</style> for <style=cIsDamage>80%</style> damage on up to <style=cIsDamage>3 <style=cStack>(+2 per stack)</style></style> targets within <style=cIsDamage>20m</style> <style=cStack>(+2m per stack)</style>."),

        };
        [LanguageCollectionProvider]
        public static LanguageCollection Tier3() => new[]
        {
            Add<_Aegis>(BarrierOnOverHeal.pickupToken, $"Healing past full grants you a weak temporary barrier."),
            Add<_Aegis>(BarrierOnOverHeal.descriptionToken, $"Healing past full grants you a <style=cIsHealing>temporary barrier</style> for <style=cIsHealing>{_Aegis.barrierCoefficient:0%} <style=cStack>(+{_Aegis.barrierCoefficientPerStack:0%} per stack)</style></style> of the amount you healed, up to <style=cIsHealing>25%</style> of your <style=cIsHealing>maximum health</style>."),
            Add<_ResonanceDisc>(LaserTurbine.descriptionToken, $"Killing enemies charges the Resonance Disc. The disc launches itself toward a target for <style=cIsDamage>300%</style> base damage <style=cStack>(+300% per stack)</style>, piercing all enemies it doesn't kill, and then explodes for <style=cIsDamage>1000%</style> base damage <style=cStack>(+1000% per stack)</style>. Returns to the user, striking all enemies along the way for <style=cIsDamage>300%</style> base damage <style=cStack>(+300% per stack)</style>."),
            Add<_BensRaincoat>(ImmuneToDebuff.pickupToken, $"Chance to prevent debuffs."),
            Add<_BensRaincoat>(ImmuneToDebuff.descriptionToken, $"<style=cIsUtility>{_BensRaincoat.chance}%</style> <style=cStack>(+{_BensRaincoat.chance}% per stack)</style> chance to prevent incoming debuffs."),
            Add<_Brainstalks>(KillEliteFrenzy.descriptionToken, $"Upon killing an elite monster, <style=cIsDamage>enter a frenzy</style> for <style=cIsDamage>{_Brainstalks.duration}s</style> <style=cStack>(+{_Brainstalks.durationPerStack}s per stack)</style> where <style=cIsUtility>skills have no cooldowns</style>."),
            Add<TotalDamageToAdditiveDamage>(Behemoth.descriptionToken, $"All your <style=cIsDamage>attacks explode</style> in a <style=cIsDamage>4m </style> <style=cStack>(+1.5m per stack)</style> radius for a bonus <style=cIsDamage>60%</style> damage to nearby enemies."),
            Add<_HappiestMask>(GhostOnKill.descriptionToken, $"Killing enemies has a <style=cIsDamage>{_HappiestMask.chance}%</style> chance to <style=cIsDamage>spawn a ghost</style> of the killed enemy with <style=cIsDamage>+{_HappiestMask.damageBoost * 10}%</style> damage. Lasts <style=cIsDamage>30s</style> <style=cStack>(+30s per stack)</style>."),
            Add<_InterstellarDeskPlant>(Plant.descriptionToken, $"On kill, plant a <style=cIsHealing>healing</style> fruit seed that grows into a plant after <style=cIsUtility>5</style> seconds. \n\nThe plant <style=cIsHealing>heals</style> for <style=cIsHealing>{_InterstellarDeskPlant.healingPerSecond} health</style> every second to all allies within <style=cIsHealing>{_InterstellarDeskPlant.baseRadius + _InterstellarDeskPlant.radiusPerStack}m</style> <style=cStack>(+{_InterstellarDeskPlant.radiusPerStack}m per stack)</style>. Lasts <style=cIsUtility>10</style> seconds."),
            Add<TotalDamageToAdditiveDamage>(BounceNearby.descriptionToken, $"<style=cIsDamage>20%</style> <style=cStack>(+20% per stack)</style> chance on hit to <style=cIsDamage>fire homing hooks</style> at up to <style=cIsDamage>10</style> <style=cStack>(+5 per stack)</style> enemies for <style=cIsDamage>100%</style> damage."),
            Add<_ShatteringJustice>(ArmorReductionOnHit.descriptionToken, $"After hitting an enemy <style=cIsDamage>5</style> times, reduce their <style=cIsDamage>armor</style> by <style=cIsDamage>{_ShatteringJustice.armorReduction}</style> for <style=cIsDamage>8</style><style=cStack> (+8 per stack)</style> seconds."),
            Add<_FrostRelic>(Icicle.descriptionToken, $"Killing enemies surrounds you with an <style=cIsDamage>ice storm</style> that deals up to <style=cIsDamage>{_FrostRelic.dpsPerIcicle*_FrostRelic.maxIcicleCount:0%} <style=cStack>(+{_FrostRelic.dpsPerIciclePerStack*_FrostRelic.maxIcicleCount:0%} per stack)</style> damage per second</style> to enemies within <style=cIsDamage>{_FrostRelic.radius}m</style>."),
            Add<_H3AD5Tv2>(FallBoots.descriptionToken, $"Increase <style=cIsUtility>jump height</style>. Creates a <style=cIsDamage>10m</style> radius <style=cIsDamage>kinetic explosion</style> on hitting the ground, dealing <style=cIsDamage>{_H3AD5Tv2.baseDamageCoefficient:0%}</style> base damage that scales up with <style=cIsDamage>speed</style>. Recharges in <style=cIsDamage>10</style> <style=cStack>(-50% per stack)</style> seconds."),

        };
    }
}

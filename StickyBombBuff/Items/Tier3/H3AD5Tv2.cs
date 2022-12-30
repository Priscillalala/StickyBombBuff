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
using static RoR2.RoR2Content.Buffs;
using EntityStates.Headstompers;

namespace StickyBombBuff.Items.Tier3
{

    [Configurable]
    public class _H3AD5Tv2 : StickyBombBuffModule
    {
        public static float baseDamageCoefficient = 23f;
        public static float speedDamageCoefficient = 7f;

        static float currentImpactSpeed;
        public override void OnModInit()
        {
            IL.EntityStates.Headstompers.HeadstompersFall.OnEnter += HeadstompersFall_OnEnter;
            IL.EntityStates.Headstompers.HeadstompersFall.DoStompExplosionAuthority += HeadstompersFall_DoStompExplosionAuthority;
        }

        private void HeadstompersFall_OnEnter(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            ILLabel breakLabel = null;
            ilfound = c.TryGotoNext(MoveType.Before,
                x => x.MatchLdarg(0),
                x => x.MatchLdfld<BaseHeadstompersState>(nameof(BaseHeadstompersState.bodyMotor)),
                x => x.MatchCallOrCallvirt<UnityEngine.Object>("op_Implicit"),
                x => x.MatchBrfalse(out breakLabel),
                x => x.MatchLdarg(0),
                x => x.MatchLdfld<BaseHeadstompersState>(nameof(BaseHeadstompersState.bodyMotor)),
                x => x.MatchLdflda<CharacterMotor>(nameof(CharacterMotor.velocity))
                );
            if (ilfound)
            {
                c.Emit(OpCodes.Br, breakLabel);
            }
        }

        private void HeadstompersFall_DoStompExplosionAuthority(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            int locDamageCoefficientIndex = -1;
            ilfound = c.TryGotoNext(MoveType.Before,
                x => x.MatchLdarg(0),
                x => x.MatchLdfld<BaseHeadstompersState>(nameof(BaseHeadstompersState.body)),
                x => x.MatchCallOrCallvirt<CharacterBody>("get_damage"),
                x => x.MatchLdloc(out locDamageCoefficientIndex),
                x => x.MatchMul(),
                x => x.MatchStfld<BlastAttack>(nameof(BlastAttack.baseDamage))                
                );
            if (ilfound)
            {
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate<Func<HeadstompersFall, float>>(fall =>
                {
                    float impactSpeed = fall.bodyMotor ? (-fall.bodyMotor.velocity.y) : 0f;
                    float bonusSpeed = Mathf.Max(0f, currentImpactSpeed - HeadstompersFall.maxFallSpeed);
                    return baseDamageCoefficient + speedDamageCoefficient * bonusSpeed;
                });
                c.Emit(OpCodes.Stloc, locDamageCoefficientIndex);
            }
            ilfound = c.TryGotoPrev(MoveType.Before,
                x => x.MatchLdarg(0),
                x => x.MatchLdfld<BaseHeadstompersState>(nameof(BaseHeadstompersState.bodyMotor)),
                x => x.MatchCallOrCallvirt<Vector3>("get_zero"),
                x => x.MatchStfld<CharacterMotor>(nameof(CharacterMotor.velocity))
                );
            if (ilfound)
            {
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate<Action<HeadstompersFall>>(fall =>
                {
                    float impactSpeed = fall.bodyMotor ? (-fall.bodyMotor.velocity.y) : 0f;
                    currentImpactSpeed = impactSpeed;
                });
            }
        }
    }
}

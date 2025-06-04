using EntityStates;
using IL.RoR2.Skills;
using MadokaMagica.MamiTamoe;
using MadokaMagica.MamiTamoe.Components;
using MadokaMagica.MamiTamoe.Pickupables;
using RoR2;
using UnityEngine;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class SpawnGun : BaseSkillState
    {
        public static float damageCoefficient = MamiStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1.2f;
        public static float baseDuration = 0.7f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.7f;
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;

        private float duration;
        private float fireTime;

        public DamageSource damageSource;
        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.armor += 800;
            base.characterMotor.enabled = false;
            duration = baseDuration / attackSpeedStat;
            fireTime = duration / skillLocator.utility.stock;
            characterBody.SetAimTimer(2f);
            this.damageSource = DamageSource.Utility;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= fireTime && skillLocator.utility.stock > 0 && skillLocator.secondary.stock < skillLocator.secondary.maxStock)
            {
                skillLocator.secondary.AddOneStock();
                skillLocator.utility.stock--;
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
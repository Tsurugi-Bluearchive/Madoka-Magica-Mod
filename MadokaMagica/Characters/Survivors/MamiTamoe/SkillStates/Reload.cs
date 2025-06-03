using MadokaMagica.MamiTamoe.BaseStates;
using MadokaMagica.Modules.BaseStates;
using RoR2;
using UnityEngine;
using System.Collections;
using MadokaMagica.MamiTamoe.Pickupables;
using EntityStates;
using R2API;
using MadokaMagica.MamiTamoe.Melee;
using MadokaMagica.MamiTamoe.Achievements;
using MadokaMagica.MamiTamoe.SkillStates;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class Reload : BaseSkillState
    {
        public static float damageCoefficient = MamiStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1.2f;
        public static float baseDuration = 0.2f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0f;
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;
        public static EntityState ReloadState;

        private float duration;

        private bool restocking;
        public void ReturnOuter()
        {
            outer.SetNextStateToMain();
        }
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(2f);
        }

        public override void OnExit()
        {
            base.OnExit();
            var previousStock = skillLocator.secondary.stock;
            skillLocator.secondary.UnsetSkillOverride(this.gameObject, MamiSurvivor.reload, GenericSkill.SkillOverridePriority.Default);
            skillLocator.secondary.stock = previousStock;
        }

        public override void FixedUpdate()
        {

            if (fixedAge > duration && !restocking || inputBank.skill2.justPressed)
            {
                restocking = true;
                Log.Debug("Reloading!");
                if (skillLocator.secondary.stock >= skillLocator.primary.maxStock)
                {
                    skillLocator.primary.stock = skillLocator.primary.maxStock;
                    skillLocator.secondary.stock -= skillLocator.primary.maxStock;
                    outer.SetNextStateToMain();
                    return;
                }
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

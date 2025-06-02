using MadokaMagica.MamiTamoe.BaseStates;
using MadokaMagica.Modules.BaseStates;
using RoR2;
using UnityEngine;
using System.Collections;
using MadokaMagica.MamiTamoe.Pickupables;
using EntityStates;
using R2API;
using UnityEngine;
using MadokaMagica.MamiTamoe.Melee;
using MadokaMagica.MamiTamoe.Achievements;
using MadokaMagica.MamiTamoe.SkillStates;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class Collect : BaseSkillState
    {
        public static float damageCoefficient = MamiStaticValues.bigGunDamageCefficeient;
        public static float procCoefficient = 3f;
        public static float baseDuration = 0.4f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0f;
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;
        public static GameObject muzzleEffect;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private int removeStock = 0;
        private int previousStock;
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            if (skillLocator.primary.maxStock > skillLocator.primary.stock)
            {

                previousStock = skillLocator.secondary.stock;
                removeStock = 1;
                for (int i = 1; skillLocator.secondary.stock > removeStock && skillLocator.primary.maxStock > skillLocator.primary.stock; i++)
                {
                    removeStock = i;
                    skillLocator.primary.AddOneStock();
                }
                skillLocator.secondary.stock = previousStock - removeStock;
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            base.characterMotor.enabled = true;
            base.characterMotor.velocity = Vector3.zero;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
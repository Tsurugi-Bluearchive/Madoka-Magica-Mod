using EntityStates;
using MadokaMagica.Zilu.Content;
using MadokaMagica.Zilu.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace MadokaMagica.Zilu.SkillStates
{
    public class SpawnGun : BaseZiluSkillState
    {
        public static float damageCoefficient = ZiluStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1.2f;
        public static float baseDuration = 0.6f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.7f;
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;

        private float duration;
        private float fireTime;
        private Vector3 originalPos;

        public DamageSource damageSource;

        private int SecondaryStock;
        private int SecondaryMax;
        private void FetchFixedVairables()
        {
            SecondaryMax = skillLocator.secondary.maxStock;
            SecondaryStock = skillLocator.secondary.stock;
        }
        private void InitOnEnterVars()
        {
            originalPos = characterBody.corePosition;
            this.damageSource = DamageSource.Utility;
        }
        private void DisableMovement()
        {
            if (isAuthority)
            {
                characterMotor.Motor.SetPosition(originalPos);
                characterMotor.velocity = Vector3.zero;
            }
        }
        //SpawnGun.cs Code Start
        
        //SpawnGun.cs OnEnter()
        public override void OnEnter()
        {
            base.OnEnter();
            InitOnEnterVars();
            characterBody.SetAimTimer(2f);
        }

        //SpawnGun.cs OnExit()
        public override void OnExit()
        {
            base.OnExit();
        }

        //SpawnGun.cs FixedUpdate()
        public override void FixedUpdate()
        {
            FetchFixedVairables();
            base.FixedUpdate();
            DisableMovement();
            duration = baseDuration / attackSpeedStat;
            fireTime = duration / skillLocator.utility.stock;

            //SpawnGun.cs Reload Logic
            if (fixedAge >= fireTime && SecondaryStock > 0 && SecondaryStock < SecondaryMax )
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
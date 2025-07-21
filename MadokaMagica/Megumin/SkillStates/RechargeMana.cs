using EntityStates;
using MadokaMagica.MamiTamoe.Content;
using MadokaMagica.Megumin.Content;
using MadokaMagica.Megumin.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace MadokaMagica.Megumin.SkillStates
{
    public class SpawnGun : BaseMeguminSkillState
    {
        public static float damageCoefficient = MeguminStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1.2f;
        public static float baseDuration = 0.6f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.7f;
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;

        private float duration => baseDuration / attackSpeedStat;
        private float fireTime => duration / skillLocator.utility.stock;
        private Vector3 originalPos => characterBody.corePosition;

        public DamageSource damageSource => DamageSource.Utility;

        private int SecondaryStock
        {
            get => skillLocator.secondary.stock;
            set => skillLocator.secondary.stock = value;
        } 
        private int SecondaryMax => skillLocator.secondary.maxStock;
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
            base.FixedUpdate();
            DisableMovement();

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
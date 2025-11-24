using EntityStates;
using MadokaMagica.Crystal.SkillStates.BaseStates;
using MadokaMagica.Crystal.Content;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using static Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI;
using static UnityEngine.ParticleSystem.PlaybackState;
using UnityEngine;
using RoR2BepInExPack.GameAssetPaths;
using MadokaMagica.Megumin.Content;

namespace MadokaMagica.Crystal.SkillStates
{
    internal class Flick : BaseCrystalSkillState
    {
        public static float damageCoefficient = CrystalStaticValues.bigGunDamageCefficeient;
        public static float procCoefficient = 1.2f;
        public static float nonManaDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.7f;
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;
        public static GameObject muzzleEffect;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        private float minDuration => 0.2f;
        private bool hasFired = false;
        private Vector2 DashDirection => inputBank.moveVector;
        private string muzzleString => "Muzzle";
        private Vector3 originalpos => characterBody.corePosition;

        private float secondaryStock;
        private float secondaryStockMax;

        private float damage;
        private float charge;
        private float tick = 0f;

        private float previousHealth = 0;
        private float accumulatedSelfDamage = 0;
        private bool appliedDOT = false;
        private float DOTDamage = 0;
        public DamageSource DamageSource => DamageSource.Secondary;

        private bool dashed;
        private void InitOnEnterVars()
        {
            characterBody.SetAimTimer(2f);
        }
        private void Firing()
        {
            Fire();
            outer.SetNextStateToMain();
            return;
        }
        //PrecisionStrike.cs Code Start
        public override void OnEnter()
        {
            base.OnEnter();
            previousHealth = this.healthComponent.combinedHealth;
            InitOnEnterVars();
        }
        //PrecisionStrike.cs OnExit()
        public override void OnExit()
        {
            base.OnExit();
        }
        //PrecisionStrike.cs FixedUpdate()
        public override void FixedUpdate()
        {
  
            base.FixedUpdate();

        }
        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;
            }
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}

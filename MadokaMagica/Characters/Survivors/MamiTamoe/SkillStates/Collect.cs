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
        public static float baseDuration = 3f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.7f;
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;
        public static GameObject muzzleEffect;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.armor += 900;
            base.characterMotor.enabled = false;
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            base.OnExit();
            base.characterMotor.enabled = true;
            base.characterMotor.velocity = Vector3.zero;
            base.characterBody.armor -= 900;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
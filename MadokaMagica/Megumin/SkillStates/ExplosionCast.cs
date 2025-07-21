using EntityStates;
using EntityStates.VagrantMonster;
using IL.RoR2.Projectile;
using MadokaMagica.MamiTamoe.Content;
using MadokaMagica.Megumin.Content;
using MadokaMagica.Megumin.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
namespace MadokaMagica.Megumin.SkillStates
{
    public class ExplosionCast : BaseMeguminSkillState
    {
        public static float damageCoefficient = MeguminStaticValues.gunDamageCoefficient;
        private float m_damageCoefficient => damageCoefficient * (fixedAge / fireTime);
        public static float procCoefficient = 1.2f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.7f;
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;
        public static GameObject muzzleEffect;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        private static GameObject explosionProjectile => MegmuminAssets.MeguminPrimaryExplosion;
        private GameObject instantiatedProjectile;
        private float duration => baseDuration / attackSpeedStat;
        private float fireTime => firePercentTime * duration;
        public DamageSource damageSource => DamageSource.Secondary;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
            instantiatedProjectile = GameObject.Instantiate(explosionProjectile);
            instantiatedProjectile.GetComponent<Rigidbody>().velocity = characterBody.aimOriginTransform.position * 10;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
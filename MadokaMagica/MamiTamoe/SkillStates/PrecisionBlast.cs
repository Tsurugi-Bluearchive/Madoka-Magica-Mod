using EntityStates;
using MadokaMagica.MamiTamoe.Content;
using MadokaMagica.MamiTamoe.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class PrecisionBlast : BaseMamiSkillState
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

        public DamageSource damageSource;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.armor += 900;
            base.characterMotor.enabled = false;
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(3f);
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            damageSource = DamageSource.Special;
        }

        public override void OnExit()
        {
            base.OnExit();
            base.characterMotor.enabled = true;
            base.characterMotor.velocity = Vector3.zero;
            base.characterBody.armor -= 900;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= fireTime)
            {
                Fire();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryShootPistol", gameObject);
                if (isAuthority)
                {
                    var aimRay = GetAimRay();
                    AddRecoil(-0.5f * recoil, -0.5f * recoil, 1f * recoil, 2f * recoil);
                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damageCoefficient * damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageTypeCombo.GenericSecondary,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = range,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = RollCrit(),
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = true,
                        procChainMask = default,
                        procCoefficient = procCoefficient,
                        radius = 5f,
                        sniper = false,
                        stopperMask = LayerIndex.world.mask,
                        weapon = null,
                        tracerEffectPrefab = tracerEffectPrefab,
                        spreadPitchScale = 1f,
                        spreadYawScale = 1f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    }.Fire();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
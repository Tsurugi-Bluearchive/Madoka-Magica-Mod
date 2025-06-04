using EntityStates;
using IL.RoR2.Skills;
using MadokaMagica.MamiTamoe;
using MadokaMagica.MamiTamoe.Components;
using MadokaMagica.MamiTamoe.Pickupables;
using RoR2;
using UnityEngine;
using MadokaMagica.MamiTamoe.SkillStates;
using MadokaMagica.Modules;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class PrecisionStrkie : BaseSkillState
    {
        public static float damageCoefficient = MamiStaticValues.gunDamageCoefficient;
        private float m_damageCoefficient = damageCoefficient;
        public static float procCoefficient = 1.2f;
        public static float baseDuration = 1f;
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
        private Vector3 originalpos;

        public DamageSource damageSource;

        private bool dashed;

        public override void OnEnter()
        {
            dashed = false;
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            damageSource = DamageSource.Primary;
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
            originalpos = characterBody.corePosition;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (skillLocator.primary.stock == 0)
            {
                var previousStock = skillLocator.secondary.stock;
                skillLocator.secondary.SetSkillOverride(this.gameObject, MamiSurvivor.reload, GenericSkill.SkillOverridePriority.Default);
                skillLocator.secondary.stock = previousStock;
                
            }
            characterBody.isSprinting = true;
        }

        public override void FixedUpdate()
        {
            var DashDirection = inputBank.moveVector;
            base.FixedUpdate();

            if (isAuthority)
            {
                characterMotor.Motor.SetPosition(originalpos);
                characterMotor.velocity = Vector3.zero;
            }
            if (fixedAge >= fireTime && isAuthority && inputBank.skill1.down || fireTime <= 0.2)
            {
                m_damageCoefficient = damageCoefficient;
                Fire();
                outer.SetNextStateToMain();
                return;
            }
            if (inputBank.skill1.justReleased && fixedAge >= 0.2f)
            {
                m_damageCoefficient = damageCoefficient * (fixedAge/fireTime);
                Fire();
                outer.SetNextStateToMain();
                return;
            }
            else if (fixedAge < 0.2f && !inputBank.skill1.down)
            {
                skillLocator.primary.stock++;
                outer.SetNextStateToMain();
                return;
            }

            if (inputBank.jump.justPressed && isAuthority)
            {
                characterBody.characterMotor.velocity = new Vector3(DashDirection.x * 100, 0, DashDirection.y * 100);
                characterBody.characterMotor.jumpCount++;
                outer.SetNextStateToMain();
                return;
            }
            else if (isAuthority && !inputBank.jump.justPressed)
            {
                this.characterMotor.Motor.SetPosition(this.originalpos);
                this.characterBody.characterMotor.velocity = Vector3.zero;
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
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);
                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = m_damageCoefficient * damageStat,
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
                        radius = 0.75f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
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
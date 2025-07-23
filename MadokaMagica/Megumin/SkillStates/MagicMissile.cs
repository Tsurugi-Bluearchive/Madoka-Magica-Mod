using EntityStates;
using HG;
using MadokaMagica.MamiTamoe.Content;
using MadokaMagica.Megumin.Content;
using MadokaMagica.Megumin.MeguminComponents;
using MadokaMagica.Megumin.SkillStates.BaseStates;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MadokaMagica.Megumin.SkillStates
{
    public class MagicMissile : GenericProjectileBaseState
    {
        public float damageCoefficient = MeguminStaticValues.bigGunDamageCefficeient;
        public float procCoefficient = 3f;
        public float baseDuration = 3f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.7f;
        public static float recoil = 10f;
        public static float range = 256f;
        public static GameObject muzzleEffect;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        private bool hasFired;
        private BullseyeSearch magicSearch;
        private Vector3 originalpos;

        public void DisableMovement()
        {
            if (isAuthority)
            {
                characterMotor.Motor.SetPosition(originalpos);
                characterMotor.velocity = Vector3.zero;
            }
        }
        public override void OnEnter()
        {
            originalpos = characterBody.corePosition;
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(3f);
            magicSearch = new BullseyeSearch();
            magicSearch.minAngleFilter = 180f;
            magicSearch.maxAngleFilter = 180f;
            magicSearch.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
            magicSearch.FilterOutGameObject(this.gameObject);

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
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
            DisableMovement();
            base.FixedUpdate();
            float tick = fixedAge % 0.2f;

            if (inputBank.skill3.down && isAuthority && tick > 0.1f && !hasFired)
            {
                Fire();
                hasFired = true;
            }
            else if (tick < 0.1f && inputBank.skill3.down)
            {
                hasFired = false;
            }
            else if (inputBank.skill3.justReleased)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            // Generate a random index
            magicSearch.RefreshCandidates();
            List<HurtBox> hurtboxPool = new List<HurtBox>();
            Log.Debug($"{MeguminAssets.magicMissle}");
            if (magicSearch.GetResults() != null)
            {
                hurtboxPool = new List<HurtBox>(magicSearch.GetResults());
                int randomIndex = UnityEngine.Random.Range(0, hurtboxPool.Count - 1);
                Log.Debug($"hurtpool count {hurtboxPool.Count}, random index {randomIndex}");
                HurtBox hurtBox = hurtboxPool[randomIndex];
                if (hurtBox != null && hurtboxPool.Count > 0)
                {
                    HurtBox randomTarget = hurtboxPool[randomIndex];
                    // Check conditions and fire missile
                    if (randomTarget == hurtBox)
                    {
                        var missile = new FireProjectileInfo
                        {
                            projectilePrefab = MeguminAssets.magicMissle,
                            position = this.characterBody.aimOrigin + (characterDirection.forward * 3),
                            owner = this.teamComponent.gameObject,
                            damage = this.damageStat * damageCoefficient,
                            crit = RollCrit(),
                            force = 5,
                            damageColorIndex = DamageColorIndex.Default,
                            target = hurtBox.gameObject,
                            speedOverride = 10,
                            maxDistance = 200,
                            procChainMask = default,
                            damageTypeOverride = DamageType.Generic
                        };
                        ModifyProjectileInfo(ref missile);
                        ProjectileManager.instance.FireProjectile(missile);
                    }
                }
            }                   
        }

        public override void ModifyProjectileInfo(ref FireProjectileInfo fireProjectileInfo)
        {
            base.ModifyProjectileInfo(ref fireProjectileInfo);
            DamageTypeCombo damage = DamageTypeCombo.Generic;
            DamageAPI.AddModdedDamageType(ref damage, MeguminCustomDamageTypes.HealorHurt);
            fireProjectileInfo.damageTypeOverride = damage;
            Log.Debug($"{damage} Damage Type");
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
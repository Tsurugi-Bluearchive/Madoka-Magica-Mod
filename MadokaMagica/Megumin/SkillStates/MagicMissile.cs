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
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace MadokaMagica.Megumin.SkillStates
{
    public class MagicMissile : BaseMeguminSkillState
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
        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.SetAimTimer(3f);
            magicSearch = new BullseyeSearch();
            magicSearch.minAngleFilter = 0f;
            magicSearch.maxAngleFilter = 180f;
            magicSearch.maxDistanceFilter = float.MaxValue;
            magicSearch.minDistanceFilter = 5f;
            magicSearch.viewer = this.characterBody;
            magicSearch.searchOrigin = this.characterBody.aimOrigin;
            magicSearch.searchDirection = this.characterDirection.forward;
            magicSearch.sortMode = BullseyeSearch.SortMode.Angle;
            magicSearch.teamMaskFilter = TeamMask.all;
            magicSearch.FilterOutGameObject(this.characterBody.gameObject);

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
            float tick = fixedAge % 0.3f;
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
            List<HurtBox> list = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
            foreach (HurtBox result in magicSearch.GetResults())
            {
                list.Add(result);
            }

            Util.ShuffleList(list);

            var randomHurtbox = list.FirstOrDefault();

            Log.Debug($"{randomHurtbox}");

            CollectionPool<HurtBox, List<HurtBox>>.ReturnCollection(list);
            if (randomHurtbox != null)
            {
                var missile = new FireProjectileInfo
                {
                    projectilePrefab = MeguminAssets.magicMissle,
                    position = this.characterBody.aimOrigin + (characterDirection.forward * 3),
                    owner = this.teamComponent.gameObject,
                    damage = this.damageStat * damageCoefficient,
                    crit = RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    target = randomHurtbox.gameObject,
                    maxDistance = 200,
                    procChainMask = default,
                    damageTypeOverride = DamageType.Generic,
                };
                ModifyProjectileInfo(ref missile);
                ProjectileManager.instance.FireProjectile(missile);
            }
        }                   
        

        public void ModifyProjectileInfo(ref FireProjectileInfo fireProjectileInfo)
        {
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
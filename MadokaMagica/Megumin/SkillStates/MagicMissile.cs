using EntityStates;
using HG;
using MadokaMagica.MamiTamoe.Content;
using MadokaMagica.Megumin.Content;
using MadokaMagica.Megumin.MeguminComponents;
using MadokaMagica.Megumin.SkillStates.BaseStates;
using R2API;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.SendMouseEvents;

namespace MadokaMagica.Megumin.SkillStates
{
    public class MagicMissile : BaseMeguminSkillState
    {
        public float damageCoefficient = MeguminStaticValues.magicMissleCoeffiicent;
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
        private bool setNewPosition = false;
        private MagicMissleOrb missile;
        private float arrivalTime;
        private bool appliedDOT = false;
        private HurtBox previousRandomHurtbox;
        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.SetAimTimer(3f);
            magicSearch = new BullseyeSearch();
            magicSearch.minAngleFilter = 0f;
            magicSearch.maxAngleFilter = 90f;
            magicSearch.maxDistanceFilter = float.MaxValue;
            magicSearch.minDistanceFilter = 5f;
            magicSearch.viewer = this.characterBody;
            magicSearch.searchOrigin = this.characterBody.transform.position;
            magicSearch.sortMode = BullseyeSearch.SortMode.Angle;
            magicSearch.teamMaskFilter = TeamMask.all;
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
 
            originalpos = !setNewPosition ? new Vector3(
    characterMotor.transform.position.x,
    characterMotor.transform.position.y + 20,
    characterMotor.transform.position.z) : originalpos;
            setNewPosition = true;

            DisableMovement();
            base.FixedUpdate();
            float tick = fixedAge % 0.2f;

            if (tick > 0.1f && !appliedDOT && fixedAge > 1f)
            {
                appliedDOT = true;
                var overcharge = new InflictDotInfo
                {
                    attackerObject = this.gameObject,
                    victimObject = this.gameObject,
                    totalDamage = this.healthComponent.fullCombinedHealth * 0.05f,
                    damageMultiplier = 2,
                    dotIndex = CrystalBuffs.PrimaryOverCharge,
                    maxStacksFromAttacker = 1u,
                    duration = 0.4f
                };
                DotController.InflictDot(ref overcharge);
            }
            else if (tick < 0.1f)
            {
                appliedDOT = false;
            }
            if (isAuthority && inputBank.skill1.down)
            {
                DisableMovement();
            }

            if (inputBank.skill3.down && isAuthority && tick > 0.1f && !hasFired)
            {;
                magicSearch.RefreshCandidates();
                List<HurtBox> list = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
                foreach (HurtBox result in magicSearch.GetResults())
                {
                    list.Add(result);
                }
                Util.ShuffleList(list);
                var randomHurtbox = list.FirstOrDefault();
                CollectionPool<HurtBox, List<HurtBox>>.ReturnCollection(list);
                hasFired = true;

                arrivalTime = Vector3.Distance(this.transform.position, randomHurtbox.transform.position) / 300;
                missile = new MagicMissleOrb
                {
                    arrivalTime = arrivalTime,
                    origin = this.transform.position,
                    target = randomHurtbox,
                    damageCoefficient = damageCoefficient,
                    damageStat = damageStat,
                    teamIndex = this.teamComponent.teamIndex,
                    gameObject = this.gameObject,
                    targetPos = randomHurtbox.transform.position,
                    nextOrb = missile
                };
                OrbManager.instance.AddOrb(missile);
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
        }
        
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
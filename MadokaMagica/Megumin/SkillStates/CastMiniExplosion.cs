using EntityStates;
using HG;
using MadokaMagica.MamiTamoe.Content;
using MadokaMagica.Megumin.SkillStates.BaseStates;
using Newtonsoft.Json.Utilities;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using R2API;
using MadokaMagica.Megumin.Content;
namespace MadokaMagica.Megumin.SkillStates
{
    public class CastMiniExplosion : BaseMeguminSkillState
    {
        public static float damageCoefficient = MamiStaticValues.gunDamageCoefficient;
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
            DOTDamage = appliedDOT ? DOTDamage : DOTDamage + this.healthComponent.fullCombinedHealth * 0.1f;
            base.FixedUpdate();
            charge = Mathf.Min(base.fixedAge, 1f);
            damage = (damageCoefficient * damageStat * charge) + (DOTDamage);
            previousHealth = this.healthComponent.combinedHealth;
            tick = fixedAge % 0.4f;
            Log.Debug($"Damage from DOT {DOTDamage}");
            if (tick > 0.1f && !appliedDOT && fixedAge > 1f)
            {
                appliedDOT = true;
                var overcharge = new InflictDotInfo
                {
                    attackerObject = this.gameObject,
                    victimObject = this.gameObject,
                    totalDamage = this.healthComponent.fullCombinedHealth * 0.1f,
                    damageMultiplier = 2,
                    dotIndex = MeguminBuffs.PrimaryOverCharge,
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
            else if (inputBank.skill1.justReleased)
            {
                Fire();
                outer.SetNextStateToMain();
                return;
            }
            //PrecisionStrike.cs Disable Movement
        }
        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                var aimRay = GetAimRay();
                var bulletAttack = new BulletAttack();
                bulletAttack.bulletCount = 1;
                bulletAttack.aimVector = aimRay.direction;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.damage = 0f;
                bulletAttack.damageColorIndex = DamageColorIndex.Default;
                bulletAttack.damageType = DamageTypeCombo.GenericSecondary;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                bulletAttack.maxDistance = range;
                bulletAttack.force = force;
                bulletAttack.hitMask = LayerIndex.CommonMasks.bullet;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = 0f;
                bulletAttack.isCrit = RollCrit();
                bulletAttack.owner = gameObject;
                bulletAttack.muzzleName = muzzleString;
                bulletAttack.smartCollision = true;
                bulletAttack.procChainMask = default;
                bulletAttack.procCoefficient = procCoefficient;
                bulletAttack.radius = 5f;
                bulletAttack.sniper = false;
                bulletAttack.stopperMask = LayerIndex.CommonMasks.bullet;
                bulletAttack.weapon = null;
                bulletAttack.tracerEffectPrefab = tracerEffectPrefab;
                bulletAttack.spreadPitchScale = 1f;
                bulletAttack.spreadYawScale = 1f;
                bulletAttack.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                bulletAttack.hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab;


                bulletAttack.modifyOutgoingDamageCallback += delegate (BulletAttack bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo)
                {
                    bulletAttack.damage = 0f;
                    new BlastAttack
                    {
                        attacker = base.gameObject,
                        inflictor = base.gameObject,
                        teamIndex = base.teamComponent.teamIndex,
                        baseDamage = damage,
                        damageType = DamageType.Generic,
                        baseForce = 0.2f,
                        position = hitInfo.point,
                        radius = 10f,
                        falloffModel = BlastAttack.FalloffModel.None,
                        bonusForce = hitInfo.direction
                    }.Fire();

                    EffectData effectData = new EffectData
                    {
                        origin = hitInfo.point,
                        start = bulletAttack.origin
                    };
                };
                bulletAttack.Fire();
            }
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
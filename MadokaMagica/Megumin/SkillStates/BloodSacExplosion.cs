using RoR2;
using UnityEngine;
using EntityStates;
using MadokaMagica.Megumin.Content;
using MadokaMagica.Megumin.SkillStates.BaseStates;
using EntityStates.BrotherMonster;
using BepInEx.Configuration;
using MadokaMagica.MamiTamoe.Content;
using System.Linq;
using System;

namespace MadokaMagica.Megumin.SkillStates
{
    public class BloodSacExplosion : BaseMeguminSkillState
    {
        public static float damageCoefficient = MeguminStaticValues.BloodSacExplosionCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
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
        BlastAttack blastAttack = null;
        public DamageSource DamageSource => DamageSource.Secondary;

        private bool dashed;

        private bool restocking;

        public DamageSource damageSource => DamageSource.Secondary;
        
        private bool secondaryActive()
        {
            if (inputBank.skill2.down || inputBank.skill2.justPressed)
            {
                return true;
            }
            return false;
        }

        private void InitEnterVars()
        {
            characterBody.SetAimTimer(2f);
        }
        
        //CeaselessBarrage.cs Code Start
        
            //CeaselessBarage.cs OnEnter()
        public override void OnEnter()
        {
            base.OnEnter();
            InitEnterVars();
        }
            //CeaselessBarrage.cs FixedUpdate()
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            DisableMovement();

            base.FixedUpdate();
            charge = Mathf.Min(base.fixedAge, 3f);
            DOTDamage = appliedDOT ? DOTDamage : DOTDamage + this.healthComponent.fullCombinedHealth * 0.5f;
            damage = DOTDamage * 2;
            tick = fixedAge % 0.1f;
            Log.Debug($"Secondary Damage from DOT {DOTDamage * 2}");
            if (tick > 0.05f && !appliedDOT)
            {
                appliedDOT = true;
                var overcharge = new InflictDotInfo
                {
                    attackerObject = this.gameObject,
                    victimObject = this.gameObject,
                    totalDamage = this.healthComponent.fullCombinedHealth * 0.5f,
                    damageMultiplier = 4,
                    dotIndex = CrystalBuffs.PrimaryOverCharge,
                    maxStacksFromAttacker = 1u,
                    duration = 0.1f
                };
                DotController.InflictDot(ref overcharge);
            }
            else if (tick < 0.05f)
            {
                appliedDOT = false;
            }
            
            
            if (isAuthority && inputBank.skill1.down && !inputBank.skill1.justReleased)
            {
                DisableMovement();
            }
            else if (inputBank.skill2.justReleased || fixedAge > baseDuration)
            {
                Fire();
                outer.SetNextStateToMain();
                return;
            }
        }
        //CeaselessBarrage.cs OnExit()
        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            characterBody.AddSpreadBloom(1.5f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
            Util.PlaySound("HenryShootPistol", gameObject);
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
                    blastAttack = new BlastAttack
                    {
                        attacker = base.gameObject,
                        inflictor = base.gameObject,
                        teamIndex = base.teamComponent.teamIndex,
                        baseDamage = damage,
                        damageType = DamageType.Generic,
                        baseForce = 0.2f,
                        position = hitInfo.point,
                        radius = charge * 10,
                        falloffModel = BlastAttack.FalloffModel.None,
                        bonusForce = hitInfo.direction
                    };
                    blastAttack.Fire();

                    EffectData effectData = new EffectData
                    {
                        origin = hitInfo.point,
                        start = bulletAttack.origin
                    };
                };
                bulletAttack.Fire();
                var blastAttackHits = blastAttack.CollectHits();

                this.healthComponent.Heal(Mathf.RoundToInt(this.healthComponent.fullCombinedHealth) * 0.3f * ((blastAttackHits.Count())), default, false);
            }

        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
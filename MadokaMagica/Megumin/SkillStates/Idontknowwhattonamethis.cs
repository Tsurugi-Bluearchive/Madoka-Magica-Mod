using RoR2;
using UnityEngine;
using EntityStates;
using MadokaMagica.Megumin.Content;
using MadokaMagica.Megumin.SkillStates.BaseStates;
using EntityStates.BrotherMonster;
using BepInEx.Configuration;
using MadokaMagica.MamiTamoe.Content;

namespace MadokaMagica.Megumin.SkillStates
{
    public class Idontknowwhattonamethis : BaseMeguminSkillState
    {
        public static float damageCoefficient = MeguminStaticValues.barrageDamageCefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1.3f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;
        public static GameObject muzzleEffect;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration => baseDuration / attackSpeedStat;
        private int primaryStock
        {
            get => skillLocator.primary.stock;
            set => skillLocator.primary.stock = value;
        }
        private int secondaryStock
        {
            get => skillLocator.secondary.stock;
            set => skillLocator.secondary.stock = value;
        }
        private bool eatingSecondary;
        private float blastDuration => duration / (skillLocator.primary.stock + skillLocator.secondary.stock);
        private string muzzleString => "Muzzle";
        private bool shotBarrage = false;
        private Vector3 originalPos => characterBody.corePosition;

        private float tick;

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
        private void Firing() { Fire(); tick = 0; }
        private void DisableMovement() { characterMotor.Motor.SetPosition(originalPos); characterMotor.velocity = Vector3.zero; }
        
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

            tick += Time.fixedDeltaTime;

            //CeaselessBarrage.cs Firing Logic
            if (isAuthority && tick >= blastDuration && secondaryActive() && !shotBarrage)
            {
                eatingSecondary = secondaryStock > 0 ? true : false;
                primaryStock = secondaryStock > 0 && !eatingSecondary ? primaryStock-- : primaryStock;
                secondaryStock = secondaryStock > 0 && eatingSecondary? secondaryStock-- : secondaryStock;
                shotBarrage = secondaryStock > 0 && primaryStock > 0 ? false : true;
            }

            //CeaselessBarrage.cs Release Logic
            else if(secondaryActive())
            {
                outer.SetNextStateToMain();
                return;
            }
        }
        //CeaselessBarrage.cs OnExit()
        public override void OnExit()
        {
            base.OnExit();
            var previousStock = secondaryStock;
            if (primaryStock < 1)
            {
                skillLocator.secondary.SetSkillOverride(this.gameObject, MeguminSurvivor.reload, GenericSkill.SkillOverridePriority.Default);
                secondaryStock = previousStock;
            }
        }

        private void Fire()
        {
            characterBody.AddSpreadBloom(1.5f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
            Util.PlaySound("HenryShootPistol", gameObject);
            if (isAuthority)
            {
                var aimRay = GetAimRay();
                AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);
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


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
using RoR2;
using UnityEngine;
using EntityStates;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class CeaselessBarrage : BaseSkillState
    {
        public static float damageCoefficient = MamiStaticValues.barrageDamageCefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1.3f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;
        public static GameObject muzzleEffect;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float blastDuration;
        private string muzzleString;
        private int stocks;
        private bool shotBarrage = false;
        private Vector3 originalPos;

        private float tick;

        private bool restocking;

        public DamageSource damageSource;

        private void InitEnterVars()
        {
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(2f);
            stocks = skillLocator.primary.stock + skillLocator.secondary.stock;
            blastDuration = duration / stocks;
            muzzleString = "Muzzle";
            originalPos = characterBody.corePosition;
            this.damageSource = DamageSource.Secondary;
        }
        private void Firing() { skillLocator.secondary.stock--; Fire(); tick = 0; }
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
            if (stocks > 0 && isAuthority && tick >= blastDuration && (inputBank.skill2.down || inputBank.skill2.justPressed))
            {
                stocks--;
                if (skillLocator.secondary.stock > 0) { Firing(); }
                else if (skillLocator.primary.stock > 0)  { Firing(); }
                else { shotBarrage = true; }
            }

            //CeaselessBarrage.cs Release Logic
            else if(shotBarrage || inputBank.skill2.justReleased)
            {
                outer.SetNextStateToMain();
                return;
            }
        }
        //CeaselessBarrage.cs OnExit()
        public override void OnExit()
        {
            base.OnExit();
            var previousStock = skillLocator.secondary.stock;
            if (skillLocator.primary.stock < 1)
            {
                skillLocator.secondary.SetSkillOverride(this.gameObject, MamiSurvivor.reload, GenericSkill.SkillOverridePriority.Default);
                skillLocator.secondary.stock = previousStock;
            }
        }

        private void Fire()
        {
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
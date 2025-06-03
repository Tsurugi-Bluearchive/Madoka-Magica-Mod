using RoR2;
using UnityEngine;
using EntityStates;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class CeaselessBarrage : BaseSkillState
    {
        public static float damageCoefficient = MamiStaticValues.barrageDamageCefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
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

        private float tick;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(2f);
            stocks = skillLocator.primary.stock + skillLocator.secondary.stock;
            blastDuration = duration/stocks;
            muzzleString = "Muzzle";
            base.characterBody.armor += 800;
            base.characterMotor.enabled = false;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            tick += Time.fixedDeltaTime;
            if (stocks > 0 && isAuthority && tick >= blastDuration && (inputBank.skill2.down || inputBank.skill2.justPressed))
            {
                stocks--;
                if (skillLocator.secondary.stock > 0)
                {
                    skillLocator.secondary.stock--;
                    Fire();
                    tick = 0;
                }
                else if (skillLocator.primary.stock > 0)
                {
                    skillLocator.primary.stock--;
                    Fire();
                    tick = 0;
                }
                else
                {
                    shotBarrage = true;
                }
            }
            else if(shotBarrage || inputBank.skill2.justReleased)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            var previousStock = skillLocator.secondary.stock;
            base.characterMotor.enabled = true;
            base.characterMotor.velocity = Vector3.zero;
            base.characterBody.armor -= 800;
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
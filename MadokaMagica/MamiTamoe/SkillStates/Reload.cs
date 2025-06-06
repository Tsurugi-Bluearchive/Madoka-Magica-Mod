using RoR2;
using EntityStates;
using MadokaMagica.MamiTamoe.Content;
using MadokaMagica.MamiTamoe.SkillStates.BaseStates;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class Reload : BaseMamiSkillState
    {
        public static float damageCoefficient = MamiStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1.2f;
        public static float baseDuration = 0.2f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0f;
        public static float force = 5000f;
        public static float recoil = 10f;
        public static float range = 256f;
        public static EntityState ReloadState;

        private float duration;

        private bool restocking;

        private int secondaryStock;
        private int secondaryMax;
        private int primaryStock;
        private int primaryMax;

        public DamageSource damageSource;
        private void InitOnEnterVars() 
        { 
            damageSource = DamageSource.Secondary; 
            duration = baseDuration / attackSpeedStat; 
        }
        private void FetchFixedVars()
        {
            secondaryStock = skillLocator.secondary.stock;
            secondaryMax = skillLocator.secondary.maxStock;
            primaryStock = skillLocator.primary.stock;
            primaryMax = skillLocator.primary.maxStock;
        }
        //Reload.cs Code Start
        
        //Reload.cs OnEnter()
        public override void OnEnter()
        {
            InitOnEnterVars();
            base.OnEnter();
            characterBody.SetAimTimer(2f);
        }

        //Reload.cs OnExit()
        public override void OnExit()
        {
            base.OnExit();
            var previousStock = skillLocator.secondary.stock;
            skillLocator.secondary.UnsetSkillOverride(this.gameObject, MamiSurvivor.reload, GenericSkill.SkillOverridePriority.Default);
            skillLocator.secondary.stock = previousStock - skillLocator.primary.stock;
        }

        //Reload.cs FixedUpdate()
        public override void FixedUpdate()
        {
            FetchFixedVars();

            //Reload.cs Reload Logic
            if (fixedAge > duration && !restocking || inputBank.skill2.justPressed)
            {
                restocking = true;
                if (secondaryStock >= skillLocator.primary.maxStock)
                {
                    skillLocator.primary.stock = primaryMax;
                    outer.SetNextStateToMain();
                    return;
                }
                else
                {
                    skillLocator.primary.stock = secondaryStock;
                    outer.SetNextStateToMain();
                    return;
                }
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}

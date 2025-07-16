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

        private float duration => baseDuration / attackSpeedStat;

        private bool restocked;

        private int secondaryStock
        {
            get => skillLocator.secondary.stock;
            set => skillLocator.secondary.stock = value;
        }
         private int secondaryMax => skillLocator.secondary.maxStock;
         private int primaryStock
        {
            get => skillLocator.primary.stock;
            set => skillLocator.primary.stock = value;
        }
         private int primaryMax => skillLocator.primary.maxStock;

        public DamageSource damageSource => DamageSource.Secondary;
        //Reload.cs Code Start
        
        //Reload.cs OnEnter()
        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.SetAimTimer(2f);
        }

        //Reload.cs OnExit()
        public override void OnExit()
        {
            base.OnExit();
            skillLocator.secondary.UnsetSkillOverride(this.gameObject, MamiSurvivor.reload, GenericSkill.SkillOverridePriority.Default);
        }

        //Reload.cs FixedUpdate()
        public override void FixedUpdate()
        {

            //Reload.cs Reload Logic
            primaryStock = primaryStock <= secondaryStock && !restocked ? primaryMax : secondaryStock;
            secondaryStock = primaryStock <= secondaryStock && !restocked ? secondaryStock - primaryMax : 0;
            restocked = true;
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

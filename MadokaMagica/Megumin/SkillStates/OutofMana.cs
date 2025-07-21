using RoR2;
using EntityStates;
using MadokaMagica.Megumin.Content;
using MadokaMagica.Megumin.SkillStates.BaseStates;

namespace MadokaMagica.Megumin.SkillStates
{
    public class OutofMana : BaseMeguminSkillState
    {

        public DamageSource damageSource => DamageSource.Secondary;
        //Reload.cs Code Start
        
        //Reload.cs OnEnter()
        public override void OnEnter()
        {
            base.OnEnter();
        }

        //Reload.cs OnExit()
        public override void OnExit()
        {
            base.OnExit();
        }

        //Reload.cs FixedUpdate()
        public override void FixedUpdate()
        {
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}

using EntityStates;
using UnityEngine;
namespace MadokaMagica.Megumin.SkillStates.BaseStates
{
    public class BaseMeguminSkillState : BaseSkillState
    {
        private Vector3 originalpos;

        public override void OnEnter()
        {
            originalpos = characterBody.corePosition;
            base.OnEnter();
        }
        public void DisableMovement()
        {
            if (isAuthority)
            {
                characterMotor.Motor.SetPosition(originalpos);
                characterMotor.velocity = Vector3.zero;
            }
        }
    }
}

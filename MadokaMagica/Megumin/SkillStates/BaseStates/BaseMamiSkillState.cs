using EntityStates;
using UnityEngine;
namespace MadokaMagica.Megumin.SkillStates.BaseStates
{
    public class BaseMeguminSkillState : BaseSkillState
    {
        private Vector3 originalpos => characterBody.corePosition;
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

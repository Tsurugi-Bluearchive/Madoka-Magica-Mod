using UnityEngine;
using EntityStates;
using RoR2;

namespace MadokaMagica.Zilu.SkillStates.BaseStates
{
    public class JetCharacterMain : GenericCharacterMain
    {
        private int SecondaryStock => skillLocator.secondary.stock;
        private int SecondaryMax => skillLocator.secondary.maxStock;
        private float SprintMult => characterBody.sprintingSpeedMultiplier;
        private bool Grounded => characterMotor.isGrounded;
        private int UtilityStock => skillLocator.utility.stock;
        private int UtilityMax => skillLocator.utility.maxStock;
        private float AttackSpeed => characterBody.attackSpeed;
        private int JumpCount => characterBody.characterMotor.jumpCount;
        private Vector3 CharacterVelocity => characterBody.characterMotor.velocity;

        public EntityStateMachine Scarf;
        public EntityState PrecisionStrike;

        private int dashCount;

        //MamiCharacterMain.cs Code Start        
        public override void FixedUpdate()
        {
            //Init
            base.FixedUpdate();
            if (inputBank.jump.justPressed && dashCount > 0)
            {
                characterBody.characterMotor.velocity = new Vector3(CharacterVelocity.x * 5, -5, CharacterVelocity.z * 5);
            }

        }
    }
}

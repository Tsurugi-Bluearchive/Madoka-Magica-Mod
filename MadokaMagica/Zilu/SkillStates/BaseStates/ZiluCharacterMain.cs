using UnityEngine;
using EntityStates;
using RoR2;

namespace MadokaMagica.Zilu.SkillStates.BaseStates
{
    public class ZiluCharacterMain : GenericCharacterMain
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

        private float tick;
        private float tick2;
        private bool justJumped;

        //MamiCharacterMain.cs Code Start        
        public override void FixedUpdate()
        {
            //Init
            base.FixedUpdate();


        }
    }
}

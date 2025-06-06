using UnityEngine;
using EntityStates;
using RoR2;
using MadokaMagica.MamiTamoe.Components;

namespace MadokaMagica.MamiTamoe.SkillStates.BaseStates
{
    public class MamiCharacterMain : GenericCharacterMain
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
        public MamiGunPassive Mami;
        public EntityState PrecisionStrike;

        private float tick;
        private bool justJumped;
        
        private void FetchFixedVars()
        {
            Mami ??= this.GetComponent<MamiGunPassive>();

            characterBody.characterMotor.airControl = 1.5f;
        }

        private Vector3 ClampedDashVelocity(float x, float y, float z) => 
            new Vector3(
                Mathf.Clamp(CharacterVelocity.x * x, 70, 70),
                Mathf.Clamp(CharacterVelocity.y * y, -40, 40),
                Mathf.Clamp(CharacterVelocity.z * z, 70, 70));

        private void FetchTimers() => tick += Time.fixedDeltaTime;

        //MamiCharacterMain.cs Code Start        
        public override void FixedUpdate()
        {
            //Init
            base.FixedUpdate();
            FetchFixedVars();
            FetchTimers();

            //MamiCharacterMain.cs Collection
            if (Mami.mmmgun != null && SecondaryMax > SecondaryStock && isAuthority)
            {
                Destroy(Mami.mmmgun.gameObject);
                skillLocator.secondary.AddOneStock();
            }

            //MAmiCharacterMain.cs Utility restock logic
            if (UtilityStock < UtilityMax && tick > 12f * AttackSpeed)
            {
                skillLocator.utility.stock++;
                tick = 0;
            }

            //MamiCharacterMain.cs Aerial Dash Controller
            if (inputBank.jump.justReleased)
                justJumped = false;

            if (inputBank.jump.justPressed && !isGrounded && JumpCount > 0 && !justJumped)
            {
                justJumped = true;
                characterBody.characterMotor.velocity = ClampedDashVelocity(17, 1, 17);
            }
        }
    }
}

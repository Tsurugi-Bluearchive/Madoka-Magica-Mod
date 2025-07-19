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
        private float tick2;
        private bool justJumped;

        private bool Dashable()
        {
            if (inputBank.jump.justPressed && !isGrounded && JumpCount > 0 && !justJumped)
            {
                return true;
            }
            return false;
        }

        private bool SecondaryIsReloadable()
        {
            if (Mami.mmmgun != null && SecondaryMax > SecondaryStock && isAuthority)
            {
                Destroy(Mami.mmmgun.gameObject);
                return true;
            }
            return false;
        }
        private void FetchFixedVars()
        {
            Mami ??= this.GetComponent<MamiGunPassive>();
        }

        private void FetchTimers()
        {
            tick += Time.fixedDeltaTime;
            tick2 += Time.fixedDeltaTime;
        }

        //MamiCharacterMain.cs Code Start        
        public override void FixedUpdate()
        {
            //Init
            base.FixedUpdate();
            FetchFixedVars();
            FetchTimers();

            //MamiCharacterMain.cs Collection
            skillLocator.secondary.stock = SecondaryIsReloadable() ? skillLocator.secondary.stock++ : skillLocator.secondary.stock;

            //MamiCharacterMain.cs Aerial Dash Controller
            tick2 = inputBank.jump.justReleased && tick2 > 0.5f && justJumped && JumpCount > 0 ? 0 : tick2;
            characterBody.characterMotor.velocity = !Dashable() ? CharacterVelocity : new Vector3(CharacterVelocity.x * 3, CharacterVelocity.y, CharacterVelocity.z * 3);
            justJumped = !Dashable() ? false : true;
        }
    }
}

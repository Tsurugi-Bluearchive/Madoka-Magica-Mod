using UnityEngine;
using EntityStates;
using RoR2;
using MadokaMagica.MamiTamoe.SkillStates;
using UnityEngine.Networking;

namespace MadokaMagica.MamiTamoe.BaseStates
{
    public class MamiCharacterMain : GenericCharacterMain
    {
        public EntityStateMachine Scarf;
        public MamiGunPassive Mami;
        public EntityState PrecisionStrike;
        private float precisionTick;
        private bool setAirControl = false;
        private EntityState reloadState;
        private float tick;
        private bool justJumped;
        private int secondaryStock;
        private int secondaryMax;
        private float sprintMult;
        private float grounded;
        private int utilityStock;
        private int utilityMax;
        private float attackSpeed;
        private int jumpCount;
        private Vector3 characterVelocity;
        private void FetchFixedVars()
        {
            secondaryStock = skillLocator.secondary.stock;
            secondaryMax = skillLocator.secondary.maxStock;
            utilityStock = skillLocator.utility.stock;
            utilityMax = skillLocator.utility.maxStock;
            attackSpeed = characterBody.attackSpeed;

            sprintMult = characterBody.sprintingSpeedMultiplier;
            jumpCount = characterBody.characterMotor.jumpCount;

            characterVelocity = characterBody.characterMotor.velocity;

            Mami = this.gameObject.GetComponent<MamiGunPassive>();
        }

        private void FetchTimers() => tick += Time.fixedDeltaTime;

        //MamiCharacterMain.cs Code Start        
        public override void FixedUpdate()
        {
            //Init
            base.FixedUpdate();
            FetchFixedVars();
            FetchTimers();
            
            //MamiCharacterMain.cs Collection
            if (Mami.mmmgun != null && secondaryMax > secondaryStock && isAuthority)
            {
                Destroy(Mami.mmmgun.gameObject);
                skillLocator.secondary.AddOneStock();
            }

            //MamiCharacterMain.cs Aerial Speed Controller
            if (!isGrounded && !setAirControl)
            {
                    characterBody.sprintingSpeedMultiplier = sprintMult * 1.5f;
                    setAirControl = true;
            }
            else if (setAirControl && isGrounded)
            {
                characterBody.sprintingSpeedMultiplier = sprintMult / 1.5f;
                setAirControl = false;
            }

            //MAmiCharacterMain.cs Utility restock logic
            if (utilityStock < utilityMax && tick > 6f * attackSpeed)
            {
                skillLocator.utility.stock++;
                tick = 0;
            }

            //MamiCharacterMain.cs Aerial Dash Controller
            if (inputBank.jump.justReleased)
            {
                justJumped = false;
            }
            if (inputBank.jump.justPressed && !isGrounded && jumpCount > 0 && !justJumped)
            {
                justJumped = true;
                characterBody.characterMotor.velocity = new Vector3(characterVelocity.x * 3, characterVelocity.y, characterVelocity.z * 3);
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);

        }
    }
}

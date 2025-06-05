using UnityEngine;
using EntityStates;
using RoR2;
using UnityEngine.Networking;

namespace MadokaMagica.MamiTamoe.SkillStates.BaseStates
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

            characterBody.characterMotor.airControl = 1.5f;
        }
        private Vector3 ClampedDashVelocity(float x, float y, float z)
        {
            return this.characterVelocity = new Vector3(
                Mathf.Clamp(characterVelocity.x * x, 0, 10),
                Mathf.Clamp(characterVelocity.y * y, 0, 10),
                Mathf.Clamp(characterVelocity.z * z, 0, 10));
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

            //MAmiCharacterMain.cs Utility restock logic
            if (utilityStock < utilityMax && tick > 12f * attackSpeed)
            {
                skillLocator.utility.stock++;
                tick = 0;
            }

            //MamiCharacterMain.cs Aerial Dash Controller
            if (inputBank.jump.justReleased)
                justJumped = false;
            if (inputBank.jump.justPressed && !isGrounded && jumpCount > 0 && !justJumped)
            {
                justJumped = true;
                characterBody.characterMotor.velocity = ClampedDashVelocity(3, 1, 3);
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);

        }
    }
}

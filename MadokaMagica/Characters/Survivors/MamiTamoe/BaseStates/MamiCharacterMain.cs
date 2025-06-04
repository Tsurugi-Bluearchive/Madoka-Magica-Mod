using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MadokaMagica.MamiTamoe.Pickupables;
using EntityStates;
using RoR2;
using MadokaMagica.MamiTamoe;
using MadokaMagica.Modules;
using MadokaMagica.Modules.Characters;
using IL.RoR2.Skills;
using EntityStates.TitanMonster;
using MadokaMagica.MamiTamoe.SkillStates;
using Rewired.Utils;
using MonoMod.RuntimeDetour;
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
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Mami = this.gameObject.GetComponent<MamiGunPassive>();
            tick += Time.fixedDeltaTime;
            Scarf = EntityStateMachine.FindByCustomName(this.gameObject, "Weapon");
            if (Mami.mmmgun != null && skillLocator.secondary.maxStock > skillLocator.secondary.stock && isAuthority)
            {
                Destroy(Mami.mmmgun.gameObject);
                skillLocator.secondary.AddOneStock();
            }

            if (!characterBody.characterMotor.isGrounded && !setAirControl)
            {
                    characterBody.sprintingSpeedMultiplier = characterBody.sprintingSpeedMultiplier * 1.5f;
                    setAirControl = true;
            }
            else if (setAirControl && characterBody.characterMotor.isGrounded)
            {
                characterBody.sprintingSpeedMultiplier = characterBody.sprintingSpeedMultiplier / 1.5f;
                setAirControl = false;
            }
            if (skillLocator.utility.stock < skillLocator.utility.maxStock && tick > 2f * characterBody.attackSpeed)
            {
                skillLocator.utility.AddOneStock();
                tick = 0;
            }
            if (inputBank.jump.justReleased)
            {
                justJumped = false;
            }
            if (inputBank.jump.justPressed && !characterBody.characterMotor.isGrounded && characterBody.characterMotor.jumpCount > 0 && !justJumped)
            {
                justJumped = true;
                characterBody.characterMotor.velocity = new Vector3(characterBody.characterMotor.velocity.x * 3, characterBody.characterMotor.velocity.y, characterBody.characterMotor.velocity.z * 3);
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);

        }
    }
}

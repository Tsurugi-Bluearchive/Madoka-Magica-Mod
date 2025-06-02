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

namespace MadokaMagica.MamiTamoe.BaseStates
{
    public class MamiCharacterMain : GenericCharacterMain
    {
        public EntityStateMachine Scarf;
        public MamiGunPassive Mami;
        public EntityState PrecisionStrike;
        private float precisionTick;
        private bool setAirControl = false;
        public override void FixedUpdate()
        {;
            Mami = this.gameObject.GetComponent<MamiGunPassive>();
            Scarf = EntityStateMachine.FindByCustomName(this.gameObject, "Weapon");
            if (Mami.mmmgun != null && skillLocator.secondary.maxStock > skillLocator.secondary.stock && isAuthority)
            {
                Destroy(Mami.mmmgun.gameObject);
                skillLocator.secondary.AddOneStock();
            }

            base.FixedUpdate();

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
            if (setAirControl && inputBank.jump.justPressed && isAuthority && characterBody.maxJumpCount > characterBody.characterMotor.jumpCount)
            {
                characterBody.characterMotor.velocity += new Vector3(characterBody.characterMotor.velocity.x * 2f, 2f, characterBody.characterMotor.velocity.z * 2f);
            }
        }
    }
}

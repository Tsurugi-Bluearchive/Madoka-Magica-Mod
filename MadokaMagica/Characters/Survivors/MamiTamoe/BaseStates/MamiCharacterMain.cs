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
        public EntityState Scarf;
        public MamiGunPassive Mami;
        public EntityState PrecisionStrike;
        private float precisionTick;
        private bool setAirControl = false;
        public override void FixedUpdate()
        {
            var muzzleEffect = MamiAssets.MamiGunEffect;
            Scarf = EntityStateMachine.FindByCustomName(this.gameObject, "Weapon2").state;
            PrecisionStrike = EntityStateMachine.FindByCustomName(this.gameObject, "Weapon").state;
            Mami = this.gameObject.GetComponent<MamiGunPassive>();
            base.FixedUpdate();
            if (Scarf != null && Mami.mmmgun != null && Scarf.fixedAge <= 0.1f && skillLocator.secondary.maxStock > skillLocator.secondary.stock && Scarf.isAuthority && isAuthority)
            {
                Destroy(Mami.mmmgun.gameObject);
                skillLocator.primary.AddOneStock();
            }
            if (!characterBody.characterMotor.isGrounded && !setAirControl && isAuthority)
            {
                    characterBody.sprintingSpeedMultiplier = characterBody.sprintingSpeedMultiplier * 2;
                    setAirControl = true;
            }
            else if (setAirControl && characterBody.characterMotor.isGrounded && isAuthority)
            {
                characterBody.sprintingSpeedMultiplier = characterBody.sprintingSpeedMultiplier / 2;
                setAirControl = false;
            }
            if (setAirControl && inputBank.jump.justPressed && isAuthority && characterBody.maxJumpCount > characterBody.characterMotor.jumpCount)
            {
                characterBody.characterMotor.velocity += new Vector3(characterBody.characterMotor.velocity.x * 1.3f, 2f, characterBody.characterMotor.velocity.z * 1.3f);
            }
        }
    }
}

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

namespace MadokaMagica.MamiTamoe.BaseStates
{
    public class MasterMamiSkillStates : GenericCharacterMain
    {
        public EntityState Scarf;
        public MamiGunPassive Mami;
        public EntityState PrecisionStrike;
        private bool something = false;
        public override void FixedUpdate()
        {
            
            Scarf = EntityStateMachine.FindByCustomName(this.gameObject, "Weapon").state;
            PrecisionStrike = EntityStateMachine.FindByCustomName(this.gameObject, "Weapon2").state;
            Mami = this.gameObject.GetComponent<MamiGunPassive>();
            base.FixedUpdate();
            if (Scarf != null && Mami.mmmgun != null && Scarf.age <= 0.1f && skillLocator.secondary.maxStock > skillLocator.secondary.stock)
            {
                Destroy(Mami.mmmgun.gameObject);
                skillLocator.secondary.AddOneStock();
            }
        }
    }
}

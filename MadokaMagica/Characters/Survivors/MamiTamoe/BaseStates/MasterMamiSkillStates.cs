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

namespace MadokaMagica.MamiTamoe.BaseStates
{
    public class MasterMamiSkillStates : GenericCharacterMain
    {
        public long gunCount;
        public long gunMax;
        public long gunsHeld;
        public bool pickupGun;

        public void PickupGun(MamiGun collectedGun)
        {
            var collectedGunObject = collectedGun.Pickup;
            if (pickupGun && gunMax <= gunCount)
            {
                gunCount++;
                skillLocator.secondary.stock += 1;
            }

        }
    }
}

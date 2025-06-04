using MadokaMagica;
using MadokaMagica.Modules;
using MadokaMagica.Characters.UniversalBases;
using MadokaMagica.MamiTamoe.BaseStates;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using MadokaMagica.MamiTamoe.Pickupables;
using Rewired.Utils;
using UnityEngine.UIElements;

namespace MadokaMagica.MamiTamoe.Pickupables
{
    internal class MamiGunWorldCollider : MonoBehaviour
    {
        public MamiGun MamiGun;
        //MamiGunWorldCollider.cs Code Start
        public void OnTriggerStay(Collider collision)
        {
            MamiGun.impactedworld = true;
            Destroy(MamiGun.thisBody);
            if (MamiGun == null)
            {
                Debug.Log("Erorr with mamigun!");
            }
        }
    }
}

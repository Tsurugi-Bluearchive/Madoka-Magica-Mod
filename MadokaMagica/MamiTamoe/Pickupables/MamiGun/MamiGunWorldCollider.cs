using MadokaMagica.Modules;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Rewired.Utils;
using UnityEngine.UIElements;

namespace MadokaMagica.MamiTamoe.Pickupables.MamiGun
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
                Debug.Log("Erorr with mamigun!");
        }
    }
}

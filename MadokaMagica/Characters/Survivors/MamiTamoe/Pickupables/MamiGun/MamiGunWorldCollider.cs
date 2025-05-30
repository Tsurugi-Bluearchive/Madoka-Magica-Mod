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
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerIndex.world.intVal)
            {
                MamiGun.impactedworld = true;
                Destroy(this.gameObject);
            }
            if (MamiGun == null)
            {
                Debug.Log("Erorr with mamigun!");
            }
        }
    }
}

using HenryMod;
using HenryMod.Modules;
using MadokaMagica.Characters.UniversalBases;
using MadokaMagica.MamiTamoe.BaseStates;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using MadokaMagica.MamiTamoe.Pickupables;

namespace MadokaMagica.MamiTamoe.Pickupables
{
    internal class MamiGunWorldCollider : MonoBehaviour
    {
        [SerializeField]
        public MamiGun MamiGun;
        public void OnTriggerStay(Collider collision)
        {
            if (collision.gameObject.layer == LayerIndex.world.intVal)
            {
                Log.Debug("Madokagun has third impacted!");
                MamiGun.impactedworld = true;
            }
        }
    }
}

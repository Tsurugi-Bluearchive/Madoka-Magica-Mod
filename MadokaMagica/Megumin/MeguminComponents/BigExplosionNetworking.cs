using IL.RoR2;
using MadokaMagica.Megumin.SkillStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace MadokaMagica.Megumin.MeguminComponents
{
    public class BigExplosionNetworking : NetworkBehaviour
    {
        [SyncVar]
        public GameObject masterCaster;
        [SyncVar]
        public float damage;


    }
}

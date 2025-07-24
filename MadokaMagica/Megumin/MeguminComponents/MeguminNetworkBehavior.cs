using MadokaMagica.Megumin.SkillStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace MadokaMagica.Megumin.MeguminComponents
{
    internal class MeguminNetworkBehavior : NetworkBehaviour
    {
        [SyncVar]
        public BigExplosionNetworking bigExplosionNW;
        [SyncVar]
        public GameObject bigExplosion;
        [SyncVar]
        public GameObject masterCaster;
        [SyncVar]
        public CastBigExplosion masterBigExplosionSkill;
        [SyncVar]
        public float damage;
    }
}

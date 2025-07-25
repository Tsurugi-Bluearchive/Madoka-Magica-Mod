using RoR2;
using MadokaMagica.Megumin.Content;
using MadokaMagica.Megumin.SkillStates;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace MadokaMagica.Megumin.MeguminComponents
{
    internal class MeguminNetworkBehavior : NetworkBehaviour
    {
        [SyncVar]
        public GameObject bigExplosion;

        [SyncVar(hook = nameof(CmdSpawnExplosion))]
        public GameObject masterCaster;

        [SyncVar(hook = nameof(CmdUpdateLocation))]
        public Vector3 explosionLocation;

        [SyncVar]
        public CastBigExplosion masterBigExplosionSkill;
        
        [SyncVar]
        public float damage;

        [Command]
        public void CmdSpawnExplosion(GameObject masterCaster)
        {
            bigExplosion = GameObject.Instantiate<GameObject>(MeguminAssets.bigExplosion);
            var networking = bigExplosion.GetComponent<BigExplosionNetworking>();
            networking.SpawnExplosionPrefab();
            networking.ExplosionPosition = explosionLocation;
            networking.Caster = masterCaster;
            RpcUpdateCasters();
        }

        [ClientRpc]
        private void RpcUpdateCasters()
        {
            if (this.gameObject != masterCaster)
            {
                this.gameObject.transform.GetComponent<SkillLocator>().special.SetSkillOverride(this.gameObject, MeguminSurvivor.coChannel, GenericSkill.SkillOverridePriority.Default);
            }
            else if (masterCaster == null)
            {
                this.gameObject.transform.GetComponent<SkillLocator>().special.UnsetSkillOverride(this.gameObject, MeguminSurvivor.coChannel, GenericSkill.SkillOverridePriority.Default);
            }
        }
        void CmdUpdateLocation(Vector3 explosionLocation)
        {
            var networking = bigExplosion.GetComponent<BigExplosionNetworking>();
            networking.ExplosionPosition = explosionLocation;
            networking.UpdatePosition();
            RpcUpdateCasters();
        }
    }
}

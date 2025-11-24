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

        [SyncVar]
        public GameObject masterCaster;

        [SyncVar(hook = nameof(UpdateLocation))]
        public Vector3 explosionLocation;

        [SyncVar]
        public CastBigExplosion masterBigExplosionSkill;
        
        [SyncVar]
        public float damage;

        [Command]
        public void CmdSpawnExplosion(GameObject masterCaster)
        {
            if (bigExplosion != null) { Log.Debug("Already Spawned Explosion! Aborting"); return; }
            if (MeguminAssets.bigExplosion == null) { Log.Error($"The fuck you mean {MeguminAssets.bigExplosion}"); return; }
            if (masterCaster == null) { Log.Error("You forgot to pass the master caster!"); return; }
            bigExplosion = GameObject.Instantiate<GameObject>(MeguminAssets.bigExplosion, masterCaster.transform);
            NetworkServer.Spawn(this.bigExplosion);
            var explosionNetworking = bigExplosion.GetComponent<BigExplosionNetworking>();
            this.masterCaster = masterCaster;
        }


        [ClientRpc]
        private void RpcUpdateCasters()
        {
            if (this.gameObject != masterCaster && masterCaster != null)
            {
                this.gameObject.transform.GetComponent<SkillLocator>().special.SetSkillOverride(this.gameObject, MeguminSurvivor.coChannel, GenericSkill.SkillOverridePriority.Default);
            }
            else if (masterCaster == null)
            {
                this.gameObject.transform.GetComponent<SkillLocator>().special.UnsetSkillOverride(this.gameObject, MeguminSurvivor.coChannel, GenericSkill.SkillOverridePriority.Default);
            }
        }

        void UpdateLocation(Vector3 explosionLocation)
        {
            var networking = bigExplosion.GetComponent<BigExplosionNetworking>();
            networking.ExplosionPosition = explosionLocation;
            RpcUpdateCasters();
        }
    }
}

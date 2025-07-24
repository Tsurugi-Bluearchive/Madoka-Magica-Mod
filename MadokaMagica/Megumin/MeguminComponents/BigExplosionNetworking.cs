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
        public Vector3 ExplosionPosition;
        [SyncVar]
        public byte ExplosionStage;
        
        public void FixedUpdate()
        {
            this.transform.position = this.ExplosionPosition;
            GameObject.Find("ExplosionLaser").GetComponent<LineBetweenTransforms>().enabled = false;
            GameObject.Find("TargetingLaser").GetComponent<LineBetweenTransforms>().enabled = true;
            GameObject.Find("ExplosionLaser").GetComponent<Light>().enabled = false;
        }

        public void SpawnExplosionPrefab()
        {
            NetworkServer.Spawn(this.gameObject);
        }
        public void Explode()
        {
            GameObject.Find("ExplosionLaser").GetComponent<LineBetweenTransforms>().enabled = true;
            GameObject.Find("TargetingLaser").GetComponent<LineBetweenTransforms>().enabled = false;
            GameObject.Find("ExplosionLaser").GetComponent<Light>().enabled = true;
        }
    }
}

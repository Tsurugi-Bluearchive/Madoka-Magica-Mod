using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Projectile;
using MadokaMagica.Megumin.Content;
using R2API;

namespace MadokaMagica.Megumin.MeguminComponents
{
    internal class MagicMissleController : MonoBehaviour
    {
        private GameObject missile;
        public BoxCollider boxCollider;
        public MissileController missileController;
        public float damage;
        public Vector3 castDirection;
        private Rigidbody rb;
        public Transform target;
        public Vector3 originPos;
        public TeamIndex teamIndex;
        private ProjectileController projectileController;
        public void Awake()
        {
            missile = GameObject.Find("MagicMissle");
            missileController = missile.gameObject.GetComponent<MissileController>();
            boxCollider = missile.gameObject.AddComponent<BoxCollider>();
            rb = missile.gameObject.GetComponent<Rigidbody>();
            projectileController = gameObject.GetComponent<ProjectileController>();

            rb.velocity = castDirection;
            missileController.targetComponent.target = target;
            GetComponent<TeamComponent>().teamIndex = teamIndex;
            missile.transform.position = originPos;
        }
    }
}

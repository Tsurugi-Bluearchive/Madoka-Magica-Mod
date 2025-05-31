using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using EntityStates;
using RoR2;
using R2API.Utils;

namespace MadokaMagica.MamiTamoe.Melee
{
    public class ScarfObjectLogic : MonoBehaviour
    {
        public float dmaageValue;
        public GameObject attacker;
        public TeamIndex team;
        public Transform SearchOrigin;
        public Vector3 searchRadius;
        public bool isCrit;
        public float procCoeff;
        public DamageColorIndex damageCol = DamageColorIndex.Default;
        public DamageType damageType = DamageType.Generic;
        private float timeSpentAlive;
        public void OnTriggerEnter(Collider enemy)
        {
            DamageInfo damageInfo = new DamageInfo
            {
                damage = this.dmaageValue,
                attacker = this.attacker,
                crit = true,
                procCoefficient = this.procCoeff,
                position = this.transform.position,
                damageColorIndex = this.damageCol,
                damageType = this.damageType
            };

            HurtBox hurtEnemy = enemy.GetComponent<HurtBox>();
            var targetHit = enemy.GetComponent<HealthComponent>();
                if (hurtEnemy != null)
            {
                GlobalEventManager.instance.OnHitEnemy(damageInfo, enemy.gameObject);
                GlobalEventManager.instance.OnHitAll(damageInfo, enemy.gameObject);
            }
            else if (targetHit != null)
            {
                targetHit.TakeDamage(damageInfo);
            }
            else
            {
                Transform hurtbox = enemy.transform.Find("Hurtbox");
                if (hurtbox != null)
                {
                    GlobalEventManager.instance.OnHitEnemy(damageInfo,hurtbox.gameObject);
                    GlobalEventManager.instance.OnHitAll(damageInfo, hurtbox.gameObject);
                }
            }
        }
        public void FixedUpdate()
        {
            timeSpentAlive = Time.fixedDeltaTime;
            if (timeSpentAlive >= 0.5f) 
            {
                Destroy(gameObject);
            }

        }
    }
}

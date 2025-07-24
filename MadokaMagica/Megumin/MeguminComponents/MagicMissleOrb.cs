
using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MadokaMagica.Megumin.Content;

namespace MadokaMagica.Megumin.MeguminComponents
{
    internal class MagicMissleOrb : GenericDamageOrb
    {
        public GameObject gameObject;
        public TeamIndex teamIndex;
        public float damageStat;
        public float damageCoefficient;
        public Vector3 targetPos;
        public override void OnArrival()
        {
            var damage = DamageTypeCombo.Generic;
            DamageAPI.AddModdedDamageType(ref damage, MeguminCustomDamageTypes.HealorHurt);
            new BlastAttack
            {
                attacker = this.gameObject,
                inflictor = this.gameObject,
                teamIndex = this.teamIndex,
                baseDamage = this.damageStat * this.damageCoefficient,
                damageType = damage,
                baseForce = 0.2f,
                position = this.targetPos,
                radius = 3f,
                falloffModel = BlastAttack.FalloffModel.None,
                bonusForce = Vector3.zero,
                damageColorIndex = DamageColorIndex.Default,
                crit = true,
                procChainMask = default
            }.Fire();
        }

        public override GameObject GetOrbEffect()
        {
            return OrbStorageUtility.Get("Prefabs/Effects/OrbEffects/ArrowOrbEffect");
        }
    }
}

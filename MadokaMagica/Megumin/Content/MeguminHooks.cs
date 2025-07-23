using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.AddressableAssets;
using R2API;
using RoR2.UI;
using RoR2.CharacterAI;
using RoR2.Projectile;
using HG;
using RoR2.Orbs;
using RoR2.Skills;
using MadokaMagica.Modules;

namespace MadokaMagica.Megumin.Content
{
    internal class MeguminHooks
    {
        internal static void Init()
        {
            On.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_ProcessHitEnemy;
        }
        private static void GlobalEventManager_ProcessHitEnemy(On.RoR2.GlobalEventManager.orig_ProcessHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            TeamIndex targetTeamComponent = victim.TryGetComponent<TeamComponent>(out var targetTeamComponentExists) ? victim.GetComponentInParent<TeamComponent>().teamIndex : TeamIndex.Monster;
            TeamComponent attackerTeamComponent = damageInfo.attacker.gameObject.TryGetComponent<TeamComponent>(out var attackerTeamComponentExists) ? damageInfo.attacker.GetComponentInParent<TeamComponent>() : null;
            TeamIndex attackerTeamType = damageInfo.attacker != null && attackerTeamComponent != null ? attackerTeamComponent.teamIndex : TeamIndex.None;
            HurtBox victimHurtbox = victim.TryGetComponent<HurtBox>(out var victimHurtboxExists) ? victim.GetComponentInParent<HurtBox>() : null;

            if (NetworkServer.active && self && victimHurtbox)
            {
                if (damageInfo.HasModdedDamageType(MeguminCustomDamageTypes.HealorHurt))
                {


                    if (targetTeamComponent == attackerTeamType && victimHurtbox)
                    {
                        victim.GetComponent<HurtBox>().healthComponent.Heal(victimHurtbox.healthComponent.fullCombinedHealth * 0.1f + damageInfo.damage, damageInfo.procChainMask, false);
                    }
                    victimHurtbox.healthComponent.TakeDamage(damageInfo);
                }
            }
            orig(self, damageInfo, victim);
        }
    }
}

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
using System.Collections.Generic;
using System;
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

            On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;
        }

        private static void HealthComponent_TakeDamageProcess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (NetworkServer.active && self.body && self.alive)
            {
                if (damageInfo.HasModdedDamageType(MeguminCustomDamageTypes.HealorHurt))
                {
                    TeamIndex targetType = self.GetComponent<TeamComponent>().teamIndex;

                    TeamComponent attackerTeamComponent = damageInfo.attacker.TryGetComponent<TeamComponent>(out var teamComponentExists) ? damageInfo.attacker.GetComponent<TeamComponent>() : null;
                    TeamIndex attackerTeamType = damageInfo.attacker != null && attackerTeamComponent != null ? attackerTeamComponent.teamIndex : TeamIndex.None;

                    if (targetType == attackerTeamType)
                    {
                        self.Heal(self.fullCombinedHealth * 0.1f + damageInfo.damage, damageInfo.procChainMask, false);
                    }
                    self.TakeDamage(damageInfo);
                }
            }
            orig(self, damageInfo);
        }


    }
}

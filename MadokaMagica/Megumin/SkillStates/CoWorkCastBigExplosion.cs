using RoR2;
using EntityStates;
using MadokaMagica.Megumin.Content;
using MadokaMagica.Megumin.SkillStates.BaseStates;
using UnityEngine.Networking;
using IL.RoR2.Projectile;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HG;
using MadokaMagica.Megumin.MeguminComponents;

namespace MadokaMagica.Megumin.SkillStates
{
    public class CoWorkCastBigExplosion : BaseMeguminSkillState
    {

        public DamageSource damageSource => DamageSource.Secondary;
        bool MasterCaster;
        GameObject bigExplosion;
        BigExplosionNetworking bigExplosionObjectNW;
        float castAddition;
        float passedTime;
        MeguminNetworkBehavior meguminNetworkBehavior;
        BullseyeSearch magicSearch;
        float damageCoefficient = MeguminStaticValues.bigExplosionDamageCoefficient;
        private bool master;
        public override void OnEnter()
        {
            base.OnEnter();
            meguminNetworkBehavior = this.gameObject.GetComponent<MeguminNetworkBehavior>();
        }

        //Reload.cs OnExit()
        public override void OnExit()
        {
            base.OnExit();
        }

        //Reload.cs FixedUpdate()
        public override void FixedUpdate()
        {
            if (fixedAge <= 70 && inputBank.skill4.down && meguminNetworkBehavior.masterCaster != null)
            {
                castAddition = ((fixedAge - passedTime) / 70) * (damageStat * damageCoefficient);
                meguminNetworkBehavior.damage += castAddition;
                passedTime = fixedAge;
            }
            else if (inputBank.skill4.justReleased || meguminNetworkBehavior.masterCaster == null)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}

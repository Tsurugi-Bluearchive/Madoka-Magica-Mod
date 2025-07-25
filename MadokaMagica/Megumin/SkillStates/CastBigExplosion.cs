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
    public class CastBigExplosion : BaseMeguminSkillState
    {

        public DamageSource damageSource => DamageSource.Secondary;
        bool MasterCaster;
        GameObject bigExplosion;
        float castAddition;
        float passedTime;
        MeguminNetworkBehavior meguminNetworkBehavior;
        BullseyeSearch magicSearch;
        float damageCoefficient = MeguminStaticValues.bigExplosionDamageCoefficient;
        private bool master;
        bool initialized;
        public override void OnEnter()
        {
            base.OnEnter();

            meguminNetworkBehavior = this.gameObject.GetComponent<MeguminNetworkBehavior>();

            magicSearch = new BullseyeSearch();
            magicSearch.minAngleFilter = 0f;
            magicSearch.maxAngleFilter = 90f;
            magicSearch.maxDistanceFilter = float.MaxValue;
            magicSearch.minDistanceFilter = 0f;
            magicSearch.viewer = this.characterBody;
            magicSearch.searchOrigin = this.characterBody.transform.position;
            magicSearch.sortMode = BullseyeSearch.SortMode.Angle;
            magicSearch.teamMaskFilter = TeamMask.AllExcept(TeamIndex.Player);

            meguminNetworkBehavior.CmdSpawnExplosion(this.gameObject);
            meguminNetworkBehavior.explosionLocation = this.gameObject.transform.position;
            initialized = true;
        }

        //Reload.cs OnExit()
        public override void OnExit()
        {
            base.OnExit();
            var crit = this.RollCrit();

            new BlastAttack
            {
                position = meguminNetworkBehavior.explosionLocation,
                damageColorIndex = DamageColorIndex.Default,
                baseDamage = meguminNetworkBehavior.damage * 0.5f,
                radius = float.MaxValue,
                damageType = DamageType.Generic,
                procChainMask = default,
                procCoefficient = default,
                attacker = this.gameObject,
                inflictor = this.gameObject,
                teamIndex = this.teamComponent.teamIndex,
                crit = crit,
                falloffModel = BlastAttack.FalloffModel.None
            }.Fire();

            new BlastAttack
            {
                position = meguminNetworkBehavior.explosionLocation,
                damageColorIndex = DamageColorIndex.Default,
                baseDamage = meguminNetworkBehavior.damage * 0.5f,
                radius = 100,
                damageType = DamageType.Generic,
                procChainMask = default,
                procCoefficient = default,
                attacker = this.gameObject,
                inflictor = this.gameObject,
                teamIndex = this.teamComponent.teamIndex,
                crit = crit,
                falloffModel = BlastAttack.FalloffModel.None
            }.Fire();
            bigExplosion.GetComponent<BigExplosionNetworking>().Explode();
            meguminNetworkBehavior.masterCaster = null;
        }
        //Reload.cs FixedUpdate()
        public override void FixedUpdate()
        {
            if (fixedAge <= 70 && inputBank.skill4.down && isAuthority && meguminNetworkBehavior.bigExplosion != null)
            {

                magicSearch.RefreshCandidates();
                List<HurtBox> list = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
                foreach (HurtBox result in magicSearch.GetResults())
                {
                    list.Add(result);
                }
                var Hurtbox = list.FirstOrDefault();
                CollectionPool<HurtBox, List<HurtBox>>.ReturnCollection(list);
                meguminNetworkBehavior.explosionLocation = Hurtbox.transform.position;
                castAddition = ((fixedAge - passedTime) / 70) * (damageStat * damageCoefficient);
                meguminNetworkBehavior.damage += castAddition;
                passedTime = fixedAge;
            }
            else if (inputBank.skill4.justReleased && isAuthority)
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

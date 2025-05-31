using MadokaMagica.MamiTamoe.BaseStates;
using MadokaMagica.Modules.BaseStates;
using RoR2;
using UnityEngine;
using System.Collections;
using MadokaMagica.MamiTamoe.Pickupables;
using EntityStates;
using R2API;
using UnityEngine;
using MadokaMagica.MamiTamoe.Melee;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class Scarf : BaseSkillState
    {
        public static float damageCoefficient = MamiStaticValues.swordDamageCoefficient;
        public static float procCoefficient = 1.2f;
        public static float baseDuration = 0.5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        private GameObject MamiScarf;
        public static float duration;
        public static float fireTime = 0.7f;
        public override void OnEnter()
        {
            base.characterBody.armor += 400;
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            base.OnEnter();
            Swing();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= fireTime)
            {

            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Swing()
        {
            MamiScarf = MamiAssets.MamiScarf;
            GameObject MamiSpawnedScarf = GameObject.Instantiate(MamiScarf);
            MamiSpawnedScarf.transform.position = this.characterBody.transform.position;
            ScarfObjectLogic MamiScarfScript = MamiSpawnedScarf.transform.Find("Scarf").GetComponent<ScarfObjectLogic>();
            MamiScarfScript.dmaageValue = this.damageStat * damageCoefficient;
            MamiScarfScript.procCoeff = procCoefficient;
            MamiScarfScript.SearchOrigin = this.gameObject.transform;
            MamiScarfScript.attacker = this.gameObject;
            MamiScarfScript.team = this.teamComponent.teamIndex;
        }
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
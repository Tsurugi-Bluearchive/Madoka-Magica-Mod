using EntityStates;
using MadokaMagica.MamiTamoe;
using MadokaMagica.MamiTamoe.Pickupables;
using RoR2;
using UnityEngine;
using MadokaMagica;
using MadokaMagica.MamiTamoe.BaseStates;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class MamiGunPassive : MonoBehaviour
    {

        private float cooldown;
        private GameObject MamiGun;
        public Scarf Scarf;
        public MamiGun mmmgun;
        public GameObject muzzleEffect;
        public void Awake()
        {
            MamiGun = MamiAssets.MamiGun;
            muzzleEffect = MamiAssets.MamiGunEffect;
        }

        public void FixedUpdate()
        {
            float precisionTick = 0f;
            cooldown += Time.fixedDeltaTime;
            if (MamiGun != null && cooldown >= 3f)
            {
                GameObject clone = GameObject.Instantiate(MamiGun);
                clone.name = "MamiGunSpawned";
                MamiGun cloneScript = clone.GetComponent<MamiGun>();
                cloneScript.OnDrop(this.transform.position);
                cloneScript.AddMe(this.gameObject);
                cloneScript.Master = this.gameObject;
                cloneScript.MasterScript = this;
                cooldown = 0f;
            }
            else if (MamiGun == null)
            {
                Log.Error("Can't find Mami's Gun!");
            }

            var PrecisionStrike = EntityStateMachine.FindByCustomName(this.gameObject, "Weapon2").state;
            if (PrecisionStrike != null && PrecisionStrike.fixedAge >= 0.5f & precisionTick >= 1f && PrecisionStrike.isAuthority)
            {
                GameObject.Instantiate(muzzleEffect).transform.parent = this.gameObject.transform;
                Transform Muzzle = this.gameObject.transform.Find("MamiGunMuzzleEffect");
                Muzzle.transform.position = this.gameObject.transform.position;
                Muzzle.transform.rotation = this.gameObject.transform.rotation;
            }
            if (PrecisionStrike != null && PrecisionStrike.fixedAge >= 0)
            {
                precisionTick += Time.fixedDeltaTime;
            }


        }

        public void PickingUpGun(MamiGun pass)
        {
            mmmgun = pass;
        }
    }
}

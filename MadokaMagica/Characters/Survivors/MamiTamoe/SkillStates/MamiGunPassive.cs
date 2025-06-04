using EntityStates;
using MadokaMagica.MamiTamoe;
using MadokaMagica.MamiTamoe.Pickupables;
using RoR2;
using UnityEngine;
using MadokaMagica;
using MadokaMagica.MamiTamoe.BaseStates;
using MadokaMagica.MamiTamoe.Components;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class MamiGunPassive : MonoBehaviour
    {

        private float cooldown;
        private GameObject MamiGun;
        public CeaselessBarrage Scarf;
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
            if (MamiGun != null && cooldown >= 4f)
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
        }

        public void PickingUpGun(MamiGun pass)
        {
            mmmgun = pass;
        }
    }
}

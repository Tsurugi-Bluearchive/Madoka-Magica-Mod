using EntityStates;
using HenryMod.Survivors.Henry;
using MadokaMagica.MamiTamoe.Pickupables;
using RoR2;
using UnityEngine;
using HenryMod;
using MadokaMagica.MamiTamoe.BaseStates;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class MamiGunPassive : MonoBehaviour
    {

        private float cooldown;
        private GameObject MamiGun;
        public void Awake()
        {
            MamiGun = MamiAssets.MamiGun;
        }

        public void FixedUpdate()
        {
            cooldown += Time.fixedDeltaTime;
            if (MamiGun != null && cooldown >= 1.5f)
            {
                GameObject clone = GameObject.Instantiate(MamiGun);
                clone.name = "MamiGunSpawned";
                MamiGun cloneScript = clone.GetComponent<MamiGun>();
                cloneScript.OnDrop(this.transform.position);
                cloneScript.AddMe(this.gameObject);
                cloneScript.Master = this.gameObject;
                cloneScript.MasterScript = this.gameObject.GetComponent<MasterMamiSkillStates>();
                cooldown = 0f;
            }
            else if (MamiGun == null)
            {
                Log.Error("Can't find Mami's Gun!");
            }
        }
    }
}

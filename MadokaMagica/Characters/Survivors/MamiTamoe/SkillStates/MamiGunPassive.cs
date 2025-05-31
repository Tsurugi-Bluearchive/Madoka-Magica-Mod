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
        public void Awake()
        {
            MamiGun = MamiAssets.MamiGun;
        }

        public void FixedUpdate()
        {
            Scarf = gameObject.GetComponent<Scarf>();
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
        }

        public void PickingUpGun(MamiGun pass)
        {
            mmmgun = pass;
        }
    }
}

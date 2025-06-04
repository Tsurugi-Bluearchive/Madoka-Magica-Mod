using MadokaMagica.MamiTamoe.Pickupables;
using UnityEngine;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class MamiGunPassive : MonoBehaviour
    {

        private float cooldown;
        private GameObject MamiGun;
        public CeaselessBarrage Scarf;
        public MamiGun mmmgun;
        public GameObject muzzleEffect;

        //MamiGunPassive.cs Code Start
        public void Awake()
        {
            MamiGun = MamiAssets.MamiGun;
            muzzleEffect = MamiAssets.MamiGunEffect;
        }
        // MamiGunPassive.cs FixedUpdate()
        public void FixedUpdate()
        {
            float precisionTick = 0f;
            cooldown += Time.fixedDeltaTime;
            
            //MamiGunPassive.cs Gun Spawning Logic
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
        // MamiGunPassive.cs PickingUpGun()
        public void PickingUpGun(MamiGun pass)
        {
            mmmgun = pass;
        }
    }
}

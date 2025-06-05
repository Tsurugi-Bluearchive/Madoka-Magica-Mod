using MadokaMagica.MamiTamoe.Content;
using MadokaMagica.MamiTamoe.Pickupables.MamiGun;
using UnityEngine;

namespace MadokaMagica.MamiTamoe.SkillStates
{
    public class MamiGunPassive : MonoBehaviour
    {

        private float cooldown;
        public CeaselessBarrage Scarf;
        public MamiGun mmmgun;
        public GameObject muzzleEffect;

        //MamiGunPassive.cs Code Start
        public void Awake()
        {
            muzzleEffect = MamiAssets.MamiGunEffect;
        }
        // MamiGunPassive.cs FixedUpdate()
        public void FixedUpdate()
        {
            cooldown += Time.fixedDeltaTime;
            
            //MamiGunPassive.cs Gun Spawning Logic
            if (cooldown >= 4f)
            {
                var clone = GameObject.Instantiate(MamiAssets.MamiGun);
                clone.name = "MamiGunSpawned";

                var cloneScript = clone.GetComponent<MamiGun>();
                cloneScript.OnDrop(this.transform.position);
                cloneScript.AddMe(this.gameObject);
                cloneScript.Master = this.gameObject;
                cloneScript.MasterScript = this;
                cooldown = 0f;
            }
        }
        // MamiGunPassive.cs PickingUpGun()
        public void PickingUpGun(MamiGun pass)
        {
            mmmgun = pass;
        }
    }
}

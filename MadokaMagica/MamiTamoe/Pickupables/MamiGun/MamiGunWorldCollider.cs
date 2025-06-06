using UnityEngine;

namespace MadokaMagica.MamiTamoe.Pickupables.MamiGun
{
    public class MamiGunWorldCollider : MonoBehaviour
    {
        public MamiGun MamiGun;
        //MamiGunWorldCollider.cs Code Start
        public void OnTriggerStay(Collider collision)
        {
            MamiGun.impactedworld = true;
            Destroy(MamiGun.thisBody);
            if (MamiGun == null)
                Debug.Log("Erorr with mamigun!");
        }
    }
}

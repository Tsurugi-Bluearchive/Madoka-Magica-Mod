using UnityEngine;

namespace MadokaMagica.MamiTamoe.Components
{
    internal class BulletRotato : MonoBehaviour
    {
        public void FixedUpdate()
        {
            var acc =+ 20f;
            this.transform.rotation = Quaternion.Euler(0, 0, acc);
        }
    }
}

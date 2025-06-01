using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MadokaMagica.MamiTamoe.Components
{
    internal class MuzzleFlash : MonoBehaviour
    {
        // Start is called before the first frame update
        private float timeAlive;

        public void FixedUpdate()
        {
            timeAlive += Time.fixedDeltaTime;
            if (timeAlive >= 0.1f)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}

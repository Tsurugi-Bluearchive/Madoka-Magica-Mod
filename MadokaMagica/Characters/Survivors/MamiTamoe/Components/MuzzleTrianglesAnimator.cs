using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadokaMagica.MamiTamoe.Components
{
    internal class MuzzleTrianglesAnimator : MonoBehaviour
    {
        private float acc;
        private float existanceTime;
        public void Awake()
        {
            acc = 0f;
            Rigidbody rb = this.transform.GetComponent<Rigidbody>();
            rb.velocity += transform.forward * Random.Range(10f, 15f);
            rb.velocity += transform.up * Random.Range(2f, -2f);
            rb.velocity += transform.right * Random.Range(2f, -2f);
        }
        public void FixedUpdate()
        {
            acc += Random.Range(20f, 30f);
            this.transform.rotation = Quaternion.Euler(acc, acc, acc);
            this.existanceTime += Time.fixedDeltaTime;
            if (existanceTime > 0.4f)
            {
                GameObject.Destroy(this.gameObject);
            }

        }
    }
}

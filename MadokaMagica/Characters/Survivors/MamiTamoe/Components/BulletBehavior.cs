using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MadokaMagica.MamiTamoe.Components
{
    internal class BulletBehavior : MonoBehaviour
    {
        private float acc;
        public float distanceToTravel = 10f;

        private Vector3 startPosition;
        private Rigidbody rb;

        public void Awake()
        {
            rb = GetComponent<Rigidbody>();
            startPosition = transform.position;
            rb.velocity = transform.right * -10;
        }

        public void FixedUpdate()
        {

            transform.rotation = Quaternion.Euler(0f, 0f, acc);
            acc += 20f;
            float dist = Vector3.Distance(startPosition, transform.position);
            if (dist >= distanceToTravel)
            {
                Destroy(gameObject);
            }
        }
    }
}
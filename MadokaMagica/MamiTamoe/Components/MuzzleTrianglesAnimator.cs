using UnityEngine;

namespace MadokaMagica.MamiTamoe.Components
{
    public class MuzzleTrianglesAnimator : MonoBehaviour
    {
        private float acc;
        private float existanceTime;
        public void Awake()
        {
            acc = 0f;
            var rb = this.transform.GetComponent<Rigidbody>();
            var parent = this.gameObject.transform.parent;
            this.transform.position = parent.position;
            this.transform.position += new Vector3(0.75f, 0.95f, 0f);
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
                gameObject.SetActive(false);
            }

        }
        public void ReAwake()
        {
            acc = 0f;
            var rb = this.transform.GetComponent<Rigidbody>();
            var parent = this.gameObject.transform.parent;
            this.transform.position = parent.position;
            this.transform.position += new Vector3(0.75f, 0.95f, 0f);
            rb.velocity += transform.forward * Random.Range(10f, 15f);
            rb.velocity += transform.up * Random.Range(2f, -2f);
            rb.velocity += transform.right * Random.Range(2f, -2f);
        }
    }
}

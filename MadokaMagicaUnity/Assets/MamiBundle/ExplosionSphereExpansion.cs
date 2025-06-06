using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSphereExpansion : MonoBehaviour
{
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        this.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
    }
}

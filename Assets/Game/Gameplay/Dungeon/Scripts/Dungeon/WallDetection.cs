using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, .01f);

        foreach (Collider col in colliders)
        {
            if(col.tag == "Wall")
            {
                Destroy(col.gameObject);
                return;
            }
        }
    }

}

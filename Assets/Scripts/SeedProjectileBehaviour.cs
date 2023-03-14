using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedProjectileBehaviour : MonoBehaviour
{

    public GameObject particleEffect;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject psystemInstance = Instantiate(particleEffect);
        psystemInstance.transform.position = collision.contacts[0].point;
        Destroy(gameObject);
    }
}

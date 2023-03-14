using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{

    public GameObject Fire;


    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<Flammable>() != null) 
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Flammable>().FireDamage = 0;
            }

            if (!other.GetComponent<Flammable>().onFire)
            {
                GameObject FireInstance = Instantiate(Fire);
                FireInstance.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                FireInstance.transform.parent = other.transform;
                other.GetComponent<Flammable>().FireInstance = FireInstance;
                other.GetComponent<Flammable>().whenOnFire();
                other.GetComponent<Flammable>().onFire = true;
            }
        }
    }  
}

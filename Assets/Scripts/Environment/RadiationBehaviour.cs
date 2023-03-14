using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationBehaviour : MonoBehaviour
{



    private float contamination;
    public bool inRadiationZone;
    private GameObject player;
    private void Update()
    {
        if (inRadiationZone)
        {
            contamination = 1 - Vector3.Distance(player.transform.position, transform.position) / GetComponent<SphereCollider>().radius ;

            GetComponent<FMODUnity.StudioEventEmitter>().SetParameter("Contamination", contamination);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") 
        {
            player = other.gameObject;
            inRadiationZone = true;
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRadiationZone = false;

        }
    }

}

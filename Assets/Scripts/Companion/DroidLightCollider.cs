using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidLightCollider : MonoBehaviour
{
    public bool inScanPosition;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Cactus")
        {
            inScanPosition = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cactus")
        {
            inScanPosition = false;
        }
    }
}

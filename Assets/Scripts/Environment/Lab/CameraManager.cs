using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject MainCamera;
    public GameObject Player;
    public float coverAmount;
    public ParticleSystem Rain;
    private GameManager gmanager;
    public bool isIndoors;
    // Start is called before the first frame update
    void Start()
    {
        gmanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        virtualCam = transform.GetChild(0).gameObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
          gmanager.inCover(coverAmount);
          virtualCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = true;
          Rain.Stop();
            if (isIndoors) 
            {
                gmanager.insideBuilding();
            }
            
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            gmanager.outsideBuilding();
            gmanager.inCover(0.0f);
            virtualCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = false;
            Rain.Play();           
        }
    }   
}


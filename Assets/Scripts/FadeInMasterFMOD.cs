using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;

public class FadeInMasterFMOD : MonoBehaviour
{
    //comment!

    [Range(0f, 1f)]
    public float volume;
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MasterVol", volume);
    }
}

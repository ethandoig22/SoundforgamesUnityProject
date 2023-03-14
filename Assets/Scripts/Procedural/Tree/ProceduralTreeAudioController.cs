using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
public class ProceduralTreeAudioController : MonoBehaviour
{

    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;

    [SerializeField]
    [Range(0f, 1f)]
    private float scale;

    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }

    void Update()
    {
        instance.setParameterByName("Scale", scale);

    }
}

